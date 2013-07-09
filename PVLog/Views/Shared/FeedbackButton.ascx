<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% if (!Request.IsLocal)
	 { %>
<script type="text/javascript" id="fbzScript" src="http://feedeebuzz.de/widget/js/feedeebuzz.js?to=Patrickeps%40gmx.de&amp;p=l&amp;c=bl&amp;l=de"></script>
<%} %>