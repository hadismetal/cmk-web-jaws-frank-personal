<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="MasterPoint.aspx.cs" inherits="Page_UpdateProcess_MU" %>

<asp:content contentplaceholderid="h1" runat="server">
	Master Point
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
		<form id="Form1" runat="server" class="form-inline">
			<div class="table-responsive">
				<asp:table id="tb" runat="server" cssclass="table-list text-sm">
					<asp:TableHeaderRow>
                        <asp:tableheadercell width="40" />
						<asp:tableheadercell width="40">No.</asp:tableheadercell>
						<asp:tableheadercell width="100">Point Lama</asp:tableheadercell>
                        <asp:tableheadercell width="100">Point Baru</asp:tableheadercell>
                        <asp:tableheadercell width="100">Rupiah Lama</asp:tableheadercell>
                        <asp:tableheadercell width="100">Rupiah Baru</asp:tableheadercell>
						<asp:tableheadercell width="150">Operator</asp:tableheadercell>
						</asp:tableheaderrow>
				</asp:table>
			</div>
			<div class="button-left">
				<asp:dropdownlist id="process_id" runat="server" visible="false" />
				<button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
					<i class="icon-fixed-width icon-check"></i>
					Approved All
				</button>
			</div>
		</form>
	</div>

</asp:content>