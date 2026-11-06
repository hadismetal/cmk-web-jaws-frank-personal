<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="StockAuditLD_DetailExcel.aspx.cs" Inherits="Page_LD" %>

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
                        <asp:TableHeaderCell Width="150">Kode Karet</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">Notes</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Info</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Operator</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Tanggal</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>

