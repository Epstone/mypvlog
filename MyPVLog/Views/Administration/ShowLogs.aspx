<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Administration.Master" Inherits="System.Web.Mvc.ViewPage<PVLog.Models.Administration.ShowLogsModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Title("Logging")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		ShowLogs</h2>
	<% using (Html.BeginForm())
		{ %>
	<%:Html.DropDownList("loglevel", PVLog.Utility.SeverityLevel.Info.ToSelectList(), new { id = "dd-loglevel" })%>
	<%} %>
	<table id="log-table">
		<thead>
			<tr>
				<th>
					Date
				</th>
				<th>
					Level
				</th>
				<th>
					Message
				</th>
				<th>
					Exception Message
				</th>
				<th class="stacktrace">
					Stacktrace
				</th>
			</tr>
		</thead>
		<tbody>
			<%:Html.DisplayFor(m=>m.Logs) %></tbody>
	</table>
	<script type="text/javascript">
		$("#log-table").tablesorter();

		$("#dd-loglevel").change(function () {


			$(this).parent("form").submit();

		});
	</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
	<%=Html.JavascriptImport("/Scripts/jquery.tablesorter.min.js")%>
	<style type='text/css'>
		table {
			width: 100%;
		}
		th {
			min-width: 20%;
			white-space: normal;
		}
		.stacktrace {
			max-width: 200px;
			overflow: auto;
		}
	</style>
</asp:Content>
