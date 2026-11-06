<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="LD_Submit.aspx.cs" inherits="Page_Register_LD_Submit" %>

<%@ register src="MenuLD.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Loose Diamond
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Nomor :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		transaction form
	</div>
	<div class="row">
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">Warehouse</td>
					<td>:</td>
					<td id="warehouse" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Market Brand</td>
					<td>:</td>
					<td id="brand" runat="server" />
				</tr>
			</table>
		</div>
		<br class="visible-sm visible-xs" />
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">Keterangan</td>
					<td>:</td>
					<td id="keterangan" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Tanggal</td>
					<td>:</td>
					<td id="tgl" runat="server" />
				</tr>
			</table>
		</div>
	</div>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		transaction data
	</div>
	<div class="row">
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">Supplier</td>
					<td>:</td>
					<td id="suppliernama" runat="server" />
				</tr>
				<tr>
					<td class="text-label">No. Supplier</td>
					<td>:</td>
					<td id="suppliernomor" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Req. Outlet</td>
					<td>:</td>
					<td id="requestoutletnama" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Req. Customer</td>
					<td>:</td>
					<td id="requestcustomernama" runat="server" />
				</tr>
			</table>
		</div>
		<br class="visible-sm visible-xs" />
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">Stock Keeper</td>
					<td>:</td>
					<td id="humanstocknama" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Expired Product</td>
					<td>:</td>
					<td id="expiredproduct" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Expired Request</td>
					<td>:</td>
					<td id="expiredrequest" runat="server" />
				</tr>
			</table>
		</div>
	</div>
	<hr />
    <div class="file-subheader hidden-sm hidden-xs">
		stone
	</div>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="250">Stone</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">P</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">L</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">T</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right">Carat / Butir</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-center">Total Butir</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right">Total Carat</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		picture
	</div>
	<asp:image id="imgpicture" runat="server" cssclass="img-thumbnail" width="500" />
	<hr />
	<form runat="server">
		<div class="button-left">
			<button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
				<i class="icon-check"></i>
				Finish
			</button>
		</div>
	</form>
</asp:content>

