<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery Stock Return
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
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
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Dari
                </div>

                <asp:TextBox CssClass="form-control text-center" type="text" placeholder="Dari" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" Width="110" />

            </div>
            <div class="form-group">
                <div class="text-muted">
                    Sampai
                </div>
                <asp:TextBox CssClass="form-control text-center" type="text" placeholder="Sampai" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" Width="110" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="Input PLU" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Stock Keeper
                </div>
                <asp:DropDownList ID="sk" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Stock Keeper :</asp:ListItem>
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
                    Supplier Asal:
                </div>
                <asp:DropDownList ID="supplier" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="">Supplier</asp:ListItem>
                </asp:DropDownList>
                    
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Supplier Tujuan:
                </div>
                <asp:DropDownList ID="suppliertujuan" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="">Supplier</asp:ListItem>
                </asp:DropDownList>
                    
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                        <i class="icon-search"></i>
                        Display
                    </button>
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Print">
                        <i class="icon-search"></i>
                        Print
                    </button>
                    <button id="Button2" runat="server" class="btn btn-primary" onserverclick="Export">
                        <i class="icon-search"></i>
                        Export
                    </button>
                </div>
            </div>
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">No. Dokumen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="90">Tanggal Input</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="90">Tanggal Approve</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="140">Nama Supplier</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Product Category</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Segmen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="90">Qty</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Gross Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Frame Price</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Parcel</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="70">T. Butir</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="70">T. Carat</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">GIA</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Total Harga</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">NO</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">RO</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Finishing</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Stone Setting</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">Harga M <span class="text-muted">USD</span></asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">Harga Jual <span class="text-muted">Rp</span></asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">Keterangan</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>

