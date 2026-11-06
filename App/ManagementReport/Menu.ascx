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
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Register
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a id="A5" runat="server" href="~/Page/Register/BudgetStock.aspx">Budget Stock</a></li>
		</ul>
	</li>
    <li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
            <li><a runat="server" href="~/Page/Report/Sales.aspx">Sales</a></li>
            <li><a id="A7" runat="server" href="~/Page/Report/SalesRekap.aspx">Sales Rekap</a></li>
            
            <li><a runat="server" href="~/Page/Report/InvoiceDJ.aspx">Sales DJ Detail</a></li>
            <li><a runat="server" href="~/Page/Report/InvoiceDJ_Rekap.aspx">Sales DJ Group By</a></li>
            <li><a runat="server" href="~/Page/Report/InvoicePG.aspx">Sales PG Detail</a></li>
            <li><a runat="server" href="~/Page/Report/InvoicePG_Rekap.aspx">Sales PG Group By</a></li>
            <li><a id="A8" runat="server" href="~/Page/Report/ItemRekap.aspx">Stock Item Rekap</a></li>
            <li><a id="A9" runat="server" href="~/Page/Report/DP_Rekap.aspx">DP Rekap</a></li>
            <li><a id="A10" runat="server" href="~/Page/Report/DPPO.aspx">DPPO Rekap</a></li>
            <li><a runat="server" href="~/Page/Report/Payment.aspx">Payment</a></li>
            <li><a runat="server" href="~/Page/Report/Resell.aspx">Resell</a></li>
            <li><a runat="server" href="~/Page/Report/TradeIn.aspx">Trade In</a></li>
            <li><a id="A1" runat="server" href="~/Page/Report/ProductOrder.aspx">Outstanding SPK</a></li>
            <li><a id="A3" runat="server" href="~/Page/Report/Profit.aspx">Profit</a></li>
            <li><a id="A4" runat="server" href="~/Page/Report/Omset.aspx">Omset</a></li>
            <li><a id="A6" runat="server" href="~/Page/Report/BudgetStock.aspx">Budget Stock</a></li>
		</ul>
	</li>
</ul>
