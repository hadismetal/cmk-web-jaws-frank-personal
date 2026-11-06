<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_Rekap.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery Rekap
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Group By
                </div>
                <asp:DropDownList ID="groupby" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="1">Product Item</asp:ListItem>
                    <asp:ListItem Value="2">Product Level</asp:ListItem>
                    <asp:ListItem Value="3">Product Category</asp:ListItem>
                    <asp:ListItem Value="4">Segmen</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
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
                    Substorage
                </div>
                <asp:DropDownList ID="substorage" runat="server" CssClass="form-control" Width="175">
                    <%--<asp:ListItem Value="0">Substorage</asp:ListItem>
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
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                        <i class="icon-search"></i>
                        Display
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
        </form>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableRow>
                    <asp:TableHeaderCell Width="140">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Stock</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="300">Harga</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>

            
        </div>

    </div>
</asp:Content>

