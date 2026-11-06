<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="UserUpdateRole.aspx.cs" inherits="Page_UserUpdateRole" %>

<%@ register src="MenuFile.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	User Account
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	<asp:literal id="h1_st" runat="server" />
</asp:content>
<asp:content contentplaceholderid="h1_id" runat="server">
	ID:<asp:literal id="h1_id" runat="server" />
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="180">
					<a id="create" runat="server" href="UserUpdateRoleAdd.aspx" class="btn btn-success">
						<i class="icon-plus"></i>
						Add Module
					</a>
				</asp:tableheadercell>
				<asp:tableheadercell width="250">Module</asp:tableheadercell>
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
				location.href = 'UserUpdateRoleDelete.aspx?id=' + fooid + '&km=' + fookm;
		}
	</script>
</asp:content>

