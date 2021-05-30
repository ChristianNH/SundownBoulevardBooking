using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace SundownBoulevardBooking
{
	public class Location
	{
		private string _ConnectionString;

		private string loc_id = null;
		private string loc_name = null;
		private string loc_seats = null;
		
		private string dalocaseLocation = "Location";

		public int Id
		{
			get { return Convert.ToInt32(loc_id); }
			set { loc_id = value.ToString(); }
		}

		public string Name
		{
			get { return loc_name; }
			set { loc_name = value; }
		}
		public int Seats
		{
			get { return Convert.ToInt32(loc_seats); }
			set { loc_seats = value.ToString(); }
		}


		public Location()
		{
		}

		public Location(string ConnectionString)
		{
			_ConnectionString = ConnectionString;
		}

		public Location(int Id, string ConnectionString)
		{
			_ConnectionString = ConnectionString;

			SqlConnection con = new SqlConnection(_ConnectionString);
			SqlCommand cmd = new SqlCommand("SELECT loc_id,loc_name,loc_seats FROM " + dalocaseLocation + " WHERE loc_id = @loc_id", con);

			cmd.Parameters.AddWithValue("@loc_id", Id);

			SqlDataReader dr;

			con.Open();
			dr = cmd.ExecuteReader();

			if (dr.Read())
			{
				loc_id = dr["loc_id"].ToString();
				loc_name = dr["loc_name"].ToString();
				loc_seats = dr["loc_start"].ToString();
			}

			dr.Close();
			con.Close();
		}

		public List<Location> ListOfLocations(string ConnectionString)
		{
			List<Location> locList = new List<Location>();

			_ConnectionString = ConnectionString;

			SqlConnection con = new SqlConnection(_ConnectionString);
			SqlCommand cmd = new SqlCommand("SELECT loc_id, loc_name, loc_seats FROM " + dalocaseLocation, con);

			//cmd.Parameters.AddWithValue("@ehm_gui_id", guideId);

			SqlDataReader dr;

			con.Open();
			dr = cmd.ExecuteReader();

			while (dr.Read())
			{
				Location loc = new Location();

				loc.Id = Convert.ToInt32(dr["loc_id"].ToString());
				loc.Name = dr["loc_name"].ToString();
				loc.Seats = Convert.ToInt32(dr["loc_seats"].ToString());
				locList.Add(loc);
			}

			dr.Close();
			con.Close();

			return locList;

		}

		public void Insert()
		{
			SqlConnection con = new SqlConnection(_ConnectionString);
			SqlCommand cmd = con.CreateCommand();

			string SQL = "INSERT INTO " + dalocaseLocation + " (loc_name,loc_seats) VALUES (@loc_name,@loc_seats)";

			cmd.CommandText = SQL;


			cmd.Parameters.AddWithValue("@loc_name", NullCheck(loc_name));
			cmd.Parameters.AddWithValue("@loc_seats", NullCheck(loc_seats));

			con.Open();
			cmd.ExecuteNonQuery();

			SQL = "SELECT IDENT_CURRENT('" + dalocaseLocation + "')";
			cmd.CommandText = SQL;

			loc_id = cmd.ExecuteScalar().ToString();

			con.Close();
		}

		public void Update()
		{
			SqlConnection con = new SqlConnection(_ConnectionString);
			SqlCommand cmd = con.CreateCommand();

			string SQL = "UPDATE " + dalocaseLocation + " SET loc_name = @loc_name, loc_seats = @loc_seats WHERE loc_id = @loc_id;";

			cmd.CommandText = SQL;

			cmd.Parameters.AddWithValue("@loc_name", NullCheck(loc_name));
			cmd.Parameters.AddWithValue("@loc_seats", NullCheck(loc_seats));
			cmd.Parameters.AddWithValue("@loc_id", loc_id);

			con.Open();
			cmd.ExecuteNonQuery();
			con.Close();
		}

		public void Delete()
		{
			SqlConnection con = new SqlConnection(_ConnectionString);
			SqlCommand cmd = con.CreateCommand();

			string SQL = "DELETE FROM " + dalocaseLocation + " WHERE loc_id = @loc_id;";

			cmd.CommandText = SQL;

			cmd.Parameters.AddWithValue("@loc_id", loc_id);

			con.Open();
			cmd.ExecuteNonQuery();
			con.Close();
		}

		public DataSet Select(string SQLQuery)
		{
			SqlConnection con = new SqlConnection(_ConnectionString);
			SqlDataAdapter adpt = new SqlDataAdapter(SQLQuery, con);

			DataSet ds = new DataSet();
			adpt.Fill(ds);

			return ds;
		}

		private object NullCheck(object Value)
		{
			if (Value == null)
			{
				return System.DBNull.Value;
			}
			else
			{
				return Value;
			}
		}

	}
}