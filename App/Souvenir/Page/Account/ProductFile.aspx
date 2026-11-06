<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="ProductFile.aspx.cs" inherits="Page_Account_SouvenirFile" %>

<%@ register src="MenuProduct.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Souvenir
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	<asp:literal id="h1_st" runat="server" />
</asp:content>
<asp:content contentplaceholderid="h1_id" runat="server">
	ID:<asp:literal id="h1_id" runat="server" />
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<div class="row">
		<div class="col-md-10">
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
					<td class="text-label">Status</td>
					<td>:</td>
					<td id="disable" runat="server" />
				</tr>
			</table>
			<hr />
			<div class="file-subheader hidden-sm hidden-xs">
				address
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
		</div>
		<div class="col-md-2">
			<hr class="visible-sm visible-xs" />
			<asp:literal id="ap" runat="server" />
		</div>
	</div>
</asp:content>

