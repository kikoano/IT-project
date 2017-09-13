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
        private string CreateHtmlPageTable()
        {
            string html = @"<html> <head> <style> .datagrid table { border-collapse: collapse; text-align: right; width: 100%; }
.datagrid { font: normal 12px/150% Arial, Helvetica, sans-serif; background: #fff; overflow: hidden; border: 1px solid #006699; -webkit-border-radius: 3px; -moz-border-radius: 3px; border-radius: 3px; }
 .datagrid table td, .datagrid table th { padding: 3px 10px; }
 .datagrid table thead th { background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #006699), color-stop(1, #00557F) ); background: -moz-linear-gradient( center top, #006699 5%, #00557F 100% ); filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#006699', endColorstr='#00557F'); background-color: #006699; color: #FFFFFF; font-size: 15px; font-weight: bold; border-left: 1px solid #0070A8; }
 .datagrid table thead th:first-child { border: none; }
 .datagrid table tbody td { color: #00496B; border-left: 1px solid #E1EEF4; font-size: 12px; font-weight: normal; }
 .datagrid table tbody .alt td { background: #E1EEF4; color: #00496B; }
 .datagrid table tbody td:first-child { border-left: none; }
 .datagrid table tbody tr:last-child td { border-bottom: none; } .myBtnS { margin-bottom: 8px; }
.well {
    min-height: 20px;
    padding: 19px;
    margin-bottom: 20px;
    background-color: #f5f5f5;
    border: 1px solid #e3e3e3;
    border-radius: 4px;
    -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.05);
    box-shadow: inset 0 1px 1px rgba(0,0,0,.05);
}
 .well > table > tbody > tr > td{
 padding-left:50px;
 }
</style> </head> <body>";
            html += ViewState["Parametars"];
            html += "<div class='well'>" + ViewState["Vkupno"] + "</div>";
            html += ViewState["GeneratedTable"];
            return html;
        }
        protected void Download(object sender, EventArgs e)
        {
            Response.ContentType = "text/html";
            Response.AddHeader("content-disposition", "attachment; filename=" + ViewState["ProductName"] + ".html");
            Response.Write(CreateHtmlPageTable());
            Response.End();
        }
        protected void Send(object sender, EventArgs e)
        {

            using (MailMessage mm = new MailMessage("bankbotverify@gmail.com", emailInput.Value))
            {
                mm.Subject = "Амортизицијален план";
                string body = CreateHtmlPageTable();
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
        protected void SaveToData(object sender, EventArgs e)
        {
            FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            string userData = ticket.UserData;
            string[] data = userData.Split(',');

            string query= "INSERT INTO Calculations(UserId, Name, Data, DateCreated, Parametars, Vkupno) VALUES(@UserId, @Name, @Data, @DateCreated, @Parametars, @Vkupno); ";
            string constr = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            SqlConnection conn = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", data[2]);
            cmd.Parameters.AddWithValue("@Name", ViewState["ProductName"]);
            cmd.Parameters.AddWithValue("@Data", ViewState["GeneratedTable"]);
            cmd.Parameters.AddWithValue("@DateCreated", DateTime.Now);
            cmd.Parameters.AddWithValue("@Parametars", ViewState["Parametars"]);
            cmd.Parameters.AddWithValue("@Vkupno", ViewState["Vkupno"]);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + ViewState["ProductName"] + " е зачуван!" + "');", true);
        }
            protected void Remove(object sender, EventArgs e)
        {
            if (productsList.SelectedIndex == 0)
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
            send.Style.Remove("ds");
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
            vkp.Visible = true;
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
            generatedTable.InnerHtml += "<td>" + date.ToString("dd.MM.yyyy") + "</td>";
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
                if (i % 12 == 0)
                {
                    generatedTable.InnerHtml += "<tr style='background-color:#B0DEF2'>";
                }
                else
                    generatedTable.InnerHtml += "<tr>";
                generatedTable.InnerHtml += "<td>" + i + "</td>";
                generatedTable.InnerHtml += "<td>" + date.ToString("dd.MM.yyyy") + "</td>";
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
            ViewState["GeneratedTable"] = generatedTable.InnerHtml;
            string vkupno = "<center>Вкупно провизии: <b>" + vkupnoProvizii.ToString("#,###.00") + "</b>&nbsp&nbsp&nbsp&nbsp&nbsp Вкупно камата: <b>" + vkupnoKamata.ToString("#,###.00") + "</b>&nbsp&nbsp&nbsp&nbsp&nbsp Вкупно паричен тек: <b>" + ((vkupnoParicenTek)).ToString("#,###.00") + "</b></center>";
            vkp.InnerHtml = vkupno;
            ViewState["Vkupno"] = vkupno;
            string html = "<center class='well'><table><tbody>";
            html += "<tr>";
            html += "<td>Назив</td>"; html += "<td>" + productName.Value + "</td></tr>";
            html += "<tr>";
            html += "<td>Износ до</td>"; html += "<td>" + double.Parse(IznozDo.Value).ToString("#,###.00") + "</td></tr>";
            html += "<tr>";
            html += "<td>Рок(месеци)</td>"; html += "<td>" + int.Parse(rokMeseciDo.Value) + "</td></tr>";
            html += "<tr>";
            html += "<td>Камата стапка</td>"; html += "<td>" + double.Parse(kamStapka.Value).ToString("#,###.00") + "</td></tr>";
            html += "<tr>";
            html += "<td>Промотивен период<td>" + ToStringE(m_promoPeriod) + "</td></tr>";
            html += "<tr>";
            html += "<td>Промотивна стапка</td>"; html += "<td>" + ToStringE(m_promoStavka) + "</td></tr>";
            html += "<tr>";
            html += "<td>Вид одплата</td>"; html += "<td>" + vidOtplata.Items[vidOtplata.SelectedIndex].Text+ "</td></tr>";
            html += "<tr>";
            html += "<td>Апликација(износ)</td>"; html += "<td>" + ToStringE(m_provAplIznos) + "</td></tr>";
            html += "<tr>";
            html += "<td>Апликација(%)</td>"; html += "<td>" + ToStringE(m_provAplProcent) + "</td></tr>";
            html += "<tr>";
            html += "<td>Провизија друго</td>"; html += "<td>" + ToStringE(m_provDrugo) + "</td></tr>";
            html += "<tr>";
            html += "<td>Годишна управувачка(%)</td>"; html += "<td>" + ToStringE(m_GProvUpravProcent) + "</td></tr>";
            html += "<tr>";
            html += "<td>Годишна управувачка(износ)</td>"; html += "<td>" + ToStringE(m_GProvDrugo)+ "</td></tr>";
            html += "<tr>";
            html += "<td>Месечна управувачка(%)</td>"; html += "<td>" + ToStringE(m_MProvUpravProcent) + "</td></tr>";
            html += "<tr>";
            html += "<td>Месечна управувачка(износ)</td>"; html += "<td>" + ToStringE(m_MProvDrugo) + "</td></tr>";
            html += "</tbody></table></center>";
            ViewState["Parametars"]= html;
            ViewState["ProductName"] = productName.Value;
        }
        private string ToStringE(double d)
        {
            if (d == 0)
                return "";
            else
                return d.ToString("#,###.00");
        }
    }
}