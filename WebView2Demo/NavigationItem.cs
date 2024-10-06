using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebView2Demo
{
	[XmlRoot( "NavigationItem" )]
	public class NavigationItem
	{
		[XmlElement( "Name" )]
		public string Name { get; set; }

		[XmlElement( "Url" )]
		public string Url { get; set; }

		[XmlElement( "WebBridgeFullName" )]
		public string WebBridgeFullName { get; set; }

		[XmlElement( "DLLName" )]
		public string DLLName { get; set; }

		[XmlElement( "Label" )]
		public string Label { get; set; }

		[XmlElement( "LabelResourceKey" )]
		public string LabelResourceKey { get; set; }
	}
}
