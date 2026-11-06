<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SummaryStock.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Summary
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Location Type :
                </div>
                <asp:DropDownList ID="locationtype" runat="server" CssClass="form-control" Width="200" AutoPostBack="true" OnSelectedIndexChanged="getLokasi">
                    <asp:ListItem Value="3">Location Type</asp:ListItem>
                    <asp:ListItem Value="0">Warehouse</asp:ListItem>
                    <asp:ListItem Value="1">Outlet</asp:ListItem>
                    <asp:ListItem Value="2">Exhibition</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Location :
                </div>
                <asp:DropDownList ID="location" runat="server" CssClass="form-control" Width="200">
                    <asp:ListItem Value="3">Location</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Characteristic Product :
                </div>
                <asp:DropDownList ID="productlevel" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Level</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="productcategory" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Category</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="productitem" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Item</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Brand :
                </div>
                <asp:DropDownList ID="brand" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Brand</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Substorage
                </div>
                <asp:DropDownList ID="substorage" runat="server" CssClass="form-control" Width="175">
                    <%--<asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Jual</asp:ListItem>
                    <asp:ListItem Value="2">Resell</asp:ListItem>
                    <asp:ListItem Value="3">Trade In</asp:ListItem>
                    <asp:ListItem Value="4">Titipan Customer</asp:ListItem>
                    <asp:ListItem Value="5">Reparasi</asp:ListItem>
                    <asp:ListItem Value="6">Lebur</asp:ListItem>
                    <asp:ListItem Value="95">Ongoing Lebur</asp:ListItem>
                    <asp:ListItem Value="96">Broken</asp:ListItem>
                    <asp:ListItem Value="97">Outgoing - New / Package</asp:ListItem>
                    <asp:ListItem Value="98">Outgoing - Approved / On The Way</asp:ListItem>
                    <asp:ListItem Value="99">Lost / Trouble</asp:ListItem>--%>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Consignment :
                </div>
                <asp:DropDownList ID="consignment" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="2">All</asp:ListItem>
                    <asp:ListItem Value="1">Consignment</asp:ListItem>
                    <asp:ListItem Value="0">Non-Consignment</asp:ListItem>
                </asp:DropDownList>
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
            <asp:Table ID="tb" runat="server" CssClass="table-list" Visible="false">
                <asp:TableRow>
                    <asp:TableHeaderCell Width="120">Tipe Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Stock</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">R</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
            <asp:Table ID="tb2" runat="server" CssClass="table-list" Visible="false">
                <asp:TableRow>
                    <asp:TableHeaderCell Width="140">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Stock</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">R</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
            <asp:Table ID="tb3" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="180" CssClass="text-center">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-center">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass="text-center">M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass="text-center">R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180" CssClass="text-center">T. Harga Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" CssClass="text-center">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" CssClass="text-center">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" CssClass="text-center">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" CssClass="text-center">Total Butir</asp:TableHeaderCell>

                </asp:TableHeaderRow>
            </asp:Table>

            <asp:Table ID="tb4" runat="server" CssClass="table-list text-sm" Visible="false">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="150" CssClass="text-center">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Titipan Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Reparasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Lebur</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Hadiah</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Ongoing Lebur</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Broken</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Outgoing - Draft</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Outgoing - In Transit</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Lost / Trouble</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Total</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

