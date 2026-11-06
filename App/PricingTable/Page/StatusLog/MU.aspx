<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="MU.aspx.cs" inherits="Page_StatusLog_MU" %>

<asp:content contentplaceholderid="h1" runat="server">
	Mark-Up System
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Detail Log
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<table class="table-file">
		<tr>
			<td class="text-label">Product Category</td>
			<td>:</td>
			<td id="header_category" runat="server" class="text-bold" />
		</tr>
		<tr>
			<td class="text-label">Product Level</td>
			<td>:</td>
			<td id="header_level" runat="server" class="text-bold" />
		</tr>
		<tr>
			<td class="text-label">Stone Distribution</td>
			<td>:</td>
			<td id="header_dist" runat="server" class="text-bold" />
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
				<asp:tableheadercell width="120" cssclass="text-right">Persen Lama</asp:tableheadercell>
				<asp:tableheadercell width="120" cssclass="text-right">Persen Baru</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>
