<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sales_DataAgent_CrossBrand.aspx.cs" Inherits="Page_Create_Sales_Sales_DataAgent_CrossBrand" %>

<%@ Register Src="MenuSales.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Sales Order
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <asp:Literal ID="idbrand" runat="server" Visible="false" />
    <hr />
    <form runat="server" class="form-inline">
        <div class="form-group">
            <div class="text-muted">
                Brand :
            </div>
            <asp:DropDownList ID="brand" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="brand_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="form-group" runat="server" id="divlocation" visible="false">
            <div class="text-muted">
                Location :
            </div>
            <asp:DropDownList ID="location" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="location_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="table-responsive" runat="server" id="divjr" visible="false">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="100" />
                    <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">JPJR</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="300">Location</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass="text-center">Brand</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </form>
</asp:Content>
