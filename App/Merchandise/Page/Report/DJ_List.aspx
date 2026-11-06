<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_List.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery List
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Cabang :
                </div>
                <asp:DropDownList ID="cabang" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Cabang :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <%--<div class="form-group">
                <div class="text-muted">
                    Brand :
                </div>
                <asp:DropDownList ID="brand" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Brand</asp:ListItem>
                </asp:DropDownList>
                    
            </div>--%>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="Input PLU" />
            </div>
            <%--div class="form-group">
                <div class="text-muted">
                    Periode :
                </div>

                <asp:TextBox CssClass="form-control" type="text" placeholder="Dari" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" />

            </>
            <div--% class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox CssClass="form-control" type="text" placeholder="Sampai" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" />
            </div--%>
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
                    &nbsp;
                </div>
                <asp:DropDownList ID="productsegmen" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Segmen</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Order By :
                </div>
                <asp:DropDownList ID="orderby" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="1">Cabang</asp:ListItem>
                    <asp:ListItem Value="2">Product Level</asp:ListItem>
                    <asp:ListItem Value="3">Product Category</asp:ListItem>
                    <asp:ListItem Value="4">Product Item</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Substorage
                </div>
                <asp:DropDownList ID="substorage" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Substorage</asp:ListItem>
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
                    <asp:ListItem Value="99">Lost / Trouble</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                        <i class="icon-search"></i>
                        Display
                    </button>
                    <button id="export" runat="server" class="btn btn-primary" onserverclick="Export">
                        <i class="icon-download-alt"></i>
                        Export
                    </button>
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Print">
                        <i class="icon-print"></i>
                        Print
                    </button>
                </div>
            </div>
        </form>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">H. Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Stone Distribution</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Umur (Hari)</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <asp:Literal ID="paging_text" runat="server" />
    </div>
</asp:Content>

