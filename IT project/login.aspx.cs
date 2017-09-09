using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IT_project
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (Page.User.Identity.IsAuthenticated)
                {
                    Response.Redirect(FormsAuthentication.DefaultUrl, false);
                }
                string constr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                string activationCode = Request.QueryString["ActivationCode"];
                if (string.IsNullOrEmpty(activationCode) || activationCode.Length != 36)
                    return;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM UserActivation WHERE ActivationCode = @ActivationCode"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@ActivationCode", activationCode);
                            cmd.Connection = con;
                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();
                            con.Close();
                            if (rowsAffected == 1)
                            {
                                lblActivate.InnerHtml = "Activation successful";
                            }
                            else
                            {
                                lblActivate.InnerHtml = "Invalid Activation code";
                            }
                        }
                    }
                }
            }
        }

        protected void LoginToSide(object sender, EventArgs e)
        {
            int userId = 0;
            string constr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Validate_User"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Value);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Value);
                    cmd.Connection = con;
                    con.Open();
                    userId = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
                switch (userId)
                {
                    case -1:
                        lblError.InnerText="Username and/or password is incorrect.";
                        lblError.Attributes.Add("style","display:block");
                        break;
                    case -2:
                        lblError.InnerText="Account has not been activated.";
                        lblError.Attributes.Add("style", "display:block");
                        break;
                    default:
                        FormsAuthentication.RedirectFromLoginPage(txtUsername.Value, chkRemember.Checked);
                        break;
                }
            }
        }

        protected void RegisterToSide(object sender, EventArgs e)
        {
            int userId = 0;
            string constr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Insert_User"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", txtRusername.Value.Trim());
                        cmd.Parameters.AddWithValue("@Password", txtRpassword.Value.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtRemail.Value.Trim());
                        cmd.Connection = con;
                        con.Open();
                        userId = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();
                    }
                }
                string message = string.Empty;
                switch (userId)
                {
                    case -1:
                        lblError2.InnerText = "Username already exists. Please choose a different username.";
                        lblError2.Attributes.Add("style", "display:block");
                        break;
                    case -2:
                        lblError2.InnerText = "Supplied email address has already been used.";
                        lblError2.Attributes.Add("style", "display:block");
                        break;
                    default:
                        lblActivate.InnerHtml = "Registration successful";
                        lblError2.Attributes.Add("style", "display:none");
                        SendActivationEmail(userId);
                        break;
                }
                /*if (userId <= -1)
                    Response.Redirect(Request.Url.AbsoluteUri.Replace(Request.Url.Query, String.Empty) + "?link=register&message="+message);*/

            }
        }
        private void SendActivationEmail(int userId)
        {
            string constr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            string activationCode = Guid.NewGuid().ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO UserActivation VALUES(@UserId, @ActivationCode)"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@ActivationCode", activationCode);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            using (MailMessage mm = new MailMessage("bankbotverify@gmail.com", txtRemail.Value))
            {       
                mm.Subject = "Account Activation";
                string body = "Hello " + txtRusername.Value.Trim() + ",";
                body += "<br /><br />Please click the following link to activate your account";
                body += "<br /><a href = '" + Request.Url.AbsoluteUri.Replace(Request.Url.Query, String.Empty) +"?ActivationCode=" + activationCode + "'>Click here to activate your account.</a>";
                body += "<br /><br />Thanks";
                mm.Body = body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("bankbotverify@gmail.com", "sje931jfj29f29fj2s");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
    }
}