using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace IT_project
{
    public partial class CreateProduct : System.Web.UI.Page
    {
        private DataSet ds = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillList();
            }
        }

        private void FillList()
        {
            string constr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            string query = "SELECT * FROM CreditProducts";

            SqlConnection conn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            conn.Open();
            adapter.Fill(ds, "CreditProducts");
            conn.Close();
            adapter.Dispose();
            ViewState["CreditProducts"] = ds;
            productsList.DataTextField = "ProductName";
            productsList.DataValueField = "ProductId";
            productsList.DataSource = ds.Tables["CreditProducts"];
            productsList.DataBind();
            productsList.Items.Insert(0, new ListItem("Create New"));
            productsList.SelectedIndex = 0;
        }
        protected void ListSelected(object sender, EventArgs e)
        {
            if (productsList.SelectedIndex == 0)
            {
                addNew.InnerText = "Додади";
                remove.InnerText = "Избриши";
            }
            else
            {
                addNew.InnerText = "Промени";
                remove.InnerText = "Избриши";
            FillFields();
            }

        }
        protected void AddProduct(object sender, EventArgs e)
        {
            string query;
            if (productsList.SelectedIndex == 0)
            {
                query = "INSERT INTO CreditProducts (ProductName, IznozDo, RokMeseciDo, KamStapka, PromoPeriod, PromoStavka, VidOtplata, ProvAplIznos, ProvAplProcent, ProvDrugo, GProvUpravProcent, GProvDrugo, MProvUpravProcent, MProvDrugo) VALUES(@ProductName, @IznozDo, @RokMeseciDo, @KamStapka, @PromoPeriod, @PromoStavka, @VidOtplata, @ProvAplIznos, @ProvAplProcent, @ProvDrugo, @GProvUpravProcent, @GProvDrugo, @MProvUpravProcent, @MProvDrugo); ";
            }
            else
            {
                query = "UPDATE CreditProducts SET ProductName=@ProductName, IznozDo=@IznozDo, RokMeseciDo=@RokMeseciDo, KamStapka=@KamStapka, PromoPeriod=@PromoPeriod, PromoStavka=@PromoStavka, VidOtplata=@VidOtplata, ProvAplIznos=@ProvAplIznos, ProvAplProcent=@ProvAplProcent, ProvDrugo=@ProvDrugo, GProvUpravProcent=@GProvUpravProcent, GProvDrugo=@GProvDrugo, MProvUpravProcent=@MProvUpravProcent, MProvDrugo=@MProvDrugo WHERE ProductId = @ProductId";
            }
            string constr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString; 

            SqlConnection conn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ProductId", productsList.Value);
            cmd.Parameters.AddWithValue("@ProductName", productName.Value);
            cmd.Parameters.AddWithValue("@IznozDo", IznozDo.Value);
            cmd.Parameters.AddWithValue("@RokMeseciDo", rokMeseciDo.Value);
            cmd.Parameters.AddWithValue("@KamStapka", kamStapka.Value);
            if (!String.IsNullOrWhiteSpace(promoPeriod.Value))
                cmd.Parameters.AddWithValue("@PromoPeriod", promoPeriod.Value);
            else
                cmd.Parameters.AddWithValue("@PromoPeriod", DBNull.Value);
            if (!String.IsNullOrWhiteSpace(promoStavka.Value))
                cmd.Parameters.AddWithValue("@PromoStavka", promoStavka.Value);
            else
                cmd.Parameters.AddWithValue("@PromoStavka", DBNull.Value);
            cmd.Parameters.AddWithValue("@VidOtplata", vidOtplata.Value);

            if (!String.IsNullOrWhiteSpace(provAplIznos.Value))
                cmd.Parameters.AddWithValue("@ProvAplIznos", provAplIznos.Value);
            else
                cmd.Parameters.AddWithValue("@ProvAplIznos", DBNull.Value);
            if (!String.IsNullOrWhiteSpace(provAplProcent.Value))
                cmd.Parameters.AddWithValue("@ProvAplProcent", provAplProcent.Value);
            else
                cmd.Parameters.AddWithValue("@ProvAplProcent", DBNull.Value);
            if (!String.IsNullOrWhiteSpace(provDrugo.Value))
                cmd.Parameters.AddWithValue("@ProvDrugo", provDrugo.Value);
            else
                cmd.Parameters.AddWithValue("@ProvDrugo", DBNull.Value);
            if (!String.IsNullOrWhiteSpace(GProvUpravProcent.Value))
                cmd.Parameters.AddWithValue("@GProvUpravProcent", GProvUpravProcent.Value);
            else
                cmd.Parameters.AddWithValue("@GProvUpravProcent", DBNull.Value);
            if (!String.IsNullOrWhiteSpace(GProvDrugo.Value))
                cmd.Parameters.AddWithValue("@GProvDrugo", GProvDrugo.Value);
            else
                cmd.Parameters.AddWithValue("@GProvDrugo", DBNull.Value);
            if (!String.IsNullOrWhiteSpace(MProvUpravProcent.Value))
                cmd.Parameters.AddWithValue("@MProvUpravProcent", MProvUpravProcent.Value);
            else
                cmd.Parameters.AddWithValue("@MProvUpravProcent", DBNull.Value);
            if (!String.IsNullOrWhiteSpace(MProvDrugo.Value))
                cmd.Parameters.AddWithValue("@MProvDrugo", MProvDrugo.Value);
            else
                cmd.Parameters.AddWithValue("@MProvDrugo", DBNull.Value);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            FillList();
            EmptyFields();
        }
        protected void Remove(object sender, EventArgs e)
        {
            if (productsList.SelectedIndex == 0)
            {
                EmptyFields();
            }
            else
            {
                string constr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
                string query = "DELETE FROM CreditProducts WHERE ProductId = @ProductId";

                SqlConnection conn = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductId", ((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["ProductId"].ToString());
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                FillList();
                EmptyFields();
            }
        }
        private void EmptyFields()
        {
            productName.Value = "";
            IznozDo.Value = "";
            rokMeseciDo.Value = "";
            kamStapka.Value = "";
            promoPeriod.Value = "";
            promoStavka.Value = "";
            vidOtplata.SelectedIndex = 0;
            provAplIznos.Value = "";
            provAplProcent.Value = "";
            provDrugo.Value = "";
            GProvUpravProcent.Value = "";
            GProvDrugo.Value = "";
            MProvUpravProcent.Value = "";
            MProvDrugo.Value = "";
        }



        private void FillFields()
        {
                productName.Value = productsList.Items[productsList.SelectedIndex].ToString();
                IznozDo.Value = ((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["IznozDo"].ToString();
                rokMeseciDo.Value = ((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["RokMeseciDo"].ToString();
                kamStapka.Value = ((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["KamStapka"].ToString();
                try
                {
                    promoPeriod.Value = ((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["PromoPeriod"].ToString();
                    promoStavka.Value = ((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["PromoStavka"].ToString();
                    vidOtplata.SelectedIndex = (int)((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["VidOtplata"];

                    provAplIznos.Value = ((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["ProvAplIznos"].ToString();
                    provAplProcent.Value = ((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["ProvAplProcent"].ToString();
                    provDrugo.Value = ((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["ProvDrugo"].ToString();

                    GProvUpravProcent.Value = ((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["GProvUpravProcent"].ToString();
                    GProvDrugo.Value = ((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["GProvDrugo"].ToString();

                    MProvUpravProcent.Value = ((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["MProvUpravProcent"].ToString();
                    MProvDrugo.Value = ((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["MProvDrugo"].ToString();
                }
                catch (Exception e)
                {

                }
        }
    }
}