using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebView2Demo
{
	public class NavigationManager
	{
		public List<NavigationItem> NavigationItems { get; private set; }

		public NavigationManager()
		{
			this.NavigationItems = new List<NavigationItem>();
		}

		public void LoadNavigationItemsFromDirectory( string folderPath )
		{
			if ( folderPath == null || folderPath == string.Empty )
				return;

			if ( !Directory.Exists( folderPath ) )
			{
				throw new DirectoryNotFoundException( $"The directory '{folderPath}' does not exist." );
			}

			string[] xmlFiles = Directory.GetFiles( folderPath, "*.xml" );

			foreach ( string xmlFile in xmlFiles )
			{
				try
				{
					NavigationItem item = DeserializeNavigationItemFromXml( xmlFile );

					if ( item != null )
					{
						NavigationItems.Add( item );
						Console.WriteLine( $"Loaded NavigationItem from: {xmlFile}" );
					}
				}
				catch ( Exception ex )
				{
					Console.WriteLine( $"Failed to load NavigationItem from: {xmlFile}. Error: {ex.Message}" );
				}
			}
		}

		private NavigationItem DeserializeNavigationItemFromXml( string filePath )
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer( typeof( NavigationItem ) );
				using ( FileStream fs = new FileStream( filePath, FileMode.Open ) )
				{
					return ( NavigationItem ) serializer.Deserialize( fs );
				}
			}
			catch ( Exception ex )
			{
				Console.WriteLine( $"Error deserializing file {filePath}: {ex.Message}" );

				return null;
			}
		}
	}
}