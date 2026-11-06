<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PointRewardDJ.aspx.cs" Inherits="Page_Update_MU" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Point Reward Diamond Jewellery
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Update
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form runat="server">
        <a id="add" runat="server" class="btn btn-success" target="_blank" href="PointRewardDJAdd.aspx">
            <i class="icon-plus"></i>
            Add Item
        </a>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    
                    <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" />
                    <asp:TableHeaderCell Width="130" CssClass="text-right">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-right">Point Reward</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </form>
</asp:Content>

