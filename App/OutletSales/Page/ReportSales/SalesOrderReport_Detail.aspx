<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SalesOrderReport_Detail.aspx.cs" Inherits="SalesOrderReport_Detail" %>

<%@ Register Src="~/UC/PaymentSummary.ascx" TagName="paymentSummary" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Sales Order Detail
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
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
    
    <form id="Form1" runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-4">
                <p>
                    <span class="text-muted">Sales Order</span><br />
                    <span class="text-lg" id="header_nomor" runat="server" />
                </p>
            </div>
            <div class="col-xs-12 col-sm-4" runat="server" id="div_tradein" visible="false">
                <p>
                    <span class="text-muted">TradeIn</span><br />
                    <span class="text-lg" id="header_tradein" runat="server" />
                </p>
            </div>
           
        </div>
        <hr />
        <div class="row" runat="server" id="div_void" visible="false">
            <div class="col-xs-12 col-sm-2 col-md-2">
            </div>
            <div class="col-xs-12 col-sm-8 col-md-8">
                <asp:Literal runat="server" ID="lit_info_invalid" />
            </div>
            <div class="col-xs-12 col-sm-2 col-md-2">
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

                                </table>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12" style="width: 115%;">
                <div class="panel panel-default">
                    <div class="panel-body">

                        <div class="file-subheader hidden-sm hidden-xs">
                            Data
                        </div>

                        <div class="table-responsive">
                            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                                <asp:TableHeaderRow>
                                    <asp:TableHeaderCell Width="52"></asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="160">No. Invoice<br />PLU & Item</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="150">Harga</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="100">Discount Normal</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="100">Discount Promo</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="100">Discount Member</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="150">Discount Voucher</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="100">Discount Bank</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="100">Discount Other</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="100">Discount Round</asp:TableHeaderCell>
									<asp:TableHeaderCell Width="100">Fee Extra Ring Size</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="150">Total Bayar</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                            </asp:Table>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
               <uc1:paymentSummary ID="paymentSummary" runat="server" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="file-subheader hidden-sm hidden-xs">
                                    Payment
                                </div>
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-xs-12">
                                <div class="table-responsive">
                                    <asp:Table ID="tb3" runat="server" CssClass="table-list table-striped" Width="100%">
                                        <asp:TableHeaderRow>
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
      

    <hr />

        <div class="button-left">
            <button id="invoice" runat="server" class="btn btn-success" onserverclick="PrintSO_Click">
                <i class="icon-print"></i>
                Print Sales Order
            </button>
            <asp:Label id="lit_error" runat="server" style="color: red;" />
            <button id="booking" runat="server" class="btn btn-success" onserverclick="PrintBooking_Click">
                <i class="icon-print"></i>
                Print DP
            </button>
        </div>
    </form>
    
    <script type="text/javascript">

        $(function () {
            $('[data-toggle="popover"]').popover(html = true);
        });


    </script>
</asp:Content>

