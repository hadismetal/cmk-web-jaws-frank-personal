<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="ProductSubmit.aspx.cs" inherits="Page_Create_PackagingSubmit" %>

<%@ register src="MenuProduct.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Packaging
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
    <table class="table-file">
		<tr>
			<td class="text-label">Brand</td>
			<td>:</td>
			<td id="brand" runat="server" />
		</tr>
	</table>
	<hr />
	<p>
		<span class="text-muted">Nama :</span><br />
		<span class="text-lg" id="header_nama" runat="server" />
	</p>
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
	</table>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		information
	</div>
	<table class="table-file">
		<tr>
			<td class="text-label">Kode</td>
			<td>:</td>
			<td id="kode" runat="server" />
		</tr>
		<tr>
			<td class="text-label">Satuan</td>
			<td>:</td>
			<td id="satuan" runat="server" />
		</tr>
        <tr>
			<td class="text-label">Picture</td>
			<td>:</td>
			<td><asp:Image ID="imgPicture" runat="server" CssClass="img-thumbnail" Width="500" /></td>
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

