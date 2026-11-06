<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PreLEI_Submit.aspx.cs" Inherits="PreLEI_Submit" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    PreLOVE Reborn
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Approval
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
	<div class="modal" role="dialog" id="modal-example" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Warning</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true"></span>
                    </button>
                </div>
            <div class="modal-body text-justify">
                <h3><b style="font-family:Consolas">Nomor Retur <span runat="server" id="no_rtr"></span> akan diapprove. Nomor Retur yang sudah diapprove tidak dapat diapprove kembali!!!</b></h3>
            </div>
            <div class="modal-footer">
                <button class="btn btn-danger" runat="server" onserverclick="save_Click" onclick="issentreceipt();" >Approve</button>
                <button type="button" class="btn btn-muted" data-dismiss="modal">Cancel</button>
                <script type="text/javascript">
                    function issentreceipt() {
                        $('#modal-example').modal('hide');
                    }
                </script>
            </div>
        </div>
      </div>
    </div>
	<form runat="server" class="form-inline">
		<div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table table-bordered">
				<asp:TableHeaderRow BackColor="SkyBlue" Font-Size="Larger">
                    <asp:TableHeaderCell ColumnSpan="5"><b>HEADER</b></asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableHeaderRow BackColor="LightBlue">
                    <asp:TableHeaderCell style="text-align:center" Width="150">NO. RETURN </asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align:center" Width="100">NAMA ASAL</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align:center" Width="100">NAMA TUJUAN</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="150">OPERATOR</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="150">OPERATOR TGL</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
		</br>
		<div class="table-responsive">
            <asp:Table ID="tbitem" runat="server" CssClass="table table-bordered">
				<asp:TableHeaderRow BackColor="SkyBlue" Font-Size="Larger">
                    <asp:TableHeaderCell ColumnSpan="13"><b>DETAIL</b></asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableHeaderRow BackColor="LightBlue">
                    <asp:TableHeaderCell style="text-align:center" Width="40">NO.</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="120">GAMBAR</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="120">TIPE PRODUCT</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align:center" Width="100">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align:center" Width="100">KODE BARANG</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align:center" Width="120">ITEM</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align:center" Width="120">FRAME COLOR</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">WEIGHT ORIGINAL</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">WEIGHT QC</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">KADAR TUKARAN BELI ORIGINAL</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">KADAR TUKARAN BELI QC</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">KADAR TUKARAN JUAL ORIGINAL</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">KADAR TUKARAN JUAL QC</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

        <div class="table-responsive">
            <asp:Table ID="tbitem2" runat="server" CssClass="table table-bordered">
				<asp:TableHeaderRow BackColor="SkyBlue" Font-Size="Larger">
                    <asp:TableHeaderCell ColumnSpan="23"><b>DETAIL</b></asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableHeaderRow BackColor="LightBlue">
                    <asp:TableHeaderCell style="text-align:center" Width="40">NO.</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="120">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align:center" Width="100">BRAND</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align:center" Width="120">WAREHOUSE</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align:center" Width="120">KODE KARET</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align:center" Width="120">NOMOR SUPPLIER</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">ITEM</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">STONE DIST</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">CATEGORY</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">SEGMEN</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">NET WEIGHT</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">GROSS WEIGHT</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">GROSS WEIGHT ACTUAL</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">HARGA M</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">HARGA R</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">HARGA PARCEL</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">T BUTIR</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">T CARAT</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">STONE M</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">STONE R</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">APPROVAL</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">APPROVAL TANGGAL</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align:center" Width="100">KETERANGAN</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
		<div class="button-inline">
            <asp:Button ID="save" runat="server" CssClass="btn btn-primary" OnClick="save_Click" visible="false" Text="Confirm" OnClientClick="onconfirmexample(); return false;">
            </asp:Button>
			<script type="text/javascript">
				$(document).ready(function () {});
				
				function onconfirmexample() {
					$('#modal-example').modal('show');
				}
		</script>
        </div>
	</form>
</asp:Content>

