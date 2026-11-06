<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Page_Create_Brand, App_Web_0pfn4m0h" %>

<%@ register src="MenuBrand.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Market Brand
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form runat="server">
		<div class="form-group width-300">
			<label>
				Company :
			</label>
			<span id="company_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
			<asp:dropdownlist id="company" runat="server" cssclass="form-control text-bold" width="275" />
		</div>
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
		</div>
	</form>
</asp:content>

