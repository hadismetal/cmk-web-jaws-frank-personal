<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LD.aspx.cs" Inherits="Page_LD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Loose Diamond Stock Return
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
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
        <form id="Form1" runat="server" class="form-inline">
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
                    &nbsp;
                </div>
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="Input PLU" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Supplier Asal:
                </div>
                <asp:DropDownList ID="supplier" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="">Supplier</asp:ListItem>
                </asp:DropDownList>
                    
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Supplier Tujuan:
                </div>
                <asp:DropDownList ID="suppliertujuan" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="">Supplier</asp:ListItem>
                </asp:DropDownList>
                    
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                        <i class="icon-search"></i>
                        Display
                    </button>
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Export">
                        <i class="icon-search"></i>
                        Export
                    </button>
                    <button id="Button2" runat="server" class="btn btn-primary" onserverclick="Print">
                        <i class="icon-search"></i>
                        Print
                    </button>
                </div>
            </div>
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="30">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">No. Dokumen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Parcel</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Tanggal</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="140">Nama Supplier</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">No. GIA</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100" CssClass="text-right">Butir</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100" CssClass="text-right">Carat</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120" CssClass="text-right">Harga Total M</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120" CssClass="text-right">Harga Total R</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120" CssClass="text-right">Rappaport</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">Keterangan</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>


