<%@ control language="C#" autoeventwireup="true" codefile="Menu.ascx.cs" inherits="Menu" %>
<ul class="nav navbar-nav">
	<!-- Summary -->
	<li id="a0" runat="server">
		<a runat="server" href="~/Page/Summary.aspx" class="visible-sm visible-md visible-lg" title="Summary" style="margin-left: -15px;">
			<i class="icon-info icon-large"></i>
		</a>
		<a runat="server" href="~/Page/Summary.aspx" class="visible-xs">Summary</a>
	</li>

	<!-- Module Menu -->
	<li id="Li1" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Export
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a id="A2" runat="server" href="~/Page/Export21/Export.aspx">Standard</a></li>
            <li><a id="A1" runat="server" href="~/Page/Export21/ExportPromo.aspx">Promo</a></li>
            <li><a id="A4" runat="server" href="~/Page/Export21/ExportReval.aspx">Reval</a></li>
			<li><a id="A5" runat="server" href="~/Page/Export21/ExportPWP.aspx">PWP</a></li>
		</ul>
    </li>
	<li id="Li2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Download
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a id="A3" runat="server" href="~/Page/Download/Publish.aspx">JAWS ZPL</a></li>
		</ul>
    </li>
</ul>
