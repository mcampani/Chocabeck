using System;
using Xamarin.Forms;

namespace FlightDataRecorder.FlightList.View
{
	public class IcoMoonLabel : Label
	{
		// A label that uses the IcoMoon font
		public IcoMoonLabel () : base ()
		{
			// Set the custom font for iOS and WinPhone
			// Android uses a custom render
			this.FontFamily = Device.OnPlatform ("icomoon-ultimate", null, @"\Assets\Fonts\icomoon-ultimate.ttf#icomoon-ultimate");
		}
	}
}

