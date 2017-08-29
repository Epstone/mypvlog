<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PlantDayModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Title("Monatsübersicht")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<%:Html.DisplayFor(m=>m.HeaderModel) %>
	<div class="chart-toolbar">
		<h3>
			Monatsübersicht</h3>
		<% Html.RenderPartial("DefaultPlantViewToggleBar"); %>
		<div id="month-chooser" class="big-select-parent" style="float: right">
		</div>
		<br class="clear" />
	</div>
	<br class="clear" />
	<span id="month-year-label"></span>
	<div id="roi-bar-chart">
	</div>
	<div id="table">
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="JSPlaceHolder" runat="server">
	<%=Html.JavascriptImport("/Scripts/my_charting.js")%>
	<script type="text/javascript" src="http://www.google.com/jsapi"></script>
	<%=Html.JavascriptImport("/Scripts/jquery.monthpicker.min.js")%>
	<% Html.RenderPartial("PlantJsInitialization", Model.Plant.PlantId); %>
	<script type="text/javascript">

		  //pvChart.roiChart.render(data);
            pvChart.loadGoogleChart(["corechart","table"], function () {
				
				//setup placeholders
				pvChart.roiChart.setup("#roi-bar-chart","#table");

				// load server data into new google datatable and a cumulated view 
				var dataTableContent = <%=Model.GoogleData%>;
				pvChart.roiChart.initializeGoogleData(dataTableContent, "kwh");

				//render column chart and inverted table with the cumulated data for the first view
				pvChart.roiChart.renderCumulatedView()
			});

		$(function(){
			$("#month-chooser").monthYearPicker({onChange : updateView});
			$("#toggle-buttons").buttonset();

			// draw cumulated chart and table
			$("#rdo-cumulate").click(function(){
				pvChart.roiChart.renderCumulatedView();
			});

			//draw detailed, inverter wise chart and table
			$("#rdo-detailed").click(function(){
				pvChart.roiChart.renderDetailedView()
			});

			// bind user control event handlers
			$(".kwh-eur").change(updateView);
			
			function updateView(){

				$.getJSON("/WebService/MonthData",
								{
									plantId : $("body").data("plant-id"),
									month : $(".monthpick").val(),
									year : $(".yearpick").val(),
									yMode : $(".kwh-eur:checked").val(),
								},
								function(response){
							
									var suffix ="kwh";
									if($(".kwh-eur:checked").val() === "money")
										suffix = "€";

									//pvChart.roiChart.initializeGoogleData();
									pvChart.roiChart.updateView(response.tableContent, suffix);

								}
				);
			}

			$("#rd-money").click(function(){});
		});
	</script>
</asp:Content>
