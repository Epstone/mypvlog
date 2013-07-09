<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Title("FAQ") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
	<h2 >
		FAQ</h2>
	<%--<ul class="horizontal-menu" style="float: right">
		<li class="blue"><a href="#api">API</a> </li>
		<li class="blue"><a href="">FAQ</a> </li>
	</ul>--%>
	<br class="clear" />
	<%--	<p>
		Auf dieser Seite erhalten Sie allgemeine Informationen zum MyPVLog Dienst.</p>--%>
	<h3>
		Wie kann ich meinen Selbstbaulogger an Ihre Seite anbinden?</h3>
	<p>
		Die Anbindung des Dienstes ist für Entwickler sehr einfach per HTTP GET Request zu realisieren. Mit dem Aufruf einer bestimmten Url werden die aktuellen Betriebsdaten an das Portal übertragen. Es ist empfehlenswert diese Messwerte wenigstens einmal pro Minute zu übertragen, um eine möglichste genaue und detailierte Visualisierungen zu erhalten. <br />
		Es existieren bereits mehrere Logging Adressen, die zur Datenübermittlung verwendet werden können. Diese Adressen sind in der <%=Html.ActionLink("API Sektion","Api") %> beschrieben.
	</p>
	<h3>
		Was unterscheidet MyPVLog von anderen Datenloggern?</h3>
	<p>
		Der MyPVLog Dienst an sich ist kein eigenständiger Datenlogger. Er ist darauf angewießen, dass die Betriebsdaten der Wechselrichtern ausgelesen und an das Portal übertragen werden. Mit dem Internet verbundene Wechselrichter könnten ihre Betriebsdaten eigentlich direkt an das Portal übertragen. Leider wird dies aber noch von keinem Wechselrichtertyp unterstützt.</p>
	<h3>
		Wie ist der Dienst enstanden?</h3>
	<p>
		Das Portal ist einen Weiterentwicklung der ehemaligen solar.riedme.de Anwendung. Im Gegensatz zu dieser Seite ist es nun möglich eigene Anlagen anzulegen und diese zu verwalten. Weiterhin wurden die Diagramme erweitert und überarbeitet und viele weitere Funktionen hinzugefügt.
	</p>
	<h3>
		Wie kann ich mein Solarkraftwerk mit MyPVLog verbinden?</h3>
	<p>
		Derzeit wird der Dienst ausschließlich von Selbstbaulösungen unterstützt. Sobald ihr Datenlogger oder Generator mit myPVLog kompatibel ist, werden Sie hier darüber informiert.
	</p>
	<h3>
		Wie lange werden die Daten gespeichert? (Update 28.06.2012)</h3>
	<p>
		Die Betriebsdaten der Wechselrichter/Generatoren werden minutenweise auf dem Server für 30 Tage gespeichert. Diese Beschränkung ist notwendig, um weiterhin einen kostenfreien Betrieb zu ermöglichen. Nur in diesem Zeitraum ist der Export der 15-Minuten-Solarlog Dateien möglich. Die Tageswerte (kwh/Tag pro Generator) bleiben unbegrenzt gespeichert.
	</p>
	<h3>Welche Kosten entstehen für die Nutzung des Dienstes?</h3>
	<p>
	Der Dienst kann bis zu einer Anlagenröße von 20kwp kostenfrei genutzt werden. Möchten Sie eine größere Anlage anbinden, senden Sie bitte ein E-Mail an: Patrickeps[at]gmx.de
	</p>
	
	<%Html.RenderPartial("Disqus"); %>
</asp:Content>
