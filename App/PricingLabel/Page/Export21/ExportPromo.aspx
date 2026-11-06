<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExportPromo.aspx.cs" Inherits="Page_Export" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Export Label
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="h1_st" runat="server">
    Promo
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

            <div class="col-xs-12 col-sm-3 col-md-3">
                <!-- Promo Label -->
                <div class="panel panel-success">
                    <div class="panel-heading">
                        <i class="icon-tags"></i>&nbsp;Promo
                    </div>
                    <div class="panel-body">

                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div class="form-group">
                                    <div class="text-muted">
                                        Product Type
                                    </div>
                                    <asp:DropDownList ID="ddl_tipeproduk2" runat="server" CssClass="form-control" Width="175">
                                        <asp:ListItem Value="DJ">Diamond Jewellery</asp:ListItem>
                                        <asp:ListItem Value="PG">Plain Gold</asp:ListItem>
                                        <asp:ListItem Value="GJ">Gold Jewellery</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div class="input-group">
                                    <div class="text-muted">
                                        Tanggal Mulai Promo
                                    </div>
                                    <asp:RequiredFieldValidator ID="datepicker_RF" runat="server" />
                                </div>
                                <div class="input-group">
                                    <asp:TextBox CssClass="form-control text-center" type="text" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" />
                                    <span class="input-group-btn">
                                        <button id="btnRetrieve" runat="server" class="btn btn-primary" style="text-align: left;" onserverclick="btnRetrieve_Click">
                                            <span class="glyphicon glyphicon-refresh" aria-hidden="true"></span>
                                        </button>
                                    </span>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div class="input-group">
                                    <div class="text-muted">
                                        Search PLU
                                    </div>
                                </div>
                                <div class="input-group">
                                    <asp:TextBox CssClass="form-control text-center" type="text" ID="txt_plu" runat="server" />
                                    <span class="input-group-btn">
                                        <button id="btnRetrievePLU" runat="server" class="btn btn-primary" style="text-align: left;" onserverclick="btnRetrievePLU_Click">
                                            <span class="glyphicon glyphicon-refresh" aria-hidden="true"></span>
                                        </button>
                                    </span>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div class="form-group">
                                    <div class="text-muted">
                                        Nama Promo
                                    </div>
                                    <asp:DropDownList ID="ddl_promo" runat="server" CssClass="form-control" Width="175">
                                        <asp:ListItem Value="0">...</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Literal runat="server" ID="lit_text_test" />
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12">

                                <div class="clearfix">
                                    <br class="hidden-xs" />
                                </div>

                                <div style="padding-top: 15px;">
                                    <br />
                                    <button id="btnDisplay" runat="server" class="btn btn-primary" style="text-align: left;" onserverclick="btnDisplay_Click">
                                        <i class="icon-search"></i>
                                        Display
                                    </button>
                                </div>

                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div runat="server" id="err" visible="false">
                                    <div class="clearfix">
                                        <br class="hidden-xs" />
                                    </div>
                                    <div class="alert alert-xs alert-warning text-center" role="alert">
                                        <asp:Literal runat="server" ID="lit_err" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
            </div>

            <div class="col-xs-12 col-sm-9 col-md-9" runat="server" id="div_table">
                <!-- Table Information -->
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <div class="panel panel-success">
                            <div class="panel-heading">
                                <i class="icon-tags"></i>&nbsp;<asp:Literal runat="server" ID="heading_tipeproduk" />
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
                                    <div class="col-xs-12 col-sm-12 col-md-6">
                                        <div class="form-inline">
                                            
                                            <div class="form-group" runat="server" id="div_ddl_location">

                                                <div class="text-muted">
                                                    Location
                                                </div>
                                                <asp:DropDownList ID="ddl_location" runat="server" CssClass="form-control" Width="175">
                                                    <asp:ListItem Value="0">...</asp:ListItem>
                                                </asp:DropDownList>
                                                <%--<div class="form-group">
                                                    <div class="checkbox">
                                                        <asp:RadioButton runat="server" GroupName="rdo_loc" ID="rdo_loc_1" Text=" by Location" Checked="true" /></div>
                                                    <div class="checkbox">
                                                        <asp:RadioButton runat="server" GroupName="rdo_loc" ID="rdo_loc_w" Text=" All Warehouse" /></div>
                                                </div>--%>
                                            </div>
                                            <div class="form-group">
                                                <br />
                                                <asp:HiddenField runat="server" ID="hf_promo_tipe" />
                                                <asp:HiddenField runat="server" ID="hf_promo_id" />
                                                <button id="btnExport" runat="server" class="btn btn-primary" style="text-align: left;" onserverclick="btnExport_Click">
                                                    <i class="icon-download-alt"></i>
                                                    Export
                                                </button>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-6">
                                        <table>
                                            <tr>
                                                <td style="vertical-align:top;">Location Summary</td>
                                                <td style="vertical-align:top;">&nbsp;:&nbsp;</td>
                                                <td style="vertical-align:top;">
                                                    <asp:Literal runat="server" ID="lit_location_summary" />
                                                </td>
                                            </tr>
                                        </table>
                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <div class="panel panel-default">
                            <div class="panel-body">
                                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm" Style="width: 100%;">
                                    <asp:TableRow>
                                        <asp:TableHeaderCell Width="50" />
                                        <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="100">PLU</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="150">Item</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="80">Category</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="80">Level</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="200">Current Status</asp:TableHeaderCell>
                                        <asp:TableHeaderCell>Current Location</asp:TableHeaderCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </div>

    </form>
    <script>
        $(document).ready(function () {
            $('#<%=datepicker.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
        });
    </script>
    <script>
        $(document).ready(function () {
            $("select").select2({ width: '100%' });
        });

    </script>
    <%--<script>
        $(document).ready(function () {
            $("select").select2({ width: '100%' });
        });


        $(function () {
            $('[data-toggle="popover"]').popover(html = true);

            
        });


        $('#myTabs a').click(function (e) {
            e.preventDefault()
            $(this).tab('show')
        })
        
        var rdo1 = document.getElementById('content_rdo_loc_1');
        var rdow = document.getElementById('content_rdo_loc_w');
        
        $("#content_rdo_loc_1").on("click", function () {
          $("#content_ddl_location").prop("disabled", false);
        });

        $("#content_rdo_loc_w").on("click", function () {
          $("#content_ddl_location").prop("disabled", true);
        });
       
    </script>--%>
</asp:Content>

