using System;
using FlightDataRecorder.FlightList.Model;
using FlightDataRecorder.FlightAdd.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FlightDataRecorder.FlightRecord.Model;

namespace FlightDataRecorder.FlightAdd.ViewModel
{
	public class FlightViewModel
	{
		private Flight flight;

		public FlightViewModel ()
		{
			flight = new Flight (new FlightInfo(), new FlightData());
		}

		public FlightViewModel (Flight flight)
		{
			this.flight = flight;
		}

		public Flight Flight
		{
			get { return flight; }
			set { flight = value; }
		}
	}
}

