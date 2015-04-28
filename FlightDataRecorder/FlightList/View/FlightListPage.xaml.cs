using System;
using System.Collections.Generic;

using Xamarin.Forms;
using FlightDataRecorder.FlightList.ViewModel;
using FlightDataRecorder.FlightAdd.View;
using FlightDataRecorder.FlightAdd.Model;
using FlightDataRecorder.FlightRecord.View;
using FlightDataRecorder.FlightList.Model;

namespace FlightDataRecorder.FlightList.View
{
	public partial class FlightListPage : ContentPage
	{
		private bool unsubscribe;

		private FlightListViewModel flightListViewModel;

		public FlightListPage ()
		{
			this.flightListViewModel = new FlightListViewModel ();
			this.BindingContext = this.flightListViewModel;

			InitializeComponent ();
		}

		protected override async void OnAppearing ()
		{
			unsubscribe = true;

			if (flightListViewModel.ItemsSource.Count == 0)
			{
				await flightListViewModel.LoadFlightsAsync ();
			}
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();

			if (unsubscribe)
			{
				flightListViewModel.unsubscribeToMessaging ();
			}
		}

		public async void OnItemSelected(object sender, ItemTappedEventArgs e)
		{
			var item = e.Item as Flight;

			if (item == null)
				return;

			unsubscribe = false;

			((ListView)sender).SelectedItem = null;

			var page = new FlightRecordPage (item);

			await Navigation.PushAsync(page);
		}

		public async void OnAddClicked(object sender, EventArgs e)
		{
			unsubscribe = false;

			var page = new FlightAddPage ();

			await Navigation.PushAsync(page);
		}

		public async void OnDelete (object sender, EventArgs e) 
		{
			var item = ((MenuItem)sender).CommandParameter as Flight;

			if (item.Info.IsRecording)
			{
				await DisplayAlert(StringResources.error, StringResources.errorDeleting, StringResources.ok);
			}
			else
			{
				MessagingCenter.Send (this, "DeleteFlight", item);
			}
		}
	}
}

