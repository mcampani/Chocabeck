using System;
using SQLite.Net;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Locations;
using FlightDataRecorder.FlightRecord.Model;
using FlightDataRecorder.FlightList.Model;
using System.IO;
using FlightDataRecorder.Droid.Services;
using System.Timers;

namespace FlightDataRecorder.Droid
{
	[Activity(Theme = "@style/AppTheme", MainLauncher = true, NoHistory = true, Label = "Flight Data Recorder", Icon = "@drawable/icon")]
	public class SplashActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.splash_screen);

			Timer timer = new Timer();
			timer.Interval = 1000; 
			timer.AutoReset = false; 
			timer.Elapsed += (object sender, ElapsedEventArgs e) =>
			{
				StartActivity(typeof(MainActivity));
			};

			timer.Start();
		}
	};

	[Activity (Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		private IDataService dataService;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

			var documentsPath = this.BaseContext.GetExternalFilesDir ("").AbsolutePath;
			var locationManager = this.GetSystemService (Context.LocationService) as LocationManager;

			//			dataService = new MockDataService (documentsPath);
			dataService = new DataService (locationManager, documentsPath);

			LoadApplication (new App (dataService));
		}

		protected override void OnDestroy ()
		{
			dataService.Stop ();

			base.OnDestroy ();
		}
	}
}