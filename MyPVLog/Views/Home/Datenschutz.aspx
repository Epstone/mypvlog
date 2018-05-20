<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
        <%=Html.Title("Datenschutz")%>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h1>Informationen zur Erfassung von Daten des Nutzers</h1>
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

        <h2>Datenspeicherung zu Vertragserf&uuml;llung</h2>
        <p>Diese Webseite dient der Speicherung und Auswertung von Betriebsdaten von Photovoltaikanlagen und anderen Kleinkraftwerken. Hierzu ist es notwendig, die vom Nutzer bereitgestellten Messwerte zu speichern und auszuwerten. Gespeichert werden Betriebsdaten wie Wattstunden pro Minute oder Temparatur der Wechselrichter. S&auml;mtliche erhobenen Daten dienen ausschlie&szlig;lich zur Bereitstellung des Dienstes. Sie werden automatisiert statistisch verwendet, um dem Nutzer auf dieser Webseite Daten zur Verf&uuml;gung stellen zu k&ouml;nnen. <br />
        Die anonymen Daten der Server-Logfiles werden getrennt von allen durch eine betroffene Person angegebenen personenbezogenen Daten gespeichert.
        </p>

        <h2>Datenl&ouml;schung</h2>
        <p>Die L&ouml;schung der eigenen Daten kann per Email an epstone2+mypvlog@gmail.com angefragt werden. Alternativ auch per Post an die im Impressum hinterlegte Adresse.</p>

<h2>Cookies</h2>
<p>
    Die Internetseiten verwendet Cookies nur nach Login des Nutzers. Durch den Einsatz dieses Cookies können Einstellungen des Nutzers temporär auf dem Computer des Nutzers gespeichert werden. Die betroffene Person kann die Setzung von Cookies durch unsere Internetseite jederzeit mittels einer entsprechenden Einstellung des genutzten Internetbrowsers verhindern und damit der Setzung von Cookies dauerhaft widersprechen. Ferner können bereits gesetzte Cookies jederzeit über einen Internetbrowser oder andere Softwareprogramme gelöscht werden. Dies ist in allen gängigen Internetbrowsern möglich. Deaktiviert die betroffene Person die Setzung von Cookies in dem genutzten Internetbrowser, sind unter Umständen nicht alle Funktionen unserer Internetseite vollumfänglich nutzbar.
</p>

    </asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    </asp:Content>