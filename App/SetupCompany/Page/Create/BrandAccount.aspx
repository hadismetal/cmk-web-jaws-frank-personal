<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Page_Create_BrandAccount, App_Web_0pfn4m0h" %>

<%@ register src="MenuBrand.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Market Brand
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Company :</span><br />
		<span class="text-bold" id="company" runat="server" />
	</p>
	<hr />
	<form runat="server">
		<div class="form-group width-500">
			<div class="row">
				<div class="col-xs-7">
					<label>
						Nama :
					</label>
					<asp:requiredfieldvalidator id="nama_RF" runat="server" />
					<asp:textbox id="nama" runat="server" cssclass="form-control text-bold" maxlength="50" />
				</div>
				<div class="col-xs-3">
					<label>
						Kode :
					</label>
					<asp:requiredfieldvalidator id="namakode_RF" runat="server" />
					<asp:textbox id="namakode" runat="server" cssclass="form-control text-bold text-center" maxlength="20" />
				</div>
			</div>
		</div>
		<div class="form-group width-500">
			<label>
				Keterangan :
			</label>
			<asp:textbox id="keterangan" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
		</div>
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<a href="Brand.aspx" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:content>

