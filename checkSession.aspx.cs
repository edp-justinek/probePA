using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class checkSession : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DS_User myUser = new DS_User();
        bool rtValue = false;
        string rqMessage = string.Empty;
        string rtMessage;
        try
        {
            if (Request["token"].ToString() == Checksum.Md5(Request["action"].ToString() + Request["sessionid"].ToString() + ConfigurationManager.AppSettings["Token"].ToString(), Checksum.Mode.String))
            {

                DS_Session mySessionDS = Shop_Lib.LoadSession();

                switch (Request["action"].ToString())
                {
                    case "state":
                        if (mySessionDS.DT_Session.Select("ServiceSessionID = '" + Request["sessionid"] + "'").Length <= int.Parse(ConfigurationManager.AppSettings["MaxLogins"].ToString()))
                        {
                            ((DS_Session.DT_SessionRow)mySessionDS.DT_Session.Select("ServiceSessionID = '" + Request["sessionid"] + "'")[0]).TimeStamp = DateTime.Now;
                            Shop_Lib.SaveSession(mySessionDS);
                            rtValue = true;
                            Response.Write(rtValue);
                        }
                        else
                        {
                            Response.Write(rtValue);
                        }

                        break;

                    case "infos":
                        if (mySessionDS.DT_Session.Select("ServiceSessionID = '" + Request["sessionid"] + "'").Length <= int.Parse(ConfigurationManager.AppSettings["MaxLogins"].ToString()))
                        {
                            string rqMessage2 = "fisVLRegCheck" + (char)4 + "BenutzerID" + (char)5 + ((DS_Session.DT_SessionRow)mySessionDS.DT_Session.Select("ServiceSessionID = '" + Request["sessionid"] + "'")[0]).BenutzerID + (char)6 + "email" + (char)5 + string.Empty + (char)6 + "FremdsysID" + (char)5 + "" + (char)6 + "userident" + (char)5 + "" + (char)6 + "passwort" + (char)5 + string.Empty + (char)6 + "service" + (char)5 + "" + (char)6 + "zugrgrup" + (char)5 + "" + (char)6 + "firma" + (char)5 + "";
                            string mainAdressString = Venezia.SendMessage(rqMessage2);

                            if (mainAdressString.Contains("Fehlercode" + (char)5 + "999"))
                            {
                                Response.Write("error");
                            }
                            else
                            {
                                // write user id
                                Response.Write(((DS_Session.DT_SessionRow)mySessionDS.DT_Session.Select("ServiceSessionID = '" + Request["sessionid"] + "'")[0]).BenutzerID + (char)6);
                                Response.Write((String)Venezia.GetField(mainAdressString, "Vorname", string.Empty, false, false, true)["0"] + (char)6);
                                Response.Write((String)Venezia.GetField(mainAdressString, "Name", string.Empty, false, false, true)["0"] + (char)6);
                                Response.Write((String)Venezia.GetField(mainAdressString, "email", string.Empty, false, false, true)["0"] + (char)6);
                                try
                                {
                                    Response.Write((String)Venezia.GetField(mainAdressString, "lastlogdate", string.Empty, false, false, true)["0"] + (char)6);
                                    Response.Write((String)Venezia.GetField(mainAdressString, "lastlogtime", string.Empty, false, false, true)["0"] + (char)6);
                                }
                                catch
                                {

                                }

                            }
                        }
                        else
                        {
                            Response.Write(rtValue);
                        }

                        break;

                    case "kill":
                        if (mySessionDS.DT_Session.Select("ServiceSessionID = '" + Request["sessionid"] + "'").Length <= int.Parse(ConfigurationManager.AppSettings["MaxLogins"].ToString()))
                        {
                            string sessionid = ((DS_Session.DT_SessionRow)mySessionDS.DT_Session.Select("ServiceSessionID = '" + Request["sessionid"] + "'")[0]).SessionID;
                            mySessionDS.DT_Session.Select("ServiceSessionID = '" + Request["sessionid"] + "'")[0].Delete();
                            Shop_Lib.SaveSession(mySessionDS);

                            rtValue = true;
                            Response.Write(rtValue);
                        }
                        else
                        {
                            Response.Write(rtValue);
                        }
                        break;

                    case "services":
                        // 1. first get adressnr
                        //string rqMessage3 = "fisVLRegCheck" + (char)4 + "BenutzerID" + (char)5 + ((DS_Session.DT_SessionRow)mySessionDS.DT_Session.Select("ServiceSessionID = '" + Request["sessionid"] + "'")[0]).BenutzerID + (char)6 + "email" + (char)5 + string.Empty + (char)6 + "FremdsysID" + (char)5 + "" + (char)6 + "userident" + (char)5 + "" + (char)6 + "passwort" + (char)5 + string.Empty + (char)6 + "service" + (char)5 + "" + (char)6 + "zugrgrup" + (char)5 + "" + (char)6 + "firma" + (char)5 + "";      
                        //string mainAdressString2 = Venezia.SendMessage(rqMessage3);
                        //string adressNr = (String)Venezia.GetField(mainAdressString2, "adressnr", string.Empty, false, false, true)["0"];

                        // get services
                        myUser = new DS_User();
                        rqMessage = "fisVLServices" + (char)4 + "BenutzerID" + (char)5 + ((DS_Session.DT_SessionRow)mySessionDS.DT_Session.Select("ServiceSessionID = '" + Request["sessionid"] + "'")[0]).BenutzerID + (char)6 + "passwort" + (char)5 + "" + (char)6 + "firma" + (char)5 + "";
                        rtMessage = Venezia.SendMessage(rqMessage);

                        //rtMessage = Venezia.GetAbos(((DS_Session.DT_SessionRow)mySessionDS.DT_Session.Select("ServiceSessionID = '" + Request["sessionid"] + "'")[0]).BenutzerID);

                        if (rtMessage.Contains("Fehlercode" + (char)5 + "999"))
                        {
                            Response.Write("error");
                        }
                        else
                        {
                            int counter = 0;
                            foreach (string serviceString in rtMessage.Split((char)7))
                            {
                                if (counter > 0 && !string.IsNullOrEmpty((String)Venezia.GetField(serviceString, "service", string.Empty, false, false, true)["0"]))
                                {
                                    if ((String)Venezia.GetField(serviceString, "service", string.Empty, false, false, true)["0"] == "ADMIN")
                                    {
                                        //
                                    }
                                    else
                                    {
                                        DS_User.DT_ServicesRow myNewServiceRow = myUser.DT_Services.NewDT_ServicesRow();
                                        myNewServiceRow.Zugrgrup = (String)Venezia.GetField(serviceString, "zugrgrup", string.Empty, false, false, true)["0"];
                                        myNewServiceRow.Service = (String)Venezia.GetField(serviceString, "service", string.Empty, false, false, true)["0"];
                                        myNewServiceRow.Bezeichnung = (String)Venezia.GetField(serviceString, "text", string.Empty, false, false, true)["0"];
                                        myNewServiceRow.Zugriffe = 0;
                                        myNewServiceRow.Logins = int.Parse(Venezia.GetField(serviceString, "logins", string.Empty, false, false, true)["0"].ToString());
                                        myNewServiceRow.BezeichnungExt = myNewServiceRow.Bezeichnung + " (" + myNewServiceRow.Zugriffe + " benutzt von " + myNewServiceRow.Logins + ")";
                                        myNewServiceRow.Full = false;

                                        // access2service
                                        myNewServiceRow.ServiceAktiv = false;
                                        if ((String)Venezia.GetField(serviceString, "zugrok", string.Empty, false, false, true)["0"].ToString().ToLower() == "j")
                                        {
                                            myNewServiceRow.ServiceAktiv = true;
                                        }

                                        // generate key for dropdowns etc.
                                        myNewServiceRow.Key = myNewServiceRow.Zugrgrup + "|" + (String)Venezia.GetField(serviceString, "zugrok", string.Empty, false, false, true)["0"].ToString();
                                        if (myNewServiceRow.Key == "31.12.9999")
                                        {
                                            //myNewServiceRow.Key = DateTime.Today.AddDays(365).ToShortDateString();
                                        }

                                        // new key = aboende
                                        myNewServiceRow.Key = (String)Venezia.GetField(serviceString, "aboende", string.Empty, false, false, true)["0"].ToString();

                                        myNewServiceRow.Bezeichnung = (String)Venezia.GetField(serviceString, "text", string.Empty, false, false, true)["0"];
                                        myUser.DT_Services.AddDT_ServicesRow(myNewServiceRow);
                                    }
                                }
                                counter++;
                            }

                            foreach (DS_User.DT_ServicesRow myServiceRow in myUser.DT_Services.Select())
                            {
                                if (myServiceRow.ServiceAktiv)
                                {
                                    Response.Write(myServiceRow.Zugrgrup + (char)6 + myServiceRow.Service + (char)6 + myServiceRow.Key + (char)7);
                                }
                            }

                            Response.Write(" ");
                        }



                        break;


                    case "newPWD":
                        try
                        {
                            string newPWD = Venezia.NewPWD(Request["email"].ToString());
                            if (!String.IsNullOrEmpty(newPWD))
                            {
                                Response.Write(newPWD);
                            }
                            else
                            {
                                Response.Write("false");
                            }
                        }
                        catch
                        {
                            Response.Write("false");
                        }

                        break;

                    case "servicesext":
                        // 1. first get adressnr
                        //string rqMessage3 = "fisVLRegCheck" + (char)4 + "BenutzerID" + (char)5 + ((DS_Session.DT_SessionRow)mySessionDS.DT_Session.Select("ServiceSessionID = '" + Request["sessionid"] + "'")[0]).BenutzerID + (char)6 + "email" + (char)5 + string.Empty + (char)6 + "FremdsysID" + (char)5 + "" + (char)6 + "userident" + (char)5 + "" + (char)6 + "passwort" + (char)5 + string.Empty + (char)6 + "service" + (char)5 + "" + (char)6 + "zugrgrup" + (char)5 + "" + (char)6 + "firma" + (char)5 + "";      
                        //string mainAdressString2 = Venezia.SendMessage(rqMessage3);
                        //string adressNr = (String)Venezia.GetField(mainAdressString2, "adressnr", string.Empty, false, false, true)["0"];

                        // get services
                        myUser = new DS_User();
                        rqMessage = "fisVLAboAllExt" + (char)4 + "BenutzerID" + (char)5 + ((DS_Session.DT_SessionRow)mySessionDS.DT_Session.Select("ServiceSessionID = '" + Request["sessionid"] + "'")[0]).BenutzerID + (char)6 + "passwort" + (char)5 + "" + (char)6 + "firma" + (char)5 + "";
                        // test: rqMessage = "fisVLAboAllExt" + (char)4 + "BenutzerID" + (char)5 + "100005" + (char)6 + "passwort" + (char)5 + "" + (char)6 + "firma" + (char)5 + "";

                        rtMessage = Venezia.SendMessage(rqMessage);
                        if (rtMessage.Contains("Fehlercode" + (char)5 + "999"))
                        {
                            Response.Write("error");
                        }
                        else
                        {
                            int counter = 0;
                            foreach (string serviceString in rtMessage.Split((char)7))
                            {
                                if (counter > 0)
                                {
                                    foreach (string myField in serviceString.Split((char)6))
                                    {
                                        Response.Write(myField + (char)6);
                                    }
                                    Response.Write((char)7);
                                }
                                counter++;
                            }
                        }

                        Response.Write(" ");

                        break;
                }

            }
            else
            {
                Response.Write("false");
                Response.Write("<!-- Token Invalid -->");
            }
        }
        catch (Exception ex)
        {
            Response.Write("false");
            //Response.Write("<!-- " + ex.ToString() + " -->");
        }

        Response.End();
    }
}