<%@ Application Language="C#" %>

<%@ Import Namespace="System.IO" %>

<script runat="server">


    void Application_Start(object sender, EventArgs e) 
    {
        
        // Code, der beim Anwendungsstart ausgeführt wird
        RouteConfig.RegisterRoutes(System.Web.Routing.RouteTable.Routes);
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code, der beim Herunterfahren der Anwendung ausgeführt wird

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code, der ausgeführt wird, wenn ein nicht behandelter Fehler auftritt

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code, der ausgeführt wird, wenn eine neue Sitzung gestartet wird

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code, der ausgeführt wird, wenn eine Sitzung beendet wird. 
        // Hinweis: Das Ereignis "Session_End" wird nur ausgelöst, wenn der Modus "sessionstate"
        // in der Datei "Web.config" auf "InProc" festgelegt ist. Wenn der Sitzungsmodus auf "StateServer" 
        // oder "SQLServer" festgelegt ist, wird das Ereignis nicht ausgelöst.

    }
       
</script>
