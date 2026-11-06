<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Shape04.aspx.cs" inherits="Page_Settings_Shape04" %>

<asp:content contentplaceholderid="h1" runat="server">
	Semi Precious
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Shape Control
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="100" />
				<asp:tableheadercell width="125">Kode Parcel</asp:tableheadercell>
				<asp:tableheadercell width="200">Nama</asp:tableheadercell>
				<asp:tableheadercell width="100" cssclass="text-right">Shape</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

