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
			<li><a runat="server" href="~/Page/Register/DJ/Design.aspx"><%= ProductType1 %></a></li>
            <%--<li><a runat="server" href="~/Page/Register/GJ/Design.aspx">Gold Jewellery</a></li>--%>
		</ul>
	</li>
    <li id="a4"  runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Update
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Update/DJ/DJ.aspx"><%= ProductType1 %></a></li>
            <%--<li><a id="A5" runat="server" href="~/Page/Update/GJ/GJ.aspx">Gold Jewellery</a></li>--%>
		</ul>
	</li>
	<li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Approval
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Approval/DJ/Design.aspx"><%= ProductType1 %></a></li>
            <%--<li><a runat="server" href="~/Page/Approval/GJ/Design.aspx">Gold Jewellery</a></li>--%>
		</ul>
	</li>
	<li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Report/DJ/Design.aspx"><%= ProductType1 %></a></li>
            <%--<li><a runat="server" href="~/Page/Report/GJ/Design.aspx">Gold Jewelry</a></li>--%>
		</ul>
	</li>
</ul>
