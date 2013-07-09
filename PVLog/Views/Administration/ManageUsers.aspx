<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Administration.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Html.Title("Benutzerverwaltung") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	<%--<%= MyHtml.TableCss()%>--%>
	<%= MyHtml.StyleCss()%>
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		Benutzerverwaltung</h2>
	<%=MyHtml.UserTableArea()%>
	<div class="add-user-box">
		<h2>
			Neuen Benutzer hinzufügen</h2>
		<%=MyHtml.AddUserForm()%>
	</div>
	<div class="manage-roles-box">
		<h2>
			Rollen Verwaltung</h2>
		<%=MyHtml.ManageRolesForm()%>
	</div>
	<br class="clear" />
	<%= MyHtml.UiJavascript()%>
</asp:Content>
