<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="UserFile.aspx.cs" inherits="Page_UserFile" %>

<%@ register src="MenuFile.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	User Account
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
					<td class="text-label">Nama</td>
					<td>:</td>
					<td id="nama" runat="server" />
				</tr>
                <tr>
					<td class="text-label">NIK</td>
					<td>:</td>
					<td id="nik" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Keterangan</td>
					<td>:</td>
					<td id="keterangan" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Alamat Email</td>
					<td>:</td>
					<td id="email" runat="server" />
				</tr>
				<tr>
					<td class="text-label">SMS</td>
					<td>:</td>
					<td id="sms" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Status</td>
					<td>:</td>
					<td id="status" runat="server" />
				</tr>
			</table>
			<hr />
			<div class="file-subheader hidden-sm hidden-xs">
				login & password
			</div>
			<table class="table-file">
				<tr>
					<td class="text-label">Blokir Account</td>
					<td>:</td>
					<td id="blokir" runat="server" class="text-warning" />
				</tr>
				<tr>
					<td class="text-label">Last Login</td>
					<td>:</td>
					<td id="lastlogin" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Password</td>
					<td>:</td>
					<td id="pass" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Update Password</td>
					<td>:</td>
					<td id="recyclelast" runat="server" />
				</tr>
			</table>
		</div>
		<div class="col-md-2">
			<hr class="visible-sm visible-xs" />
			<asp:literal id="ap" runat="server" />
		</div>
	</div>
</asp:content>
