using System;
using Xamarin.Forms;
using FlightDataRecorder.FlightList.View;
using FlightDataRecorder.Droid.Renders;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Graphics;

[assembly: ExportRenderer (typeof (IcoMoonLabel), typeof (IcoMoonLabelRender))]
namespace FlightDataRecorder.Droid.Renders
{
	public class IcoMoonLabelRender : LabelRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Label> e) 
		{
			base.OnElementChanged (e);

			var label = (TextView) Control; 

			int color = label.CurrentTextColor;

			Typeface font = Typeface.CreateFromAsset (Forms.Context.Assets, "IcoMoon-Free.ttf");

			label.Typeface = font;
		}
	}
}

