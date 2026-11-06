<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="SPK.aspx.cs" inherits="Page_Approval_SPK" %>

<asp:content contentplaceholderid="h1" runat="server">
	Product Order
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
						<asp:tableheadercell width="90" />
                        <asp:tableheadercell width="40" />
						<asp:tableheadercell width="40" />
                        <asp:TableHeaderCell Width="120" />
						<asp:tableheadercell width="175">Operator</asp:tableheadercell>
						<asp:tableheadercell width="150">No. Referensi</asp:tableheadercell>
						<asp:tableheadercell width="100">SPK Brand</asp:tableheadercell>
						<asp:tableheadercell width="120">Product</asp:tableheadercell>
						<asp:tableheadercell width="75" />
						<asp:tableheadercell width="100" />
                        <asp:tableheadercell width="80">Status Approve</asp:tableheadercell>
                        <asp:tableheadercell width="100">Alasan </asp:tableheadercell>
					</asp:tableheaderrow>
				</asp:table>
			</div>
            <div class="button-left">
			    <button id="save" runat="server" class="btn btn-success" onserverclick="Approve_Click">
				    <i class="icon-check"></i>
				    Approve
			    </button>&nbsp;&nbsp;
		    </div>
		</form>
	</div>
</asp:content>

