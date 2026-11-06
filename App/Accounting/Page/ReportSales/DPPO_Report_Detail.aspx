<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DPPO_Report_Detail.aspx.cs" Inherits="Page_Create_ReportSales_DPPO_Report_Detail" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Titipan Customer
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Detail
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style type="text/css">
        .form-group .select2-container {
            position: relative;
            z-index: 2;
            float: left;
            width: 100%;
            margin-bottom: 0;
            display: table;
            table-layout: fixed;
        }

        .popover-content {
            font-size: 12px;
        }

        table.table-list td {
            padding: 5px;
        }
    </style>
    <form id="Form1" runat="server" class="form-inline">

        <div runat="server" id="div_finished" visible="false">
            <div class="row">
                <div class="col-xs-12 col-sm-3 col-md-3">
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="panel panel-default">

                        <div class="panel-body">
                            <asp:Literal runat="server" ID="lit_info_invalid" />
                            <button id="redirect_summary" runat="server" style="width: 100%;" class="btn btn-default" onserverclick="redirect_report_Click" visible="false">
                                <span class="glyphicon glyphicon-arrow-left" aria-hidden="true"></span>
                                &nbsp;&nbsp;Back to Report DP
                            </button>

                        </div>

                    </div>
                </div>
                <div class="col-xs-12 col-sm-3 col-md-3">
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col-xs-12">
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
                                        <td class="text-label">Tgl Transaksi</td>
                                        <td>:</td>
                                        <td id="tgl" runat="server" />
                                    </tr>
                                    <tr>
                                        <td class="text-label">Type</td>
                                        <td>:</td>
                                        <td id="tipe_dp" runat="server" />
                                    </tr>

                                </table>
                            </div>
                            <br class="visible-sm visible-xs" />
                            <div class="col-md-5">
                                <table class="table-file">
                                    <tr>
                                        <td class="text-label">Customer</td>
                                        <td>:</td>
                                        <td id="customer" runat="server" />
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
                                        <td class="text-label">Keterangan</td>
                                        <td>:</td>
                                        <td id="keterangan" runat="server" />
                                    </tr>
									<tr id="rowgwp" runat="server">
                                        <td class="text-label text-danger">Free GWP</td>
                                        <td>:</td>
                                        <td id="freegwp" runat="server" />
                                    </tr>
                                    <tr>
                                        <td class="text-label">Estimasi Harga</td>
                                        <td>:</td>
                                        <td id="estimate" runat="server" />
                                    </tr>

                                </table>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div>
            <asp:Literal ID="lit_error" runat="server"></asp:Literal>
        </div>



        <div class="row">
            <div class="col-xs-12">
                <div class="panel panel-default" id="divrowpayment">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="file-subheader hidden-sm hidden-xs">
                                    Payment
                                </div>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-xs-12 col-sm-12 col-md-6">
                                <div class="row">
                                    <div class="col-xs-12">
                                        <div class="panel panel-default">
                                            <div class="panel-body">
                                                <div class="panel panel-default">
                                                    <div class="panel-body">

                                                        <div class="col-xs-12 col-sm-4">
                                                            <span class="text-muted">Total Amount DP</span><br />
                                                            <span class="text-md" style="color: green; font-weight: bold;" id="spn_payment_amountpaid" runat="server" />
                                                        </div>

                                                    </div>
                                                </div>

                                                <div class="table-responsive">
                                                    <asp:Table ID="tb3" runat="server" CssClass="table-list table-striped" Width="100%">
                                                        <asp:TableHeaderRow>
                                                            <asp:TableHeaderCell Width="30"></asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="300">Detail</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="150">Amount</asp:TableHeaderCell>
                                                        </asp:TableHeaderRow>
                                                    </asp:Table>
                                                </div>

                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <hr />
            <div class="col-xs-12">
                <div runat="server" id="div_print" visible="false">
                    <div style="padding-top: 15px;">
                        <button id="print" runat="server" class="btn btn-success" onserverclick="print_Click" disabled="disabled">
                            <span class="glyphicon glyphicon-print" aria-hidden="true"></span>
                            &nbsp;<span runat="server" id="txt_btn_print">Print</span>
                        </button>
						<button id="exportSPK" runat="server" class="btn btn-primary" onserverclick="exportSPK_ServerClick" disabled="disabled">
                            <span class="glyphicon glyphicon-download" aria-hidden="true"></span>
                            &nbsp;<span runat="server" id="Span1">Export SPK</span>
                        </button>
                    </div>
                </div>
            </div>



        </div>






    </form>
    <script type="text/javascript">

        $(document).ready(function () {
            $("select").select2({ width: '100%' });
        });

        $(function () {
            $('[data-toggle="popover"]').popover(html = true);
        });

        function UserConfirmation() {
            txtConfirm =
                "Lanjutkan Proses PEMBAYARAN ?\n\n" +
                "transaksi sifatnya adalah permanen.\n" +
                "Dianjurkan untuk melakukan backup terlebih dahulu.";
            if (confirm(txtConfirm))
                return true;
            else
                return false;
        }
    </script>
</asp:Content>

