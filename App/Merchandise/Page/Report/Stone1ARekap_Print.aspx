<%@ page language="C#" masterpagefile="~/MasterPagePrint.master" autoeventwireup="true" codefile="Stone1ARekap_Print.aspx.cs" inherits="Page_Account_Stone" %>

<asp:content contentplaceholderid="content" runat="server">
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="120">Nama</asp:tableheadercell>
                <asp:tableheadercell width="120">Shape</asp:tableheadercell>
                <asp:tableheadercell width="120">Colour</asp:tableheadercell>
                <asp:tableheadercell width="120">Clarity</asp:tableheadercell>
				<asp:tableheadercell width="120">Total Butir</asp:tableheadercell>
				<asp:tableheadercell width="120">Total Carat</asp:tableheadercell>
				
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

