using System;
using Xamarin.Forms;

namespace FlightDataRecorder.Resources
{
	public class ResourceManager
	{
		public ResourceManager ()
		{
			addResources ();
		}

		public void addResources() {

			Application.Current.Resources = new ResourceDictionary ();

			Application.Current.Resources.Add ("BarTextColor", Color.White);
			Application.Current.Resources.Add ("BarBackgroundColor", Color.FromHex("#006FB9"));

			Application.Current.Resources.Add ("ItemListTextColor", Color.FromHex("#363739"));
			Application.Current.Resources.Add ("ItemListBackgroundColor", Color.White);

			Application.Current.Resources.Add ("ListBackgroundColor", Color.FromHex("#E2E7E9"));

			Application.Current.Resources.Add ("ButtonTextColor", Color.White);
			Application.Current.Resources.Add ("ButtonBackgroundColor", Color.Blue);
		}

		/**
		 * searches for global resources. 	
		*/
		public object getResource(String name) {
			object r;
			if (Application.Current.Resources.TryGetValue (name, out r)) {
				return r;
			} else {
				throw new ArgumentNullException(string.Format("Can't find a global resource for key {0}", name));
			}
		}
	}
}