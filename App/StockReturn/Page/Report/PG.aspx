<%@ Page Language="C#" masterpagefile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PG.aspx.cs" Inherits="Page_Report_PG_List" %>

<asp:content contentplaceholderid="h1" runat="server">
	Plain Gold
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
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
                    Search :
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Periode :
                </div>
                <asp:TextBox CssClass="form-control" type="text" placeholder="Dari" id="datepicker" runat="server" data-date-format="dd-mm-yyyy" />
                        
            </div>
            <div class="form-group">
                <div class="text-muted">&nbsp;</div>
                <asp:TextBox CssClass="form-control" type="text" placeholder="Sampai" id="datepicker2" runat="server" data-date-format="dd-mm-yyyy" />
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Brand :
                </div>
                <asp:DropDownList ID="brand" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Brand :</asp:ListItem>
                </asp:DropDownList>
            </div>
            
            <div class="form-group">
                <div class="text-muted">
                    Characteristic Product :
                </div>
                <asp:DropDownList ID="productitem" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Item</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
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
                <asp:DropDownList ID="framecolor" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Frame Color</asp:ListItem>
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
        </form>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Image</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. Dokumen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">No. PLU</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="100">Kode Barang</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Tanggal Approval</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Nama Supplier</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Product Level</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Model</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Kadar Logam</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Frame Color</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="80">Ring Size</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Supplier</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="100">Total Resell Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Ongkos</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">HPPM</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>
