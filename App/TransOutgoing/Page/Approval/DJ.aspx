<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ.aspx.cs" Inherits="DJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewelry
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Approval
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
    <form runat="server" class="form-inline">
        <div class="form-group">
            <div class="text-muted">
                Dari :
            </div>

            <asp:TextBox CssClass="form-control text-center" Width="110" type="text" placeholder="Dari" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" />
            <script>
                $(document).ready(function () { $('#content_datepicker').datepicker({ format: 'dd-mm-yyyy' }); });
            </script>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Sampai :
            </div>
            <asp:TextBox CssClass="form-control text-center" Width="110" type="text" placeholder="Sampai" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" />
            <script>
                $(document).ready(function () { $('#content_datepicker2').datepicker({ format: 'dd-mm-yyyy' }); });
            </script>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Market Brand :
            </div>
            <asp:DropDownList ID="brand" runat="server" CssClass="form-control" Width="200">
                <asp:ListItem Value="0">All</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Tipe Lokasi
            </div>
            <asp:DropDownList ID="tipelokasi" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasi" AutoPostBack="true">
                <asp:ListItem Value="-1">Tipe</asp:ListItem>
                <asp:ListItem Value="0">Warehouse</asp:ListItem>
                <asp:ListItem Value="1">Outlet</asp:ListItem>
                <asp:ListItem Value="2">Exhibition</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Lokasi
            </div>
            <asp:DropDownList ID="lokasi" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Lokasi</asp:ListItem>
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
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90" />
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">From</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">To</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160">User & Tanggal</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </form>
</asp:Content>

