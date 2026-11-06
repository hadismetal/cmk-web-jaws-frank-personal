<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="FM.aspx.cs" inherits="Page_StatusLog_FM" %>

<asp:content contentplaceholderid="h1" runat="server">
	Mounting Fee
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Detail Log
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<table class="table-file">
		<tr>
			<td class="text-label">Market Brand</td>
			<td>:</td>
			<td id="header_brand" runat="server" class="text-bold" />
		</tr>
		<tr>
			<td class="text-label">Product Item</td>
			<td>:</td>
			<td id="header_item" runat="server" class="text-bold" />
		</tr>
		<tr>
			<td class="text-label">Cons. Process</td>
			<td>:</td>
			<td id="header_cons" runat="server" class="text-bold" />
		</tr>
	</table>
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
				<asp:tableheadercell width="200">Approval</asp:tableheadercell>
				<asp:tableheadercell width="120" cssclass="text-right"><small>IDR</small><br />Biaya Lama</asp:tableheadercell>
				<asp:tableheadercell width="120" cssclass="text-right"><small>IDR</small><br />Biaya Baru</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>
