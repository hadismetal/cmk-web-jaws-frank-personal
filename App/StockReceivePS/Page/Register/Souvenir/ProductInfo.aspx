<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="ProductInfo.aspx.cs" inherits="Page_Register_DJ_Stone" %>

<%@ register src="Menu.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Souvenir
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="180">
					<a id="add" runat="server" class="btn btn-success" href="ProductAdd.aspx">
						<i class="icon-plus"></i>
						Add Product
					</a>
				</asp:tableheadercell>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="250">Name</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">Qty</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-center">Harga</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<form runat="server">
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
		</div>
	</form>
	<script type="text/javascript">
		function goDelete(fooid, footid) {
			txtConfirm =
				"Lanjutkan Proses DELETE ?\n\n" +
				"Penghapusan data / transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";

			if (confirm(txtConfirm))
				location.href = 'ProductDelete.aspx?id=' + fooid + '&tid=' + footid;
		}
	</script>
</asp:content>

