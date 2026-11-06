<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MemberTiering.aspx.cs" Inherits="Page_Update_MU" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Member Tiering
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Update
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    
                    <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" />
                    <asp:TableHeaderCell Width="130" CssClass="text-right">Tipe Member</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-right">Purchase <span class="text-sm text-silver">IDR</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-right">Discount <span class="text-sm text-silver">%</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-right">Birthday Surprise <span class="text-sm text-silver">IDR</span></asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </form>
</asp:Content>

