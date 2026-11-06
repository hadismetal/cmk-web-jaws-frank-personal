<%@ page language="C#" masterpagefile="~/MasterPagePrint.master" autoeventwireup="true" codefile="Stone3_Print.aspx.cs" inherits="Page_Account_Stone" %>

<asp:content contentplaceholderid="content" runat="server">
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
                <asp:tableheadercell width="120">PLU</asp:tableheadercell>
                <asp:tableheadercell width="120">Tipe Lokasi</asp:tableheadercell>
                <asp:tableheadercell width="200">Lokasi</asp:tableheadercell>
				<asp:tableheadercell width="150">Nama</asp:tableheadercell>
                <asp:tableheadercell width="150">Shape</asp:tableheadercell>
                <asp:tableheadercell width="150">Colour</asp:tableheadercell>
                <asp:tableheadercell width="220">Clarity</asp:tableheadercell>
				<asp:tableheadercell Width="100" CssClass ="text-right">Butir</asp:tableheadercell>
				<asp:tableheadercell Width="100" CssClass ="text-right">Carat</asp:tableheadercell>
                <asp:tableheadercell Width="170" CssClass ="text-right">Harga Per Karat USD</asp:tableheadercell>
                <asp:tableheadercell Width="170" CssClass ="text-right">Total Harga USD</asp:tableheadercell>
				<asp:tableheadercell Width="170" CssClass ="text-right">Harga Satuan M</asp:tableheadercell>
                <asp:tableheadercell Width="170" CssClass ="text-right">Harga Total M</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
    <asp:Literal ID="paging_text" runat="server" />
</asp:content>

