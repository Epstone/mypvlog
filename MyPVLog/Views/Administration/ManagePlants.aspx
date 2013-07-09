<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Administration.Master" Inherits="System.Web.Mvc.ViewPage<ManagePlantsModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ManagePlants
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	
	<div class="admin-box">
		<h2>Photovoltaikanlagen</h2>
	<%Html.RenderPartial("PlantInfoTable",  Model.RealPlants); %>
	</div>
	<div class="admin-box">
		<h2>
			Demo Anlage</h2>
		<% if (Model.DemoPlant != null)
	{ %>
		Demo Photovoltaikanlage ist im System vorhanden.
		<%Html.RenderPartial("PlantInfoTable", new SolarPlant[]{ Model.DemoPlant}); %>
		<% }
	else
	{ %>
		<%=Html.ActionLink("Erstelle Test Anlage","CreateTestPlant") %>
		<% } %>
	</div>
</asp:Content>
