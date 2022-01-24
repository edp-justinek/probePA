using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

public partial class dtest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
		
		
       
        try  {
			 DS_Warenkorb myWarenkorbDS = (DS_Warenkorb)Session["shoppingCard"];
		 Response.Write(myWarenkorbDS.DT_Warenkorb.Select().Length);
            Label1.Text = (string)Session["test"].ToString();
             }
        catch(Exception ex)
        {
			Response.Write(ex.ToString());
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Session.Add("test", "True");
        Label1.Text = (string)Session["test"].ToString();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string saferpay_title = "test";
        string saferpay_orderID = "123";
        string saferpay_amount = "12345";
        string saferpay_currency = "CHF";

        string paymentSuccessPage = Server.UrlEncode(Request.Url.ToString().Split('?')[0] + "?saferpay=success");
        string paymentCancelPage = Server.UrlEncode(Request.Url.ToString().Split('?')[0]); // keine "&" erlaubt! wegen stripe... 
        string paymentPage = Venezia.MakeWebRequest(ConfigurationManager.AppSettings["PaymentPage"].ToString(), "action=savecard&config=" + ConfigurationManager.AppSettings["PaymentConfig"].ToString() + "&orderID=" + saferpay_orderID + "&redirectUrl=" + paymentSuccessPage + "&cancelUrl=" + paymentCancelPage + "&failUrl=" + paymentCancelPage + "&successUrl=" + paymentSuccessPage + "&description=" + saferpay_title + " (" + saferpay_orderID + ")&amount=" + Double.Parse(saferpay_amount).ToString("####0.00;").Replace(".", string.Empty) + "&currency=" + saferpay_currency);
        Response.Redirect(JObject.Parse(paymentPage)["PaymentPage"].ToString());

    }
}