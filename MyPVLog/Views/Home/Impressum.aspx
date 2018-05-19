<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Title("Impressum")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h1>
		Impressum</h1>
	<p>
		Angaben gemäß § 5 TMG:
	</p>
	<p>
		Patrick Epstein<br />
		Bruchackerhof, 1<br />
		64560 Riedstadt<br />
		E-Mail: Patrick.Epstein@gmx.de
	</p>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
