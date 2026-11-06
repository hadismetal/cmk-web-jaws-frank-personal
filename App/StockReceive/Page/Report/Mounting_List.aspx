<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Mounting_List.aspx.cs" inherits="Page_Mounting" %>

<asp:content contentplaceholderid="h1" runat="server">
	Mounting Summary
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
                    Cabang :
                </div>
                <asp:DropDownList ID="cabang" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Cabang :</asp:ListItem>
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
                    Stock Keeper
                </div>
                <asp:DropDownList ID="sk" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Stock Keeper :</asp:ListItem>
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
                <asp:DropDownList ID="supplier" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="">Supplier</asp:ListItem>
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
                    <button id="Button3" runat="server" class="btn btn-primary" onserverclick="Export">
                        <i class="icon-download-alt"></i>
                        Export
                    </button>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Print">
                        <i class="icon-print"></i>
                        Print
                    </button>
                </div>
            </div>
        </form>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list table-striped text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tanggal Approve</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Stock Keeper</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Barang</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. SPK</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Stone Dist.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Frame Price M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Frame Price R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">NO M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">NO M Lot</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">NO R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">RO M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">RO R</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Finishing M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Finishing R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Ongkos Lain M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Ongkos Lain R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">T. Harga M <span class="text-muted">USD</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">T. Harga R <span class="text-muted">USD</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Mark Up <span class="text-muted">%</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">H. Jual <span class="text-muted">Rp</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Supplier</asp:TableHeaderCell>
                    
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

