<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="ProductOrderList.aspx.cs" inherits="Page_Summary" %>

<asp:content contentplaceholderid="h1" runat="server">
	Report
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <uc1:menu id="menu" runat="server" />
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
                    &nbsp;
                </div>
                <asp:TextBox ID="dari" runat="server" CssClass="form-control text-center" Width="110" placeholder="Dari" />
                <script>
                    $(document).ready(function () { $('#<%=dari.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>

            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="sampai" runat="server" CssClass="form-control text-center" Width="110" placeholder="Sampai" />
                <script>
                    $(document).ready(function () { $('#<%=sampai.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>

            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="text-muted">
                Characteristic Design :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="designcategory" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Design Category :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="designconcept" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Design Concept :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="targetage" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Target Age :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="targetgender" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Target Gender :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
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
                <asp:DropDownList ID="productcategory" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Category</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="-1">Status</asp:ListItem>
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Approved</asp:ListItem>
                    <asp:ListItem Value="2">Draft</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="designer" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Designer</asp:ListItem>
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
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="200" />
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="120" />
                    <asp:TableHeaderCell Width="140">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Product</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="75" />
                    <asp:TableHeaderCell Width="75" />
                    <asp:TableHeaderCell Width="75" />
                    <asp:TableHeaderCell Width="75" />
                    <asp:TableHeaderCell Width="75" />
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

