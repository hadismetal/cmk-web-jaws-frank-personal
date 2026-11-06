<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TGP.aspx.cs" Inherits="Page_Report_TGP" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    TGP
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    PG
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form runat="server" class="form-inline">
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
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="go" runat="server" class="btn btn-primary" onserverclick="fillTable">
                    <i class="icon-search"></i>
                    Display
                </button>
                <button id="export" runat="server" class="btn btn-primary" onserverclick="fillTableExcel">
                    <i class="icon-download-alt"></i>
                    Export
                </button>
            </div>
        </div>
    </form>
    <div class="table-responsive">
        <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="130">Tanggal</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">Jam</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">37.5 % Chain (White) Jual</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">37.5 % Chain (White) Beli</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">75.0% Chain (Yellow) Jual</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">75.0% Chain (Yellow) Beli</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">75.0% Chain (White) Jual</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">75.0% Chain (White) Beli</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">75.0% Non-Chain (Yellow) Jual</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">75.0% Non-Chain (Yellow) Beli</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">75.0% Non-Chain (White) Jual</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">75.0% Non-Chain (White) Beli</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">99.9% Non-Chain Jual</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">99.9% Non-Chain Beli</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">75.5% CZ Jewelry (Hala) Jual</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">75.5% CZ Jewelry (Hala) Beli</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">75.5% CZ Jewelry (Celeb) Jual</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">75.5% CZ Jewelry (Celeb) Beli</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">37.5% CZ Jewelry Jual</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">37.5% CZ Jewelry Beli</asp:TableHeaderCell>
				<asp:TableHeaderCell Width="140">PRELEI Jual</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">PRELEI Beli</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
</asp:Content>
