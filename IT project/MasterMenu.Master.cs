using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IT_project
{
    public partial class MasterMenu : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            else if (!IsPostBack)
            {
                username.InnerText = HttpContext.Current.User.Identity.Name;
                username2.InnerText = HttpContext.Current.User.Identity.Name;
                FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                string userData = ticket.UserData;
                string[] data = userData.Split(',');
                userEmail.InnerText = data[1];
            }
        }
        protected void LogOutClick(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/login.aspx");

        }
        protected void ChangePassword(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Change_Password"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", HttpContext.Current.User.Identity.Name);
                    cmd.Parameters.AddWithValue("@Password", password1.Value);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteScalar();
                    con.Close();
                }
            }
            FormsAuthentication.SignOut();
            Response.Redirect("~/login.aspx");
        }
    }
}