using System;
using FlightDataRecorder.FlightAdd.Model;
using FlightDataRecorder.FlightRecord.Model;

namespace FlightDataRecorder.FlightList.Model
{
	public class Flight
	{
		private FlightInfo flightInfo;

		private FlightData flightData;

		public Flight (FlightInfo flightInfo, FlightData flightData)
		{
			this.flightInfo = flightInfo;
			this.flightData = flightData;
		}

		public FlightInfo Info
		{
			get { return flightInfo; }
			set { flightInfo = value; }
		}

		public FlightData Data
		{
			get { return flightData; }
			set { flightData = value; }
		}
			
		public bool IsValid
		{
			get { return flightInfo.IsValid && flightData.IsValid; }
		}
	}
}

