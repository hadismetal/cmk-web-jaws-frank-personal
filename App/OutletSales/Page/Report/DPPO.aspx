<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DPPO.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Titipan Customer PO
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
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
            <div class="form-group">
                <div class="text-muted">
                    Search :
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="dari" runat="server" CssClass="form-control text-center" Width="110" placeholder="Dari" />
                <script>
                    $(document).ready(function () { $('#<%=dari.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>

            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
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
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Status</asp:ListItem>
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="2">Used</asp:ListItem>
                    <asp:ListItem Value="3">Void</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                        <i class="icon-search"></i>
                        Display
                    </button>
                    <button id="Button2" runat="server" class="btn btn-primary" onserverclick="Export">
                        <i class="icon-search"></i>
                        Export
                    </button>
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Print">
                        <i class="icon-search"></i>
                        Print
                    </button>
                </div>
            </div>
        </form>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="200"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. Dokumen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Sales/JR</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Sales Order</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total DP</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Estimasi Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Persentase</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Cash / Tunai</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Kartu Kredit</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Transfer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Debit</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Voucher</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Exchange</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Poin</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">DP PO</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Bank</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:Content>

