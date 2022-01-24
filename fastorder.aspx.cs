using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class fastorder : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string itemID = Request["itemid"].ToString();
            string itmetitle = Request["itemtitle"].ToString();
            string startdate = Request["startdate"];
            string defaultpage = Request["defaultpage"].ToString();
            string loggedinpage = Request["loggedinpage"].ToString();
            bool islgd = false;
            if (HttpContext.Current.Session["loggedIn"] != null)
            {
                if ((bool)(HttpContext.Current.Session["loggedIn"]))
                {
                    islgd = true;
                }
            }

            // kill warenkorb
            bool reccuringAbo = false;
            try
            {
                // prüfe auf wiederkehrendes zahlungs merkmal
                string rec = Venezia.GetObjDetails(itemID, false);
                if (rec.Split((char)6)[1].Split((char)8)[38].ToUpper() == "RECURR")
                {
                    reccuringAbo = true;
                }
            }
            catch
            {

            }

            // reset warenkorb
            DS_Warenkorb myWarenkorbDS = new DS_Warenkorb();
            Session.Add("shoppingCard", myWarenkorbDS);
            Session.Add("geschenkAdressNr", null);
            //add flag to session
            Session.Add("Recurring", reccuringAbo);

            // add as new
            DS_Warenkorb.DT_WarenkorbRow myNewWarenkorbrow = myWarenkorbDS.DT_Warenkorb.NewDT_WarenkorbRow();
            myNewWarenkorbrow.ObjektID = itemID;
            myNewWarenkorbrow.ItemKey = itemID;
            myNewWarenkorbrow.Titel = itmetitle;
            myNewWarenkorbrow.Menge = 1;
            myNewWarenkorbrow.FototecaBildNr = string.Empty;
            myNewWarenkorbrow.WaehrungCollection = Venezia.GetObjCurrencyWaehrungString(itemID, false);
            myNewWarenkorbrow.BetragCollection = Venezia.GetObjCurrencyBetragString(itemID, false);
            myNewWarenkorbrow.SetStartdatumNull();
            if (!string.IsNullOrEmpty(startdate))
            {
                try
                {
                    myNewWarenkorbrow.Startdatum = DateTime.Parse(startdate);
                }
                catch
                {
                    // invalid date
                }
            }

            // artmerkmals
            myNewWarenkorbrow.ArtMerkmals = string.Empty;

            // get price
            myNewWarenkorbrow.Preis = double.Parse(myNewWarenkorbrow.BetragCollection.Split('¦')[0]);

            // add
            myWarenkorbDS.DT_Warenkorb.AddDT_WarenkorbRow(myNewWarenkorbrow);

            // save warenkorb
            Session.Add("shoppingCard", myWarenkorbDS);

            // rederict
            if (islgd)
            {
                Response.Redirect("/?" + loggedinpage + "&fastorder=true");
            }
            else
            {
                Response.Redirect("/?" + defaultpage + "&fastorder=true");
            }

        }
        catch(Exception ex)
        {
            Response.Write("Fehler... wurden alle Parameter übergeben? " + ex.ToString());
        }
    }
}