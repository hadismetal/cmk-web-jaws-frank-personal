<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_List.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery Revaluation
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <label class="control-label">Periode</label>
                <br />
                <asp:TextBox class="form-control" type="text" placeholder="Dari" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" Width="110" />
                <script>
                    $(document).ready(function () { $('#<%=datepicker.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>
                <asp:RequiredFieldValidator ID="datepicker_RF" runat="server" />
                <span id="datepicker_ERR" runat="server" class="text-validator" visible="false">Format</span>
            </div>
            <div class="form-group">
                <label class="control-label">&nbsp;</label>
                <br />
                <asp:TextBox class="form-control" type="text" placeholder="Sampai" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" Width="110" />
                <script>
                    $(document).ready(function () { $('#<%=datepicker2.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>
                <asp:RequiredFieldValidator ID="datepicker2_RF" runat="server" />
                <span id="datepicker2_ERR" runat="server" class="text-validator" visible="false">Format</span>
            </div>
            <div class="form-group">
                <label class="control-label">&nbsp;</label>
                <br />
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="Input PLU" />
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
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Order By :
                </div>
                <asp:DropDownList ID="orderby" runat="server" CssClass="form-control" Width="175">
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
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Export">
                        <i class="icon-search"></i>
                        Export
                    </button>
                </div>
            </div>
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="90">Tanggal</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Harga Sebelum</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Harga Sesudah</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">Mutasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">M Sebelum</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">M Sesudah</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">R Sebelum</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">R Sesudah</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Net Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Gross Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Butir</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Carat</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Segmen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Stone Distribution</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>

