<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GJ_KodeKaretList.aspx.cs" Inherits="Page_SPK_DesignList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Gold Jewellery
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
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
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="text-muted">
                Characteristic Design :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="designcategory" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Design Category :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="designconcept" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Design Concept :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="designprocess" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Design Process :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="targetage" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Target Age :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="targetgender" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Target Gender :</asp:ListItem>
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
                    <asp:ListItem>Product Item</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="productcategory" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Product Category</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="productlevel" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Product Level</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="stonedistribution" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Stone Distribution</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="framematerial" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Frame Material</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="framefinishing" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Frame Finishing</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="framecolor" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Frame Color</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="processconstruction" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Process Construction</asp:ListItem>
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
                    <asp:TableHeaderCell Width="90" />
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="150">No. Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Product</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="75" />
                    <asp:TableHeaderCell Width="75" />
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

