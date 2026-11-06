<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DJ.aspx.cs" inherits="Page_Approval_DJ" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewelry
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	QC
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
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="Input PLU" />
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
			<div class="table-responsive">
				<label id="lbl_top_transaction" class="text-danger">Display Only Top 20 Data...</label>
				<asp:table id="tb" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
						<asp:tableheadercell width="90" />
						<asp:tableheadercell width="40" />
                        <asp:tableheadercell width="120" />
						<asp:tableheadercell width="120">Nomor</asp:tableheadercell>
						<asp:tableheadercell width="120">Product</asp:tableheadercell>
						<asp:tableheadercell width="120">Kategori</asp:tableheadercell>
						<asp:tableheadercell width="120">Level</asp:tableheadercell>
					</asp:tableheaderrow>
				</asp:table>
			</div>
		</form>
	</div>
</asp:content>

