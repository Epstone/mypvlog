<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PlantSummaryTableModel>" %>
<table class="kwh-summary">
	<thead>
		<tr>
			<th>
				Zeitraum
			</th>
			<th>
				Ertrag
			</th>
			<th>
				Vergütung
			</th>
		</tr>
	</thead>
	<tr>
		<td>
			Eispeisung heute (<%=DateTimeUtils.GetGermanNow().ToShortDateString() %>)
		</td>
		<td>
			<%=Model.Today.Kwh.ToKwhString() %>
		</td>
		<td>
			<%=Model.Today.Euro.ToEuroString() %>
		</td>
	</tr>
	<tr>
		<td>
			<%=  DateTimeUtils.GetGermanNow().ToString("MMMM") %>, <%=  DateTimeUtils.GetGermanNow().ToString("yyyy") %>
		</td>
		<td>
			<%=Model.ThisMonth.Kwh.ToKwhString() %>
		</td>
		<td>
			<%=Model.ThisMonth.Euro.ToEuroString() %>
		</td>
	</tr>
	<tr>
		<td>
			Dieses Jahr
		</td>
		<td>
			<%=Model.ThisYear.Kwh.ToKwhString()%>
		</td>
		<td>
			<%=Model.ThisYear.Euro.ToEuroString() %>
		</td>
	</tr>
</table>
