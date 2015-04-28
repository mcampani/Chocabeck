using System;
using Android.Locations;
using FlightDataRecorder.FlightRecord.Model;
using System.Threading.Tasks;
using FlightDataRecorder.FlightList.Model;
using System.IO;
using System.Json;

namespace FlightDataRecorder.Droid
{
	public class MockDataService : Java.Lang.Object, IDataService
	{
		private bool run;

		private float latitude;
		private float longitude;

		private Flight flight;

		private StreamWriter stream;

		private string documentsPath;

		public MockDataService (string documentsPath)
		{
			this.documentsPath = documentsPath;
		}
			
		#region IDataService implementation

		public string Check ()
		{
			if (run)
			{
				return "Another recording is being processed";
			}

			return null;
		}
			
		public void Start (Flight flight)
		{
			run = true;

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

			latitude = 0;
			longitude = 100;

			Task.Run(() => {  

				while (run)
				{
					latitude++;
					longitude++;

					flight.Data.GpsLatitude = latitude;
					flight.Data.GpsLongitude = longitude;

					JsonObject data = new JsonObject();
					data.Add ("Time", DateTime.Now.ToString());
					data.Add ("Gps Latitude", latitude);
					data.Add ("Gps Longitude", longitude);

					stream.Write (data.ToString ());
					stream.WriteLine ();

					System.Threading.Thread.Sleep(flight.Data.Frequency * 1000);
				}
			});
		}

		public void Delete (Flight flight)
		{
			var fileName = flight.Info.Number + "_" + flight.Info.CreationDate.ToString("yyyyMMddHHmmss") + ".txt";
			var filePath = Path.Combine (documentsPath, fileName);

			File.Delete (filePath);
		}
			
		public void Stop ()
		{
			run = false;

			flight = null;

			stream.Close ();
		}

		#endregion
	}
}

