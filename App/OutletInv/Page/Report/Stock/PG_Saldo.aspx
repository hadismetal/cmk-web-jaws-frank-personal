<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PG_Saldo.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Plain Gold Saldo
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
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
                    Group By
                </div>
                <asp:DropDownList ID="groupby" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="1">Product Item</asp:ListItem>
                    <asp:ListItem Value="5">Supplier</asp:ListItem>
                </asp:DropDownList>
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
                <asp:TableRow>
                    <asp:TableHeaderCell Width="140">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" Width="580" colspan="5">Saldo Awal</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" Width="580" colspan="5">Penerimaan</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" Width="580" colspan="5">Penjualan</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" Width="580" colspan="5">Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" Width="580" colspan="5">Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" Width="580" colspan="5">Transfer In</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" Width="580" colspan="5">Transfer Out</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" Width="580" colspan="5">Retur</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" Width="580" colspan="5">Lebur</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" Width="580" colspan="5">Saldo Akhir</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>

            
        </div>

    </div>
</asp:Content>

