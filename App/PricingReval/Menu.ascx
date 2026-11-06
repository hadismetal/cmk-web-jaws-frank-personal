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
    <li id="a1" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Register
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a id="A5" runat="server" href="~/Page/DJ/DJ_RegisterSingle_LocationType.aspx">Diamond Jewellery (Single)</a></li>
            <li><a id="A4" runat="server" href="~/Page/DJ/DJ_Register_LocationType.aspx">Diamond Jewellery (Bulk)</a></li>
            <li><a runat="server" href="~/Page/PG/PG_Register.aspx">Plain Gold</a></li>
            <li><a id="A6" runat="server" href="~/Page/GJ/GJ_Register_LocationType.aspx">Gold Jewellery (Bulk)</a></li>
		</ul>
	</li>
    <li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Revaluation
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a id="A9" runat="server" href="~/Page/DJ/DJ_Single_v2.aspx">Diamond Jewellery (Single)</a></li>
            <li><a id="A7" runat="server" href="~/Page/DJ/DJ_List_v2.aspx">Diamond Jewellery (Bulk)</a></li>
            <li><a runat="server" href="~/Page/PG/PG_List.aspx">Plain Gold</a></li>
            <li><a id="A8" runat="server" href="~/Page/GJ/GJ_List.aspx">Gold Jewellery (Bulk)</a></li>
		</ul>
	</li>

    <li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Report/DJ_List.aspx">Diamond Jewellery</a></li>
            <li><a runat="server" href="~/Page/Report/GJ_List.aspx">Gold Jewellery</a></li>
		</ul>
	</li>
</ul>
