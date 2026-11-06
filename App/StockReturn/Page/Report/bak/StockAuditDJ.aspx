<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StockAuditDJ.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery Stock Audit
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <label class="control-label">Periode</label>
                <br />
                <asp:TextBox class="form-control" type="text" placeholder="Dari" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" Width="110" />
                <script>
                    $(document).ready(function () { $('#<%=datepicker.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>
                <asp:RequiredFieldValidator ID="datepicker_RF" runat="server" />
                <span id="datepicker_ERR" runat="server" class="text-validator" visible="false">Format</span>
            </div>
            <div class="form-group">
                <label class="control-label">&nbsp;</label>
                <br />
                <asp:TextBox class="form-control" type="text" placeholder="Sampai" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" Width="110" />
                <script>
                    $(document).ready(function () { $('#<%=datepicker2.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>
                <asp:RequiredFieldValidator ID="datepicker2_RF" runat="server" />
                <span id="datepicker2_ERR" runat="server" class="text-validator" visible="false">Format</span>
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
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="150"></asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">ID</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Tipe Lokasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Nama Lokasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Waktu Mulai</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Waktu Selesai</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Total Item Sistem</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Total Item Fisik</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>

