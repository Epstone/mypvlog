<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ForgettPasswordModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Title("Passwort Vergessen") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		Passwort Vergessen</h2>
	 <p>
        Bitte geben Sie Ihre E-Mail Adresse an. An diese Adresse wird ein Link gesendet mit dem Sie Ihr Passwort neu vergeben können.
    </p>

    <% using (Html.BeginForm()) { %>
        <%: Html.ValidationSummary(true, "Es ist ein Fehler aufgetreten") %>
        <div>
            <fieldset>
                <legend>E-Mail Adresse</legend>
                
                <div class="editor-label">
                    <label for="tbx-email">Ihre E-Mail Adresse:</label>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(m => m.Email) %>
                    <%: Html.ValidationMessageFor(m => m.Email)%>
                </div>
                <p>
                    <input type="submit" value="Absenden" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
