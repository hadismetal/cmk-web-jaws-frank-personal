<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Mounting.aspx.cs" inherits="Page_Approval_Mounting" %>

<asp:content contentplaceholderid="h1" runat="server">
	Mounting (WIP)
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
						<asp:tableheadercell width="120">Nomor</asp:tableheadercell>
						<asp:tableheadercell width="175">Operator</asp:tableheadercell>
						<asp:tableheadercell width="100">Form</asp:tableheadercell>
						<asp:tableheadercell width="150">No. Referensi</asp:tableheadercell>
						<asp:tableheadercell width="150">Stock Keeper</asp:tableheadercell>
						<asp:tableheadercell width="120">Product</asp:tableheadercell>
						<asp:tableheadercell width="75" />
						<asp:tableheadercell width="75" />
					</asp:tableheaderrow>
				</asp:table>
			</div>
		</form>
	</div>
</asp:content>

