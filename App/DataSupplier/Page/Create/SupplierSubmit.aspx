<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="SupplierSubmit.aspx.cs" inherits="Page_Create_SupplierSubmit" %>

<%@ register src="MenuSupplier.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Supplier
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Nama :</span><br />
		<span class="text-lg" id="header_nama" runat="server" />
	</p>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		master
	</div>
	<table class="table-file">
		<tr>
			<td class="text-label">Group</td>
			<td>:</td>
			<td id="group" runat="server" />
		</tr>
		<tr>
			<td class="text-label">Market Brand</td>
			<td>:</td>
			<td id="brand" runat="server" />
		</tr>
	</table>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		account
	</div>
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
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		address
	</div>
	<table class="table-file">
		<tr>
			<td class="text-label">Alamat Domisili</td>
			<td>:</td>
			<td id="addralamat" runat="server" />
		</tr>
		<tr>
			<td class="text-label">No. Telepon</td>
			<td>:</td>
			<td id="addrnotelp" runat="server" />
		</tr>
		<tr>
			<td class="text-label">No. Fax</td>
			<td>:</td>
			<td id="addrnofax" runat="server" />
		</tr>
		<tr>
			<td class="text-label">Alamat Email</td>
			<td>:</td>
			<td id="addremail" runat="server" />
		</tr>
	</table>
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

