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
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Diamond
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Parcel1/01.aspx">Shape</a></li>
			<li><a runat="server" href="~/Page/Parcel1/02.aspx">Size</a></li>
			<li><a runat="server" href="~/Page/Parcel1/03.aspx">Colour</a></li>
			<li><a runat="server" href="~/Page/Parcel1/04.aspx">Clarity</a></li>
			<li><a runat="server" href="~/Page/Parcel1/05.aspx">Cutting</a></li>
			<li><a runat="server" href="~/Page/Parcel1/06.aspx">Brand</a></li>
			<li><a runat="server" href="~/Page/Parcel1/07.aspx">Category</a></li>
		</ul>
	</li>
	<li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Colour Diamond
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Parcel2/01.aspx">Shape</a></li>
			<li><a runat="server" href="~/Page/Parcel2/02.aspx">Colour</a></li>
			<li><a runat="server" href="~/Page/Parcel2/03.aspx">Size</a></li>
			<li><a runat="server" href="~/Page/Parcel2/04.aspx">Colour Grading</a></li>
			<li><a runat="server" href="~/Page/Parcel2/05.aspx">Clarity</a></li>
			<li><a runat="server" href="~/Page/Parcel2/06.aspx">Material</a></li>
			<li><a runat="server" href="~/Page/Parcel2/07.aspx">Category</a></li>
		</ul>
	</li>
	<li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Precious Stone
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Parcel3/01.aspx">Item</a></li>
			<li><a runat="server" href="~/Page/Parcel3/02.aspx">Shape</a></li>
			<li><a runat="server" href="~/Page/Parcel3/03.aspx">Size</a></li>
            <li><a runat="server" href="~/Page/Parcel3/04.aspx">Colour</a></li>
            <li><a runat="server" href="~/Page/Parcel3/06.aspx">Clarity</a></li>
			<%--<li><a runat="server" href="~/Page/Parcel3/05.aspx">Colour Grading</a></li>
            <li><a runat="server" href="~/Page/Parcel3/09.aspx">Color Comment</a></li>
            <li><a runat="server" href="~/Page/Parcel3/10.aspx">Brilliance</a></li>
            <li><a runat="server" href="~/Page/Parcel3/07.aspx">Treatment</a></li>
            <li><a runat="server" href="~/Page/Parcel3/11.aspx">Proportions</a></li>	
			<li><a runat="server" href="~/Page/Parcel3/08.aspx">Country of Origin</a></li>         
            <li><a runat="server" href="~/Page/Parcel3/12.aspx">Supplier</a></li>--%>
		</ul>
	</li>
	<li id="a4" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Semi Precious
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Parcel4/01.aspx">Item</a></li>
			<li><a runat="server" href="~/Page/Parcel4/02.aspx">Shape</a></li>
			<li><a runat="server" href="~/Page/Parcel4/03.aspx">Size</a></li>
		</ul>
	</li>
	<li id="a5" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Pearl
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Parcel5/01.aspx">Code</a></li>
			<li><a runat="server" href="~/Page/Parcel5/02.aspx">Source</a></li>
			<li><a runat="server" href="~/Page/Parcel5/03.aspx">Colour</a></li>
			<li><a runat="server" href="~/Page/Parcel5/04.aspx">Shape</a></li>
			<li><a runat="server" href="~/Page/Parcel5/05.aspx">Overtone</a></li>
			<li><a runat="server" href="~/Page/Parcel5/06.aspx">Diameter</a></li>
			<li><a runat="server" href="~/Page/Parcel5/07.aspx">Surface</a></li>
			<li><a runat="server" href="~/Page/Parcel5/08.aspx">Luster</a></li>
			<li><a runat="server" href="~/Page/Parcel5/09.aspx">Treatment</a></li>
		</ul>
	</li>
    <li id="a7" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Diamond Repair
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/ParcelRepair/01.aspx">Shape</a></li>
			<li><a runat="server" href="~/Page/ParcelRepair/02.aspx">Size</a></li>
		</ul>
	</li>
	<li id="a6" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Settings
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li class="dropdown-header">Size Control</li>
			<li><a runat="server" href="~/Page/Settings/Size01.aspx">Diamond</a></li>
			<li><a runat="server" href="~/Page/Settings/Size02.aspx">Colour Diamond</a></li>
			<li><a runat="server" href="~/Page/Settings/Size03.aspx">Precious Stone</a></li>
			<li><a runat="server" href="~/Page/Settings/Size04.aspx">Semi Precious</a></li>
            <li><a runat="server" href="~/Page/Settings/SizeRepair.aspx">Diamond Repair</a></li>
			<li class="divider"></li>
			<li class="dropdown-header">Shape Control</li>
			<li><a runat="server" href="~/Page/Settings/Shape01.aspx">Diamond</a></li>
			<li><a runat="server" href="~/Page/Settings/Shape02.aspx">Colour Diamond</a></li>
			<li><a runat="server" href="~/Page/Settings/Shape03.aspx">Precious Stone</a></li>
			<li><a runat="server" href="~/Page/Settings/Shape04.aspx">Semi Precious</a></li>
			<li><a runat="server" href="~/Page/Settings/Shape05.aspx">Pearl</a></li>
            <li><a runat="server" href="~/Page/Settings/ShapeRepair.aspx">Diamond Repair</a></li>
		</ul>
	</li>
</ul>
