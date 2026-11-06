<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="TGP_V2.aspx.cs" inherits="Page_StatusLog_TGP_V2" %>

<asp:content contentplaceholderid="h1" runat="server">
	Today Gold Price
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Detail Log
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p>
		<span class="text-muted">Nama :</span><br />
		<span class="text-lg" id="header_nama" runat="server" />
	</p>
	<hr />
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Periode :
			</div>
			<asp:dropdownlist id="bulan" runat="server" cssclass="form-control" width="150" />
			<asp:dropdownlist id="tahun" runat="server" cssclass="form-control" width="100" />
		</div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
			</div>
		</div>
	</form>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-report">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="90">Tgl.</asp:tableheadercell>
				<asp:tableheadercell width="75">Jam</asp:tableheadercell>
				<asp:tableheadercell width="170">Approval</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-right"><small>IDR</small><br />Harga Jual Lama</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-right"><small>IDR</small><br />Harga Jual Baru</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-right"><small>IDR</small><br />Harga Beli Lama</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-right"><small>IDR</small><br />Harga Beli Baru</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>
