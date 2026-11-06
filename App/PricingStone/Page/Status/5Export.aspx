<%@ page language="C#" masterpagefile="~/MasterPageExport.master" autoeventwireup="true" codefile="5Export.aspx.cs" inherits="Page_Status_5" %>

<asp:content contentplaceholderid="content" runat="server">
	
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="250">Nama</asp:tableheadercell>
				<asp:tableheadercell width="175">Approval</asp:tableheadercell>
				<asp:tableheadercell width="175">Tanggal</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right"><small>USD</small> Cost</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right"><small>USD</small> Harga</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right"><small>USD</small> MU</asp:tableheadercell>
				<asp:tableheadercell width="130" />
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

