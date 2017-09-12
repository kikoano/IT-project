using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IT_project
{
    public partial class CreditCalculator : System.Web.UI.Page
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
            productsList.Items.Insert(0, new ListItem("Custom Product"));
            productsList.SelectedIndex = 0;
            productName.Value = productsList.Items[productsList.SelectedIndex].Value;
        }
        protected void ListSelected(object sender, EventArgs e)
        {
            if (productsList.SelectedIndex == 0)
            {
                productName.Value = productsList.Items[productsList.SelectedIndex].Value;
                DisableFields(false);
            }
            else
            {
                FillFields();
                DisableFields(true);
            }
        }
        protected void Remove(object sender, EventArgs e)
        {
            if (productsList.SelectedIndex == 0)
            {
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
            else
            {
                IznozDo.Value = "";
                rokMeseciDo.Value = "";
            }
        }
        private void DisableFields(bool val)
        {
            productName.Disabled = val;
            kamStapka.Disabled = val;
            promoPeriod.Disabled = val;
            promoStavka.Disabled = val;
            vidOtplata.Disabled = val;
            provAplIznos.Disabled = val;
            provAplProcent.Disabled = val;
            provDrugo.Disabled = val;
            GProvUpravProcent.Disabled = val;
            GProvDrugo.Disabled = val;
            MProvUpravProcent.Disabled = val;
            MProvDrugo.Disabled = val;
        }
        protected void Calculate(object sender, EventArgs e)
        {
            if (productsList.SelectedIndex > 0)
            {
                Decimal iznoz = (Decimal)((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["IznozDo"];
                int meseci = (int)((DataSet)ViewState["CreditProducts"]).Tables["CreditProducts"].Rows[productsList.SelectedIndex - 1]["RokMeseciDo"];
                if (Decimal.Parse(IznozDo.Value) > iznoz)
                {
                    lblError3.InnerText = "Износот треба да биде до " + iznoz;
                    lblError3.Style.Add("display", "block");
                    return;
                }
                else if (int.Parse(rokMeseciDo.Value) > meseci)
                {
                    lblError3.InnerText = "Рок на месеци треба да биде до " + meseci;
                    lblError3.Style.Add("display", "block");
                    return;
                }
            }
            GenerateTable();
            save.Disabled = false;
            send.Disabled = false;
            download.Disabled = false;
        }
        private void FillFields()
        {
            productName.Value = productsList.Items[productsList.SelectedIndex].ToString();
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
        private double GetAnnuity(double kredit, int meseci, double stapka)
        {
            double r = 1 + (stapka / 12 / 100);
            return kredit * ((Math.Pow(r, meseci)) * (r - 1) / ((Math.Pow(r, meseci)) - 1));
        }

        private void GenerateTable()
        {
            generatedTable.InnerHtml = @"<div class='datagrid'>
    <table>
        <thead>
            <tr>
                <th>#</th>
                <th>Датум</th>
                <th>Остаток на главница</th>
                <th>Камата</th>
              	<th>Главница</th>
                <th>Рата</th>
                <th>Провизии</th>
                <th>Паричен тек</th>
            </tr>
        </thead>
        <tbody>";
            DateTime date = DateTime.Now;
            double m_promoPeriod = string.IsNullOrWhiteSpace(promoPeriod.Value) ? 0 : double.Parse(promoPeriod.Value);
            double m_promoStavka = string.IsNullOrWhiteSpace(promoStavka.Value) ? 0 : double.Parse(promoStavka.Value);
            double annuity = GetAnnuity(double.Parse(IznozDo.Value), int.Parse(rokMeseciDo.Value), m_promoStavka == 0 ? double.Parse(kamStapka.Value) : m_promoStavka);
            double ostatok = double.Parse(IznozDo.Value);

            double m_provAplIznos = string.IsNullOrWhiteSpace(provAplIznos.Value) ? 0 : double.Parse(provAplIznos.Value);
            double m_provDrugo = string.IsNullOrWhiteSpace(provDrugo.Value) ? 0 : double.Parse(provDrugo.Value);
            double m_provAplProcent = string.IsNullOrWhiteSpace(provAplProcent.Value) ? 0 : double.Parse(provAplProcent.Value);
            double m_GProvUpravProcent = string.IsNullOrWhiteSpace(GProvUpravProcent.Value) ? 0 : double.Parse(GProvUpravProcent.Value);
            double m_GProvDrugo = string.IsNullOrWhiteSpace(GProvDrugo.Value) ? 0 : double.Parse(GProvDrugo.Value);
            double m_MProvUpravProcent = string.IsNullOrWhiteSpace(MProvUpravProcent.Value) ? 0 : double.Parse(MProvUpravProcent.Value);
            double m_MProvDrugo = string.IsNullOrWhiteSpace(MProvDrugo.Value) ? 0 : double.Parse(MProvDrugo.Value);

            double prov = m_provAplIznos + m_provDrugo + ostatok * m_provAplProcent / 100;
            double paricenTek = -prov + double.Parse(IznozDo.Value);

            generatedTable.InnerHtml += "<tr>";
            generatedTable.InnerHtml += "<td>" + 0 + "</td>";
            generatedTable.InnerHtml += "<td>" + date.ToString() + "</td>";
            generatedTable.InnerHtml += "<td>" + ostatok.ToString("#,###.00") + "</td>";
            generatedTable.InnerHtml += "<td>" + "" + "</td>";
            generatedTable.InnerHtml += "<td>" + "" + "</td>";
            generatedTable.InnerHtml += "<td>" + "" + "</td>";
            generatedTable.InnerHtml += "<td>" + prov + "</td>";
            generatedTable.InnerHtml += "<td>" + paricenTek.ToString("#,###.00") + "</td>";
            generatedTable.InnerHtml += "</tr>";

            double vkupnoProvizii = prov;
            double vkupnoKamata = 0;
            double vkupnoParicenTek = paricenTek;
            for (int i = 1; i <= int.Parse(rokMeseciDo.Value); i++)
            {
                double rataGlav;
                double kamata = ostatok * (m_promoPeriod > 0 && i <= m_promoPeriod ? m_promoStavka : double.Parse(kamStapka.Value)) / (12 * 100);
                if (int.Parse(vidOtplata.Value) == 1)
                    rataGlav = annuity - kamata;
                else
                    rataGlav = double.Parse(IznozDo.Value) / int.Parse(rokMeseciDo.Value);
                ostatok -= rataGlav;
                prov = m_MProvDrugo + ostatok * m_MProvUpravProcent / 100;
                if (i % 12 == 0)
                {
                    prov += m_GProvDrugo + ostatok * m_GProvUpravProcent / 100;
                }
                paricenTek = (-1) * (kamata + rataGlav + prov);
                date = date.AddMonths(1);
                generatedTable.InnerHtml += "<tr>";
                generatedTable.InnerHtml += "<td>" + i + "</td>";
                generatedTable.InnerHtml += "<td>" + date.ToString() + "</td>";
                generatedTable.InnerHtml += "<td>" + ostatok.ToString("#,###.00") + "</td>";
                generatedTable.InnerHtml += "<td>" + kamata.ToString("#,###.00") + "</td>";
                generatedTable.InnerHtml += "<td>" + rataGlav.ToString("#,###.00") + "</td>";
                generatedTable.InnerHtml += "<td>" + (kamata + rataGlav).ToString("#,###.00") + "</td>";
                generatedTable.InnerHtml += "<td>" + prov.ToString("#,###.00") + "</td>";
                generatedTable.InnerHtml += "<td>" + paricenTek.ToString("#,###.00") + "</td>";
                generatedTable.InnerHtml += "</tr>";
                if (i == m_promoPeriod)
                {
                    annuity = GetAnnuity(ostatok, int.Parse(rokMeseciDo.Value) - i, double.Parse(kamStapka.Value));
                }
                vkupnoProvizii += prov;
                vkupnoKamata += kamata;
                vkupnoParicenTek += paricenTek;

            }
            generatedTable.InnerHtml += "</tbody></table></div>";
        }
    }
}