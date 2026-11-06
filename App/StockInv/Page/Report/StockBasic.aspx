<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StockBasic.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Basic Item
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
                    &nbsp;
                </div>
                <asp:DropDownList ID="productsegmen" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Segmen</asp:ListItem>
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
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="120"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Picture</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Qty</asp:TableHeaderCell>
                    
                    <asp:TableHeaderCell Width="120">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Product Level</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Product Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">H. Min</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">H. Max</asp:TableHeaderCell>

                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <script type="text/javascript">
            function goDelete(id) {
                txtConfirm =
                    "Lanjutkan Proses DELETE ?\n\n" +
                    "Penghapusan data / transaksi sifatnya adalah permanen.\n" +
                    "Dianjurkan untuk melakukan backup terlebih dahulu.";

                if (confirm(txtConfirm))
                    location.href = 'StockBasic_Delete.aspx?id=' + id;
            }
	</script>
    </div>
</asp:Content>

