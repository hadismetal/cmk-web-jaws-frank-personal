<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="ProductOrderList.aspx.cs" inherits="Page_Summary" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewellery SPK Report
</asp:content>

<asp:content contentplaceholderid="content" runat="server">

    <div id="zero" runat="server" visible="false">
        <i class="icon-ok icon-5x pull-left text-success"></i>
        <h4>Proses Selesai</h4>
        <p class="text-muted">
            Semua data / transaksi dalam kondisi sudah menjadi SPK.
        </p>
        <div class="clearfix"></div>
        <hr />
    </div>
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
                    Dari&nbsp;
                </div>
                <asp:TextBox ID="dari" runat="server" CssClass="form-control text-center" Width="110" placeholder="Dari" />
                <script>
                    $(document).ready(function () { $('#<%=dari.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>

            </div>
            <div class="form-group">
                <div class="text-muted">
                    Sampai&nbsp;
                </div>
                <asp:TextBox ID="sampai" runat="server" CssClass="form-control text-center" Width="110" placeholder="Sampai" />
                <script>
                    $(document).ready(function () { $('#<%=sampai.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>

            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>

            <div class="form-group">
                <div class="text-muted">
                    Design Category&nbsp;
                </div>
                <asp:DropDownList ID="designcategory" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">...</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Design Concept&nbsp;
                </div>
                <asp:DropDownList ID="designconcept" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">...</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Target Age&nbsp;
                </div>
                <asp:DropDownList ID="targetage" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">...</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Target Gender&nbsp;
                </div>
                <asp:DropDownList ID="targetgender" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">...</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Product Item&nbsp;
                </div>
                <asp:DropDownList ID="productitem" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">...</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Product Category&nbsp;
                </div>
                <asp:DropDownList ID="productcategory" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">...</asp:ListItem>
                </asp:DropDownList>
            </div>
            
            <div class="form-group">
                <div class="text-muted">
                    Designer
                </div>
                <asp:DropDownList ID="designer" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">...</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Status SPK
                </div>
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">...</asp:ListItem>
                    <asp:ListItem Value="1">Approved</asp:ListItem>
                    <asp:ListItem Value="2">Draft</asp:ListItem>
                </asp:DropDownList>
            </div>
             <div class="form-group">
                <div class="text-muted">
                    Status Order
                </div>
                <asp:DropDownList ID="statorder" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">...</asp:ListItem>
                     <asp:ListItem Value="1">New Order</asp:ListItem>
                     <asp:ListItem Value="2">Repeat Order</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                        <i class="icon-search"></i>
                        Display
                    </button>
					<button id="excel" runat="server" class="btn btn-primary" onserverclick="refreshPage_Excel">
                        Excel
                    </button>
                </div>
            </div>
        </form>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="200" />
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="120" />
                    <asp:TableHeaderCell Width="150">No. Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Operator</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Designer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Kelas Harga 1</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Kelas Harga 2</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

