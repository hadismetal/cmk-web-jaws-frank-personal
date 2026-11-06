<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="2_Submit.aspx.cs" inherits="Page_Create_2_Submit" %>

<%@ register src="Menu2.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Colour Diamond
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
		parcel
	</div>
	<p class="text-lg text-danger" id="kodeparcel" runat="server" />
	<table class="table-file">
		<tr>
			<td class="text-label">Shape</td>
			<td>:</td>
			<td id="parcel01" runat="server" />
		</tr>
		<tr>
			<td class="text-label">Colour</td>
			<td>:</td>
			<td id="parcel02" runat="server" />
		</tr>
		<tr>
			<td class="text-label">Size</td>
			<td>:</td>
			<td id="parcel03" runat="server" />
		</tr>
		<tr>
			<td class="text-label">Colour Grading</td>
			<td>:</td>
			<td id="parcel04" runat="server" />
		</tr>
		<tr>
			<td class="text-label">Clarity</td>
			<td>:</td>
			<td id="parcel05" runat="server" />
		</tr>
		<tr>
			<td class="text-label">Material</td>
			<td>:</td>
			<td id="parcel06" runat="server" />
		</tr>
		<tr>
			<td class="text-label">Category</td>
			<td>:</td>
			<td id="parcel07" runat="server" />
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

