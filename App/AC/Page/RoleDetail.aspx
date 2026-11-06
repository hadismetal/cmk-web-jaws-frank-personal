<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="RoleDetail.aspx.cs" inherits="Page_RoleDetail" %>

<asp:content contentplaceholderid="h1" runat="server">
	Role Settings
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p class="text-nowrap">
		<span class="text-warning" id="header_group" runat="server" />
		<br />
		<span class="text-lg" id="header_module" runat="server" />
	</p>
	<hr />
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Role :
			</div>
			<asp:dropdownlist id="role" runat="server" cssclass="form-control" width="200">
				<asp:listitem>Semua</asp:listitem>
				<asp:listitem>Manager</asp:listitem>
				<asp:listitem>Operator</asp:listitem>
				<asp:listitem>Download File</asp:listitem>
				<asp:listitem>Download Excel</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
			</div>
		</div>
	</form>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="180">
					<a id="create" runat="server" href="RoleUserAdd.aspx" target="_blank" class="btn btn-success">
						<i class="icon-plus"></i>
						Add User
					</a>
				</asp:tableheadercell>
				<asp:tableheadercell width="50">#</asp:tableheadercell>
				<asp:tableheadercell width="250">Nama / NIK</asp:tableheadercell>
				<asp:tableheadercell width="350">Email / Keterangan</asp:tableheadercell>
				<asp:tableheadercell width="90" cssclass="text-center">Manager</asp:tableheadercell>
				<asp:tableheadercell width="90" cssclass="text-center">Operator</asp:tableheadercell>
				<asp:tableheadercell width="90" cssclass="text-center">Download<br />File</asp:tableheadercell>
				<asp:tableheadercell width="90" cssclass="text-center">Download<br />Excel</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<script type="text/javascript">
		function goDelete(fooid, fookm) {
			txtConfirm =
				"Lanjutkan Proses DELETE ?\n\n" +
				"Penghapusan data / transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";

			if (confirm(txtConfirm))
				location.href = 'RoleUserDelete.aspx?id=' + fooid + '&km=' + fookm;
		}
	</script>
</asp:content>

