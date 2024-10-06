using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebView2Demo.WebBridges.Navigator;

namespace WebView2Demo
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		NavigatorBridge _navigatorBridge;
		WebBridgeBase _contentBridge;

		WebBridgeManager _webBridgeManager;
		NavigationManager _navigationManager;

		public MainWindow()
		{
			InitializeComponent();
			InitializeWebViewAsync();
		}

		private async void InitializeWebViewAsync()
		{
			string baseRoot = AppDomain.CurrentDomain.BaseDirectory;
			string navigationsConfigPath = System.IO.Path.Combine( baseRoot, "Config/Navigations/" );

			_navigationManager = new NavigationManager( );
			_navigationManager.LoadNavigationItemsFromDirectory( navigationsConfigPath );

			_webBridgeManager = new WebBridgeManager();
			_webBridgeManager.RegistWithNavigationItems( _navigationManager.NavigationItems );

			await this.NavigatorWebView2.EnsureCoreWebView2Async( null );
			await this.ContentWebView2.EnsureCoreWebView2Async( null );

			_navigatorBridge = new NavigatorBridge( _navigationManager.NavigationItems );
			_navigatorBridge.SendMessageToWebEvent += NavigationWebView2SendMessageToWebEventAsync;
			_navigatorBridge.OnNavigatorTrigger += OnNavigatorTrigger;

			string navigatorRelativePath = @"Assets\Navigator\Navigator.html";

			this.NavigatorWebView2.CoreWebView2.Navigate( System.IO.Path.Combine( baseRoot, navigatorRelativePath ) );
			this.NavigatorWebView2.WebMessageReceived += NavigatorWebView2_WebMessageReceivedAsync;

			string contentWebView2InitalUrl = System.IO.Path.Combine( baseRoot, _navigationManager.NavigationItems[ 0 ].Url );
			_contentBridge = _webBridgeManager.GetInstanceByNavigationItem( _navigationManager.NavigationItems[ 0 ] );

			this.ContentWebView2.CoreWebView2.Navigate( contentWebView2InitalUrl );
			this.ContentWebView2.WebMessageReceived += ContentWebView2_WebMessageReceivedAsync;
		}

		private void OnNavigatorTrigger( object? sender, NavigationItem e )
		{
			var newWebBridge = _webBridgeManager.GetInstanceByNavigationItem( e );

			if ( newWebBridge != null )
			{
				newWebBridge.SendMessageToWebEvent -= ContentWebView2SendMessageToWebEventAsync;
				newWebBridge.SendMessageToWebEvent += ContentWebView2SendMessageToWebEventAsync;
			}

			string baseRoot = AppDomain.CurrentDomain.BaseDirectory;
			string url = System.IO.Path.Combine( baseRoot, e.Url );

			Application.Current.Dispatcher.Invoke( () => this.ContentWebView2.CoreWebView2.Navigate( url ) );

			if ( _contentBridge != null )
				_contentBridge.SendMessageToWebEvent -= ContentWebView2SendMessageToWebEventAsync;

			_contentBridge = newWebBridge;
		}

		private async void NavigationWebView2SendMessageToWebEventAsync( object? sender, string e )
		{
			await Application.Current.Dispatcher.InvokeAsync( async () =>
			{
				await this.NavigatorWebView2.CoreWebView2.ExecuteScriptAsync( e );
			} );
		}

		private async void ContentWebView2SendMessageToWebEventAsync( object? sender, string e )
		{
			await Application.Current.Dispatcher.InvokeAsync( async () =>
			{
				await this.ContentWebView2.CoreWebView2.ExecuteScriptAsync( e );
			} );
		}

		private async void NavigatorWebView2_WebMessageReceivedAsync( object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e )
		{
			string message = e.WebMessageAsJson;

			await Task.Run( () => { _navigatorBridge.ReceiveMessageFromWeb( message ); } );
		}

		private async void ContentWebView2_WebMessageReceivedAsync( object? sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e )
		{
			string message = e.WebMessageAsJson;

			await Task.Run( () => { _contentBridge.ReceiveMessageFromWeb( message ); } );
		}
	}
}