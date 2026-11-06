<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DJ_Data01.aspx.cs" inherits="Page_Register_DJ_Data01" %>

<%@ register src="MenuDJ.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewelry
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<hr />
	<form runat="server">
		<table class="table-form">
			<tr>
				<td class="fixed-150">No. Prototype<br />
					<i class="text-muted">Kode Karet</i>
				</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="#" class="btn btn-info" id="search_proto" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="protonomor" runat="server" cssclass="form-control" maxlength="50" width="200" />
					</div>
				</td>
                <td>
					<asp:requiredfieldvalidator id="protonomor_RF" runat="server" />
				</td>
			</tr>
			<tr>
				<td class="fixed-150">No. Product Order<br />
					<i class="text-muted">No. SPK</i>
				</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="DJ_DataProductOrder.aspx" class="btn btn-info" id="search_productorder" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="productordernomor" runat="server" cssclass="form-control" maxlength="50" width="200" />
					</div>
				</td>
			</tr>
            <%--<tr>
				<td class="fixed-150">
                    No. Validation<br />
					<i class="text-muted">No. Validasi</i>
				</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="DJ_DataValidasi.aspx" class="btn btn-info" id="search_validation" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="validationnomor" runat="server" cssclass="form-control" maxlength="50" width="200" />
					</div>
				</td>
			</tr>--%>
		</table>
		<table class="table-form">
			<tr>
				<td class="fixed-150">Supplier</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="DJ_DataSupplier.aspx" class="btn btn-info" id="search_supplier" runat="server">
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
							<a href="DJ_DataOutlet.aspx" class="btn btn-info" id="search_outlet" runat="server">
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
							<a href="DJ_DataCustomer.aspx" class="btn btn-info" id="search_customer" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="requestcustomernama" runat="server" cssclass="form-control" maxlength="50" width="300" />
					</div>
				</td>
			</tr>
			
			<tr>
				<td class="fixed-150">No Validasi</td>
				<td>:</td>
				<td>
				<asp:TextBox
					ID="requestnovalidasi"
					runat="server"
					CssClass="form-control"
					MaxLength="50"
					Width="300" />

				<asp:Label
					ID="lblNoValidError"
					runat="server"
					ForeColor="Red"
					Visible="false" />

				</td>
		    </tr>
		</table>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click" validationgroup="save">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
		</div>
	</form>
</asp:content>

