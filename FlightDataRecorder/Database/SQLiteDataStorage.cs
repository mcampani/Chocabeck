using System;
using SQLite.Net;
using System.Collections.Generic;
using FlightDataRecorder.Database;
using SQLite.Net.Interop;
using Xamarin.Forms;
using System.Threading.Tasks;
using FlightDataRecorder.FlightList.Model;
using FlightDataRecorder.FlightAdd.Model;
using FlightDataRecorder.FlightRecord.Model;

namespace FlightDataRecorder.Database
{
	public class SQLiteDataStorage : IFlightsDataStorage
	{
		static object locker = new object ();

		SQLiteConnection database;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
		/// if the database doesn't exist, it will create the database and all the tables.
		/// </summary>
		/// <param name='path'>
		/// Path.
		/// </param>
		public SQLiteDataStorage()
		{
			database = DependencyService.Get<ISQLite> ().GetConnection ();

			// create the tables
			database.CreateTable<FlightInfo>();
			database.CreateTable<FlightData>();
		}

		#region IFlightsDataStorage implementation

		public async Task<List<Flight>> GetFlightsAsync ()
		{
			List<Flight> flights = new List<Flight> ();

			await Task.Run (() => {
					
				lock (locker) 
				{
					var queryInfo = database.Table<FlightInfo>();

					foreach (var flightInfo in queryInfo)
					{
						var queryData = database.Table<FlightData>().Where(x => x.FlightId == flightInfo.Id);

						var flightData = queryData.First();

						Flight flight = new Flight(flightInfo, flightData);

						flights.Add(flight);
					}
				}
			});

			return flights;
		}
						
		public void AddFlight (Flight flight)
		{
			lock (locker) 
			{
				database.Insert(flight.Info);

				flight.Data.FlightId = flight.Info.Id;

				database.Insert(flight.Data);
			}
		}

		public void DeleteFlight (Flight flight)
		{
			lock (locker) 
			{
				database.Execute("DELETE FROM FlightData WHERE FlightId = ?", flight.Info.Id);

				database.Delete<FlightInfo> (flight.Info.Id);
			}
		}

		public void UpdateFlightInfo (FlightInfo flightInfo)
		{
			lock (locker) 
			{
				database.Update (flightInfo);
			}
		}

		#endregion
	}
}

