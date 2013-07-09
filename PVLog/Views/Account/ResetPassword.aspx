<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PVLog.Models.ResetPasswordModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Title("Passwort reset") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		Passwort Reset</h2>
	<%using (Html.BeginForm("ResetPassword", "Account"))
	 { %>
	<%: Html.ValidationSummary(true, "Es ist ein Fehler aufgetreten.") %>
	<div>
		<fieldset>
			<legend>Passwort Reset</legend>
			<div class="editor-label">
				<%=Html.LabelFor(m=>m.NewPassword) %></div>
			<div class="editor-field">
				<%=Html.PasswordFor(m => m.NewPassword)%>
			</div>
			<div class="editor-label">
				<%=Html.LabelFor(m=>m.PasswordConfirmation) %>
			</div>
			<div class="editor-field">
				<%=Html.PasswordFor(m=>m.PasswordConfirmation) %>
			</div>
			<%=Html.HiddenFor(m=>m.ResetKey) %>
			<%=Html.ValidationSummary() %>
			<input type="submit" value="Passwort Vergeben" />
		</fieldset>
	</div>
	<%} %>
</asp:Content>
