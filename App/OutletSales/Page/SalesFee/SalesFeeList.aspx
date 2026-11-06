<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SalesFeeList.aspx.cs" Inherits="Page_SalesFee" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Sales Fee Summary
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div>
                    Tahun :
                </div>

                <asp:TextBox ID="tahun" runat="server" CssClass="form-control text-bold" MaxLength="4" Width="100" />
            </div>
            <div class="form-group">
                <div>
                    Bulan :
                </div>

                <asp:TextBox ID="bulan" runat="server" CssClass="form-control text-bold" MaxLength="2" Width="100" />
                
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
        </form>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30">No</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Tahun</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Bulan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Total Poin</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Full</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">AE</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">DJ</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">PG</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">LM</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Pacing 1</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Pacing 2</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Pacing 3</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Pacing 4</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Inc 1</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Inc 2</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Inc 3</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Inc 4</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Kom 1</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Kom 2</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

