using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FlightDataRecorder.FlightList.Model;
using FlightDataRecorder.FlightAdd.Model;
using FlightDataRecorder.FlightRecord.Model;

namespace FlightDataRecorder.Database
{
	public interface IFlightsDataStorage
	{
		Task<List<Flight>> GetFlightsAsync ();

		void DeleteFlight (Flight flight);

		void UpdateFlightInfo (FlightInfo flightInfo);

		void AddFlight (Flight flight);
	}
}

