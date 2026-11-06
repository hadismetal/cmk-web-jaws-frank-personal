<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DJ.aspx.cs" inherits="Page_Summary" %>

<%@ register src="MenuSummary.ascx" tagname="Menu" tagprefix="uc1" %>
<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewellery
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Summary
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <script>
        $(document).ready(function () {
            $('#<%=datepicker.ClientID%>').datepicker({
            format: 'dd-mm-yyyy'
        });
        $('#<%=datepicker2.ClientID%>').datepicker({
            format: 'dd-mm-yyyy'
        });
    });
    </script>
    <uc1:menu id="menu" runat="server" />
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
                    Dari :
                </div>
                <asp:TextBox CssClass="form-control" type="text" placeholder="Dari" id="datepicker" runat="server" data-date-format="dd-mm-yyyy" />
                        
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Sampai :
                </div>
                <asp:TextBox CssClass="form-control" type="text" placeholder="Sampai" id="datepicker2" runat="server" data-date-format="dd-mm-yyyy" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Brand :
                </div>
                <asp:DropDownList ID="brand" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Brand :</asp:ListItem>
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
                <asp:DropDownList ID="productlevel" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Level</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="stonedistribution" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Stone Distribution</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="framematerial" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Frame Material</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="framefinishing" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Frame Finishing</asp:ListItem>
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
                    <asp:ListItem Value="0">Frame Color</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="processconstruction" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Process Construction</asp:ListItem>
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
                    <asp:ListItem Value="3">Pending Approval</asp:ListItem>
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
                    <asp:TableHeaderCell Width="30" />
                    <asp:TableHeaderCell Width="200" />
                    <asp:TableHeaderCell Width="180">No. Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Operator</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180">Tanggal Approve</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Approval</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Kode Barang</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Supplier</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Product</asp:TableHeaderCell>
                    
                    
                    <asp:TableHeaderCell Width="75" />
                    <asp:TableHeaderCell Width="75" />
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

