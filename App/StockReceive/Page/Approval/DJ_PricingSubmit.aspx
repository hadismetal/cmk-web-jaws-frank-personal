<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="DJ_PricingSubmit.aspx.cs" Inherits="Page_Approval_DJ_PricingSubmit" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewelry
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Approval
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <!-- Add inside your head or CSS file -->
    <style>
        .switch {
            position: relative;
            display: inline-block;
            width: 50px;
            height: 24px;
        }

            .switch input {
                display: none;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            background-color: #ccc;
            transition: .4s;
            height: 100%;
            width: 100%;
            border-radius: 24px;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 18px;
                width: 18px;
                left: 3px;
                bottom: 3px;
                background-color: white;
                transition: .4s;
                border-radius: 50%;
            }

        input:checked + .slider {
            background-color: #5cb85c; /* Bootstrap 3 success green */
        }

            input:checked + .slider:before {
                transform: translateX(26px);
            }
    </style>
    <form runat="server">
        <p>
            <span class="text-muted">Nomor :</span><br />
            <span class="text-lg" id="header_nomor" runat="server" />
        </p>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            frame
        </div>
        <table class="table-file">
            <tr>
                <td class="text-label">Gold Rate</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150" id="goldrate" runat="server" />
                <td><small class="text-silver">USD</small></td>
                <td id="efektifgold" runat="server" />
            </tr>
            <tr>
                <td class="text-label">Netto Weight</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150" id="netto" runat="server" />
                <td><small class="text-silver">Gram</small></td>
            </tr>
            <tr>
                <td class="text-label">Kadar Tukaran</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150" id="kadar" runat="server" />
                <td><small class="text-silver">%</small></td>
            </tr>
            <tr>
                <td class="text-label">Total</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150" id="totalproduct" runat="server" />
                <td><small class="text-silver">USD</small></td>
            </tr>
        </table>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            stone
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-report">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Stone</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tgl.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-right"><small>USD</small> Harga Satuan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-center">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-center">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-right"><small>USD</small> Harga Total</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            product summary
        </div>
        <div class="row">
            <div class="col-md-3">
                <p>
                    <span class="text-muted">Frame :</span><br />
                    <span class="text-lg" id="sum_frame" runat="server" />
                    <small class="text-silver">USD</small>
                </p>
            </div>
            <div class="col-md-3">
                <p>
                    <span class="text-muted">Stone :</span><br />
                    <span class="text-lg" id="sum_stone" runat="server" />
                    <small class="text-silver">USD</small>
                </p>
            </div>
            <div class="col-md-3">
                <p>
                    <span class="text-muted">Total :</span><br />
                    <span class="text-lg" id="sum_total" runat="server" />
                    <small class="text-silver">USD</small>
                </p>
            </div>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            mounting fee
        </div>
        <div class="row">
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Brand</td>
                        <td>:</td>
                        <td id="brand1" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Product Item</td>
                        <td>:</td>
                        <td id="item1" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Cons. Process</td>
                        <td>:</td>
                        <td id="cons1" runat="server" />
                    </tr>
                </table>
            </div>
            <br class="visible-sm visible-xs" />
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Biaya NO</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-sm" id="no1" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                    </tr>
                    <tr>
                        <td class="text-label">Biaya RO</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-sm" id="ro1" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                    </tr>
                    <tr>
                        <td class="text-label">Total Biaya</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-bold text-sm" id="total1" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                        <td id="efektifmounting" runat="server" />
                    </tr>
                </table>
            </div>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            finishing fee
        </div>
        <div class="table-responsive">
            <asp:Table ID="tbff" runat="server" CssClass="table-report">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Finishing</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tgl.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180" CssClass="text-right"><small>IDR</small> Biaya</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            fee summary
        </div>
        <div class="row">
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Mounting</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-sm" id="fee10" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                        <td id="fee11" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Pasang Batu</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-sm" id="fee20" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                        <td id="fee21" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Finishing</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-sm" id="fee30" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                    </tr>
                    <tr>
                        <td class="text-label">Ongkos Lain-lain</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-sm" id="fee31" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                    </tr>
                    <tr>
                        <td class="text-label">Total Biaya</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-bold text-sm" id="feerupiah" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                    </tr>
                </table>
            </div>
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">USD Rate</td>
                        <td>:</td>
                        <td class="fixed-180 text-right" id="feerate" runat="server" />
                        <td class="text-silver"></td>
                        <td id="efektifrate" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Total Biaya</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-bold" id="feetotal" runat="server" />
                        <td class="text-silver"><small>USD</small></td>
                    </tr>
                </table>
            </div>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            mark-up
        </div>
        <div class="row">
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Category</td>
                        <td>:</td>
                        <td id="cat1" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Level</td>
                        <td>:</td>
                        <td id="lvl1" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Stone Dist.</td>
                        <td>:</td>
                        <td id="dist1" runat="server" />
                    </tr>
                </table>
                <br />
                <table class="table-file">
                    <tr>
                        <td class="text-label">Base Price (USD)</td>
                        <td>:</td>
                        <td class="fixed-180 text-right" id="modal" runat="server" />
                        <td class="text-silver"><small>USD</small></td>
                    </tr>
                    <%--tr>
					<td class="text-label">USD Rate</td>
					<td>:</td>
					<td class="fixed-180 text-right" id="modalrate" runat="server" />
					<td class="text-silver"><small>IDR</small></td>
					<td id="murate" runat="server" />
				</--%>
                    <tr>
                        <td class="text-label">Rate Jual</td>
                        <td>:</td>
                        <td class="fixed-180 text-right" id="modalrate" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                        <td id="murate" runat="server" />
                    </tr>
                    <%--tr>
					<td class="text-label">Base Price (IDR)</td>
					<td>:</td>
					<td class="fixed-180 text-right text-bold" id="modalrupiah" runat="server" />
					<td class="text-silver"><small>IDR</small></td>
				</--%>
                    <tr id="unitbox" runat="server">
                        <td class="text-label">Unit Price (IDR)</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-bold" id="unitprice" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                    </tr>
                </table>
            </div>
            <br class="visible-sm visible-xs" />
            <div class="col-md-5">
                <p>
                    <span class="text-xl" id="mu_persen" runat="server" />
                    <small class="text-silver">%</small>
                </p>
                <br />
                <p>
                    <span class="text-muted">Final Price (USD) :</span><br />
                    <span class="text-lg" id="hargausd" runat="server" />
                    <small class="text-silver">USD</small><br />

                </p>
                <br />
                <p>
                    <span class="text-muted">Final Price (IDR) :</span><br />
                    <span class="text-lg text-orange" id="hargaidr" runat="server" />
                    <small class="text-silver">IDR</small><br />
                </p>
            </div>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            segmen
        </div>
        <div class="row">
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Category</td>
                        <td>:</td>
                        <td id="cat2" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Level</td>
                        <td>:</td>
                        <td id="lvl2" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Stone Dist.</td>
                        <td>:</td>
                        <td id="dist2" runat="server" />
                    </tr>
                </table>
                <br />
                <table class="table-file">
                    <tr>
                        <td class="text-label">Segmen Sistem</td>
                        <td>:</td>
                        <td id="segmensistem" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Minimum</td>
                        <td>:</td>
                        <td id="minimum" runat="server" />
                        <td id="efektifsegmen" runat="server" />
                    </tr>
                </table>
                <br />
                <table class="table-file">
                    <tr>
                        <td class="text-label">Carat</td>
                        <td>:</td>
                        <td id="carat" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Harga (USD)</td>
                        <td>:</td>
                        <td id="hargasegmen" runat="server" />
                    </tr>
                </table>
            </div>
            <br class="visible-sm visible-xs" />
            <div class="col-md-5">
                <p>
                    <span class="text-xl" id="segmen" runat="server" />
                </p>
                <br />
            </div>
        </div>
        <div runat="server" id="div_one_price">
            <hr />
            <div class="file-subheader hidden-sm hidden-xs">
                one price
            </div>
            <div class="row">
                <label class="col-md-12" style="color: darkcyan; font-family: 'Lucida Console'; font-weight: 700">SWITCH TOMBOL DI BAWAH INI JIKA INGIN MENGIKUTI HARGA PROMO ONE PRICE YANG AKTIF.</label>
                <div class="col-md-12">
                    <!-- HTML Switch -->
                    <label class="switch">
                        <input type="hidden" id="idpromo" runat="server" />
                        <input type="checkbox" id="ischoosepromo" runat="server" />
                        <span class="slider" id="switch_button"></span>
                        <script>
                            $('#content_ischoosepromo').change(function () {
                                var isChoosePromo = $('#content_ischoosepromo').prop('checked');
                                console.log(isChoosePromo);
                            });
                        </script>
                    </label>
                </div>
                <div class="col-md-5">
                    <table class="table-file">
                        <tr>
                            <td class="text-label">Nama Promo</td>
                            <td>:</td>
                            <td id="nama_promo" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Keterangan</td>
                            <td>:</td>
                            <td id="keterangan_promo" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Tgl Start.</td>
                            <td>:</td>
                            <td id="tgl_start" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Tgl Expire.</td>
                            <td>:</td>
                            <td id="tgl_end" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Modifikasi</td>
                            <td>:</td>
                            <td id="modipikasi" runat="server" />
                        </tr>
                    </table>
                </div>
                <div class="col-md-5">
                    <table class="table-file">
                        <tr>
                            <td class="text-label">Tipe Promo</td>
                            <td>:</td>
                            <td id="tipe_promo" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Harga Promo</td>
                            <td>:</td>
                            <td id="harga_promo" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Ring Size</td>
                            <td>:</td>
                            <td id="ring_size" runat="server" />
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <hr />
        <div class="row">
            <div class="col-xs-12 col-sm-12">
                <asp:Literal runat="server" ID="lit_error_msg" />
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-2">
                <button id="btnApprove" runat="server" class="btn btn-success" onserverclick="approve_Click">
                    <i class="icon-ok"></i>
                    &nbsp;Approve
                </button>
                &nbsp;
                <button id="btnFix" runat="server" class="btn btn-danger" onserverclick="fix_Click">
                    <i class="icon-wrench"></i>
                    &nbsp;Fix
                </button>
                &nbsp;
                <button id="btnPrint" runat="server" class="btn btn-info" onserverclick="print_Click">
                    <i class="icon-print"></i>
                    &nbsp;Print
                </button>
            </div>


        </div>
    </form>
</asp:Content>

