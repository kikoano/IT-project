using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IT_project
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { 
            if (!IsPostBack)
            {
                FillTable();
                
            }
        }
        private void FillTable()
        {
            string constr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            string query = "SELECT * FROM Calculations ORDER BY DateCreated DESC";

            SqlConnection conn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            conn.Open();
            adapter.Fill(ds, "Calculations");
            conn.Close();
            adapter.Dispose();
            viewCalc1.InnerHtml += "<table id='myTable' class='table table-hover table-striped'><thead><tr><th>#</th><th>Име</th><th>Дата на креација</th><th>Акција<th><tr><thead><tbody>";
            if (ds.Tables["Calculations"].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables["Calculations"].Rows.Count; i++)
                {
                    viewCalc1.InnerHtml += "<tr>";
                    viewCalc1.InnerHtml += "<td>" + (i + 1) + "</td>";
                    viewCalc1.InnerHtml += "<td>" + ds.Tables["Calculations"].Rows[i]["Name"].ToString() + "</td>";
                    viewCalc1.InnerHtml += "<td>" + ds.Tables["Calculations"].Rows[i]["DateCreated"].ToString() + "</td>";
                    viewCalc1.InnerHtml += @"<td class='text-center'><a class='btn btn-info btn-xs myView' value='" + ds.Tables["Calculations"].Rows[i]["CalculationId"].ToString() + "' href='#'><span class='glyphicon glyphicon-eye-open'></span>View</a>&nbsp&nbsp<a href='#' class='btn btn-danger btn-xs myDelete' value='" + ds.Tables["Calculations"].Rows[i]["CalculationId"].ToString() + "' class='btn btn-danger btn-xs'><span class='glyphicon glyphicon-remove'></span>Delete</a></td>";
                    viewCalc1.InnerHtml += "</tr>";
                }

            }
            viewCalc1.InnerHtml += "</tbody></table>";
            if (ds.Tables["Calculations"].Rows.Count <= 0)
                viewCalc1.InnerHtml += "<div id='empty' class='well'>Empty</div>";
        }
        [WebMethod]   
        public static string GetCalculationsData(string id)
        {
            string constr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            string query = "SELECT Data FROM Calculations WHERE CalculationId=@CalculationId";
            string data;
            SqlConnection conn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CalculationId", id);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            data = reader[0].ToString();
            reader.Close();
            conn.Close();
            return data;
        }
        [WebMethod]
        public static void DeleteCalculationsData(string id)
        {
            string constr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            string query = "DELETE FROM Calculations WHERE CalculationId=@CalculationId";
            SqlConnection conn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CalculationId", id);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}