<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StockAuditPG_DetailList.aspx.cs" Inherits="Page_PG" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Plain Gold Stock Audit Detail
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            
            <span class="text-lg" id="title" runat="server"></span><br />
            <span class="text-lg" id="title2" runat="server"></span><br />

            <span id="title3" runat="server"></span><br />

            <span class="text-lg" id="title4" runat="server"></span><br />

            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="Export">
                        <i class="icon-search"></i>
                        Export
                    </button>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Print">
                        <i class="icon-search"></i>
                        Print
                    </button>
                </div>
            </div>

            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="120">No. Meja</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Notes</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>

