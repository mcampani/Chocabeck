using System;
using SQLite.Net.Attributes;
using System.ComponentModel;

namespace FlightDataRecorder.FlightRecord.Model
{
	public class FlightData : INotifyPropertyChanged
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[Indexed]
		public int FlightId { get; set; }

		private int frequency;

		private double gpsLatitude;
		private double gpsLongitude;

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public FlightData ()
		{
		}

		public FlightData (int frequency)
		{
			this.frequency = frequency;
		}

		public int Frequency
		{
			get { return frequency; }
			set { frequency = value; }
		}

		public double GpsLatitude
		{
			get { return gpsLatitude; }
			set { 
				gpsLatitude = value; 
				OnPropertyChanged ("GpsLatitude");
			}
		}

		public double GpsLongitude
		{
			get { return gpsLongitude; }
			set { 
				gpsLongitude = value; 
				OnPropertyChanged ("GpsLongitude");
			}
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged (this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public bool IsValid
		{
			get { return frequency > 0; }
		}
	}
}

