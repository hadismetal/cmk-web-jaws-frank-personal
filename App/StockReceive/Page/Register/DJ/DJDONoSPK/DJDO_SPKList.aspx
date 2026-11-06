<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJDO_SPKList.aspx.cs" Inherits="Page_Register_DJDO_SPKList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="Server">
    Diamond Jewelry
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="Server">
    Register
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="Server">
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
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <div class="form-group">
                        <div class="text-muted">
                            Search (Kode Karet / No. SPK)
                        </div>
                        <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="250" placeholder="Input Keyword..." />
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Product Item
                        </div>
                        <asp:DropDownList ID="productitem" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Product Category
                        </div>
                        <asp:DropDownList ID="productcategory" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Product Level
                        </div>
                        <asp:DropDownList ID="productlevel" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem></asp:ListItem>
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
                </div>
                
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <div>
                        <br />
                        <asp:Literal runat="server" ID="lit_countdata" />
                    </div>
                    <div class="table-responsive">
                        <asp:Table ID="tb" runat="server" CssClass="table-list">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell Width="50" />
                                <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="150">Kode Karet</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="150">No. SPK</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="150">Item</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="250">Category</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="250">Level</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </div>
                </div>
            </div>

        </form>
        

    </div>
</asp:Content>

