<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="GJ_List.aspx.cs" inherits="Page_GJ" %>

<asp:content contentplaceholderid="h1" runat="server">
	Gold Jewellery List
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
                    Supplier :
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
                    <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Stone Distribution</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Nett Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Frame Price M</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Frame Price R</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">NO M</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">NO M Lot</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">NO R</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">RO M</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">RO R</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Finishing M</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Finishing R</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Ongkos Lain M</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Ongkos Lain R</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Stone Setting M</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Stone Setting R</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Total M (USD)</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Total R (USD)</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Mark Up</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Harga Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">No validasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Supplier</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

