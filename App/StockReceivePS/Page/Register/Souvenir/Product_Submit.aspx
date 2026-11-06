<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Product_Submit.aspx.cs" inherits="Page_Register_DJ_Submit" %>

<%@ register src="Menu.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Souvenir
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<div class="row">
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">No. PO</td>
					<td>:</td>
					<td id="nopo" runat="server" />
				</tr>
				<tr>
					<td class="text-label">No. DO</td>
					<td>:</td>
					<td id="nodo" runat="server" />
				</tr>
                <tr>
					<td class="text-label">Tanggal</td>
					<td>:</td>
					<td id="tgl" runat="server" />
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
			</table>
		</div>
	</div>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		Product
	</div>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="250">Product</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">Kode PLU</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">Qty</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-center">Harga</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
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

