<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="Menu" %>
<ul class="nav navbar-nav">
    <!-- Summary -->
    <li id="a0" runat="server">
        <a runat="server" href="~/Page/Summary.aspx" class="visible-sm visible-md visible-lg" title="Summary" style="margin-left: -15px;">
            <i class="icon-info icon-large"></i>
        </a>
        <a runat="server" href="~/Page/Summary.aspx" class="visible-xs">Summary</a>
    </li>

    <li id="a1" runat="server" class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Update
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li><a runat="server" href="~/Page/Update/Stone1B.aspx">Stone Rapaport GIA</a></li>
        </ul>
    </li>
    <li id="a3" runat="server" class="dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
        </a>
        <ul class="dropdown-menu">
            <li id="Li1" runat="server">
                <a runat="server" href="~/Page/Report/Stone1B.aspx">Stone Rapaport GIA</a></li>
                </a>
            </li>
        </ul>
    </li>
</ul>
