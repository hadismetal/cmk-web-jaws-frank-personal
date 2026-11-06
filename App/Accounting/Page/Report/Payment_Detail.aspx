<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Payment_Detail.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Payment
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
    <form id="Form1" runat="server" class="form-inline">

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
                Tipe Lokasi
            </div>
            <asp:DropDownList ID="tipelokasi" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasi" AutoPostBack="true">
                <asp:ListItem Value="-1">Tipe</asp:ListItem>
                <asp:ListItem Value="1">Outlet</asp:ListItem>
                <asp:ListItem Value="2">Exhibition</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Lokasi
            </div>
            <asp:DropDownList ID="lokasi" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Lokasi</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="clearfix">
            <br class="hidden-xs" />
        </div>
        <div class="form-group">
            <div class="text-muted">
                EDC
            </div>
            <asp:DropDownList ID="edc" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">EDC</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Bank Issuer
            </div>
            <asp:DropDownList ID="bank" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Bank Issuer</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Card Type
            </div>
            <asp:DropDownList ID="card" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Card Type</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Program
            </div>
            <asp:DropDownList ID="program" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Program</asp:ListItem>
            </asp:DropDownList>
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
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Print">
                    <i class="icon-print"></i>
                    Print
                </button>
            </div>
        </div>
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="btnExport" runat="server" class="btn btn-primary" onserverclick="Export">
                    <i class="icon-print"></i>
                    Export
                </button>
            </div>
        </div>
    </form>
    <div id="form_area" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="130">Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180">Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">EDC</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Bank Issuer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">CC Number</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">CC Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">CC Type</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">MDR</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

