<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Size01.aspx.cs" inherits="Page_Settings_Size01" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Size Control
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="100" />
				<asp:tableheadercell width="125">Kode Parcel</asp:tableheadercell>
				<asp:tableheadercell width="200">Nama</asp:tableheadercell>
				<asp:tableheadercell width="100" cssclass="text-right"><small>Carat</small> Min.</asp:tableheadercell>
				<asp:tableheadercell width="100" cssclass="text-right"><small>Carat</small> Max.</asp:tableheadercell>
				<asp:tableheadercell width="100" cssclass="text-right">Pricing</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

