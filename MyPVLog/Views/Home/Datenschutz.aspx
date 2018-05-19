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

    </asp:Content>
    <asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    </asp:Content>