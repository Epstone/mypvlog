<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PVLog.Utility.AppLog>" %>
<tr>
	<td>
		<%: Model.Date %>
	</td>
	<td>
		<%: Model.LogLevel%>
	</td>
	<td>
		<%: Model.CustomMessage %>
	</td>
	<td>
		<%: Model.ExceptionMessage %>
	</td>
	<td class="stacktrace">
		<%: Model.ExceptionStacktrace %>
	</td>
</tr>
