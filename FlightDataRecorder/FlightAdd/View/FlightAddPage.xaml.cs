using System;
using System.Collections.Generic;

using Xamarin.Forms;
using FlightDataRecorder.FlightAdd.ViewModel;

namespace FlightDataRecorder.FlightAdd.View
{
	public partial class FlightAddPage : ContentPage
	{
		bool isBusy = false;

		private FlightViewModel viewModel;

		public FlightAddPage ()
		{
			viewModel = new FlightViewModel ();

			this.BindingContext = viewModel;

			InitializeComponent ();
		}

		public async void OnSaveClicked(object sender, EventArgs e)
		{
			if (isBusy)
				return;

			isBusy = true;

			if (viewModel.Flight.IsValid) 
			{
				MessagingCenter.Send (this, "FlightSaved", viewModel.Flight);

				await Navigation.PopAsync ();
			} 
			else 
			{
				await DisplayAlert(StringResources.error, StringResources.error_mandatory_values, StringResources.ok);						

				isBusy = false;
			}
		}
	}
}