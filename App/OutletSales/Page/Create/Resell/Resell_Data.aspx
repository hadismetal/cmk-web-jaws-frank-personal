<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Resell_Data.aspx.cs" Inherits="Page_Register_Resell_Item" %>

<%@ Register Src="MenuResell.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Resell
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <form id="Form1" runat="server">
        <div class="row">
            <div class="col-xs-12">
				<div class="panel panel-default">
					<div class="panel-heading">
                            Form Resell
					</div>
                    <div class="panel-body">
						<div class="table-responsive">
							<asp:Table ID="tb" runat="server" CssClass="table-list">
								<asp:TableHeaderRow>
									<asp:TableHeaderCell Width="180">
										<a id="add" runat="server" class="btn btn-success" href="Resell_ItemAdd.aspx">
											<i class="icon-plus"></i>
											Add Item
										</a>
									</asp:TableHeaderCell>
									<asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
									<asp:TableHeaderCell Width="150">PLU</asp:TableHeaderCell>
									<asp:TableHeaderCell Width="150">Product Item</asp:TableHeaderCell>
									<asp:TableHeaderCell Width="150">Harga Acuan</asp:TableHeaderCell>
									<asp:TableHeaderCell Width="150">Dokumen QC</asp:TableHeaderCell>
									<asp:TableHeaderCell Width="150">Berat Timbang</asp:TableHeaderCell>
									<asp:TableHeaderCell Width="150">TGP</asp:TableHeaderCell>
									<asp:TableHeaderCell Width="150">Harga Beli</asp:TableHeaderCell>
								</asp:TableHeaderRow>
							</asp:Table>
						</div>
					</div>
				</div>
            </div>
		</div>			
		<div class="row">
            <div class="col-xs-12">
                <asp:Literal runat="server" ID="lit_error_resell" />
                <asp:Literal runat="server" ID="lit_error_sap" />
            </div>
            <div class="col-xs-12">			
				<div class="alert alert-danger" visible="false" runat="server" id="divlblwarning">
					<small class="text-danger" runat="server" id="lblwarning" visible="false"></small>
				</div>
				<div class="button-inline">
                    <button id="save" runat="server" class="btn btn-primary" onclick="if ( ! UserConfirmation()) return false;" onserverclick="save_Click">
                        <i class="icon-arrow-right"></i>
                        Finish
                    </button>
                </div>
            </div>
		</div>
    </form>
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            document.querySelectorAll('.idr-format').forEach(function (el) {
                el.addEventListener('blur', function () {
                    formatToRupiah(el);
                });
            });
        });
        function goDelete(fooid, fosi, footid) {
            txtConfirm =
                "Lanjutkan Proses DELETE ?\n\n" +
                "Penghapusan data / transaksi sifatnya adalah permanen.\n" +
                "Dianjurkan untuk melakukan backup terlebih dahulu.";

            if (confirm(txtConfirm))
                location.href = 'Resell_ItemDelete.aspx?id=' + fooid + '&itemindex=' + fosi + '&iid=' + footid;
        }
        function UserConfirmation() {
            txtConfirm =
                "Lanjutkan Proses SALES ORDER ?\n\n" +
                "transaksi sifatnya adalah permanen.\n" +
                "Dianjurkan untuk melakukan backup terlebih dahulu.";
            if (confirm(txtConfirm))
                return true;
            else
                return false;
        }

        function formatToRupiah(el) {
            let value = el.value.replace(/[^0-9]/g, '');
            if (value) {
                el.value = new Intl.NumberFormat('id-ID', {
                    style: 'currency',
                    currency: 'IDR',
                    minimumFractionDigits: 0
                }).format(value);
            } else {
                el.value = '';
            }
        }
    </script>
</asp:Content>

