<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="ProductCategory.aspx.cs" inherits="Page_Settings_ProductCategory" %>

<asp:content contentplaceholderid="h1" runat="server">
	Product Category
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Settings
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="100" />
				<asp:tableheadercell width="250">Product Category</asp:tableheadercell>
				<asp:tableheadercell width="100" cssclass="text-center">Prototype Code</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

