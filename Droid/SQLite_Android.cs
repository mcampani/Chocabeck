using System;
using Xamarin.Forms;
using FlightDataRecorder.Droid;
using System.IO;
using FlightDataRecorder.Database;

[assembly: Dependency (typeof (SQLite_Android))]

namespace FlightDataRecorder.Droid
{
	public class SQLite_Android : ISQLite
	{
		public SQLite_Android ()
		{
		}

//		https://github.com/xamarin/xamarin-forms-samples/blob/master/Todo/PCL/Todo.Android/SQLite_Android.cs

		#region ISQLite implementation

		public SQLite.Net.SQLiteConnection GetConnection ()
		{
			var sqliteFilename = "FlightDataSQLite.db3";

			// Documents folder
			string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal); 

			var path = Path.Combine(documentsPath, sqliteFilename);

			var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
			var connection = new SQLite.Net.SQLiteConnection(platform, path);

			// Return the database connection 
			return connection;
		}

		#endregion
	}
}