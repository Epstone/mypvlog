<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<int>" %>
<div class="export-link-box">
	<%
		string baseVars = Url.ActionAbsolute("base_vars", "Export", new { plantId = Model });
		string months = Url.ActionAbsolute("months", "Export", new { plantId = Model });
		string daysHist = Url.ActionAbsolute("days_hist", "Export", new { plantId = Model });
		string minDay = Url.ActionAbsolute("min_day", "Export", new { plantId = Model });
	%>
	<h4>
		SolarLog Export</h4>
	<ul>
		<li><a href="<%=baseVars %>">
			<%=baseVars%></a></li><li><a href="<%=months %>">
				<%=months %></a></li>
		<li><a href="<%=daysHist %>">
			<%=daysHist%></a></li>
		<li><a href="<%=minDay %>">
			<%=minDay%></a></li>
	</ul>
</div>
