<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sales_Data.aspx.cs" Inherits="Page_Register_Sales_Item" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Sales Order
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <form id="Form1" runat="server">
        <style type="text/css">
            table.table-list td {
                padding:5px;
            }
        </style>
        <div runat="server" id="div_finished" visible="false">
            <div class="row">
                <div class="col-xs-12 col-sm-3 col-md-3">
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="panel panel-default">
                        
                        <div class="panel-body">
                            <asp:Literal runat="server" ID="lit_info_invalid" />
                            <button id="redirect_payment" runat="server" style="width: 100%;" class="btn btn-default" onserverclick="redirect_payment_Click" visible="false">
                                <span class="glyphicon glyphicon-arrow-right" aria-hidden="true"></span>
                                &nbsp;&nbsp;Go to Payment Sales Order
                            </button>
                            <button id="redirect_register" runat="server" style="width: 100%;" class="btn btn-default" onserverclick="redirect_register_Click" visible="false">
                                <span class="glyphicon glyphicon-arrow-left" aria-hidden="true"></span>
                                &nbsp;&nbsp;Back to Register
                            </button>
                        </div>
                        
                    </div>
                </div>
                <div class="col-xs-12 col-sm-3 col-md-3">
                    
                </div>
            </div>

        </div>

        <div runat="server" id="div_unfinished" visible="false">
            <div class="table-responsive">
                <div class="row">
                    <div class="col-xs-12 col-sm-12">
                        <div class="panel panel-default">
                            <div class="panel-body">

                                <div class="file-subheader hidden-sm hidden-xs">
                                    Form
                                </div>

                                <div class="row">
                                    <div class="col-md-5">
                                        <table class="table-file">
                                            <tr>
                                                <td class="text-label">Brand</td>
                                                <td>:</td>
                                                <td id="brand" runat="server" />
                                            </tr>
                                            <tr>
                                                <td class="text-label">Location</td>
                                                <td>:</td>
                                                <td id="location" runat="server" />
                                            </tr>
                                            <tr>
                                                <td class="text-label">Operator</td>
                                                <td>:</td>
                                                <td id="operator_input" runat="server" />
                                            </tr>
                                            <tr>
                                                <td class="text-label">Transaction Date</td>
                                                <td>:</td>
                                                <td id="tgl" runat="server" />
                                            </tr>
                                        </table>
                                    </div>
                                    <br class="visible-sm visible-xs" />
                                    <div class="col-md-5">
                                        <table class="table-file" style="width:100%;">
                                            <tr>
                                                <td class="text-label">Customer</td>
                                                <td>:</td>
                                                <td id="customer" runat="server" />
                                            </tr>
											<tr>
                                                <td class="text-label">Given To</td>
                                                <td>:</td>
                                                <td id="Receiver" runat="server">
                                                    <label id="txtReceiver" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="text-label">Membership</td>
                                                <td>:</td>
                                                <td id="membership" runat="server" />
                                            </tr>
                                            <tr>
                                                <td class="text-label">JP / JR</td>
                                                <td>:</td>
                                                <td id="jpjr" runat="server" />
                                            </tr>
                                            <tr>
                                                <td class="text-label">
                                                    Description<br />
                                                    <div runat="server" id="div_btn_keterangan">
                                                        <div id="div_keterangan_button_1" style="display:block;">
                                                            <button onclick="updateDescription(); return false;" class="btn btn-warning btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span>&nbsp;Update</button>
                                                        </div>
                                                    </div>
                                                    
                                                </td>
                                                <td>:</td>
                                                <td>
                                                    <div id="div_keterangan_update_1" style="display:block;">
                                                        <span id="description" runat="server"></span>
                                                        
                                                    </div>
                                                    <div id="div_keterangan_update_2" style="display:none;">
                                                        
                                                        <asp:TextBox ID="keterangan" runat="server" CssClass="form-control" style="resize: none;" TextMode="MultiLine" Rows="3" />
                                                        
                                                    </div>
                                                    <div id="div_keterangan_button_2" style="display:none;">
                                                        <button onclick="updateDescription(); return false;" class="btn btn-danger btn-xs pull-left"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span>&nbsp;Cancel</button>
                                                        &nbsp;
                                                        <button id="update_keterangan" runat="server" class="btn btn-xs btn-success pull-right" onserverclick="update_keterangan_Click">
                                                            <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                                                            Update
                                                        </button>
                                                    </div>
                                                    <script type="text/javascript">
                                                        function updateDescription() {
                                                            var x = document.getElementById("div_keterangan_update_1");
                                                            if (x.style.display === "none") {
                                                                x.style.display = "block";
                                                            } else {
                                                                x.style.display = "none";
                                                            }
                                                            var x = document.getElementById("div_keterangan_update_2");
                                                            if (x.style.display === "none") {
                                                                x.style.display = "block";
                                                            } else {
                                                                x.style.display = "none";
                                                            }
                                                            var x = document.getElementById("div_keterangan_button_1");
                                                            if (x.style.display === "none") {
                                                                x.style.display = "block";
                                                            } else {
                                                                x.style.display = "none";
                                                            }
                                                            var x = document.getElementById("div_keterangan_button_2");
                                                            if (x.style.display === "none") {
                                                                x.style.display = "block";
                                                            } else {
                                                                x.style.display = "none";
                                                            }
                                                        } 
                                                    </script>
                                                </td>
                                            </tr>
											<tr id="trpromo" style="display:none" runat="server">
                                                <td class="text-label">Promo</td>
                                                <td>:</td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlPromo" Width="400">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="panel panel-default">
                            <div class="panel-body">

                                <div class="file-subheader hidden-sm hidden-xs">
                                    Item Sales
                                </div>

                                <div class="row">
                                    <div class="col-xs-12 col-sm-3">
                                        
                                        <div class="input-group">
                                            
                                            <asp:TextBox runat="server" ID="txt_plu" CssClass="form-control" placeholder="Search for..." />
                                            <span class="input-group-btn">
                                            
                                            <asp:LinkButton ID="btn_add_plu" CssClass="btn btn-success pull-right" runat="server" OnClick="add_plu_Click">
                                                <i class="icon-plus"></i>
                                                Add PLU
                                            </asp:LinkButton>
                                            </span>
											
                                        </div>
										<asp:Label runat="server" ID="lblwarningplu" CssClass="text-danger"></asp:Label>
                                    </div>
                                </div>
                                <div>
                                    <asp:Literal runat="server" ID="lit_test" />
                                </div>
                                <br />
                                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm" Style="width: 100%;">
                                    <asp:TableHeaderRow>
                                        <asp:TableHeaderCell RowSpan="2" Width="44"></asp:TableHeaderCell>
                                        <asp:TableHeaderCell RowSpan="2" Width="40">No.</asp:TableHeaderCell>
                                        <asp:TableHeaderCell RowSpan="2">PLU<br />&<br />Item</asp:TableHeaderCell>
                                        <asp:TableHeaderCell RowSpan="2" Width="120">Harga</asp:TableHeaderCell>
                                        <asp:TableHeaderCell RowSpan="2" Width="114">Discount Normal</asp:TableHeaderCell>
                                        <asp:TableHeaderCell RowSpan="2" Width="114">Discount Promo</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="114">Discount Membership
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell RowSpan="2" Width="114">Discount Voucher
                                            <asp:Literal runat="server" ID="lit_promo_membership" />
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell RowSpan="2" Width="114">Discount Program Bank</asp:TableHeaderCell>
                                        <asp:TableHeaderCell RowSpan="2" Width="114">Discount Other</asp:TableHeaderCell>
                                        <asp:TableHeaderCell RowSpan="2" Width="114">Discount Round
                                            <span data-toggle="tooltip" title="" data-placement="bottom" class="glyphicon glyphicon-info-sign" style="font-size:13px; color:gray; font-weight:normal;" data-original-title="Maksimal 1% dari harga awal"></span>
                                        </asp:TableHeaderCell>
                                        <asp:TableHeaderCell RowSpan="2" Width="114">Total Bayar</asp:TableHeaderCell>
                                    </asp:TableHeaderRow>
                                    <asp:TableHeaderRow>
                                        <asp:TableHeaderCell>
                                            <asp:Literal runat="server" ID="lit_membership" /></asp:TableHeaderCell>
                                    </asp:TableHeaderRow>
                                </asp:Table>
                                
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-12">
                        <asp:Literal runat="server" ID="lit_error_salesorder" />
                        <asp:Literal runat="server" ID="lit_error_resell" />
                        <asp:Literal runat="server" ID="lit_error_sap" />
                    </div>
                    
                </div>
            </div>

            <div class="button-inline" runat="server" id="div_review" visible="true">
                <button id="review" runat="server" class="btn btn-warning" onserverclick="review_Click">
                    <span class="glyphicon glyphicon-saved" aria-hidden="true"></span>
                    Review
                </button>

                <span id="next_err_voucher" runat="server" style="color: red;" visible="false">Total Discount Voucher belum 100% digunakan. Silahkan dicheck kembali pengalokasian nilai voucher hingga 100% digunakan.</span>
                <span id="next_err_nominal" runat="server" style="color: red;" visible="false">Terdapat Nilai Invoice dibawah 0 Rupiah. Silahkan gunakan discount lain-lain (diinput minus) untuk menambah nilai invoice.</span>
                <span id="next_err_1" runat="server" class="text-validator" visible="false">Terjadi Kesalahan Teknis #001</span>
                <span id="next_err_2" runat="server" class="text-validator" visible="false">Terjadi Kesalahan Teknis #002</span>
                <span id="next_err_3" runat="server" class="text-validator" visible="false">Terjadi Kesalahan Teknis #003</span>
                
            </div>

            <div runat="server" id="div_confirm" visible="false">
                <div class="alert alert-danger" role="alert">
                    <div class="row">
                        <div class="col-xs-12 col-sm-6" style="padding-bottom: 5px;">
                            This transaction is <b>permanent</b>. Please check again to make sure there is no mistake input.<br />
                            <b>Are you sure you want to proceed to Payment?</b>

                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <div class="col-xs-12" style="padding: 0 0 5px 0;">
                                <button id="cancel" runat="server" style="width: 100%;" class="btn btn-danger" onserverclick="cancel_Click">
                                    <span class="glyphicon glyphicon-remove" aria-hidden="true"></span>
                                    &nbsp;&nbsp;No, I'd like to revise my action.
                                </button>
                            </div>
                            <div class="col-xs-12" style="padding: 0 0 5px 0;">

                                <button id="save" runat="server" style="width: 100%;" class="btn btn-success" onserverclick="save_Click">
                                    <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                                    &nbsp;&nbsp;Yes, I'm sure. Please continue to Payment.
                                </button>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

    </form>
    <script type="text/javascript">
        $(function () {
            $('[data-toggle="tooltip"]').tooltip();
			/*$('#voucherdp').multiselect({
                includeSelectAllOption: true,
                enableCaseInsensitiveFiltering: true,
                numberDisplayed: 0,
                nonSelectedText: 'Select Facility',
                maxHeight: 350
                //includeSelectAllOption: true
            });*/
        })

        $(function () {
            $('[data-toggle="popover"]').popover(html = true);
        });

        function goDelete(fooid, fosi, footid) {
            txtConfirm =
				"Lanjutkan Proses DELETE ?\n\n" +
				"Penghapusan data / transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";

            if (confirm(txtConfirm))
                location.href = 'Sales_ItemDelete.aspx?id=' + fooid + '&itemindex=' + fosi + '&iid=' + footid;
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
    </script>
</asp:Content>

