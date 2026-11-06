<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="StockKeeperUpdateAddr.aspx.cs" inherits="Page_Account_StockKeeperUpdateAddr" %>

<%@ register src="MenuStockKeeper.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Stock Keeper
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	<asp:literal id="h1_st" runat="server" />
</asp:content>
<asp:content contentplaceholderid="h1_id" runat="server">
	ID:<asp:literal id="h1_id" runat="server" />
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form runat="server">
		<div class="form-group width-500">
			<label>
				Alamat Domisili :
			</label>
			<asp:textbox id="addralamat" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
		</div>
		<table class="table-form">
			<tr>
				<td>No. Telepon</td>
				<td>:</td>
				<td>
					<asp:textbox id="addrnotelp" runat="server" cssclass="form-control" width="300" />
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
				<i class="icon-ok"></i>
				Save
			</button>
		</div>
	</form>
</asp:content>

