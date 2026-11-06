<%@ Page Language="C#" masterpagefile="~/MasterPage.master" CodeFile="DesignerPerformance.aspx.cs" Inherits="Page_Report_DP_DesignerPerformance" %>

<asp:content contentplaceholderid="h1" runat="server">
	Design Report
</asp:content>

<asp:content contentplaceholderid="content" runat="server">

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
            <div class="form-group">
                <asp:DropDownList ID="productlevel" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Level</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="productcategory" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Category</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="productitem" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Item</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="designer" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Designer</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <asp:DropDownList ID="stonedistribution" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Stone Distribution</asp:ListItem>
                </asp:DropDownList>
            </div>
             <div class="form-group">
                <asp:DropDownList ID="segmen" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Segmen</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="designcategory" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Design Category</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="designconcept" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Design Concept</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Order By</asp:ListItem>
                    <asp:ListItem Value="1">Product Level</asp:ListItem>
                    <asp:ListItem Value="2">Product Category</asp:ListItem>
                    <asp:ListItem Value="3">Product Item</asp:ListItem>
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
                        <i class="icon-search"></i>
                        Export
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
            <asp:Table ID="tb" runat="server" CssClass="table-list table-striped text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Product Level</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Stone Distribution</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Design Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Design Concept</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Naming</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Designer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">QTY Product</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="100">QTY Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Harga Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Foto</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>