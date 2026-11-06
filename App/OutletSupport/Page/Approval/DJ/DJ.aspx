<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DJ.aspx.cs" inherits="Page_Approval_Product" %>

<asp:content contentplaceholderid="h1" runat="server">
	Promo Diamond Jewellery
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Approval
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div id="zero" runat="server" visible="false">
		<i class="icon-ok icon-5x pull-left text-success"></i>
		<h4>Proses Selesai</h4>
		<p class="text-muted">
			Semua data / transaksi dalam kondisi sudah approval.
		</p>
		<div class="clearfix"></div>
		<hr />
	</div>
	<div id="form_area" runat="server">
		<form runat="server" class="form-inline">
			<div class="table-responsive">
				<asp:table id="tb" runat="server" cssclass="table-list text-sm">
					<asp:tableheaderrow>
						<asp:tableheadercell width="100" />
                        <asp:tableheadercell width="40" />
						<asp:tableheadercell width="40">No.</asp:tableheadercell>
						<asp:tableheadercell width="170">Nama</asp:tableheadercell>
                        <asp:tableheadercell width="60">Exc. /<br />Resell</asp:tableheadercell>
                        <asp:tableheadercell width="60">Disc.<br />Member</asp:tableheadercell>
                        <asp:tableheadercell width="60">Disc.<br />Normal</asp:tableheadercell>
						<asp:tableheadercell width="130">Operator</asp:tableheadercell>
						<asp:tableheadercell width="130">Start</asp:tableheadercell>
						<asp:tableheadercell width="130">Expired </asp:tableheadercell>
                        <asp:tableheadercell width="130">Tipe </asp:tableheadercell>
						<asp:tableheadercell width="80" cssclass="text-right"> Discount Persen <small>%</small></asp:tableheadercell>
                        <asp:tableheadercell width="130" cssclass="text-right"> Discount Nominal <small>IDR</small></asp:tableheadercell>
						<asp:tableheadercell width="130" cssclass="text-right"> Point Reward</asp:tableheadercell>
                        <asp:tableheadercell width="130" cssclass="text-right"> Promo 1 Harga</asp:tableheadercell>
						
					</asp:tableheaderrow>
				</asp:table>
			</div>
			<div class="button-left">
				<asp:dropdownlist id="process_id" runat="server" visible="false" />
				<button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
					<i class="icon-fixed-width icon-check"></i>
					Approved All Checked
				</button>
			</div>
		</form>
	</div>
</asp:content>

