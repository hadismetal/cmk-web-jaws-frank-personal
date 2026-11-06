<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" inherits="Page_Account_CompanyFile, App_Web_bssg1hku" %>

<%@ register src="MenuCompany.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Company
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
				<tr>
					<td class="text-label">Alamat Website</td>
					<td>:</td>
					<td id="addrwebsite" runat="server" />
				</tr>
			</table>
		</div>
		<div class="col-md-2">
			<hr class="visible-sm visible-xs" />
			<asp:literal id="ap" runat="server" />
		</div>
	</div>
</asp:content>

