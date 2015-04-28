using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using FlightDataRecorder.FlightList.Model;
using FlightDataRecorder.FlightAdd.View;
using FlightDataRecorder.FlightAdd.Model;
using FlightDataRecorder.FlightRecord.View;
using FlightDataRecorder.FlightList.View;

namespace FlightDataRecorder.FlightList.ViewModel
{
	public class FlightListViewModel : INotifyPropertyChanged
	{
		private ObservableCollection<Flight> itemsSource;

		private bool isListVisible;

		public event PropertyChangedEventHandler PropertyChanged;

		public FlightListViewModel ()
		{
			itemsSource = new ObservableCollection<Flight> ();

			itemsSource.CollectionChanged += (sender, e) => 
			{
				OnPropertyChanged("ItemsSource");
				this.IsListVisible = (itemsSource != null && itemsSource.Count > 0);
			};

			MessagingCenter.Subscribe<FlightAddPage, Flight> (this, "FlightSaved", (sender, model) => 
			{
				model.Info.CreationDate = DateTime.Now;

				App.FlightsDataStorage.AddFlight(model);

				itemsSource.Add(model);
			});

			MessagingCenter.Subscribe<FlightRecordPage, Flight> (this, "StartStopRecording", (sender, model) => 
				{
					if (model.Info.IsPending)
					{
						model.Info.StartRecordingDate = DateTime.Now;
					}
					else if (model.Info.IsRecording)
					{
						model.Info.StopRecordingDate = DateTime.Now;
					}

					App.FlightsDataStorage.UpdateFlightInfo(model.Info);

					if (model.Info.IsRecording)
					{
						App.DataService.Start (model);
					}
					else if (model.Info.IsStopped)
					{
						App.DataService.Stop ();
					}
				});

			MessagingCenter.Subscribe<FlightListPage, Flight> (this, "DeleteFlight", (sender, model) => 
				{
					App.FlightsDataStorage.DeleteFlight(model);

					App.DataService.Delete(model);

					itemsSource.Remove(model);
				});
		}

		public void unsubscribeToMessaging ()
		{
			MessagingCenter.Unsubscribe<FlightAddPage, Flight> (this, "FlightSaved");
			MessagingCenter.Unsubscribe<FlightRecordPage, Flight> (this, "StartStopRecording");
			MessagingCenter.Unsubscribe<FlightListPage, Flight> (this, "DeleteFlight");
		}

		public async Task LoadFlightsAsync ()
		{
			List<Flight> flights = await App.FlightsDataStorage.GetFlightsAsync ();

			foreach (Flight flight in flights)
			{
				itemsSource.Add (flight);
			}

			this.IsListVisible = (itemsSource != null && itemsSource.Count > 0);
		}

		public ObservableCollection<Flight> ItemsSource 
		{ 
			get { return itemsSource; } 
			set
			{
				if ((itemsSource == value) || ((itemsSource.Count == 0) && (value.Count == 0)))
					return;

				itemsSource = value;
				OnPropertyChanged ("ItemsSource");

				this.IsListVisible = (itemsSource != null && itemsSource.Count > 0);
			}
		}

		public bool IsListVisible
		{
			get { return isListVisible; } 
			set
			{
				if (isListVisible == value)
					return;

				isListVisible = value;
				OnPropertyChanged ("IsListVisible");
			}
		}

		protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler (this, new PropertyChangedEventArgs (propertyName));
		}
	}
}