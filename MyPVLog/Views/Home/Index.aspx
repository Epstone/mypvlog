<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Html.Title("Ein Datenlogger Service für Solarkraftwerke")%>
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadContent" ID="Content3" runat="server">
    <meta name="description" content="myPVlog ist ein herstellerunabhängiger Datenlogger Service für Solarkraftwerke mit Echtzeit-Übertragung aktueller Betriebsdaten. Die Einspeiseleistung und andere Betriebsdaten können Live in unserem Portal nachverfolgt werden.">
    <meta name="keywords" content="Datenlogger, Photovoltaik, Kraftwerk, Regenerative-Energien, Live-Monitoring, Eigenbau, Kaco, SMA, Wechselrichter">
    <meta name="page-topic" content="Wissenschaft">
    <meta http-equiv="content-language" content="de">
    <meta name="robots" content="index, follow">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Ein herstellerunabhängiger Datenlogger Service
    </h2>
    <p>
        myPVLog liefert die aktuellen Betriebsdaten ihrer Photovoltaikanlage in Echtzeit, überwacht und berechnet die Erträge über Jahre hinweg. Legen Sie Zettel und Stift beiseite!
    </p>
    <h2 class="main-page">
        Funktionsweise
    </h2>
    <ol class="how-it-works">
        <li>Betriebsdaten Auslesen</li>
        <li>Aufruf der Logging URL</li>
        <li>Verarbeitung durch myPVLog</li>
    </ol>
    <img src="../../Content/img/simple-architecture.gif" class="simple-architecture" alt="Aufbau" />
    <br class="clear" />
    <h2 class="main-page">
        Im Detail
    </h2>
    <p>
        Im ersten Schritt müssen die Betriebsdaten des Generators durch einen beliebigen Client Computer ausgelesen werden. Hierfür eignen sich auch herkömmliche DSL Router mit alternativer Linux Firmware. Beim Aufbau ist der Client der schwierigste Punkt, da die meisten Wechselrichter unterschiedliche Schnittstellen und Protokolle besitzen.
    </p>
    <p>
        Die soeben ausgelesenen Betriebsdaten werden nun vom Client in eine URL integriert und per GET Request an den myPVLog Server gesendet. Das Auslesen und Senden sollte einmal pro Minute oder häufiger erfolgen, um möglichst genaue Daten zu erhalten.
    </p>
    <p>
        Der Server legt die Daten nun in einer Datenbank ab und verarbeitet diese kontinuierlich. Auf der Anlagenseite werden die gerade gesendeten Betriebsdaten sofort dargestellt und stehen zu Ihrer Analyse bereit.
    </p>
    <p>
        <i>Um einen besseren Eindruck zu erhalten, sehen Sie sich gleich eine der aktiven
		<%=Html.ActionLink("Anlagen", "List", "Plant")%>
		an.</i>
    </p>
    <h2>News</h2>
    <h3>09.07.2013 Serverumzug, Logging Adresse überprüfen!!! </h3>
    <p>
        Nach dem Serverumzug gab es ein paar Probleme mit nicht nachvollziehbaren Abstürzen des Apache Web Servers in Kombination mit Mono (.NET Framework Alternative).
        Die Seite ist deshalb nun weiter in die Azure Cloud umgezogen und läuft hoffentlich von nun an wieder reibungslos für die nächsten Jahre. <br />
    </p>
    <p>
        <strong>Wichtig:</strong> Nach dem Umzug muss unbedingt im Client die Log Adresse mit der Subdomain "WWW" beginnen, also <strong>http://www.mypvlog.de/...</strong> sonst werden die Logs nicht gespeichert! Dies ist notwendig, damit das Portal den Azure Service kostenfrei nutzen kann. <br />
        Außerdem sollte der Client morgen neugestartet werden, damit die DNS Änderung greift.

    </p>

    <h2>Kommentare</h2>
    <%Html.RenderPartial("Disqus"); %>
</asp:Content>
