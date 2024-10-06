using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebView2Demo.WebBridges.Pages
{
	public class AboutMeWebBridge : WebBridgeBase
	{
		protected override void ReceivedJsonFromWeb( string json )
		{
			throw new NotImplementedException();
		}

		protected override void ReceivedStringFromWeb( string str )
		{
			throw new NotImplementedException();
		}

		protected override void ReceivedWebMessageFromWeb( WebMessage webMessage )
		{
			throw new NotImplementedException();
		}
	}
}
