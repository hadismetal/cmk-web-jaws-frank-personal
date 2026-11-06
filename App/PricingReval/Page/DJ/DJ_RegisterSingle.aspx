<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_RegisterSingle.aspx.cs" Inherits="DJ_RegisterSingle" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery
</asp:Content>

<asp:content contentplaceholderid="h1_st" runat="server">
	(Single)
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
                <asp:TextBox ID="kw" CssClass="form-control text-center" runat="server" placeholder="..." />
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

