<%@ page language="C#" masterpagefile="~/MasterAP.master" autoeventwireup="true" codefile="Diary.aspx.cs" inherits="AP_Diary" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diary
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div id="div_create" runat="server">
		<a id="create" runat="server" href="DiaryCreate.aspx" class="btn btn-success">
			<i class="icon-plus"></i>
			Create New
		</a>
		<hr />
	</div>
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Diurutkan :
			</div>
			<asp:dropdownlist id="order" runat="server" cssclass="form-control" width="200">
				<asp:listitem>Input Terlama</asp:listitem>
				<asp:listitem>Input Terbaru</asp:listitem>
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
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="500">Catatan</asp:tableheadercell>
				<asp:tableheadercell width="200">Author</asp:tableheadercell>
				<asp:tableheadercell width="85" />
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<script type="text/javascript">
		function goDelete(fooid) {
			txtConfirm =
				"Lanjutkan Proses DELETE ?\n\n" +
				"Penghapusan data / transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";

			if (confirm(txtConfirm))
				location.href = 'DiaryDelete.aspx?id=' + fooid;
		}
	</script>
</asp:content>

