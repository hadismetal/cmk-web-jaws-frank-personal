<%@ page language="C#" masterpagefile="~/MasterAP.master" autoeventwireup="true" codefile="Attach.aspx.cs" inherits="AP_Attach" %>

<asp:content contentplaceholderid="h1" runat="server">
	Attachment
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div id="div_create" runat="server">
		<a id="create" runat="server" href="AttachCreate.aspx" class="btn btn-success">
			<i class="icon-plus"></i>
			Upload New
		</a>
		<hr />
	</div>
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Diurutkan :
			</div>
			<asp:dropdownlist id="order" runat="server" cssclass="form-control" width="200">
				<asp:listitem>Upload Terlama</asp:listitem>
				<asp:listitem>Upload Terbaru</asp:listitem>
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
				<asp:tableheadercell width="380">Nama File</asp:tableheadercell>
				<asp:tableheadercell width="120" />
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
				location.href = 'AttachDelete.aspx?id=' + fooid;
		}
	</script>
</asp:content>

