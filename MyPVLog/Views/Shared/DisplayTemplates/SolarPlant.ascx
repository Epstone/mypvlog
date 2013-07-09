<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PVLog.Models.SolarPlant>" %>
<li><a href="<%=Url.PlantUrl(Model.Name,"View",Model) %>" class="plant-list-button ui-corner-all">
	<div class="list-box-head">
		<%:Model.Name%>
	</div>
	<img src="/Content/img/pv.png" class="pv-module" alt="modul" />
</a></li>
