using System;
using Xamarin.Forms;
using FlightDataRecorder.FlightAdd.Model;
using FlightDataRecorder.FlightList.Model;

namespace FlightDataRecorder.FlightList.View
{
	public class FlightViewCell : ViewCell
	{
		public FlightViewCell () : base ()
		{
		}

		protected override void OnBindingContextChanged ()
		{
			var item = this.BindingContext as Flight;

			item.Info.PropertyChanged += (sender, e) => 
			{
				if (e.PropertyName.Equals("StartRecordingDate") || e.PropertyName.Equals("StopRecordingDate"))
				{
					setIconLayout (sender as FlightInfo);
				}
			};

			setIconLayout (item.Info);

			base.OnBindingContextChanged ();
		}

		private void setIconLayout(FlightInfo flightInfo)
		{
			IcoMoonLabel icon = this.FindByName<IcoMoonLabel> ("recordingStateIcon");

			if (flightInfo.IsStopped)
			{				
				icon.Text = StringResources.iconProcessed;
				icon.TextColor = Color.Green;
			}
			else if (flightInfo.IsRecording)
			{
				icon.Text = StringResources.iconRecording;
				icon.TextColor = Color.Red;
			}
			else
			{
				icon.Text = "";
			}
		}
	}
}

