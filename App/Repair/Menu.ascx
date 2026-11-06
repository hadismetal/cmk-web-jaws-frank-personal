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
			<!-- <li><a id="A11" target="_blank" runat="server" href="~/Page/Create/Repair/Repair.aspx">Repair (Old)</a></li>
			<li><a id="A12" target="_blank" runat="server" href="~/Page/Create/Repair/RepairInvoice.aspx">Repair Invoice (Old)</a></li>
			<li><a id="A13" runat="server" href="~/Page/Create/Payment/PaymentRepair.aspx">Payment Repair (Old)</a></li> -->
         	<li><a id="RG1" runat="server" href="~/Page/Create/RepairOrder/">Form Reparasi</a></li>
         	<li><a id="RG8" runat="server" href="~/Page/Create/RepairOrderPusat/">Form Reparasi Pusat</a></li>
            <li><a id="RG3" runat="server" href="~/Page/Create/RepairDeliveryOrder/">Pengiriman Barang Reparasi</a></li>
            <li><a id="RG4" runat="server" href="~/Page/Create/RepairReturnOrder/">Pengembalian Ke Outlet Asal</a></li>
            <li><a id="RG5" runat="server" href="~/Page/Create/RepairDeliveryOrderFactory/">Penerimaan Reparasi dari Pabrik</a></li>
            <li><a id="RG2" runat="server" href="~/Page/Create/RepairServicePoint/">Reparasi Service Point</a></li>
            <li><a id="RG6" runat="server" href="~/Page/Create/RepairOrderPaymentList">Pembayaran Reparasi</a></li>
            <li><a id="RG7" runat="server" href="~/Page/Create/RepairOrderFinish">Pengembalian ke Customer</a></li>            
            <li><a id="RG9" runat="server" href="~/Page/Create/RepairOrderAddStone/">Reparasi Penambahan PLU Setting</a></li>            
            <li><a id="RG10" runat="server" href="~/Page/Create/RepairConfirmation/">Konfirmasi Customer</a></li>
		</ul>
	</li>
	<li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Approval
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a id="AP1" runat="server" href="~/Page/Approval/RepairDeliveryOrder/">Pengiriman Barang Reparasi</a></li>
			<li><a id="AP2" runat="server" href="~/Page/Approval/RepairReturnOrder/">Pengembalian Ke Outlet Asal</a></li>
            <li><a id="AP3" runat="server" href="~/Page/Approval/RepairReceiveOrder/">Penerimaan dari Outlet</a></li>
            <li><a id="AP4" runat="server" href="~/Page/Approval/RepairReceiveOrder_ServicePoint/">Penerimaan dari Service Point / CS Pusat</a></li>
            <!-- <li><a id="AP5" runat="server" href="~/Page/Approval/RepairOrderFree/">Free Payment Repair</a></li> -->
		</ul>
	</li>
    <li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a id="RE1" runat="server" href="~/Page/Report/RepairOrder">Report Reparasi</a></li>			
			<li><a id="RE3" runat="server" href="~/Page/Report/RepairOrderManagement">Report Management Reparasi</a></li>			
			<li><a id="RE5" runat="server" href="~/Page/Report/RepairServicePoint">Report Service Point</a></li>			
			<li><a id="RE2" runat="server" href="~/Page/Report/RepairDOKirim">Pengiriman Barang Reparasi</a></li>			
			<li><a id="RE4" runat="server" href="~/Page/Report/RepairDOTerima">Penerimaan Barang Reparasi</a></li>			
			<li><a id="RE6" runat="server" href="~/Page/Report/RepairLedger">Ledger Reparasi</a></li>			
			<!-- <li><a id="A23" runat="server" href="~/Page/Report/Repair.aspx">Repair (Old)</a></li> -->
		</ul>
	</li>
    <!-- <li id="a4" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Master
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a id="MS1" runat="server" href="~/Page/Master/ListPayments">Pembayaran</a></li>			
			<li><a id="MS2" runat="server" href="~/Page/Master/LocationServicePoints">Service Point</a></li>			
			<li><a id="MS3" runat="server" href="~/Page/Master/SettingRepair">Setting</a></li>						
		</ul>
	</li> -->
    <li id="a5" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Perajin
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a id="PN1" runat="server" href="~/Page/Pengrajin/AccountPengrajin">Account</a></li>			
			<li><a id="PN2" runat="server" href="~/Page/Pengrajin/RepairServicePoint">Service Point</a></li>				
		</ul>
	</li>
</ul>
