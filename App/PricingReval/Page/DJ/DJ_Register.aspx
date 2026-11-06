<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_Register.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery
</asp:Content>

<asp:content contentplaceholderid="h1_st" runat="server">
	(Bulk)
</asp:content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            
            <div class="form-group">
                <div class="text-muted">
                    Cabang :
                </div>
                <asp:DropDownList ID="cabang" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Cabang :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    PLU :
                </div>
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="..." />
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
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="parcel104a" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Clarity :</asp:ListItem>
                    <asp:ListItem Value="1">VVS</asp:ListItem>
                    <asp:ListItem Value="2">VS</asp:ListItem>
                    <asp:ListItem Value="3">RED</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="stonedistribution" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Distribution Stone :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="segmen" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Segmen :</asp:ListItem>
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
                    <asp:ListItem Value="1">Cabang</asp:ListItem>
                    <asp:ListItem Value="2">Product Level</asp:ListItem>
                    <asp:ListItem Value="3">Product Category</asp:ListItem>
                    <asp:ListItem Value="4">Product Item</asp:ListItem>
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

            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <a class="btn btn-default" href="javascript:checkAll(true)"><span class="glyphicon glyphicon-check" aria-hidden="true"></span> Check All Data</a>
                <a class="btn btn-default" href="javascript:checkAll(false)"><span class="glyphicon glyphicon-unchecked" aria-hidden="true"></span> Uncheck All Data</a>
                    
                    <button id="save" runat="server" class="btn btn-success" onserverclick="Save">
                        <i class="icon-ok"></i>
                        Register Checked
                    </button>
                <%--<a href="javascript:checkAll(true)">Check</a>&nbsp;
				<a href="javascript:checkAll(false)">Uncheck</a>--%>
            </div>
            <div>
                <asp:Literal runat="server" ID="lit_msg" />
            </div>
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="30"></asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="110">No. PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="70">M</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="70">R</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Final Price</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="70">Nett Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="70">Gross Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="70">Butir</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="70">Carat</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Segmen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Stone Distribution</asp:TableHeaderCell>
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
            }
            
        </script>
    </div>
</asp:Content>

