<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
        <%=Html.Title("Datenschutz")%>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h1>Erfassung Daten und Informationen des Nutzers</h1>
        <p>
            Diese Internetseite erfasst mit jedem Aufruf allgemeine Daten und Informationen. Diese werden in Logfiles des Servers gespeichert.
            Erfasst werden
            <ul>
                <li>Zeit und Datum</li>
                <li>Browsertype und Versionen</li>
                <li>Betriebssystem und Version</li>
                <li>Quellseite, falls der Nutzer durch einen Hyperlink auf diese Seite kam (Referrer)</li>
                <li>Zieladresse (Url)</li>
                <li>Anonymiserte IP Adresse</li>
            </ul>
            Diese Daten diene der Gefahrenabwehr im Falle von Angriffen auf diese Website.
        </p>

        <h2>Datenspeicherung zu Vertragserfüllung</h2>
        <p>Diese Webseite dient der Speicherung und Auswertung von Betriebsdaten von Photovoltaikanlagen und anderen Kleinkraftwerken. Hierzu ist es notwendig, 
            die vom Nutzer bereitgestellten Messwerte zu speichern und auszuwerten. Gespeichert werden Betriebsdaten wie Wattstunden pro Minute oder Temparatur der Wechselrichter. Sämtliche erhobenen Daten dienen ausschließlich zur Bereitstellung des Dienstes.  
        </p>

        <h2>Datenlöschung</h2>
        <p> Die Löschung der eigenen Daten kann per Email an epstone2+mypvlog@gmail.com angefragt werden. Alternativ auch per Post an die im Impressum hinterlegte Adresse.</p>

    </asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    </asp:Content>