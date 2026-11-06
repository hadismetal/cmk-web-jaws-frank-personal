<%@ page language="C#" masterpagefile="~/MasterPageExport.master" autoeventwireup="true" codefile="Stone1B_Export.aspx.cs" inherits="Page_Account_Stone" %>

<asp:content contentplaceholderid="content" runat="server">
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
                <asp:tableheadercell width="120">PLU</asp:tableheadercell>
                <asp:tableheadercell width="120">Product Item</asp:tableheadercell>
                <asp:tableheadercell width="120">Tipe Lokasi</asp:tableheadercell>
                <asp:tableheadercell width="200">Lokasi</asp:tableheadercell>
				<asp:tableheadercell width="120">Nama</asp:tableheadercell>
                <asp:tableheadercell width="120">Shape</asp:tableheadercell>
                <asp:tableheadercell width="120">Colour</asp:tableheadercell>
                <asp:tableheadercell width="120">Clarity</asp:tableheadercell>
                <asp:tableheadercell width="120">Cutting</asp:tableheadercell>
                <asp:tableheadercell width="120">Brand</asp:tableheadercell>
				<asp:tableheadercell width="90">Butir</asp:tableheadercell>
				<asp:tableheadercell width="90">Carat</asp:tableheadercell>
                <asp:tableheadercell width="90">Harga Per Karat USD</asp:tableheadercell>
                <asp:tableheadercell width="90">Total Harga USD</asp:tableheadercell>
				<asp:tableheadercell width="120">GIA</asp:tableheadercell>
                <asp:tableheadercell width="120">Tanggal Penerimaan</asp:tableheadercell>
                <asp:tableheadercell width="120">Usia Stock <span class="text-sm text-silver">hari</span></asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
    <asp:Literal ID="paging_text" runat="server" />
</asp:content>

