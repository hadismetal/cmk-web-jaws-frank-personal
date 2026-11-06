<%@ page language="C#" masterpagefile="~/MasterPageExport.master" autoeventwireup="true" codefile="1B_Excel.aspx.cs" inherits="Page_Account_1B" %>

<asp:content contentplaceholderid="content" runat="server">
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="250">Nama</asp:tableheadercell>
				<asp:tableheadercell width="90">Tgl.</asp:tableheadercell>
				<asp:tableheadercell width="60">Status</asp:tableheadercell>
				<asp:tableheadercell width="250">Kode Parcel</asp:tableheadercell>
				<asp:tableheadercell width="200">Shape</asp:tableheadercell>
				<asp:tableheadercell width="200">Size</asp:tableheadercell>
				<asp:tableheadercell width="200">Color</asp:tableheadercell>
				<asp:tableheadercell width="200">Clarity</asp:tableheadercell>
				<asp:tableheadercell width="200">Cutting</asp:tableheadercell>
				<asp:tableheadercell width="200">Brand</asp:tableheadercell>
				<asp:tableheadercell width="200">Category</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

