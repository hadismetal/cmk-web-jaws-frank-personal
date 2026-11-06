<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="Summary_Print_bak.aspx.cs" Inherits="Page_Summary" %>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb3" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="180" CssClass="text-center">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-center">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180" CssClass="text-center">T. Harga Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" CssClass="text-center">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" CssClass="text-center">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" CssClass="text-center">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" CssClass="text-center">Total Butir</asp:TableHeaderCell>

                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

