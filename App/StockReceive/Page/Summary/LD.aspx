<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="LD.aspx.cs" inherits="Page_LD" %>

<asp:content contentplaceholderid="h1" runat="server">
	Loose Diamond
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
                    Supplier :
                </div>
                <asp:DropDownList ID="supplier" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="">Supplier</asp:ListItem>
                </asp:DropDownList>
                    
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
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
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">No. GIA</asp:TableHeaderCell>                    
                    <asp:TableHeaderCell Width="100" CssClass ="text-right">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass ="text-right">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass ="text-right">Harga Total M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass ="text-right">Harga Total R</asp:TableHeaderCell>
                    <%--asp:TableHeaderCell Width="100" CssClass ="text-right">Rappaport</--%>
                    <asp:TableHeaderCell Width="150" CssClass ="text-right">Disc M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass ="text-right">Disc R</asp:TableHeaderCell>                    
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

