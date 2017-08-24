<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PVLog.Models.SolarPlant>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Title("Anlage bearbeiten") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		Anlage bearbeiten</h2>
	<%Html.RenderPartial("Message"); %>
	<% using (Html.BeginForm())
		{%>
	<%: Html.ValidationSummary(true,"Ihre Änderungen konnten nicht gespeichert werden.") %>
	<fieldset>
		<% Html.RenderPartial("ExportLinkBox", Model.PlantId); %>
		<legend>Eigenschaften</legend><strong>Anlagen ID:</strong>
		<%:Model.PlantId%>
		<br />
		<strong>Anlagenkennwort:</strong>
		<%:Model.Password %>
		<%=Html.HiddenFor(model => model.PlantId) %>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.Name) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBoxFor(model => model.Name) %>
			<%: Html.ValidationMessageFor(model => model.Name) %>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.PeakWattage) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBoxFor(model => model.PeakWattage)%>
			<%: Html.ValidationMessageFor(model => model.PeakWattage)%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.PostalCode) %>
		</div>
	    <div class="editor-field">
			<%: Html.TextBoxFor(model => model.PostalCode)%>
			<%: Html.ValidationMessageFor(model => model.PostalCode)%>
		</div>
	    <div class="editor-field checkbox">
	        <%: Html.CheckBoxFor(model => model.EmailNotificationsEnabled)%>	        
	        <%: Html.LabelFor(model => model.EmailNotificationsEnabled) %>
	        <%: Html.ValidationMessageFor(model => model.EmailNotificationsEnabled)%>
	    </div>
	    <div class="editor-field checkbox">
	        <%: Html.CheckBoxFor(model => model.AutoCreateInverter)%>
			<%: Html.LabelFor(model => model.AutoCreateInverter) %>
			<%: Html.ValidationMessageFor(model => model.AutoCreateInverter)%>
		</div>
	    <p>
			<input type="submit" value="Übernehmen" /></p>
		<% } %>
	</fieldset>
	<fieldset>
		<legend>Wechselrichter / Generatoren</legend>
		<ol class="inverter-list">
			<% foreach (var inverter in Model.Inverters)
			{%>
			<li><a href="<%= Url.Action("Edit","Inverter",new{id = inverter.InverterId}) %>" class="edit-inverter">
				<div class="inverter-id">
					<%=inverter.PublicInverterId %></div>
				<%=inverter.Name %><div class="inverter-money">
					<%=inverter.EuroPerKwh * 100 %>
					Cent / kwh</div>
			</a></li>
			<% }%>
		</ol>
		<%:Html.ActionLink("Wechselrichter hinzufügen", "AddInverter", new { id = Model.PlantId }, new { @class = "button green" })%>
	</fieldset>
	<div>
		<%: Html.ActionLink("Zurück zur Anlage", "View", "Plant", new { id = Model.PlantId },null)%>
	</div>
</asp:Content>
