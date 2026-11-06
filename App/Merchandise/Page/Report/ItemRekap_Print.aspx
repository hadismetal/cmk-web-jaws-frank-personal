<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="ItemRekap_Print.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">H. Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">NetWeight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Carat</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <asp:Literal ID="paging_text" runat="server" />
    </div>
</asp:Content>

