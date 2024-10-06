using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebView2Demo.WebBridges.Navigator
{
	public class NavigatorBridge : WebBridgeBase
	{
		public EventHandler<NavigationItem> OnNavigatorTrigger;

		public IEnumerable<NavigationItem> NavigatorItems { get; private set; }

		public NavigatorBridge( IEnumerable<NavigationItem> navigatorItems )
		{
			this.NavigatorItems = navigatorItems;
		}

		protected override void ReceivedStringFromWeb( string message )
		{
		}

		private async void SendNavigatorItemsToJs()
		{
			string json = Newtonsoft.Json.JsonConvert.SerializeObject( this.NavigatorItems );
			this.SendMessageToWeb( $"navigatorItems = {json}; window.generateNavigation(navigatorItems);" );
		}

		protected override void ReceivedJsonFromWeb( string json )
		{
		}

		protected override void ReceivedWebMessageFromWeb( WebMessage webMessage )
		{
			if ( webMessage == null )
				return;

			switch ( webMessage.action )
			{
				case "UpdateNavigatorItems":
					SendNavigatorItemsToJs();
					break;
				case "Navigate":
					NavigateContent( webMessage.data );
					break;
				default:
					break;
			}
		}

		private void NavigateContent( dynamic data )
		{
			NavigationItem navigatorItem = JsonConvert.DeserializeObject<NavigationItem>( data.ToString() );
			this.OnNavigatorTrigger?.Invoke( this, navigatorItem );
		}
	}
}