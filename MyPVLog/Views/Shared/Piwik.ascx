<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<!-- Piwik --> 
<script type="text/javascript">
	var pkBaseURL = (("https:" == document.location.protocol) ? "https://config.riedme.de/piwik/" : "http://config.riedme.de/piwik/");
	document.write(unescape("%3Cscript src='" + pkBaseURL + "piwik.js' type='text/javascript'%3E%3C/script%3E"));
</script><script type="text/javascript">
         	try {
         		var piwikTracker = Piwik.getTracker(pkBaseURL + "piwik.php", 2);
         		piwikTracker.trackPageView();
         		piwikTracker.enableLinkTracking();
         	} catch (err) { }
</script><noscript><p><img src="http://config.riedme.de/piwik/piwik.php?idsite=2" style="border:0" alt="" /></p></noscript>
<!-- End Piwik Tracking Code -->
