using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace SundownBoulevardBooking
{
	public class Booking
	{
		private string _ConnectionString;

		private string bok_id = null;
		private string bok_name = null;
		private string bok_email = null;
		private string bok_phone = null;
		private DateTime bok_start;
		private DateTime bok_end;
		private int bok_party_size;
		private string databaseTable = "Booking";

		public int Id
		{
			get { return Convert.ToInt32(bok_id); }
			set { bok_id = value.ToString(); }
		}

		public string Name
		{
			get { return bok_name; }
			set { bok_name = value; }
		}

		public string Email
		{
			get { return bok_email; }
			set { bok_email = value; }
		}

		public string Phone
		{
			get { return bok_phone; }
			set { bok_phone = value; }
		}

		public DateTime StartDate
		{
			get { return Convert.ToDateTime(bok_start); }
			set { bok_start = value; }
		}

		public DateTime EndDate
		{
			get { return Convert.ToDateTime(bok_end); }
			set { bok_end = value; }
		}

		public int PartySize
		{
			get { return Convert.ToInt32(bok_party_size); }
			set { bok_party_size = value; }
		}

		public Booking()
		{
		}

		public Booking(string ConnectionString)
		{
			_ConnectionString = ConnectionString;
		}

		public Booking(int Id, string ConnectionString)
		{
			_ConnectionString = ConnectionString;

			SqlConnection con = new SqlConnection(_ConnectionString);
			SqlCommand cmd = new SqlCommand("SELECT bok_id,bok_name,bok_email,bok_phone,bok_start,bok_end,bok_party_size FROM " + databaseTable + " WHERE bok_id = @bok_id", con);

			cmd.Parameters.AddWithValue("@bok_id", Id);

			SqlDataReader dr;

			con.Open();
			dr = cmd.ExecuteReader();

			if (dr.Read())
			{
				bok_id = dr["bok_id"].ToString();
				bok_name = dr["bok_name"].ToString();
				bok_email = dr["bok_email"].ToString();
				bok_phone = dr["bok_phone"].ToString();
				bok_start = Convert.ToDateTime(dr["bok_start"]);
				bok_end = Convert.ToDateTime(dr["bok_end"]);
				bok_party_size = Convert.ToInt32(dr["bok_party_size"]);
			}

			dr.Close();
			con.Close();
		}

		public List<Booking> ListOfBookings(string ConnectionString)
		{
			List<Booking> bokList = new List<Booking>();

			_ConnectionString = ConnectionString;

			SqlConnection con = new SqlConnection(_ConnectionString);
			SqlCommand cmd = new SqlCommand("SELECT bok_id,bok_name,bok_email,bok_phone,bok_start,bok_end,bok_party_size FROM " + databaseTable, con);

			//cmd.Parameters.AddWithValue("@ehm_gui_id", guideId);

			SqlDataReader dr;

			con.Open();
			dr = cmd.ExecuteReader();

			while (dr.Read())
			{
				Booking bok = new Booking();

				bok.Id = Convert.ToInt32(dr["bok_id"].ToString());
				bok.Name = dr["bok_name"].ToString();
				bok.Email = dr["bok_email"].ToString();
				bok.Phone = dr["bok_phone"].ToString();
				bok.StartDate = Convert.ToDateTime(dr["bok_start"].ToString());
				bok.EndDate = Convert.ToDateTime(dr["bok_end"].ToString());
				bok.PartySize = Convert.ToInt32(dr["bok_party_size"].ToString());
				bokList.Add(bok);
			}

			dr.Close();
			con.Close();

			return bokList;

		}

		public bool BookingTimeslotAvailable(string ConnectionString)
		{
			
			_ConnectionString = ConnectionString;
			
			SqlConnection con = new SqlConnection(_ConnectionString);
			SqlCommand cmd = new SqlCommand("SELECT SUM(bok_party_size) AS [countOfBookedSeats], (SELECT SUM(loc_seats) FROM Location) AS [MaximumSeats] FROM " + databaseTable + " WHERE @bok_start BETWEEN bok_start AND bok_end OR @bok_end BETWEEN bok_start AND bok_end ", con);

			cmd.Parameters.AddWithValue("@bok_start", bok_start);
			cmd.Parameters.AddWithValue("@bok_end", bok_end.AddMinutes(-5));

			SqlDataReader dr;

			con.Open();
			dr = cmd.ExecuteReader();

			bool foundRows = dr.HasRows;

			try
			{
				if (foundRows)
				{
					int countOfBookedSeats = 0;
					int maximumAvailableBookedSeats = 0;
				
					while (dr.Read())
					{
						if (NullCheck(dr["countOfBookedSeats"]) == System.DBNull.Value)
						{
							return true;
						}
						countOfBookedSeats = Convert.ToInt32(dr["countOfBookedSeats"]);
						maximumAvailableBookedSeats = Convert.ToInt32(dr["MaximumSeats"]);
					}

					dr.Close();
					con.Close();

					if (countOfBookedSeats >= maximumAvailableBookedSeats)
					{
						return false;
					}
					else
					{
						return true;
					}

				}
				else
				{
					dr.Close();
					con.Close();
					return true;
				}
			}
			catch (Exception e)
			{

				//Elmah.ErrorSignal.FromContext(HttpContext.Current).Raise(new ApplicationException("The given key was not present in the highlight dictionary for "));
				return false;
			}

		}

		public void Insert()
		{
			SqlConnection con = new SqlConnection(_ConnectionString);
			SqlCommand cmd = con.CreateCommand();

			string SQL = "INSERT INTO " + databaseTable + " (bok_name,bok_email,bok_phone,bok_start,bok_end,bok_party_size) VALUES (@bok_name,@bok_email,@bok_phone,@bok_start,@bok_end,@bok_party_size)";

			cmd.CommandText = SQL;


			cmd.Parameters.AddWithValue("@bok_name", NullCheck(bok_name));
			cmd.Parameters.AddWithValue("@bok_email", NullCheck(bok_email));
			cmd.Parameters.AddWithValue("@bok_phone", NullCheck(bok_phone));
			cmd.Parameters.AddWithValue("@bok_start", Convert.ToDateTime(NullCheck(bok_start.ToString("yyyy-MM-dd H:mm"))));
			cmd.Parameters.AddWithValue("@bok_end", Convert.ToDateTime(NullCheck(bok_end.ToString("yyyy-MM-dd H:mm:ss"))));
			cmd.Parameters.AddWithValue("@bok_party_size", NullCheck(bok_party_size));

			con.Open();
			cmd.ExecuteNonQuery();

			SQL = "SELECT IDENT_CURRENT('" + databaseTable + "')";
			cmd.CommandText = SQL;

			bok_id = cmd.ExecuteScalar().ToString();

			con.Close();
		}

		public void Update()
		{
			SqlConnection con = new SqlConnection(_ConnectionString);
			SqlCommand cmd = con.CreateCommand();

			string SQL = "UPDATE " + databaseTable + " SET bok_name = @bok_name, bok_email = @bok_email, bok_phone = @bok_phone, bok_start = @bok_start, bok_end = @bok_end, bok_party_size = @bok_party_size WHERE bok_id = @bok_id;";

			cmd.CommandText = SQL;

			cmd.Parameters.AddWithValue("@bok_name", NullCheck(bok_name));
			cmd.Parameters.AddWithValue("@bok_email", NullCheck(bok_email));
			cmd.Parameters.AddWithValue("@bok_phone", NullCheck(bok_phone));
			cmd.Parameters.AddWithValue("@bok_start", NullCheck(bok_start.ToString("yyyy-MM-dd H:mm:ss")));
			cmd.Parameters.AddWithValue("@bok_end", NullCheck(bok_end.ToString("yyyy-MM-dd H:mm:ss")));
			cmd.Parameters.AddWithValue("@bok_party_size", NullCheck(bok_party_size));
			cmd.Parameters.AddWithValue("@bok_id", bok_id);

			con.Open();
			cmd.ExecuteNonQuery();
			con.Close();
		}

		public void Delete()
		{
			SqlConnection con = new SqlConnection(_ConnectionString);
			SqlCommand cmd = con.CreateCommand();

			string SQL = "DELETE FROM " + databaseTable + " WHERE bok_id = @bok_id;";

			cmd.CommandText = SQL;

			cmd.Parameters.AddWithValue("@bok_id", bok_id);

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