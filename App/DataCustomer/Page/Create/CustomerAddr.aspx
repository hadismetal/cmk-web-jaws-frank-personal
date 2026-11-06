<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="CustomerAddr.aspx.cs" inherits="Page_Create_CustomerAddr" %>

<%@ register src="MenuCustomer.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Customer
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
	<form runat="server">
		<div class="form-group width-500">
			<label>
				Alamat Domisili :
			</label>
			<asp:textbox id="addralamat" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
		</div>
		<table class="table-form">
			<tr>
				<td>No. Telepon 1</td>
				<td>:</td>
				<td>
					<asp:textbox id="addrnotelp1" runat="server" cssclass="form-control" width="300" />
				</td>
			</tr>
            <tr>
				<td>No. Telepon 2</td>
				<td>:</td>
				<td>
					<asp:textbox id="addrnotelp2" runat="server" cssclass="form-control" width="300" />
				</td>
			</tr>
            <tr>
				<td>No. Telepon 3</td>
				<td>:</td>
				<td>
					<asp:textbox id="addrnotelp3" runat="server" cssclass="form-control" width="300" />
				</td>
			</tr>
            <tr>
				<td>No. Handphone</td>
				<td>:</td>
				<td>
					<asp:textbox id="addrnohp" runat="server" cssclass="form-control" width="300" />
				</td>
			</tr>
            <tr>
				<td>PIN BB</td>
				<td>:</td>
				<td>
					<asp:textbox id="pinbb" runat="server" cssclass="form-control" width="300" />
				</td>
			</tr>
			<tr>
				<td>No. Fax</td>
				<td>:</td>
				<td>
					<asp:textbox id="addrnofax" runat="server" cssclass="form-control" width="300" />
				</td>
			</tr>
			<tr>
				<td>Alamat Email</td>
				<td>:</td>
				<td>
					<asp:textbox id="addremail" runat="server" cssclass="form-control" width="300" />
				</td>
			</tr>
		</table>
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
		</div>
	</form>
</asp:content>

