<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
<%=Html.Title("E-Mail versendet") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>E-Mail versendet</h2>
		Mit dem Link in der E-Mail in Ihrem Postfach können Sie nun Ihr Passwort neu vergeben. Bitte überprüfen Sie auch den Spam Ordner.
</asp:Content>


