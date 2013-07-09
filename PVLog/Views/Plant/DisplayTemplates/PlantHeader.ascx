<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PlantHeader>" %>
<div id="plant-header">
	<h2>
		
		<%:Model.Plant.Name%>
		<span class="kwp">// <%:Html.ToKwpString(Model.Plant.PeakWattage) %></span>
		<%if (Model.IsEditingAllowed)
		{ %>
		<%=Html.ActionLink("Einstellungen", "Edit", new { controller = "Plant", id = Model.Plant.PlantId }, new { id = "btn-settings" })%>
		<%} %>
	</h2>
	<ul class="plant-menu horizontal-menu">
		<li>
			<%=Html.PlantLink("Übersicht","View",Model.Plant)%></li>
		<li>
			<%=Html.PlantLink("Monatsertrag","Month",Model.Plant)%></li>
		<li>
			<%=Html.PlantLink("Jahresertrag","Year",Model.Plant)%></li>
		<li class="last">
			<%=Html.PlantLink("Ertrag in Dekade","Decade",Model.Plant)%></li>
	</ul>
</div>
<script type="text/javascript">
	$(function () {

		$("#btn-settings").button({ icons: { primary: "ui-icon-gear" }, text: false }).css({ "font-size": ".8em",
			"margin-left": "1em"
		});
	});

</script>
