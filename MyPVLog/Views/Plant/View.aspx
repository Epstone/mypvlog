<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PVLog.Models.PlantView.PlantHomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%:Html.Title("Übersicht " + Model.Plant.Name) %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<%:Html.DisplayFor(m=>m.HeaderModel) %>
	<div>
		<div id="contentwrapper">
			<div class="right-layout-col">
				<div class="outer-box">
					<div class="box-heading">
						<h3>
							Zusammenfassung</h3>
					</div>
					<div class="child-box">
						<%Html.RenderPartial("SummaryTable", Model.SummaryTable); %>
					</div>
				</div>
				<div class="wattage-box-outer outer-box">
					<div class="box-heading wattage-chart-top-bar">
						<h3>
							Einspeiseleistung</h3>
						<div id="toggle-buttons" class="detailed-toggle-buttons">
							<label for="rdo-cumulate">
								Kumuliert</label>
							<input type="radio" id="rdo-cumulate" name="mode" checked="checked" class="inverter-mode" value="Cumulated" />
							<label for="rdo-detailed">
								Detailiert</label>
							<input type="radio" name="mode" id="rdo-detailed" class="inverter-mode" value="InverterWise" />
						</div>
						<div class="day-choose">
							<button id="btn-previous">
								Vorheriger Tag</button>
							<input type="text" id="wattage_datepicker" class="datepicker" value="<%=Utils.GetGermanNow().ToShortDateString() %>" />
							<button id="btn-next">
								Nächster Tag</button>
						</div>
					</div>
					<div id="wattage-chart" class="wattage-chart child-box">
						<%--<img alt="loading" src="../../Content/img/ajax-loader.gif" />--%>
					</div>
				</div>
			</div>
			<br class="clear" />
		</div>
	</div>
	<div class="left-layout-col outer-box">
		<div class="box-heading">
			<h3>
				Aktueller Status</h3>
		</div>
		<% foreach (var inverter in Model.Plant.Inverters)
		 { %>
		<div id="inverter-<%=inverter.PublicInverterId %>" class="inverter-gauge-box child-box" data-ac-wattage="<%: inverter.ACPowerMax %>">
			<div class="gauges-header">
				<h3>
					(<%=inverter.PublicInverterId%>) <%:inverter.Name %></h3>
				<span class="measure-time"></span>
			</div>
			<div class="gauge wattage">
			</div>
			<div class="gauge temperature">
			</div>
			<br class="clear" />
		</div>
		<% } %>
	</div>
	<div>
		<br class="clear" />
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JSPlaceHolder" runat="server">
	<%=Html.JavascriptImport("/Scripts/jquery.flot.min.js")%>
	<% Html.RenderPartial("PlantJsInitialization", Model.Plant.PlantId); %>
	<%=Html.JavascriptImport("/Scripts/my_charting.js")%>
	<%=Html.JavascriptImport("/Scripts/jquery.flot.crosshair.js")%>
	<%=Html.JavascriptImport("/Scripts/plant_summary.js") %>
	<%Response.Write("<!--[if IE]><script src='/Scripts/excanvas.compiled.js'></script><![endif]-->"); %>
</asp:Content>
