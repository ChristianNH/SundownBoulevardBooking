using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft;


	public partial class _Default : System.Web.UI.Page
	{
		public string connectionString = "";
		protected void Page_Load(object sender, EventArgs e)
		{
			connectionString = ConfigurationManager.ConnectionStrings["DBbookingConString"].ConnectionString;
			if (!IsPostBack)
			{
			
			}
		}

	}
