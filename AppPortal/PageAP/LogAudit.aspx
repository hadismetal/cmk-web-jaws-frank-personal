<%@ page language="C#" masterpagefile="~/MasterAP.master" autoeventwireup="true" codefile="LogAudit.aspx.cs" inherits="AP_LogAudit" %>

<asp:content contentplaceholderid="h1" runat="server">
	Audit Log
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Format :
			</div>
			<asp:dropdownlist id="format" runat="server" cssclass="form-control" width="200">
				<asp:listitem>Summary</asp:listitem>
				<asp:listitem>Detail Report</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<div class="text-muted">
				Diurutkan :
			</div>
			<asp:dropdownlist id="order" runat="server" cssclass="form-control" width="200">
				<asp:listitem>Histori Terlama</asp:listitem>
				<asp:listitem>Histori Terbaru</asp:listitem>
			</asp:dropdownlist>
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
				<asp:tableheadercell width="200">User Account</asp:tableheadercell>
				<asp:tableheadercell width="500">Keterangan</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

