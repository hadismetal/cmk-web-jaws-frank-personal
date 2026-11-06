<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PG_Detail.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Plain Gold Detail
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <script>
        $(document).ready(function () {
            $('#<%=datepicker.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
            $('#<%=datepicker2.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
        });
    </script>
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
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="Input PLU" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Periode :
                </div>

                <asp:TextBox CssClass="form-control" type="text" placeholder="Dari" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" />

            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox CssClass="form-control" type="text" placeholder="Sampai" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" />
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
                <asp:DropDownList ID="model" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Model</asp:ListItem>
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
                    Supplier :
                </div>
                <asp:DropDownList ID="supplier" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Supplier</asp:ListItem>
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
                    <asp:ListItem Value="3">Model</asp:ListItem>
                    <asp:ListItem Value="4">Product Item</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Consignment :
                </div>
                <asp:DropDownList ID="ddl_consignment" runat="server" CssClass="form-control" Width="175">
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
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Export">
                        <i class="icon-download-alt"></i>
                        Export
                    </button>
                </div>
            </div>
        </form>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="100">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Kode Barang</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Consignment</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Product Level</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Model</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Kadar Logam</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Kadar Tukaran</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Frame Color</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Supplier</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Gold Rate</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Ongkos</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">HPPM</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Supplier SJ</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

