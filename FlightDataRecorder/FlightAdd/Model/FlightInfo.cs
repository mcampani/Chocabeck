using System;
using SQLite.Net.Attributes;
using System.ComponentModel;

namespace FlightDataRecorder.FlightAdd.Model
{
	public class FlightInfo : INotifyPropertyChanged
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		private string departureCode;
		private string destinationCode;

		private string flightNumber;
		private string flightOperator;
		private string aircraftRegistration;

		private DateTime creationDate;
		private DateTime startRecordingDate;
		private DateTime stopRecordingDate;

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public FlightInfo ()
		{
		}

		public FlightInfo (string departureCode, string destinationCode, string flightNumber, string flightOperator, string aircraftRegistration)
		{
			this.departureCode = departureCode;
			this.destinationCode = destinationCode;

			this.flightNumber = flightNumber;
			this.flightOperator = flightOperator;
			this.aircraftRegistration = aircraftRegistration;
		}

		public string DepartureCode
		{
			get { return departureCode; }
			set { departureCode = value; }
		}

		public string DestinationCode
		{
			get { return destinationCode; }
			set { destinationCode = value; }
		}

		public string Number
		{
			get { return flightNumber; }
			set { flightNumber = value; }
		}

		public string Operator
		{
			get { return flightOperator; }
			set { flightOperator = value; }
		}

		public string AircraftRegistration
		{
			get { return aircraftRegistration; }
			set { aircraftRegistration = value; }
		}
			
		public DateTime CreationDate
		{
			get { return creationDate; }
			set { creationDate = value; }
		}

		public DateTime StartRecordingDate
		{
			get { return startRecordingDate; }
			set { 
				startRecordingDate = value; 
				OnPropertyChanged ("StartRecordingDate");
			}
		}

		public DateTime StopRecordingDate
		{
			get { return stopRecordingDate; }
			set { 
				stopRecordingDate = value; 
				OnPropertyChanged ("StopRecordingDate");
			}
		}
			
		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged (this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public bool IsPending
		{
			get { return !IsRecording && !IsStopped; }
		}

		public bool IsRecording
		{
			get { return this.StartRecordingDate.CompareTo (this.CreationDate) > 0 && !IsStopped; }
		}

		public bool IsStopped
		{
			get { return this.StopRecordingDate.CompareTo(this.CreationDate) > 0; }
		}
			
		public bool IsValid
		{
			get { return departureCode != null && !departureCode.Equals ("") &&
				destinationCode != null && !destinationCode.Equals ("") &&
				flightNumber != null && !flightNumber.Equals ("") &&
				flightOperator != null && !flightOperator.Equals ("") &&
				aircraftRegistration != null && !aircraftRegistration.Equals (""); 
			}
		}
	}
}