<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<PVLog.Models.PlantListModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Html.Title("Alle Anlagen") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Alle Anlagen</h2>
    <h3 class="big inline-block">Aktiv</h3>

    <ul class="plant-list">
        <% foreach (var plant in Model.Plants.Where(y => y.IsOnline))
            {%>
        <%=Html.DisplayFor(p=> plant) %>
        <%} %>
    </ul>
    <br class="clear" />
    <h3 class="big">Inaktiv</h3>
    <a id="show-inactive" href="#">Inaktive Anlagen anzeigen</a>
    <ul class="plant-list inactive-plants" style="display: none">
        <% foreach (var plant in Model.Plants.Where(y => !y.IsOnline))
            {%>
        <%=Html.DisplayFor(p => plant)%>
        <%} %>
    </ul>
    <br class="clear" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">
        $(function () {
            $("#show-inactive").click(function () {
                $(this).hide();
                $(".inactive-plants").toggle();
            });
        });
    </script>
</asp:Content>
