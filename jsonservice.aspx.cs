using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Configuration;

namespace SundownBoulevardBooking
{

	public partial class jsonservice : System.Web.UI.Page
	{
		public string ConnectionString = "";
		protected void Page_Load(object sender, EventArgs e)
		{
			ConnectionString = ConfigurationManager.ConnectionStrings["DBbookingConString"].ConnectionString;

			if (Request.QueryString["f"] == "gettables")
			{
				GetTables();
			}
			if (Request.QueryString["f"] == "booktable")
			{
				BookTable();
			}
			if (Request.QueryString["f"] == "checktimeslot")
			{
				CheckTimeslot();
			}
			
		}

		public void GetTables()
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();

			Location location = new Location();
			List<Location> locations = location.ListOfLocations(ConnectionString);

			Response.Clear();
			Response.Write(serializer.Serialize(locations));
		}
		public void CheckTimeslot()
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();

			Booking NewBooking = new Booking(ConnectionString);
			NewBooking.PartySize = Convert.ToInt32(Request.Form["PartySize"]);
			NewBooking.StartDate = Convert.ToDateTime(Request.Form["SelectedDate"]);
			NewBooking.EndDate = Convert.ToDateTime(Request.Form["SelectedDate"]).AddHours(2);

			bool isTimeSlotAvailable = NewBooking.BookingTimeslotAvailable(ConnectionString);

			Response.Clear();
			Response.Write(serializer.Serialize(isTimeSlotAvailable.ToString()));
		}
		public void BookTable()
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();

			Booking NewBooking = new Booking(ConnectionString);

			NewBooking.Name = Request.Form["Name"];
			NewBooking.Email = Request.Form["Email"];
			NewBooking.Phone = Request.Form["Phone"];
			NewBooking.PartySize = Convert.ToInt32(Request.Form["PartySize"]);
			NewBooking.StartDate = Convert.ToDateTime(Request.Form["SelectedDate"]);
			NewBooking.EndDate = Convert.ToDateTime(Request.Form["SelectedDate"]).AddHours(2);
			NewBooking.Insert();

			//List<Location> locations = location.ListOfLocations(ConfigurationManager.ConnectionStrings["DBbookingConString"].ConnectionString.ToString());

			Response.Clear();
			Response.Write(serializer.Serialize(NewBooking));
		}
	}

}