using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Configuration;

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // always return as html page content type
        Response.ContentType = "text/HTML";
        Response.Clear();

        try
        {
            // always add every url parmeter to session
            foreach (string myKey in Request.QueryString.Keys)
            {
                if (myKey != null)
                {
                    if (myKey.Contains("?"))
                    {
                        Session.Add(myKey.Split('?')[1], Request[myKey]);
                    }
                    else
                    {
                        Session.Add(myKey, Request[myKey]);
                    }
                }
            }

            // check start page
            if (Request.QueryString.Count == 0)
            {
                Response.Redirect("/?" + ConfigurationManager.AppSettings["StartPageName"].ToString());
            }


            string pageHTML = string.Empty;
            string pageName = Request.QueryString[0].ToString();


            // check for double ??
            if (Request.QueryString.ToString().Contains("%3"))
            {
                pageName = Request.QueryString.ToString().Split('%')[0];
                pageName = pageName.Replace("+", " ");
            }
            // check end
            if (pageName.Contains(".htm"))
            {
                pageName = pageName.Replace(".htm", "");
            }

            // finally read page html
            pageHTML = File.ReadAllText(Server.MapPath(@"~/" + pageName + ".htm"), Encoding.UTF8);

            // check for masterpage
            if (pageHTML.Contains("type=\"masterPage\""))
            {
                // get master page name
                string masterpageFile = CMSParser.GetMasterPageName(pageHTML);
                // load master
                pageHTML = File.ReadAllText(Server.MapPath(@"~/" + masterpageFile), Encoding.UTF8);
                pageHTML = CMSParser.ParseIncludeHTML(pageHTML, pageName);
            }

            // parse
            pageHTML = CMSParser.ParseHTML(pageHTML, string.Empty);

            // clean
            pageHTML = CMSParser.CleanDocument(pageHTML);

            Response.Write(pageHTML);
        }
        catch (Exception ex)
        {
            Response.Write("Seite nicht gefunden.<br><br>" + ex.ToString());
        }

        Response.End();
    }
}