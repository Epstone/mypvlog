<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% if(ViewData.ContainsKey("Message")) {%>
<div class="info">
	<%=ViewData["Message"] %></div>
<%} %>
