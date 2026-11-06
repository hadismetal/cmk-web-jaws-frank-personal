<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="Log_2.aspx.cs" inherits="Page_Status_Log_2" %>

<asp:content contentplaceholderid="h1" runat="server">
	Colour Diamond
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Detail Log
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p>
		<span class="text-muted">Nama :</span><br />
		<span class="text-lg text-warning" id="header_nama" runat="server" />
	</p>
	<hr />
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Periode :
			</div>
			<%--<asp:dropdownlist id="bulan" runat="server" cssclass="form-control" width="150" />--%>
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
				<asp:tableheadercell width="120" cssclass="text-right"><small>USD</small><br />Harga Lama</asp:tableheadercell>
				<asp:tableheadercell width="120" cssclass="text-right"><small>USD</small><br />Harga Baru</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>
