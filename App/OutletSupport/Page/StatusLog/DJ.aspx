<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="DJ.aspx.cs" inherits="Page_StatusLog_FF" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewellery
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
			<td class="text-label">Keterangan</td>
			<td>:</td>
			<td id="header_keterangan" runat="server" class="text-bold" />
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
		<asp:table id="tb" runat="server" cssclass="table-report text-sm">
			<asp:tableheaderrow>
				<asp:tableheadercell width="60" />
                <asp:tableheadercell width="130">Tgl</asp:tableheadercell>
				<asp:tableheadercell width="100" cssclass="text-right">Merchandise Exchange Min <small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Merchandise Exchange Min Lama<small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Merchandise Exchange Max <small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Merchandise Exchange Max Lama<small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Merchandise Exchange <small>%</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Merchandise Exchange Lama<small>%</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Trade In Period 1 Min <small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Trade In Period 1 Min Lama<small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Trade In Period 1 Max <small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Trade In Period 1 Max Lama<small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Trade In Period 1 <small>%</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Trade In Period 1 Lama<small>%</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Trade In Period 2 Min <small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Trade In Period 2 Min Lama<small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Trade In Period 2 Max <small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Trade In Period 2 Max Lama<small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Trade In Period 2 <small>%</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Trade In Period 2 Lama<small>%</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Resell Min <small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Resell Min Lama<small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Resell Max <small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Resell Max Lama<small>month</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Resell <small>%</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Resell Lama<small>%</small></asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>
