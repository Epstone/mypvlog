<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<SolarPlant>>" %>
<table>
	<thead>
		<tr>
		<th>
		ID</th>
			<th>
				Name
			</th>
			<th>
				Inverter Anzahl
			</th>
			<th>
				Private Inverter IDs
			</th>
		</tr>
	</thead>
	<tr>
		<% foreach (var plant in Model)
	 { %>
	 <td>
	<%=plant.PlantId %></td>
		<td>
			<%=Html.ActionLink(plant.Name, "View", "Plant", new { id = plant.PlantId, name = plant.Name },null)%>
		</td>
		<td>
			<%= (plant.Inverters != null) ? plant.Inverters.Count() : 0 %>
		</td>
		<td>
			<% 
		 if (plant.Inverters != null)
		 {
			 foreach (var inverter in plant.Inverters)
			 { Response.Write(inverter.InverterId + " "); }
		 }%>
		</td>
	</tr>
		<% } %>
	
</table>
