using System;
using System.Collections.Generic;

using Xamarin.Forms;
using FlightDataRecorder.FlightAdd.Model;
using FlightDataRecorder.FlightRecord.Model;
using FlightDataRecorder.FlightList.Model;
using FlightDataRecorder.FlightAdd.ViewModel;
using System.ComponentModel;

namespace FlightDataRecorder.FlightRecord.View
{
	public partial class FlightRecordPage : ContentPage
	{
		private bool isBusy = false;

		private FlightViewModel viewModel;

		public FlightRecordPage (Flight flight)
		{
			viewModel = new FlightViewModel (flight);

			this.BindingContext = viewModel;

			InitializeComponent ();

			this.Title = StringResources.recordingFlight.Replace ("%", flight.Info.Number);

			Button button = this.FindByName<Button> ("StartStopButton");

			if (flight.Info.IsPending)
			{
				button.Text = StringResources.startRecording;
			}
			else if (flight.Info.IsRecording)
			{
				button.Text = StringResources.stopRecording;
			}
			else if (flight.Info.IsStopped)
			{
				button.IsVisible = false;			
			}
		}

		public async void OnStartRecordingClicked(object sender, EventArgs e)
		{
			if (isBusy)
				return;

			isBusy = true;

			string message = null;

			if (viewModel.Flight.Info.IsPending)
			{
				message = App.DataService.Check ();
			}

			if (message != null)
			{
				await DisplayAlert (StringResources.error, message, StringResources.ok);

				IsBusy = false;
			}
			else
			{
				MessagingCenter.Send (this, "StartStopRecording", viewModel.Flight);

				await Navigation.PopAsync ();
			}
		}
	}
}