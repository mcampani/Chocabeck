using System;
using SQLite.Net;

namespace FlightDataRecorder.Database
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection();
	}
}

