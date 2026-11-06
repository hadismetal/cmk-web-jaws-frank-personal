<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Repair_DataStone.aspx.cs" inherits="Page_Register_Repair_Stone" %>

<%@ register src="MenuRepair.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content ID="Content1" contentplaceholderid="h1" runat="server">
	Repair
</asp:content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content ID="Content3" contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
    <div class="row">
        <div class="col-xs-12 col-sm-4">
            <table class="table-file">
                <tr>
                    <td>Customer</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_customer" /></td>
                </tr>
                <tr>
                    <td>JP/JR</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_jpjr" /></td>
                </tr>
                <tr>
                    <td>PLU</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_plu" /></td>
                </tr>
                <tr>
                    <td>Tanggal</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_tgl" /></td>
                </tr>
                <tr>
                    <td>Tanggal Selesai</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_tgl2" /></td>
                </tr>
                <tr>
                    <td>Keterangan</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_keterangan" /></td>
                </tr>
            </table>
        </div>
        <div class="col-xs-12 col-sm-4">
            Repair Item :<br />
            <asp:Table runat="server" ID="tbl_repairitem" BorderStyle="None" CssClass="table-file" />
        </div>
        <div class="col-xs-12 col-sm-4">
            <table class="table-file">
                <tr>
                    <td><span class="text-muted">Gross Weight</span></td>
                    <td>&nbsp;:&nbsp;</td>
                    <td class="pull-right">
                        <span class="text-lg" id="grossweight" runat="server" />
                    </td>
                    <td><small class="text-silver">Gram</small></td>
                </tr>
                <%--<tr>
                    <td><span class="text-muted">Kadar Logam</span></td>
                    <td>&nbsp;:&nbsp;</td>
                    <td class="pull-right">
                        <span class="text-lg" id="kadarlogam" runat="server" />
                    </td>
                    <td><small class="text-silver">%</small></td>
                </tr>--%>
                <tr>
                    <td><span class="text-muted">Diameter</span></td>
                    <td>&nbsp;:&nbsp;</td>
                    <td class="pull-right">
                        <span class="text-lg" id="diameter" runat="server" />
                    </td>
                    <td><small class="text-silver"></small></td>
                </tr>
                <tr>
                    <td><span class="text-muted">Panjang</span></td>
                    <td>&nbsp;:&nbsp;</td>
                    <td class="pull-right">
                        <span class="text-lg" id="panjang" runat="server" />
                    </td>
                    <td><small class="text-silver"></small></td>
                </tr>
                <tr>
                    <td><span class="text-muted">Lebar</span></td>
                    <td>&nbsp;:&nbsp;</td>
                    <td class="pull-right">
                        <span class="text-lg" id="lebar" runat="server" />
				        </td>
                    <td><small class="text-silver"></small></td>
                </tr>
                <tr>
                    <td><span class="text-muted">Ring Size</span></td>
                    <td>&nbsp;:&nbsp;</td>
                    <td class="pull-right">
                        <span class="text-lg" id="ringsize" runat="server" />
				        </td>
                    <td><small class="text-silver"></small></td>
                </tr>
            </table>
            <p>
				<br />
				
			</p>
        </div>
    </div>
	<hr />
    <div class="file-subheader hidden-sm hidden-xs">
		current stone
	</div>
	<div class="table-responsive">
		<asp:table id="tb2" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="300">Stone</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">P</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">L</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">T</asp:tableheadercell>
				<asp:tableheadercell width="110" cssclass="text-right">Carat / Butir</asp:tableheadercell>
				<asp:tableheadercell width="100" cssclass="text-center">Total Butir</asp:tableheadercell>
				<asp:tableheadercell width="140" cssclass="text-center">Total Carat</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<hr />
	<div class="table-responsive">
        <a id="add" runat="server" class="btn btn-success" href="Repair_DataStoneAdd.aspx">
			<i class="icon-plus"></i>
			Add Stone
		</a>
        <br /><br />
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="90"></asp:tableheadercell>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="200">Stone</asp:tableheadercell>
                <asp:tableheadercell width="100">Terpasang</asp:tableheadercell>
				<asp:tableheadercell width="110" cssclass="text-right">Carat / Butir</asp:tableheadercell>
				<asp:tableheadercell width="100" cssclass="text-center">Total Butir</asp:tableheadercell>
				<asp:tableheadercell width="140" cssclass="text-center">Total Carat</asp:tableheadercell>
                <asp:tableheadercell width="250">Keterangan</asp:tableheadercell>
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
	    function goDelete(fooid, fosi, footid) {
	        txtConfirm =
				"Lanjutkan Proses DELETE ?\n\n" +
				"Penghapusan data / transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";

	        if (confirm(txtConfirm))
	            location.href = 'Repair_DataStoneDelete.aspx?id=' + fooid + '&si=' + fosi + '&tid=' + footid;
	    }
	</script>
</asp:content>

