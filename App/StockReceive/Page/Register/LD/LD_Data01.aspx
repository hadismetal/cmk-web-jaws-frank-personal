<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="LD_Data01.aspx.cs" inherits="Page_Register_LD_Data01" %>

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
	<form runat="server">
		<table class="table-form">
			<tr>
				<td class="fixed-150">Supplier</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="LD_DataSupplier.aspx" class="btn btn-info" id="search_supplier" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="suppliernama" runat="server" cssclass="form-control" maxlength="50" width="300" />
					</div>
				</td>
			</tr>
			<tr>
				<td class="fixed-150">No. Supplier<br />
					<i class="text-muted">Kode Barang</i>
				</td>
				<td>:</td>
				<td>
					<asp:textbox id="suppliernomor" runat="server" cssclass="form-control" maxlength="50" width="200" />
				</td>
			</tr>
		</table>
		<table class="table-form">
			<tr>
				<td class="fixed-150">Request Outlet</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="LD_DataOutlet.aspx" class="btn btn-info" id="search_outlet" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="requestoutletnama" runat="server" cssclass="form-control" maxlength="50" width="300" />
					</div>
				</td>
			</tr>
			<tr>
				<td class="fixed-150">Request Customer</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="LD_DataCustomer.aspx" class="btn btn-info" id="search_customer" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="requestcustomernama" runat="server" cssclass="form-control" maxlength="50" width="300" />
					</div>
				</td>
			</tr>
		</table>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
		</div>
	</form>
</asp:content>

