<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DPPO_Report.aspx.cs" Inherits="Page_ReportSales_DPPO" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Titipan Customer
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style type="text/css">
        .tbl-payment tr td {
            border: none;
            padding: 0;
            margin: 0;
        }
    </style>
    <div id="zero" runat="server" visible="false">
        <i class="icon-ok icon-5x pull-left text-success"></i>
        <h4>Proses Selesai</h4>
        <p class="text-muted">
            Tidak ada transaksi penjualan
        </p>
        <div class="clearfix"></div>
        <hr />
    </div>
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">

            <div class="row">
                <div class="col-xs-12">
                    <div class="form-group">
                        <div class="text-muted">
                            Location
                        </div>
                        <asp:DropDownList ID="ddl_location" runat="server" CssClass="select2">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Search
                            <span data-toggle="tooltip" title="" data-placement="top" class="glyphicon glyphicon-info-sign" style="font-size:13px; color:gray; font-weight:normal;" data-original-title="Nomor PO / Nama Customer"></span>
                        </div>
                        <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="200" placeholder="..." />
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Filter by date
                        </div>
                        <asp:DropDownList ID="filterDate" runat="server" CssClass="form-control" Width="170">
                            <asp:ListItem Value="0">Down Payment</asp:ListItem>
                            <asp:ListItem Value="1">Repayment</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            From
                        </div>
                        <asp:TextBox ID="dari" runat="server" CssClass="form-control text-center" Width="110" placeholder="Dari" />
                        <script>
                            $(document).ready(function () { $('#<%=dari.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                        </script>

                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            To
                            <span data-toggle="tooltip" title="" data-placement="top" class="glyphicon glyphicon-info-sign" style="font-size:13px; color:gray; font-weight:normal;" data-original-title="Max. 31 days"></span>
                        </div>
                        <asp:TextBox ID="sampai" runat="server" CssClass="form-control text-center" Width="110" placeholder="Sampai" />
                        <script>
                            $(document).ready(function () { $('#<%=sampai.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                        </script>

                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Status
                        </div>
                        <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="170">
                            <asp:ListItem Value="0">Draft</asp:ListItem>
                            <asp:ListItem Value="1">Active</asp:ListItem>
                            <asp:ListItem Value="2">Used</asp:ListItem>
                            <asp:ListItem Value="3">Void</asp:ListItem>
                            <asp:ListItem Value="9">Active / Used / Void</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="hidden-xs">&nbsp;</div>
                        <div>
                            <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                                <i class="icon-search"></i>
                                Display
                            </button>

                            <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Export_Filter" disabled="disabled">
                                <i class="icon-download-alt"></i>
                                Excel by Filter
                            </button>
                            <button id="Button2" runat="server" class="btn btn-primary" onserverclick="Export_All" disabled="disabled">
                                <i class="icon-download-alt"></i>
                                Excel All
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <br />
                    <asp:Literal runat="server" ID="lit_err" />
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <div class="table-responsive">
                        <asp:Table ID="tb" runat="server" CssClass="table-list text-sm" Style="width: 100%;">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell Width="70"></asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="130">No. Dokumen<div style='clear:both; border-bottom:solid 1px silver;'></div>Tanggal</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="175">Lokasi<div style='clear:both; border-bottom:solid 1px silver;'></div>Sales</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="175">Customer<div style='clear:both; border-bottom:solid 1px silver;'></div>Kode</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="130">Sales Order</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="100">Total DP<div style='clear:both; border-bottom:solid 1px silver;'></div>Estimasi</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="200">Payment</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Keterangan</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <div class="table-responsive">
                        <asp:Table ID="tb2" runat="server" CssClass="table-list text-sm" Style="width: 100%;" Visible ="false">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell Width="70"></asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="130">No. Dokumen<div style='clear:both; border-bottom:solid 1px silver;'></div>Tanggal</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="175">Lokasi<div style='clear:both; border-bottom:solid 1px silver;'></div>Sales</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="175">Customer<div style='clear:both; border-bottom:solid 1px silver;'></div>Kode</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="130">Sales Order</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="100">Total DP<div style='clear:both; border-bottom:solid 1px silver;'></div>Estimasi</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="200">Payment</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Keterangan</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </div>
                </div>
            </div>
        </form>


    </div>

    <script type="text/javascript">

        $(document).ready(function () {
            $(".select2").select2({ width: '300px' });
        });

        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })


    </script>
</asp:Content>

