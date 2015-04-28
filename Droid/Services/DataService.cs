using System;
using FlightDataRecorder.FlightRecord.Model;
using Android.Locations;
using FlightDataRecorder.FlightList.Model;
using System.IO;
using System.Json;

namespace FlightDataRecorder.Droid.Services
{
	public class DataService : Java.Lang.Object, IDataService, ILocationListener
	{
		private string documentsPath;

		private LocationManager locationManager;

		private bool runningService;

		private Flight flight;

		private StreamWriter stream;

		public DataService (LocationManager locationManager, string documentsPath) 
		{
			this.locationManager = locationManager;

			this.documentsPath = documentsPath;
		}

		public string Check ()
		{
			if (!locationManager.AllProviders.Contains(LocationManager.GpsProvider))
			{
				return "The GPS provider does not exist";
			}

			if (!locationManager.IsProviderEnabled (LocationManager.GpsProvider))
			{
				return "The GPS provider is not enabled";
			}

			if (runningService)
			{
				return "Another recording is being processed";
			}

			return null;
		}

		public void Start (Flight flight)
		{
			this.flight = flight;

			var fileName = flight.Info.Number + "_" + flight.Info.CreationDate.ToString("yyyyMMddHHmmss") + ".txt";
			var filePath = Path.Combine (documentsPath, fileName);

			stream = new StreamWriter (filePath);

			JsonObject obj = new JsonObject();
			obj.Add ("CreationDate", flight.Info.CreationDate.ToString());
			obj.Add ("DepartureCode", flight.Info.DepartureCode);
			obj.Add ("DestionationCode", flight.Info.DestinationCode);
			obj.Add ("FlightNumber", flight.Info.Number);
			obj.Add ("FlightOperator", flight.Info.Operator);
			obj.Add ("AircraftRegistration", flight.Info.AircraftRegistration);

			stream.Write (obj.ToString ());
			stream.WriteLine ();

			locationManager.RequestLocationUpdates (LocationManager.GpsProvider, flight.Data.Frequency * 1000, 1, this);
		}

		public void Delete (Flight flight)
		{
			var fileName = flight.Info.Number + "_" + flight.Info.CreationDate.ToString("yyyyMMddHHmmss") + ".txt";
			var filePath = Path.Combine (documentsPath, fileName);

			File.Delete (filePath);
		}
						
		public void Stop ()
		{
			flight = null;

			if (stream != null)
				stream.Close ();

			runningService = false;

			locationManager.RemoveUpdates (this);
		}

		#region ILocationListener implementation

		public void OnLocationChanged (Location location)
		{
			flight.Data.GpsLatitude = location.Latitude;

			flight.Data.GpsLongitude = location.Longitude;

			JsonObject obj = new JsonObject();
			obj.Add ("Time", DateTime.Now.ToString());
			obj.Add ("Gps Latitude", location.Latitude);
			obj.Add ("Gps Longitude", location.Longitude);

			stream.Write (obj.ToString ());
			stream.WriteLine ();
		}

		public void OnProviderDisabled (string provider)
		{
		}

		public void OnProviderEnabled (string provider)
		{
		}

		public void OnStatusChanged (string provider, Availability status, Android.OS.Bundle extras)
		{
		}

		#endregion

	}
}

