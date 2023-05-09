using System.Data;
using UnityEngine;

public class LocationDb : SqliteHelper {
	private const string Tag = "Riz: LocationDb:\t";
        
	private const string TABLE_NAME = "Locations";
	private const string KEY_ID = "id";
	private const string KEY_TYPE = "type";
	private const string KEY_LAT = "Lat";
	private const string KEY_LNG = "Lng";
	private const string KEY_DATE = "date";
	private string[] _columns = {KEY_ID, KEY_TYPE, KEY_LAT, KEY_LNG, KEY_DATE};

	public LocationDb()
	{
		var dbcmd = getDbCommand();
		dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
		                    KEY_ID + " TEXT PRIMARY KEY, " +
		                    KEY_TYPE + " TEXT, " +
		                    KEY_LAT + " TEXT, " +
		                    KEY_LNG + " TEXT, " +
		                    KEY_DATE + " DATETIME DEFAULT CURRENT_TIMESTAMP )";
		dbcmd.ExecuteNonQuery();
	}

	public void AddData(LocationEntity location)
	{
		var dbcmd = getDbCommand();
		dbcmd.CommandText =
			"INSERT INTO " + TABLE_NAME
			               + " ( "
			               + KEY_ID + ", "
			               + KEY_TYPE + ", "
			               + KEY_LAT + ", "
			               + KEY_LNG + " ) "

			               + "VALUES ( '"
			               + location._id + "', '"
			               + location._type + "', '"
			               + location._Lat + "', '"
			               + location._Lng + "' )";
		dbcmd.ExecuteNonQuery();
	}

	public override IDataReader getDataById(int id)
	{
		return base.getDataById(id);
	}

	public override IDataReader getDataByString(string str)
	{
		Debug.Log(Tag + "Getting Location: " + str);

		var dbcmd = getDbCommand();
		dbcmd.CommandText =
			"SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + str + "'";
		return dbcmd.ExecuteReader();
	}

	public override void deleteDataByString(string id)
	{
		Debug.Log(Tag + "Deleting Location: " + id);

		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandText =
			"DELETE FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + id + "'";
		dbcmd.ExecuteNonQuery();
	}

	public override void deleteDataById(int id)
	{
		base.deleteDataById(id);
	}

	public override void deleteAllData()
	{
		Debug.Log(Tag + "Deleting Table");

		base.deleteAllData(TABLE_NAME);
	}

	public override IDataReader getAllData()
	{
		return base.getAllData(TABLE_NAME);
	}

	public IDataReader GetNearestLocation(LocationInfo loc)
	{
		Debug.Log(Tag + "Getting nearest centoid from: "
		              + loc.latitude + ", " + loc.longitude);
		IDbCommand dbcmd = getDbCommand();

		string query =
			"SELECT * FROM "
			+ TABLE_NAME
			+ " ORDER BY ABS(" + KEY_LAT + " - " + loc.latitude 
			+ ") + ABS(" + KEY_LNG + " - " + loc.longitude + ") ASC LIMIT 1";

		dbcmd.CommandText = query;
		return dbcmd.ExecuteReader();
	}

	public IDataReader getLatestTimeStamp()
	{
		IDbCommand dbcmd = getDbCommand();
		dbcmd.CommandText =
			"SELECT * FROM " + TABLE_NAME + " ORDER BY " + KEY_DATE + " DESC LIMIT 1";
		return dbcmd.ExecuteReader();
	}
}