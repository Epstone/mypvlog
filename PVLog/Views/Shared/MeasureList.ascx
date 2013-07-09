<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<PVLog.Measure>>" %>

    <table>
        <tr>
            <th></th>
            <th>
                ID
            </th>
            <th>
                OutputWattage
            </th>
            <th>
                GeneratorWattage
            </th>
            <th>
                SystemID
            </th>
            <th>
                SystemTemperature
            </th>
            <th>
                SystemStatus
            </th>
            <th>
                InverterID
            </th>
            <th>
                GridVoltage
            </th>
            <th>
                GridAmperage
            </th>
            <th>
                GeneratorVoltage
            </th>
            <th>
                GeneratorAmperage
            </th>
            <th>
                DateTime
            </th>
          
            <th>
                Value
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { /* id=item.PrimaryKey */ }) %> |
                <%= Html.ActionLink("Details", "Details", new { /* id=item.PrimaryKey */ })%> |
                <%= Html.ActionLink("Delete", "Delete", new { /* id=item.PrimaryKey */ })%>
            </td>
            <td>
                <%= Html.Encode(item.MeasureId) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:F}", item.OutputWattage)) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:F}", item.GeneratorWattage)) %>
            </td>
            <td>
                <%= Html.Encode(item.PlantId) %>
            </td>
            <td>
                <%= Html.Encode(item.Temperature) %>
            </td>
            <td>
                <%= Html.Encode(item.SystemStatus) %>
            </td>
            <td>
                <%= Html.Encode(item.PublicInverterId) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:F}", item.GridVoltage)) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:F}", item.GridAmperage)) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:F}", item.GeneratorVoltage)) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:F}", item.GeneratorAmperage)) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:g}", item.DateTime)) %>
            </td>
            
            <td>
                <%= Html.Encode(String.Format("{0:F}", item.Value)) %>
            </td>
        </tr>
    
    <% } %>

    </table>



