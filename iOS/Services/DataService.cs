using System;
using FlightDataRecorder.FlightRecord.Model;
using FlightDataRecorder.FlightList.Model;
using System.IO;
using System.Json;

using CoreLocation;
using UIKit;
using Foundation;

namespace FlightDataRecorder.iOS.Services
{
	public class DataService : IDataService
	{
		private string documentsPath;

		private CLLocationManager locationManager;

		private bool runningService;

		private Flight flight;

		private StreamWriter stream;

		public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };		
		
		public DataService (string documentsPath) 
		{
			this.locationManager = new CLLocationManager();
			
			this.documentsPath = documentsPath;
			
			LocationUpdated += PrintLocation;			
		}

		public string Check ()
		{
			if (!CLLocationManager.LocationServicesEnabled))
			{
				return "GPS location services not enabled, please enable this in your Settings";
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
			
			locationManager.DesiredAccuracy = 1; // sets the accuracy that we want in meters

			// Location updates are handled differently pre-iOS 6. If we want to support older versions of iOS,
			// we want to do perform this check and let our LocationManager know how to handle location updates.
			if (UIDevice.CurrentDevice.CheckSystemVersion (6, 0)) 
			{
				locationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) => {
					// fire our custom Location Updated event
					this.LocationUpdated (this, new LocationUpdatedEventArgs (e.Locations [e.Locations.Length - 1]));
				};
			} 
			else 
			{
				// this won't be called on iOS 6 (deprecated). We will get a warning here when we build.
				locationManager.UpdatedLocation += (object sender, CLLocationUpdatedEventArgs e) => {
					this.LocationUpdated (this, new LocationUpdatedEventArgs (e.NewLocation));
				};
			}

			// Start our location updates
			locationManager.StartUpdatingLocation ();
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

			locationManager.StopUpdatingLocation ();			
		}

		public void PrintLocation (object sender, LocationUpdatedEventArgs e) 
		{
			CLLocation location = e.Location;

			flight.Data.GpsLatitude = location.Coordinate.Latitude;

			flight.Data.GpsLongitude = location.Coordinate.Longitude;

			JsonObject obj = new JsonObject();
			obj.Add ("Time", DateTime.Now.ToString());
			obj.Add ("Gps Latitude", location.Coordinate.Latitude);
			obj.Add ("Gps Longitude", location.Coordinate.Longitude);

			stream.Write (obj.ToString ());
			stream.WriteLine ();
		}
	}
}

