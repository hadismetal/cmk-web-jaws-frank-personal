<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="ProductLevel.aspx.cs" inherits="Page_Settings_ProductLevel" %>

<asp:content contentplaceholderid="h1" runat="server">
	Product Level
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Settings
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="100" />
				<asp:tableheadercell width="250">Product Level</asp:tableheadercell>
				<asp:tableheadercell width="100" cssclass="text-center">Diamond<br />Jewelry</asp:tableheadercell>
				<asp:tableheadercell width="100" cssclass="text-center">Plain<br />Gold</asp:tableheadercell>
				<asp:tableheadercell width="100" cssclass="text-center">Loose<br />Diamond</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

