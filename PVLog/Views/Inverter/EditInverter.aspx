<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PVLog.Models.Inverter>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: Html.Title("Generator: " + Model.Name +" Einstellungen") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		Wechselrichter bearbeiten</h2>
	<%Html.RenderPartial("Message");%>
	<% using (Html.BeginForm())
		{%>
	<%: Html.ValidationSummary(true,"Ihre Änderungen konnten nicht gespeichert werden.") %>
	<fieldset>
		<legend>Eigenschaften</legend>
		<%=Html.HiddenFor(m=>m.InverterId) %>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.Name) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBoxFor(model => model.Name) %>
			<%: Html.ValidationMessageFor(model => model.Name,"*") %>
		</div>
		<%=Html.HiddenFor(m=>m.PublicInverterId) %>
		<%=Html.HiddenFor(m=>m.PublicInverterId) %>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.EuroPerKwh) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBoxFor(model => model.EuroPerKwh) %>
			€
			<%: Html.ValidationMessageFor(model => model.EuroPerKwh, "*")%>
		</div>
		<div class="editor-label">
			<%: Html.LabelFor(model => model.ACPowerMax) %>
		</div>
		<div class="editor-field">
			<%: Html.TextBoxFor(model => model.ACPowerMax) %>
			Watt<br />
			<%: Html.ValidationMessageFor(model => model.ACPowerMax)%>
		</div>
		<p>
			<input type="submit" value="Übernehmen" />
			<%=Html.ActionLink("Wechselrichter Entfernen", "DeleteRequest", new { id = Model.InverterId }, new { @class = "button red", style = "float:right;" })%>
		</p>
	</fieldset>
	<% } %>
	<div>
		<%=Html.ActionLink("Zurück zu den Anlageneinstellungen","Edit","Plant",new{id= Model.PlantId},null) %>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
