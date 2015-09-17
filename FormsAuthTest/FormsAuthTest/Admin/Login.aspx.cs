using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FormsAuthTest.Admin
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnClick(object sender, EventArgs e)
        {
            if (Membership.ValidateUser(txtUsername.Text, txtPassword.Text))
            {
                FormsAuthentication.SetAuthCookie(txtUsername.Text, false);
                FormsAuthentication.RedirectFromLoginPage(txtUsername.Text, false);
            }
            else
            {
                //Handle invalid login details
            }
        }
    }
}