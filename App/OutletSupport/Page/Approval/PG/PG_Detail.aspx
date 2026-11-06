<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PG_Detail.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Promo Detail 
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableRow>
                    <asp:TableHeaderCell Width="30">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Model</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Level</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. PLU</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <form runat="server">
            <a id="go" runat="server" class="btn btn-primary">
                <i class="icon-search"></i>
                Back
            </a>
        </form>
    </div>
</asp:Content>

