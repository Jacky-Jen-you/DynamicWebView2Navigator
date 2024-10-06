using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WebView2Demo.WebBridges.Pages
{
	public class HomePageWebBridge : WebBridgeBase
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
			switch ( webMessage.action )
			{
				case "SelectFile":
					var selectedFilename = SelectFile();

					if ( selectedFilename != null ) 
						this.SendMessageToWeb( $"message = /{selectedFilename}/; window.receiveDataFromCSharp(message)" );

					break;
			}
		}

		private string SelectFile()
		{
			// 創建 OpenFileDialog 的實例
			OpenFileDialog openFileDialog = new OpenFileDialog();

			// 設置初始目錄（例如桌面或文檔）
			openFileDialog.InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.Desktop );

			// 是否允許選擇多個文件
			openFileDialog.Multiselect = false;

			// 顯示對話框並檢查用戶是否選擇了文件
			bool? result = openFileDialog.ShowDialog();

			if ( result == true )
				return openFileDialog.FileName;

			return null;
		}
	}
}