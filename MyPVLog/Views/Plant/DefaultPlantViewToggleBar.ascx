<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="toggle-buttons">
	<label for="rdo-cumulate">
		Kumuliert</label>
	<input type="radio" id="rdo-cumulate" name="mode" checked="checked" />
	<label for="rdo-detailed">
		Detailiert</label>
	<input type="radio" name="mode" id="rdo-detailed" />
	<label for="rdo-money">
		Euro</label>
	<input type="radio" id="rdo-money" name="roi-mode" value="money" class="kwh-eur" />
	<label for="rdo-kwh">
		kwh</label>
	<input type="radio" name="roi-mode" id="rdo-kwh" checked="checked" value="kwh" class="kwh-eur" />
</div>
