<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="Export.aspx.cs" Inherits="Page_Export" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Export Label
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="h1_st" runat="server">
    Standard
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style type="text/css">
        .form-group .select2-container {
            position: relative;
            z-index: 2;
            float: left;
            width: 100%;
            margin-bottom: 0;
            display: table;
            table-layout: fixed;
        }

        .form-group .select2-container {
            border: none;
            padding: 0;
        }
    </style>
    <form id="Form1" runat="server">
        <div class="row">
            <div class="col-xs-12">
                <div id="errx" runat="server" visible="false"></div>
                <asp:Literal runat="server" ID="lit_test" />
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-3 col-md-3">
                <!-- Standard Label -->
                <div class="panel panel-info">
                    <div class="panel-heading">
                        <i class="icon-tags"></i>&nbsp;Standard
                    </div>
                    <div class="panel-body">
                        <div class="form-inline">
                            <div class="form-group">
                                <div class="text-muted">Product Type</div>
                                <asp:DropDownList
                                    ID="ddl_tipeproduk"
                                    runat="server"
                                    CssClass="form-control"
                                    Width="175">
                                    <asp:ListItem Value="DJ">Diamond Jewellery</asp:ListItem>
                                    <asp:ListItem Value="PG">Plain Gold</asp:ListItem>
                                    <asp:ListItem Value="GJ">Gold Jewellery</asp:ListItem>
                                    <asp:ListItem Value="LD">Loose Diamond</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <div class="text-muted">Location</div>
                                <asp:DropDownList
                                    ID="ddl_location"
                                    runat="server"
                                    CssClass="form-control"
                                    Width="175">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <div class="text-muted">Product Item</div>
                                <asp:DropDownList
                                    ID="ddl_productitem"
                                    runat="server"
                                    CssClass="form-control"
                                    Width="175">
                                    <asp:ListItem Value="0">...</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="form-group">
                                <div class="text-muted">Product Category (DJ/GJ only)</div>
                                <asp:DropDownList
                                    ID="ddl_productcategory"
                                    runat="server"
                                    CssClass="form-control"
                                    Width="175">
                                    <asp:ListItem Value="0">...</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <div class="text-muted">Product Level (PG only)</div>
                                <asp:DropDownList
                                    ID="ddl_productlevel"
                                    runat="server"
                                    CssClass="form-control"
                                    Width="175">
                                    <asp:ListItem Value="0">...</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <div class="text-muted">Supplier (PG only)</div>
                                <asp:DropDownList
                                    ID="ddl_supplier"
                                    runat="server"
                                    CssClass="form-control"
                                    Width="175">
                                    <asp:ListItem Value="0">...</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <div class="text-muted">TGP / Fix Rate (PG Only)</div>
                                <asp:DropDownList
                                    ID="ddl_tgp_fixrate"
                                    runat="server"
                                    CssClass="form-control"
                                    Width="175">
                                    <asp:ListItem Value="0">...</asp:ListItem>
                                    <asp:ListItem Value="1">Fix Rate</asp:ListItem>
                                    <asp:ListItem Value="2">TGP</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <div class="text-muted">Kode Barang (PG only)</div>
                                <asp:TextBox
                                    ID="txt_kodeBarang"
                                    CssClass="form-control"
                                    runat="server"
                                    Width="275" />
                            </div>
                            <div class="clearfix">
                                <br class="hidden-xs" />
                            </div>
                            <div class="form-group">
                                <div class="text-muted">No. PLU</div>
                                <asp:TextBox
                                    ID="txt_from"
                                    CssClass="form-control text-center"
                                    runat="server"
                                    Width="135"
                                    placeholder="Start..." />
                            </div>
                            <div class="form-group">
                                <div class="text-muted">&nbsp;</div>
                                <asp:TextBox
                                    ID="txt_to"
                                    CssClass="form-control text-center"
                                    runat="server"
                                    Width="135"
                                    placeholder="End..." />
                            </div>
                        </div>
                        <br />
                        <div class="form-group">
                            <div class="text-muted">Skip PWP</div>
                            <asp:CheckBox
                                ID="cb_skip_pwp"
                                runat="server" Checked="true" />
                        </div>
                        <div>
                            <div class="clearfix">
                                <br class="hidden-xs" />
                            </div>
                            <div style="padding-bottom: 5px">
                                <br />
                                <button
                                    id="btnDisplay"
                                    runat="server"
                                    class="btn btn-primary"
                                    style="text-align: left"
                                    onserverclick="btnDisplay_Click">
                                    <i class="icon-search"></i>
                                    Display
                                </button>
                            </div>
                        </div>
                        <div runat="server" id="err" visible="false">
                            <div class="clearfix">
                                <br class="hidden-xs" />
                            </div>
                            <div
                                class="alert alert-xs alert-warning text-center"
                                role="alert">
                                <asp:Literal runat="server" ID="lit_err" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xs-12 col-sm-9 col-md-9" runat="server" id="div_table">
                <!-- Table Information -->
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <div class="panel panel-info">
                            <div class="panel-heading">
                                <i class="icon-tags"></i>&nbsp;<asp:Literal
                                    runat="server"
                                    ID="heading_tipeproduk" />
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-6 col-md-6">
                                        <asp:Literal runat="server" ID="lit_header1" />
                                    </div>
                                    <div class="col-xs-12 col-sm-6 col-md-6">
                                        <asp:Literal runat="server" ID="lit_header2" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12">
                                        <br />
                                        <button
                                            id="btnPrint1"
                                            runat="server"
                                            class="btn btn-primary"
                                            style="text-align: left"
                                            onserverclick="btnPrint1_Click">
                                            <i class="icon-download-alt"></i>
                                            Export
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <div class="panel panel-default">
                            <div class="panel-body">
                                <asp:Table
                                    ID="tbdj"
                                    runat="server"
                                    CssClass="table-list"
                                    Style="width: 100%">
                                    <asp:TableHeaderRow>
                                        <asp:TableHeaderCell Width="50" />
                                        <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="100">PLU</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="160">Item</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="160">Category</asp:TableHeaderCell>
                                        <asp:TableHeaderCell>Location</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="120"><span style="color: blue"
                        >Harga Standard</span
                      ></asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="120">Harga Promo</asp:TableHeaderCell>
                                    </asp:TableHeaderRow>
                                </asp:Table>
                                <asp:Table
                                    ID="tbpg"
                                    runat="server"
                                    CssClass="table-list"
                                    Style="width: 100%">
                                    <asp:TableHeaderRow>
                                        <asp:TableHeaderCell Width="50" />
                                        <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="100">PLU</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="160">Item</asp:TableHeaderCell>
                                        <asp:TableHeaderCell>Location</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="120"><span style="color: blue"
                        >Harga Standard</span
                      ></asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="120">Jenis Harga</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="120">Harga Promo</asp:TableHeaderCell>
                                    </asp:TableHeaderRow>
                                </asp:Table>
                                <asp:Table
                                    ID="tbgj"
                                    runat="server"
                                    CssClass="table-list"
                                    Style="width: 100%">
                                    <asp:TableHeaderRow>
                                        <asp:TableHeaderCell Width="50" />
                                        <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="100">PLU</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="160">Item</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="160">Category</asp:TableHeaderCell>
                                        <asp:TableHeaderCell>Location</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="120"><span style="color: blue"
                        >Harga Standard</span
                      ></asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="120">Harga Promo</asp:TableHeaderCell>
                                    </asp:TableHeaderRow>
                                </asp:Table>
                                <asp:Table
                                    ID="tbld"
                                    runat="server"
                                    CssClass="table-list"
                                    Style="width: 100%">
                                    <asp:TableHeaderRow>
                                        <asp:TableHeaderCell Width="50" />
                                        <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="100">PLU</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="160">Item</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="160">Category</asp:TableHeaderCell>
                                        <asp:TableHeaderCell>Location</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="60">Shape</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="60">Colour</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="60">Clarity</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="60">Brand</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="120">GIA</asp:TableHeaderCell>
                                    </asp:TableHeaderRow>
                                </asp:Table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <%--
  <script>
    $(document).ready(function () {
      $("#<%=datepicker.ClientID%>").datepicker({
        format: "dd-mm-yyyy",
      });
    });
  </script>
    --%>
    <script>
        $(document).ready(function () {
            $("select").select2({ width: "100%" });
        });
    </script>
</asp:Content>
