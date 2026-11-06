<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sales_DataSO.aspx.cs" Inherits="Page_Create_Packaging_Sales_DataSO" %>

<%@ Register Src="MenuSales.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Packaging
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <hr />
    <form runat="server" class="form-inline">
        <div class="form-group">
            <div class="text-muted">
                Pilih Tipe Transaksi
            </div>
            <asp:DropDownList runat="server" ID="ddltipe" class="form-control">
                <asp:ListItem Value="SO">Sales Order / Souvenir</asp:ListItem>
                <asp:ListItem Value="DP">Down Payment</asp:ListItem>
                <asp:ListItem Value="RP">Repair</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Search :
            </div>
            <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input SO Number..." />
        </div>
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                    <i class="icon-search"></i>
                    Display
                </button>
                <a id="cancel" runat="server" class="btn btn-default" href="Sales_Form.aspx">
                    <i class="icon-remove"></i>
                    Cancel
                </a>
            </div>
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90" />
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">No. Sales Order</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Tgl SO</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Customer Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Location</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Total Bayar</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Sales Name</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </form>
</asp:Content>