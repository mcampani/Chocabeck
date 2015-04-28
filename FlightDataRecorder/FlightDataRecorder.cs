using System;

using Xamarin.Forms;
using FlightDataRecorder.Database;
using FlightDataRecorder.FlightList.View;
using FlightDataRecorder.Resources;
using FlightDataRecorder.FlightList.View;
using FlightDataRecorder.FlightRecord.Model;

namespace FlightDataRecorder
{
	public class App : Application
	{
		private static IDataService dataService;
		private static IFlightsDataStorage database;
		private static ResourceManager resourceManager;

		public App (IDataService nativeApp)
		{
			dataService = nativeApp;

			database = new SQLiteDataStorage ();

			resourceManager = new ResourceManager ();

			var navigationPage = new NavigationPage (new FlightListPage ()) 
			{ 
				BarTextColor = (Color) resourceManager.getResource("BarTextColor"),
				BarBackgroundColor = (Color) resourceManager.getResource("BarBackgroundColor")
			};

			MainPage = navigationPage;
		}

		public static IDataService DataService
		{
			get { return dataService; }
		}

		public static IFlightsDataStorage FlightsDataStorage
		{
			get { return database; }
		}

		public ResourceManager ResourceManager 
		{ 
			get { return resourceManager; }
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}