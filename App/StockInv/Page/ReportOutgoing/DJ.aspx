<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ.aspx.cs" Inherits="DJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewelry
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Report
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <form runat="server" class="form-inline">

        <table class="table-form">
            <tr>
                <td>Periode</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tgl" runat="server" CssClass="form-control text-center" Width="110" />
                    <script>
                        $(document).ready(function () { $('#content_tgl').datepicker({ format: 'dd-mm-yyyy' }); });
                    </script>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="tgl_RF" runat="server" />
                    <span id="tgl_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
                <td>s/d&nbsp;&nbsp;</td>
                <td>
                    <asp:TextBox ID="tgl2" runat="server" CssClass="form-control text-center" Width="110" />
                    <script>
                        $(document).ready(function () { $('#content_tgl2').datepicker({ format: 'dd-mm-yyyy' }); });
                    </script>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="tgl2_RF" runat="server" />
                    <span id="tgl2_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
                <td>
                    <asp:TextBox ID="kw" runat="server" CssClass="form-control text-center" Width="300" placeholder="No. Dokumen" />
                </td>
            </tr>
        </table>
        <div class="form-group">
            <div class="text-muted">
                Company Brand :
            </div>
            <asp:DropDownList ID="brand" runat="server" CssClass="form-control" Width="200">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Location Type (From) :
            </div>
            <asp:DropDownList ID="locationtypefrom" runat="server" CssClass="form-control" Width="200">
                <asp:ListItem Value="-1">Semua</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Search (From) :
            </div>
            <asp:TextBox runat="server" ID="txt_from" CssClass="form-control" Width="200" />
        </div>
        <div class="form-group">
            <div class="text-muted">
                Location Type (To) :
            </div>
            <asp:DropDownList ID="locationtypeto" runat="server" CssClass="form-control" Width="200">
                <asp:ListItem Value="-1">Semua</asp:ListItem>
            </asp:DropDownList>
        </div>
        
        <div class="form-group">
            <div class="text-muted">
                Search (To) :
            </div>
            <asp:TextBox runat="server" ID="txt_to" CssClass="form-control" Width="200" />
        </div>
        <div class="form-group">
            <div class="text-muted">
                Status Outgoing :
            </div>
            <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="200">
                <asp:ListItem Value="0">Semua</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                    <i class="icon-search"></i>
                    Display
                </button>
                <button id="Button2" runat="server" class="btn btn-primary" onserverclick="Export">
                    <i class="icon-search"></i>
                    Export
                </button>
                <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Print">
                    <i class="icon-search"></i>
                    Print
                </button>
            </div>
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90" />
                    <asp:TableHeaderCell Width="90" />
                    <asp:TableHeaderCell Width="60">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. Dokumen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">From</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">To</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">Netto</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">Gross</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Harga Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Operator</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Approval</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </form>
</asp:Content>

