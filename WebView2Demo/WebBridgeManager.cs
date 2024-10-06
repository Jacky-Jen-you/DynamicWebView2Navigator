using DryIoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebView2Demo
{
	public class WebBridgeManager
	{
		public Container IoCContainer { get; private set; } = new Container();

		public WebBridgeManager()
		{
		}

		public void RegistWithNavigationItems( IEnumerable<NavigationItem> navigationItems )
		{
			foreach ( var item in navigationItems )
			{
				RegistSingleton( item );
			}
		}

		public void RegistSingleton( Type implementationType, string serviceKey )
		{
			IoCContainer.Register( typeof( WebBridgeBase ), implementationType, Reuse.Singleton, serviceKey: serviceKey );
		}

		public WebBridgeBase GetInstanceByServiceKey( string serviceKey )
		{
			return this.IoCContainer.Resolve<WebBridgeBase>( typeof( WebBridgeBase ), serviceKey: serviceKey );
		}

		public WebBridgeBase GetInstanceByNavigationItem( NavigationItem navigationItem )
		{
			return this.IoCContainer.Resolve<WebBridgeBase>( typeof( WebBridgeBase ), serviceKey: navigationItem.Name );
		}

		private void RegistSingleton( NavigationItem navigationItem )
		{
			Type webBridgeType;

			if ( navigationItem.DLLName == null || navigationItem.DLLName == string.Empty ) // Internal module
			{
				webBridgeType = FindTypeFromCurrentAssemblies( navigationItem.WebBridgeFullName );
			}
			else // External module
			{
				string _externalAssemblyPath = "";

				var assembly = Assembly.LoadFrom( Path.Combine( _externalAssemblyPath, navigationItem.DLLName ) );
				webBridgeType = assembly.GetType( navigationItem.WebBridgeFullName );
			}

			this.RegistSingleton( webBridgeType, navigationItem.Name );
		}

		private Type FindTypeFromCurrentAssemblies( string typeName )
		{
			return AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany( assembly => assembly.GetTypes() )
				.FirstOrDefault( t => t.FullName == typeName );
		}
	}
}