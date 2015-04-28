using System;
using Xamarin.Forms;
using System.Globalization;

namespace FlightDataRecorder.FlightAdd.View
{
	public class StringToIntConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int convertedValue;

			if (int.TryParse (value as string, out convertedValue))
				return convertedValue;
			else
				return 0;
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.ToString ();
		}
	}
}

