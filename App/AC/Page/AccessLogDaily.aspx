<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="AccessLogDaily.aspx.cs" inherits="Page_AccessLogDaily" %>

<asp:content contentplaceholderid="h1" runat="server">
	Access Log
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Daily Report
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div class="row">
		<div class="col-md-3">
			<p class="text-nowrap">
				<span class="text-muted">Tanggal :</span><br />
				<span class="text-lg" id="header_tgl" runat="server" />
			</p>
		</div>
		<div class="col-md-3">
			<p>
				<span class="text-muted">Aktivitas :</span><br />
				<span class="text-lg" id="header_aktivitas" runat="server" />
			</p>
		</div>
	</div>
	<hr />
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Diurutkan :
			</div>
			<asp:dropdownlist id="order" runat="server" cssclass="form-control" width="150">
				<asp:listitem value="Tgl">Jam</asp:listitem>
				<asp:listitem value="">User Account</asp:listitem>
				<asp:listitem value="IPAddr">IP Address</asp:listitem>
			</asp:dropdownlist>
			<div class="text-00 visible-xs">&nbsp;</div>
			<asp:dropdownlist id="orderby" runat="server" cssclass="form-control" width="100">
				<asp:listitem>ASC</asp:listitem>
				<asp:listitem>DESC</asp:listitem>
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
				<asp:tableheadercell width="75">Jam</asp:tableheadercell>
				<asp:tableheadercell width="100">User Number</asp:tableheadercell>
				<asp:tableheadercell width="300">User Account</asp:tableheadercell>
				<asp:tableheadercell width="300">Keterangan</asp:tableheadercell>
				<asp:tableheadercell width="75" />
				<asp:tableheadercell width="200">IP Address</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

