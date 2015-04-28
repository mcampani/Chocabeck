using System;
using System.Collections.Generic;
using System.Linq;

using FlightDataRecorder.FlightRecord.Model;

using Foundation;
using UIKit;

namespace FlightDataRecorder.iOS
{
	private string documentsPath;
	
	private IDataService dataService;
	
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();

			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) 
			{
				documentsPath = NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User) [0].AbsoluteString;			
			} 
			else 
			{
				documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);			
			}			
			
			dataService = new DataService (documentsPath);
			
			LoadApplication (new App (dataService));

			return base.FinishedLaunching (app, options);
		}
	}
}

