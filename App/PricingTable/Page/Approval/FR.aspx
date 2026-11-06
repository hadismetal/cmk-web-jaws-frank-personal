<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="FR.aspx.cs" inherits="Page_Approval_FF" %>

<asp:content contentplaceholderid="h1" runat="server">
	Repair Fee
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
				<asp:table id="tb" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
						<asp:tableheadercell width="100" />
						<asp:tableheadercell width="175">Operator</asp:tableheadercell>
                        <asp:tableheadercell width="130" cssclass="text-right"><small>IDR</small> M</asp:tableheadercell>
						<asp:tableheadercell width="130" cssclass="text-right"><small>IDR</small> M Pending</asp:tableheadercell>
						<asp:tableheadercell width="130" cssclass="text-right"><small>IDR</small> Biaya</asp:tableheadercell>
						<asp:tableheadercell width="130" cssclass="text-right"><small>IDR</small> Biaya Pending</asp:tableheadercell>
						<asp:tableheadercell width="150" cssclass="text-right">Mutasi</asp:tableheadercell>
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

