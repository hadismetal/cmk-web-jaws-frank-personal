<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="LD_List.aspx.cs" inherits="Page_LD" %>

<asp:content contentplaceholderid="h1" runat="server">
	Loose Diamond List
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
                    Cabang :
                </div>
                <asp:DropDownList ID="cabang" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Cabang :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="Input PLU" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Periode :
                </div>

                <asp:TextBox CssClass="form-control" type="text" placeholder="Dari" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" />

            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox CssClass="form-control" type="text" placeholder="Sampai" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" />
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
                    <button id="export" runat="server" class="btn btn-primary" onserverclick="Export">
                        <i class="icon-download-alt"></i>
                        Export
                    </button>
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Print">
                        <i class="icon-print"></i>
                        Print
                    </button>
                </div>
            </div>
        </form>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">PLU Lama</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Parcel</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">No. GIA</asp:TableHeaderCell>       
                    <asp:TableHeaderCell Width="100">Nama Supplier</asp:TableHeaderCell>               
                    <asp:TableHeaderCell Width="100" CssClass ="text-right">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass ="text-right">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass ="text-right">Harga Total M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass ="text-right">Harga Total R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass ="text-right">Rappaport</asp:TableHeaderCell>
                    <%--<asp:TableHeaderCell Width="150">Disc M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Disc R</asp:TableHeaderCell>--%>                    
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

