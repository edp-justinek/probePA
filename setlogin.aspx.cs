using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class setlogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // link example

        // vorgehen: 1. fisVLRegister methode verschickt mail mit link. benutzer ist aber bereits angemeldet im webshop
        // 2. link = auf /setLogin mit parameter "guid" und neu, anstatt type einfach "success" und "error"
        // 3. wenn link aufgerufen, wird bei erfolg auf die die success page gewechselt, sonst auf die error page inklsuive fehlercode als parameter "fehlercode"

        // beispiel:
        // /setlogin?guid=384515892265976711834333601959209489&success=home&error=test

        string theLoginIs = Venezia.fisVLExtLoginAktivierung(Request["guid"]);
        string fehlercodeLogin = string.Empty;
        try
        {
            fehlercodeLogin = theLoginIs.Split((char)6)[0].Split((char)5)[1];
        }
        catch
        {
            fehlercodeLogin = "-1";
        }

        // new
        if (fehlercodeLogin == "00")
        {
            Response.Redirect("/?" + Request["success"].ToString());
        }
        else
        {
            Response.Redirect("/?" + Request["error"].ToString() + "&fehlercode=" + fehlercodeLogin);
        }
    }
}