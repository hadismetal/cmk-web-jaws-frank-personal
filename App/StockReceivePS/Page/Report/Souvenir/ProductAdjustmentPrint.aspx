<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="ProductAdjustmentPrint.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="table-responsive">
            <table>
                <tr>
                    <td width="150">Brand</td>
                    <td>:</td>
                    <td id="brand" runat="server"></td>
                </tr>
            </table>
        </form>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">No. Dokumen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Tipe Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Souvenir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Kode</asp:TableHeaderCell>
                    
                    <asp:TableHeaderCell Width="100">Adjustment Type</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Keterangan</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

