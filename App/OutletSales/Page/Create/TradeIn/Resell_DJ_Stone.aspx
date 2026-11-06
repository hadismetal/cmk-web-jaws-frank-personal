<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Resell_DJ_Stone.aspx.cs" inherits="Page_Register_DJ_Stone" %>

<%@ register src="MenuDJ.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content ID="Content1" contentplaceholderid="h1" runat="server">
	Diamond Jewellery
</asp:content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content ID="Content3" contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Nomor :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<hr />
	<div class="row">
		<div class="col-md-3">
			<p>
				<span class="text-muted">Gross Weight :</span><br />
				<span class="text-lg" id="grossweight" runat="server" />
				<small class="text-silver">Gram</small>
			</p>
		</div>
	</div>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="180">
					<a id="add" runat="server" class="btn btn-success" href="Resell_DJ_StoneAdd.aspx">
						<i class="icon-plus"></i>
						Add Stone
					</a>
				</asp:tableheadercell>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="400">Nama</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">P</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">L</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">T</asp:tableheadercell>
				<asp:tableheadercell width="120" cssclass="text-right">Carat / Butir</asp:tableheadercell>
				<asp:tableheadercell width="120" cssclass="text-center">T. Butir</asp:tableheadercell>
				<asp:tableheadercell width="120" cssclass="text-right">T. Carat</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<form id="Form1" runat="server">
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
		</div>
	</form>
	<script type="text/javascript">
	    function goDelete(fooid, itemid, ro, so, fosi, footid, idDJ) {
	        txtConfirm =
				"Lanjutkan Proses DELETE ?\n\n" +
				"Penghapusan data / transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";

	        if (confirm(txtConfirm))
	            location.href = 'Resell_DJ_StoneDelete.aspx?id=' + fooid + '&itemid=' + itemid + '&ro=' + ro + '&so=' + so + '&si=' + fosi + '&tid=' + footid + "&idDJ=" + idDJ;
	    }
	</script>
</asp:content>

