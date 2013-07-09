<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PVLog.Models.Inverter>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ConfirmInverterDeletion
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		Generator Entfernen</h2>
	<fieldset>
		<legend>Bestätigung</legend>Soll der Generator / Wechselrichter
		<strong><%: Model.Name %> (<%:Model.PublicInverterId %>)</strong>
		wirklich entfernt werden?
		<p>
			Mit der Löschung werden auch alle Betriebs -und Ertragsdaten entfernt!
		</p>
		<p>
			<%=Html.ActionLink("Generator Entfernen", "Delete", new { id = Model.InverterId }, new { @class = "button red"})%>
			<%=Html.ActionLink("Abbrechen und Zurück", "Edit","Plant", new { id = Model.PlantId }, new { @class = "button blue",style="float:right" })%></p>
	</fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
