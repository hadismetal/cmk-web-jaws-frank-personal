<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="LD_PricingSubmit.aspx.cs" inherits="Page_Approval_LD_PricingSubmit" %>

<asp:content contentplaceholderid="h1" runat="server">
	Loose Diamond
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Approval
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p>
		<span class="text-muted">Nomor :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		stone
	</div>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-report">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="250">Stone</asp:tableheadercell>
				<asp:tableheadercell width="90">Tgl.</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right"><small>USD</small> Harga Satuan</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-center">Total Butir</asp:tableheadercell>
                <asp:tableheadercell width="130" cssclass="text-center">Total Carat</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right"><small>USD</small> Harga Total</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
    <hr />
	<form runat="server">
		<div class="button-left">
			<button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
				<i class="icon-check"></i>
				Approved
			</button>
            
		</div>
	</form>
</asp:content>

