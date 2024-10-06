using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebView2Demo.WebBridges;

namespace WebView2Demo
{
	public abstract class WebBridgeBase
	{
		public EventHandler<string> SendMessageToWebEvent = null;

		private const string STR_RE_PATTERN = "^\"(.*)\"$";

		public WebBridgeBase()
		{
		}

		public async void ReceiveMessageFromWeb( string message )
		{
			Match match = Regex.Match( message, STR_RE_PATTERN );

			if ( match.Success )
			{
				this.ReceivedStringFromWeb( match.Groups[ 1 ].Value );
			}
			else if ( ( message.StartsWith( "{" ) && message.EndsWith( "}" ) )
				|| ( message.StartsWith( "[" ) && message.EndsWith( "]" ) ) )
			{
				WebMessage webMessage = null;

				bool isWebMessage = true;

				try
				{
					webMessage = JsonSerializer.Deserialize<WebMessage>( message );
				}
				catch ( Exception ex )
				{
					// message can't deserialize to WebMessage
					isWebMessage = false;
				}

				if ( isWebMessage && webMessage != null && webMessage.action != null )
					this.ReceivedWebMessageFromWeb( webMessage );
				else
					this.ReceivedJsonFromWeb( message );
			}
			else
			{
				throw new Exception( "Invalid message format received from web." );
			}
		}

		protected virtual void SendMessageToWeb( string message )
		{
			this.SendMessageToWebEvent?.Invoke( this, message );
		}

		protected abstract void ReceivedWebMessageFromWeb( WebMessage webMessage );
		protected abstract void ReceivedStringFromWeb( string str );
		protected abstract void ReceivedJsonFromWeb( string json );
	}
}