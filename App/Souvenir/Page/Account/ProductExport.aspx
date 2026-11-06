<%@ page language="C#" masterpagefile="~/MasterPageExport.master" autoeventwireup="true" codefile="ProductExport.aspx.cs" inherits="Page_Account_Packaging" %>

<asp:content contentplaceholderid="content" runat="server">

	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="250">Nama</asp:tableheadercell>
				<asp:tableheadercell width="120">Kode</asp:tableheadercell>
				<asp:tableheadercell width="120">Satuan</asp:tableheadercell>
				<asp:tableheadercell width="450">Barcode</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

