<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Menu" %>
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
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Information
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li><a runat="server" href="~/Page/Catalogue/DJ.aspx">Diamond Jewellery</a></li>
            <li><a runat="server" href="~/Page/Catalogue/PG.aspx">Plain Gold</a></li>
			<li><a runat="server" href="~/Page/Catalogue/LD.aspx">Loose Diamond</a></li>
        </ul>

    </li>
    
    <li id="Li2" runat="server" class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Catalogue
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li><a id="A3" runat="server" href="~/Page/Catalogue/DJ_List.aspx">Diamond Jewellery</a></li>
            <li><a id="A4" runat="server" href="~/Page/Catalogue/PG_List.aspx">Plain Gold</a></li>
            <%--li><a id="A3" runat="server" href="~/Page/Catalogue/LD_List.aspx">Loose Diamond</a></--%>
        </ul>

    </li>
    <li id="Li1" runat="server" class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Calculator
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li><a id="A2" runat="server" href="~/Page/Calculator/DJ.aspx">Diamond Jewellery</a></li>
        </ul>

    </li>
</ul>
