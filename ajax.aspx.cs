using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class ajax : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["action"]))
        {
            switch (Request["action"].ToString())
            {

                case "setNewsletter":
                    string nletter = Venezia.fisVLSetAdrMerkmale(((DS_User.DT_UserRow)((DS_User)HttpContext.Current.Session["user"]).DT_User.Select()[0]).AdressNr, "NEWSLETT", "EVENTS", Request["flag"].ToString());

                    break;

                case "getNewsletter": 
                    string newsletter = "false";
                    try {
                        newsletter = Venezia.fisVLGetAdrMerkmale(((DS_User.DT_UserRow)((DS_User)HttpContext.Current.Session["user"]).DT_User.Select()[0]).AdressNr, "NEWSLETT", "EVENTS");

                        if (newsletter.Split((char)8)[8] == "1")
                        {
                            newsletter = "true";
                        }
                        else
                        {
                            newsletter = "false";
                        }

                    }
                    catch {
                        newsletter = "false";
                    }

                    Response.Write(newsletter);
                    Response.End();

                    break;

                case "ChangeMenge":
                    DS_Warenkorb myWarenkorbDS = (DS_Warenkorb)Session["shoppingCard"];

                    if (Venezia.GetLagerMengeStatus("001", ((DS_Warenkorb.DT_WarenkorbRow)myWarenkorbDS.DT_Warenkorb.Select("ObjektID = '" + Request["artikel"].ToString() + "'")[0]).ItemKey, ((DS_Warenkorb.DT_WarenkorbRow)myWarenkorbDS.DT_Warenkorb.Select("ObjektID = '" + Request["artikel"].ToString() + "'")[0]).Menge.ToString(), false, ((DS_Warenkorb.DT_WarenkorbRow)myWarenkorbDS.DT_Warenkorb.Select("ObjektID = '" + Request["artikel"].ToString() + "'")[0]).AusgabeSchluessel) == "0")
                    {
                        if (short.Parse(Request["menge"].ToString()) > 0) {
                        ((DS_Warenkorb.DT_WarenkorbRow)myWarenkorbDS.DT_Warenkorb.Select("ObjektID = '" + Request["artikel"].ToString() + "'")[0]).Menge = short.Parse(Request["menge"].ToString());
                        }
                    }

            // save session and rebind
            Session.Add("shoppingCard", myWarenkorbDS);


                    break;

                case "createPDF":
                    // prepare
                    string artikel = Request["pdfname"].Replace(".pdf", "");

                    if (artikel.Split('_').Length >= 3)
                    {
                        artikel = artikel.Split('_')[0] + "_" + artikel.Split('_')[1];
                    }
                    else
                    {
                        artikel = artikel.Split('_')[0];
                    }


                    string benutzerID = ((DS_User.DT_UserRow)((DS_User)HttpContext.Current.Session["user"]).DT_User.Select()[0]).BenutzerID;
                    string address = string.Empty;
                    System.Data.DataView myCheckoutView = Shop_Lib.GetEditRechnungsadresses();
                    string adressNr = myCheckoutView.Table.Select("IstLieferadresse = true")[0]["AdressNr"].ToString();
                    address += ((DS_User.DT_RechnungsadresseRow)((DS_User)HttpContext.Current.Session["user"]).DT_Rechnungsadresse.Select("AdressNr = '" + adressNr + "'")[0]).Vorname + " " + ((DS_User.DT_RechnungsadresseRow)((DS_User)HttpContext.Current.Session["user"]).DT_Rechnungsadresse.Select("AdressNr = '" + adressNr + "'")[0]).Name + ", ";
                    address += ((DS_User.DT_RechnungsadresseRow)((DS_User)HttpContext.Current.Session["user"]).DT_Rechnungsadresse.Select("AdressNr = '" + adressNr + "'")[0]).PLZ + " " + ((DS_User.DT_RechnungsadresseRow)((DS_User)HttpContext.Current.Session["user"]).DT_Rechnungsadresse.Select("AdressNr = '" + adressNr + "'")[0]).Ort + ", ";
                    address += ((DS_User.DT_RechnungsadresseRow)((DS_User)HttpContext.Current.Session["user"]).DT_Rechnungsadresse.Select("AdressNr = '" + adressNr + "'")[0]).EMail + " " + ((DS_User.DT_RechnungsadresseRow)((DS_User)HttpContext.Current.Session["user"]).DT_Rechnungsadresse.Select("AdressNr = '" + adressNr + "'")[0]).Telefon + "";
                    // 1. get artikel pdf
                    string theFile = Venezia.fisVLGetDatei(benutzerID, artikel, true);

                    // 2. save base64string
                    string base64File = (string)Venezia.GetField(theFile, "datei", string.Empty, false, false, true)["0"];

                    // 3. set unique filename
                    string fileName = (string)Venezia.GetField(theFile, "dateiname", string.Empty, false, false, true)["0"].ToString().ToLower().Replace(".pdf", "") + "_" + benutzerID + ".pdf";
                   
                    // 4. check for add watermark
                    //if ((string)Venezia.GetField(theFile, "original", string.Empty, false, false, true)["0"].ToString().ToLower() == "yes")
                    //{
                        // add watermark, first save in input
                        File.WriteAllBytes(Server.MapPath(@"~/pdfviewer/input/" + fileName), Convert.FromBase64String(base64File));

                        base64File = App_Lib.CreatePDFWatermark(fileName, address, benutzerID);

                        // save back in backoffice
                        //string backendState = Venezia.fisVLUploadDatei(benutzerID, artikel, base64File, true);

                        // delete form input
                        File.Delete(Server.MapPath(@"~/pdfviewer/input/" + fileName));
                    //}

                    // 5. save always in temp / not used currently
                    //File.WriteAllBytes(Server.MapPath(@"~/pdfviewer/temp/" + fileName), Convert.FromBase64String(base64File));

                    break;

                case "setAboSwitch":
                    Session.Add("setAboSwitch", true);

                    break;

                case "GetCurrentAbo":

                    DS_Abos myAboDS = new DS_Abos();
                    string aboMsg = Venezia.GetAbos(((DS_User.DT_UserRow)((DS_User)HttpContext.Current.Session["user"]).DT_User.Select()[0]).AdressNr);
                    foreach (string myOrderString in aboMsg.Split((char)7))
                    {
                        if (!myOrderString.Contains("Fehlercode"))
                        {
                            try
                            {
                                if (((DS_User.DT_UserRow)((DS_User)HttpContext.Current.Session["user"]).DT_User.Select()[0]).AdressNr == (string)Venezia.GetField(myOrderString, "adressnr", string.Empty, false, false, true)["0"])
                                {
                                    DS_Abos.DT_AbosRow myNewAboRow = myAboDS.DT_Abos.NewDT_AbosRow();
                                    myNewAboRow.Objekt = (string)Venezia.GetField(myOrderString, "artikel", string.Empty, false, false, true)["0"];
                                    myNewAboRow.vorgangsnr = (string)Venezia.GetField(myOrderString, "vorgangsnr", string.Empty, false, false, true)["0"];
                                    myNewAboRow.posnr = (string)Venezia.GetField(myOrderString, "posnr", string.Empty, false, false, true)["0"];
                                    myNewAboRow.artikel = (string)Venezia.GetField(myOrderString, "artikel", string.Empty, false, false, true)["0"];
                                    myNewAboRow.Bezeichnung = (string)Venezia.GetField(myOrderString, "artikelbez", string.Empty, false, false, true)["0"];
                                    myNewAboRow.Beschreibung = (string)Venezia.GetField(myOrderString, "artikelbez2", string.Empty, false, false, true)["0"];
                                    myNewAboRow.Von = DateTime.Parse(Venezia.GetField(myOrderString, "zustellvon", string.Empty, false, false, true)["0"].ToString());
                                    myNewAboRow.Bis = DateTime.Parse(Venezia.GetField(myOrderString, "zustellbis", string.Empty, false, false, true)["0"].ToString());
                                    myNewAboRow.Fakturierung = DateTime.Parse(Venezia.GetField(myOrderString, "naechstefaktura", string.Empty, false, false, true)["0"].ToString());
                                    myNewAboRow.Logins = int.Parse((string)Venezia.GetField(myOrderString, "logins", string.Empty, false, false, true)["0"]);
                                    myNewAboRow.Gesperrt = false;
                                    if ((string)Venezia.GetField(myOrderString, "gesperrt", string.Empty, false, false, true)["0"] == "J")
                                    {
                                        myNewAboRow.Gesperrt = true;
                                    }
                                    myNewAboRow.UmleitungUnterbruch = false;
                                    if ((string)Venezia.GetField(myOrderString, "umlunt", string.Empty, false, false, true)["0"] == "J")
                                    {
                                        myNewAboRow.UmleitungUnterbruch = true;
                                    }
                                    if ((string)Venezia.GetField(myOrderString, "umlunt", string.Empty, false, false, true)["0"] == "J")
                                    {
                                        myNewAboRow.UmleitungUnterbruch = true;
                                    }
                                    try
                                    {
                                        myNewAboRow.Beendet = (string)Venezia.GetField(myOrderString, "beendet", string.Empty, false, false, true)["0"];
                                    }
                                    catch
                                    {
                                        myNewAboRow.Beendet = "false";
                                    }
                                    if (myNewAboRow.Beendet == "")
                                    {
                                        myNewAboRow.Beendet = "false";
                                    }

                                    myAboDS.DT_Abos.AddDT_AbosRow(myNewAboRow);
                                }
                            }
                            catch
                            {

                            }
                        }
                    }


                    string printAboInfos = string.Empty;
                    int abo = 2; // default is only print
                    int aboCounter = myAboDS.DT_Abos.Select("Gesperrt = 0 AND Beendet = 'false'", "Bis DESC").Length;
                    if (myAboDS.DT_Abos.Select("Gesperrt = 0 AND Beendet = 'false'", "Bis DESC").Length == 0)
                    {
                        abo = -1; // no abo
                    }
                    foreach (DS_Abos.DT_AbosRow myAboRow in myAboDS.DT_Abos.Select("Gesperrt = 0 AND Beendet = 'false'", "Bis DESC"))
                    {
                        if ((myAboRow.Bezeichnung.ToLower().Contains("digital") || myAboRow.Bezeichnung.ToLower().Contains("kombi")) && aboCounter == 1)
                        {
                            // only digital abo
                            abo = 1;
                        }
                        else if ((myAboRow.Bezeichnung.ToLower().Contains("digital") || myAboRow.Bezeichnung.ToLower().Contains("kombi")) && aboCounter >= 1)
                        {
                            abo = 3;
                        }

                        if (!(myAboRow.Bezeichnung.ToLower().Contains("digital") || myAboRow.Bezeichnung.ToLower().Contains("kombi")))
                        {
                            if (myAboRow.Bis.Year == 9999)
                            {
                                printAboInfos = "Start: " + myAboRow.Von.ToShortDateString() + "<br>Ende: Bis Widerruf";
                            }
                            else
                            {
                                printAboInfos = "Start: " + myAboRow.Von.ToShortDateString() + "<br>Ende: " + myAboRow.Bis.ToShortDateString();
                            }                           
                        }
                    }

                    if (abo == 2)
                    {
                        Response.Write(printAboInfos);
                    }
                    else
                    {
                        Response.Write(false);
                    }
                    Response.End();

                    break;

                case "GetSesionValue":
                    // try to find session
                    try
                    {
                        // return dmh session (dmh = input param for horizontal navigation)
                        string svalue = Session[Request["sValue"].ToString()].ToString();
                        if (!String.IsNullOrEmpty(svalue))
                        {
                            Response.Write(svalue);
                        }
                        else
                        {
                            // return app default
                            Response.Write(Request["defValue"].ToString());
                        }
                    }
                    catch
                    {
                        // return app default
                        Response.Write(Request["defValue"].ToString());
                    }
                    Response.End();
                    break;

                case "SetSesionValue":
                    // try to find session
                    try
                    {
                        // return dmh session (dmh = input param for horizontal navigation)
                        Session.Add(Request["sName"], Request["sValue"]);
                    }
                    catch
                    {
                    }
                    Response.End();
                    break;

                case "GetWarenkorb":
                    bool warenkorbFilled = false;
                    if (HttpContext.Current.Session["shoppingCard"] != null)
                    {
                        if (((DS_Warenkorb)HttpContext.Current.Session["shoppingCard"]).DT_Warenkorb.Select().Length >= 1)
                        {
                            warenkorbFilled = true;
                        }
                    }
                    Response.Write(warenkorbFilled.ToString());
                    Response.End();
                    break;
            }
        }
    }
}