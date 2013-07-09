<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Title("API")%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		API</h2>
		<p>Die Betriebsdaten werden per Get Request an den Server übertragen. Die aufzurufende Url ist nach folgendem Schema aufzubauen:</p>
		<div class="api-note"> 
		http://www.mypvlog.de/Log/{AddressType}?{QueryParameter}
		</div>
		<p>Die verschiedenen Adresstypen sind im Folgenden aufgelistet.</p>
	<h3>
		Generische Adresse (Generic)</h3>
	<p>
		Die Generische Adresse kann beliebig nach den vorliegenden Daten zusammengestellt werden. Die zwingend benötigten Parameter sind der nachfolgenden Tabelle zu entnehmen.</p>
	<table style="margin-bottom: 20px">
		<%  Html.RenderPartial("ApiUrlTable/Head"); %>
		<%  Html.RenderPartial("ApiUrlTable/PlantId"); %>
		<%  Html.RenderPartial("ApiUrlTable/PlantPW"); %>
		<tr>
			<td>
				<a href="#system-mode">Betriebsmodus</a>
			</td>
			<td>
				systemstatus
			</td>
			<td>
				integer
			</td>
			<td>
				5
			</td>
			<td>
				Ja
			</td>
		</tr>
		<tr>
			<td>
				<a href="#feedinpower">Einspeiseleistung</a>
			</td>
			<td>
				feedinpower
			</td>
			<td>
				double
			</td>
			<td>
				3045.3
			</td>
			<td>
				Nein
			</td>
		</tr>
		<tr>
			<td>
				<a href="#temperature">Gehäusetemperatur</a>
			</td>
			<td>
				temperature
			</td>
			<td>
				double
			</td>
			<td>
				18
			</td>
			<td>
				Ja
			</td>
		</tr>
		<tr>
			<td>
				<a href="#generatorpower">Generatorleistung</a>
			</td>
			<td>
				generatorpower
			</td>
			<td>
				double
			</td>
			<td>
				2124.6
			</td>
			<td>
				Ja
			</td>
		</tr>
		<tr>
			<td>
				<a href="#generatorvoltage">Generatorspannung</a>
			</td>
			<td>
				generatorvoltage
			</td>
			<td>
				double
			</td>
			<td>
				230.6
			</td>
			<td>
				Ja
			</td>
		</tr>
		<tr>
			<td>
				<a href="#generatorcurrent">Generatorstrom</a>
			</td>
			<td>
				generatorcurrent
			</td>
			<td>
				double
			</td>
			<td>
				3.89
			</td>
			<td>
				Ja
			</td>
		</tr>
		<tr>
			<td>
				<a href="#gridvoltage">Netzspannung</a>
			</td>
			<td>
				gridvoltage
			</td>
			<td>
				double
			</td>
			<td>
				230.6
			</td>
			<td>
				Ja
			</td>
		</tr>
		<tr>
			<td>
				<a href="#gridcurrent">Netzstrom</a>
			</td>
			<td>
				gridcurrent
			</td>
			<td>
				double
			</td>
			<td>
				5.6
			</td>
			<td>
				Ja
			</td>
		</tr>
		<tr>
			<td>
				<a href="#timestamp">Unix Timestamp</a>
			</td>
			<td>
				timestamp
			</td>
			<td>
				long
			</td>
			<td>
				1328432923
			</td>
			<td>
				Ja
			</td>
		</tr>
		<% Html.RenderPartial("ApiUrlTable/InverterId"); %>
	</table>
	<h4>
		Beispiel:</h4>
	<div class="api-note">
		<%=Html.GetGenericSampleLink() %>
	</div>
	<h3>
		Kaco1 Adresse (Kaco1)</h3>
	<p>
		Diese Adresse ist für bestimmte Wechselrichter der Firma Kaco ausgelegt, welche über die RS-232 Schnittstelle ausgelesen werden. Über die Schnittstelle werden etwa alle 10 Sekunden die aktuellen Betriebsdaten der Wechselrichter ausgegeben. Diese Messwerte können dann in eine Url integriert und an das Portal übertragen werden.</p>
	<table>
		<%  Html.RenderPartial("ApiUrlTable/Head"); %>
		<%  Html.RenderPartial("ApiUrlTable/PlantId"); %>
		<%  Html.RenderPartial("ApiUrlTable/PlantPW"); %>
		<tr>
			<td>
				<a href="#kaco1">Kaco 1 Betriebsdaten</a>
			</td>
			<td>
				data
			</td>
			<td>
				String
			</td>
			<td>
				26.12.2009;23:53:00;5;158.0;3.20;134;229.6;1.34;1000;20
			</td>
			<td>
				Nein
			</td>
		</tr>
		<% Html.RenderPartial("ApiUrlTable/InverterId"); %>
	</table>
	<h4>
		Beispiel:</h4>
	<div class="api-note">
		<%=Html.GetKaco1SampleLink() %>
	</div>
	<h3>
	Kaco2 Adresse (Kaco2)</h3>
	<p>
		Diese Adresse ist für bestimmte Wechselrichter der Firma Kaco ausgelegt, welche über die RS-485 Schnittstelle ausgelesen werden. Im Gegensatz zur Kaco1 Adresse wird hier die Wechselrichteradresse im Datenstring mitübertragen</p>
	<table>
		<%  Html.RenderPartial("ApiUrlTable/Head"); %>
		<%  Html.RenderPartial("ApiUrlTable/PlantId"); %>
		<%  Html.RenderPartial("ApiUrlTable/PlantPW"); %>
		<tr>
			<td>
				<a href="#kaco2">Kaco 2 Betriebsdaten</a>
			</td>
			<td>
				data
			</td>
			<td>
				String
			</td>
			<td>
				"*020;4;378.2;3.96;1498;228.9;6.55;1438;29;5000;..."
			</td>
			<td>
				Nein
			</td>
		</tr>
	</table>
	<h4>
		Beispiel:</h4>
	<div class="api-note">
		<%=Html.GetKaco2SampleLink() %>
	</div>
	<h3>
		Parameter Informationen</h3>
	<dl>
		<dt><a name="plant-id">Anlagen ID</a></dt>
		<dd>
			ID der Anlage oder des Kraftwerks. Kann unter "Anlage" -> "Einstellungen" abgelesen werden.
		</dd>
		<dt><a name="plant-id">Anlagen Passwort</a></dt>
		<dd>
			Kennwort der Anlage für Logging Url. Das Kennwort kann auf der Seite "Anlage" -> "Einstellungen" abgerufen werden. Verwenden Sie nicht das Passwort des Benutzeraccounts in der Logging Url.
		</dd>
		<dt><a name="system-mode">Betriebsmodus</a></dt>
		<dd>
			Aktueller Betriebsmodus des Wechselrichters / Generators.
		</dd>
		<dt><a name="feedinpower">Einspeiseleistung</a></dt>
		<dd>
			Aktuelle AC-Leistung in Watt mit der ins Netz eingespeist wird.</dd>
			<dt><a name="temperature">Gehäusetemperatur</a></dt>
			<dd>Aktuelle Betriebstemperatur des Generators / Wechselrichters</dd>
			<dt><a name="generatorpower">Generatorleistung</a></dt>
			<dd>Aktuelle DC-Leistung des Generators in Watt.</dd>
			<dt><a name="generatorcurrent">Generatorstrom</a></dt>
			<dd>Aktueller Generatorstrom in Ampere.</dd>
			<dt><a name="generatorvoltage">Generatorspannung</a></dt>
			<dd>Aktuelle Betriebsspannung des Wechselrichters in Volt.</dd>
			<dt><a name="kaco1">Kaco 1 Betriebsdaten</a></dt>
		<dd>
			Durch Semikolon verknüfte Betriebsdatenstring von Kaco Wechselrichtern RS-232 Schnittstelle ausgelesen. Nach folgendem Schema aufgebaut:<div class="api-note">
				{Datum};{Uhrzeit};{Betriebsmodus};{Generatorspannung};{Generatorstrom};{Generatorleistung};{Netzspannung};{Netzstrom};{Einspeiseleistung};{Gerätetemperatur}
			</div>
			Da Datum und Uhrzeit des Wechselrichters meist ungenau sind, werden diese grundsätzlich verworfen und stattdessen die aktuelle Serverzeit verwendet.</dd>
		<dt><a name="kaco2">Kaco 2 Betriebsdaten</a></dt>
		<dd>
			Durch Semikolon verknüfte Betriebsdatenstring von Kaco Wechselrichtern per RS-485 Schnittstelle ausgelesen. Nach folgendem Schema aufgebaut:<div class="api-note">
				{WechselrichterId};{Betriebsmodus};{Generatorspannung};{Generatorstrom};{Generatorleistung};{Netzspannung};{Netzstrom};{Einspeiseleistung};{Gerätetemperatur};{Tagesenergie};{Wechselrichtertyp};{Prüfsumme}
			</div>
			Da Datum und Uhrzeit des Wechselrichters meist ungenau sind, werden diese grundsätzlich verworfen und stattdessen die aktuelle Serverzeit verwendet.</dd>
			<dt><a name="gridvoltage">Netzspannung</a></dt>
			<dd>Aktuelle Netzspannung in Volt.</dd>
			<dt><a name="gridcurrent">Netzstrom</a></dt>
			<dd>Aktueller Netzstrom in Ampere.</dd>
		<dt><a name="timestamp">Unix Timestamp</a></dt>
		<dd>
			Zeitpunkt an dem die Betriebsdaten erhoben wurden in Sekunden seit dem 1. Januar 1970. Sollte kein Timestamp übermittelt werden, so wird die aktuelle Serverzeit zugrunde gelegt.</dd>
		<dt><a name="inverter-id">Wechselrichter ID</a> </dt>
		<dd>
			Öffentliche Wechselrichter ID. Sollte ab 1 aufsteigend selbst vergeben werden.
		</dd>
		
	</dl>
	<br class="clear" />
	<%Html.RenderPartial("Disqus"); %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
