<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DJ_Stone.aspx.cs" inherits="Page_Register_DJ_Stone" %>

<%@ register src="MenuDJ.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewelry
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<hr />
	<div class="row">
		<div class="col-md-3">
			<p>
				<span class="text-muted">Gross Weight :</span><br />
				<span class="text-lg" id="grossweight" runat="server" />
				<small class="text-silver">Gram</small>
			</p>
		</div>
		<div class="col-md-3">
			<p>
				<span class="text-muted">Stone Carat :</span><br />
				<span class="text-lg" id="stonecarat" runat="server" />
				<small class="text-silver">Carat</small>
			</p>
		</div>
		<div class="col-md-3">
			<p>
				<span class="text-muted">Stone Weight :</span><br />
				<span class="text-lg" id="stoneweight" runat="server" />
				<small class="text-silver">Gram</small>
			</p>
		</div>
		<div class="col-md-3">
			<p>
				<span class="text-muted">Netto Weight :</span><br />
				<span class="text-lg" id="nettoweight" runat="server" />
				<small class="text-silver">Gram</small>
			</p>
		</div>
	</div>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="180">
					<a id="add" runat="server" class="btn btn-success" href="DJ_StoneAdd.aspx">
						<i class="icon-plus"></i>
						Add Stone
					</a>
				</asp:tableheadercell>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="250">Stone</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">P</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">L</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">T</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right">Carat / Butir</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-center">Total Butir</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right">Total Carat</asp:tableheadercell>
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
		function goDelete(fooid, fosi, footid) {
			txtConfirm =
				"Lanjutkan Proses DELETE ?\n\n" +
				"Penghapusan data / transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";

			if (confirm(txtConfirm))
				location.href = 'DJ_StoneDelete.aspx?id=' + fooid + '&si=' + fosi + '&tid=' + footid;
		}
	</script>
</asp:content>

