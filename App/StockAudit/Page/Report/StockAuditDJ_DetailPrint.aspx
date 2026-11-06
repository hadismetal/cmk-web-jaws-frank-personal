<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="StockAuditDJ_DetailPrint.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            
            <span class="text-lg" id="title" runat="server"></span><br />
            <span class="text-lg" id="title2" runat="server"></span><br />

            <span id="title3" runat="server"></span><br />

            <span class="text-lg" id="title4" runat="server"></span><br />

            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">No. Meja</asp:TableHeaderCell>
                        
                        <asp:TableHeaderCell Width="150">PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Kode Karet</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Notes</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>

