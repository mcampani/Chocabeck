using System;
using FlightDataRecorder.FlightList.Model;

namespace FlightDataRecorder.FlightRecord.Model
{
	public interface IDataService
	{
		string Check ();
		void Delete (Flight flight);
		void Start (Flight flight);
		void Stop ();
	}
}

