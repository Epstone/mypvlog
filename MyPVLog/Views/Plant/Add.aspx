<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SolarPlant>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Title("Neue Anlage") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		Neue Photovoltaikanlage anlegen</h2>

		Auf dieser Seite können Sie eine neue Anlage einrichten. 
		<p>Hinweis: Das Anlagenkennwort wird ausschließlich für die Logging-Url verwendet.</p>
	<% using (Html.BeginForm("Add", "Plant"))
		{%>
	<%=Html.ValidationSummary(true,"Bitte überprüfen Sie Ihre Angaben.") %>
	<fieldset>
		<legend>Anlagendaten</legend>
		<div class="editor-label">
			<%=Html.LabelFor(m => m.Name) %>
		</div>
		<div class="editor-field">
			<%=Html.TextBoxFor(m => m.Name) %>
			<%=Html.ValidationMessageFor(m=>m.Name) %>
		</div>
		<div class="editor-label">
			<%= Html.LabelFor(m => m.PeakWattage) %></div>
		<div class="editor-field">
			<%= Html.TextBoxFor(m => m.PeakWattage)%>
			<%=Html.ValidationMessageFor(m => m.PeakWattage)%>
		</div>
		<div class="editor-label">
			<%= Html.LabelFor(m => m.PostalCode) %></div>
		<div class="editor-field">
			<%= Html.TextBoxFor(m => m.PostalCode)%>
			<%=Html.ValidationMessageFor(m => m.PostalCode)%>
		</div>

		<div class="editor-label" style="margin-top: 50px">
			<label>
				Bitte geben Sie den angezeigten Code unten ein.</label></div>
		<%= Html.GenerateCaptcha() %>
		<input type="submit" value="Anlage anlegen" /></fieldset>
	<%} %>
</asp:Content>
