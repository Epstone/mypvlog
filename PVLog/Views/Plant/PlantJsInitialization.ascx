<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<int>" %>
<script type="text/javascript">
		$(function () {
			$("body").data("plant-id",<%=Model %>);
		});
	</script>
