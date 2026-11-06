<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PG_List.aspx.cs" Inherits="PG_List" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery
</asp:Content>

<asp:content contentplaceholderid="h1_st" runat="server">
	(Bulk)
</asp:content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
        td {
            vertical-align: top;
        }
    </style>
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="row">
                
                <div class="col-xs-12 col-sm-6">
                    <div>
                        <div class="form-group">
                            <div class="text-muted">
                                PLU
                            </div>
                            <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="..." Width="175" />
                        </div>
                        <div class="form-group">
                            <div class="text-muted">
                                Product Level
                            </div>
                            <asp:DropDownList ID="productlevel" runat="server" CssClass="form-control" Width="175">
                                <asp:ListItem Value="0">All Product Level</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <div class="text-muted">
                                Gold Model
                            </div>
                            <asp:DropDownList ID="productmodel" runat="server" CssClass="form-control" Width="175">
                                <asp:ListItem Value="0">All Gold Model</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <div class="text-muted">
                                Product Item
                            </div>
                            <asp:DropDownList ID="productitem" runat="server" CssClass="form-control" Width="175">
                                <asp:ListItem Value="0">All Product Item</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <div class="text-muted">
                                Tipe Lokasi
                            </div>
                            <asp:DropDownList ID="tipelokasi" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasi" AutoPostBack="true">
                                <asp:ListItem Value="0">Warehouse</asp:ListItem>
                                <asp:ListItem Value="1">Outlet</asp:ListItem>
                                <asp:ListItem Value="2">Exhibition</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <div class="text-muted">
                                Lokasi
                            </div>
                            <asp:DropDownList ID="lokasi" runat="server" CssClass="form-control" Width="175">
                                <asp:ListItem Value="0">All Lokasi</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <div class="text-muted">
                                Order By
                            </div>
                            <asp:DropDownList ID="orderby" runat="server" CssClass="form-control" Width="175">
                                <asp:ListItem Value="1">Cabang</asp:ListItem>
                                <asp:ListItem Value="2">Product Level</asp:ListItem>
                                <asp:ListItem Value="3">Product Category</asp:ListItem>
                                <asp:ListItem Value="4">Product Item</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div style="padding-top:15px;">
                        <div class="form-group">
                            <div>
                                <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                                    <i class="icon-search"></i>
                                    Display
                                </button>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <button id="Button1" runat="server" class="btn btn-warning" onserverclick="Execute">
                                <span class="glyphicon glyphicon-saved" aria-hidden="true"></span> Execute
                            </button>
                        </div>
                        
                    
                    </div>
                </div>
            </div>
            <div class="row" style="padding-top:15px;">
                <div class="col-xs-12">
                    <div class="form-group">
                        <a class="btn btn-default" href="javascript:checkAll(true)"><span class="glyphicon glyphicon-check" aria-hidden="true"></span> Check All Data</a>
                    </div>
                    <div class="form-group">
                        <a class="btn btn-default" href="javascript:checkAll(false)"><span class="glyphicon glyphicon-unchecked" aria-hidden="true"></span> Uncheck All Data</a>
                    </div>
                    <div class="form-group">
                        <button id="Button4" runat="server" class="btn btn-danger" onserverclick="Delete">
                            <span class="glyphicon glyphicon-trash" aria-hidden="true"></span> Delete Checked
                        </button>
                    </div>
                    <div class="form-group">
                        <button id="Button2" runat="server" class="btn btn-success" onserverclick="Apply">
                            <span class="glyphicon glyphicon-ok" aria-hidden="true"></span> Apply Checked
                        </button>
                    </div>
                </div>
                <div class="col-xs-12">
                    <br />
                    <span style="color:red;"><asp:Literal ID="err" runat="server" /></span>
                </div>
            </div>

            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow TableSection="TableHeader">
                        <asp:TableHeaderCell Width="70"></asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="70">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Gold Level</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga Jual Sebelum</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga Jual Sesudah</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Mutasi</asp:TableHeaderCell>
                        <%--<asp:TableHeaderCell Width="130">HPPM Awal</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">HPPM</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">HPPM Reval</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga Awal</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga Reval</asp:TableHeaderCell>--%>
                        <asp:TableHeaderCell Width="130">Net Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Gross Weight</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
        <script>
            function checkAll(checktoggle) {
                var checkboxes = new Array();
                checkboxes = document.getElementsByTagName('input');

                for (var i = 0; i < checkboxes.length; i++) {
                    if (checkboxes[i].type == 'checkbox') {
                        checkboxes[i].checked = checktoggle
                    }
                }

                $('#content_reval_M').prop('checked', false);
                $('#content_reval_R').prop('checked', false);
                $('#content_reval_G').prop('checked', false);
                $('#content_reval_SK').prop('checked', false);
                $('#content_reval_SB').prop('checked', false);
                $('#content_reval_MF').prop('checked', false);
                $('#content_reval_MU').prop('checked', false);
                $('#content_reval_Rate').prop('checked', false);

            }

            function checkAll_Option(checktoggle) {
                $('#content_reval_M').prop('checked', checktoggle);
                $('#content_reval_R').prop('checked', checktoggle);
                $('#content_reval_G').prop('checked', checktoggle);
                $('#content_reval_SK').prop('checked', checktoggle);
                $('#content_reval_SB').prop('checked', checktoggle);
                $('#content_reval_MF').prop('checked', checktoggle);
                $('#content_reval_MU').prop('checked', checktoggle);
                $('#content_reval_Rate').prop('checked', checktoggle);
            }
        </script>
    </div>
</asp:Content>

