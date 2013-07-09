<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="header" class="gradient myheader ">
	<h1>
		<a href="/" id="heading">myPVLog</a></h1>
	<ul class="horizontal-menu top-menu">
		<li class="orange">
			<%=Html.ActionLink( "Anlagen","List","Plant",null,null) %></li><li class="blue">
				<%:Html.ActionLink("FAQ","FAQ","Home") %></li><li class="blue">
					<%:Html.ActionLink("API","API","Home") %></li><%: Html.AdminLinkListItem() %><%:Html.AddPlantLinkListItem() %><%:Html.LogoffLinkListItem() %>
	</ul>
	<% Html.RenderPartial("LogOnUserControl", new LogOnModel()); %>
</div>
