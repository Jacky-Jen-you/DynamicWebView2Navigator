using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebView2Demo.WebBridges
{
	public class WebMessage
	{
		public string action { get; set; }

		public dynamic data { get; set; }
	}
}
