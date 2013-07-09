<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PVLog.Models.LogOnModel>" %>
<% if (!Request.IsAuthenticated)
	 {%>
<div class="logon-user-control">
	<% using (Html.BeginForm("LogOn", "Account"))
		{ %>
	<div style="float: left">
		<div class="tbx-logon-name">
			<%: Html.LabelFor(m => m.UserName)%>
			<%: Html.TextBoxFor(m => m.UserName)%>
		</div>
		<%: Html.LabelFor(m => m.Password)%>
		<%: Html.PasswordFor(m => m.Password)%>
		<%: Html.Hidden("returnUrl",Request.Url) %>
	</div>
	<input type="submit" value="Log On" id="btn-logon" class="win8box green" style="float: left; margin-left: 1em;" />
	<%=Html.ActionLink("Registrieren","Register","Account",null,new{@class ="button green"}) %>
	<% } %>
</div>
<%
	 }
%>
<%--<%: Html.ValidationMessageFor(m => m.Password)%>--%>
<%--<%: Html.CheckBoxFor(m => m.RememberMe)%>
	<%: Html.LabelFor(m => m.RememberMe)%>--%>
<%--<%: Html.ValidationMessageFor(m => m.UserName)%>--%>
<%--<%: Html.ValidationSummary(true, "Login was unsuccessful. Please correct the errors and try again.")%>--%>
<%--[ <%: Html.ActionLink("Log On", "LogOn", "Account") %> ]--%>