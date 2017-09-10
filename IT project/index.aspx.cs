using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IT_project
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + Application["role"] + "');", true);
        }
    }
}