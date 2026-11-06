<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LD_Rekap.aspx.cs" Inherits="Page_LD_Rekap" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Loose Diamond Rekap
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server" class="form-inline">
        <div class="form-group">
            <div class="text-muted">
                Search :
            </div>
            <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
        </div>
        <div class="form-group">
            <div class="text-muted">
                Cabang :
            </div>
            <asp:DropDownList ID="cabang" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Cabang :</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="clearfix">
            <br class="hidden-xs" />
        </div>
        <div class="text-muted">
            Parcel :
        </div>
        <div class="form-group">
            <asp:DropDownList ID="parcel01" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem>Shape :</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:DropDownList ID="parcel02" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem>Size :</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:DropDownList ID="parcel03" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem>Colour :</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:DropDownList ID="parcel04" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem>Clarity :</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="clearfix">
            <br class="hidden-xs" />
        </div>
        <div class="clearfix"></div>
        <div class="form-separator hidden-xs"></div>
        <div class="form-group">
            <asp:DropDownList ID="parcel05" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem>Cutting :</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:DropDownList ID="parcel06" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem>Brand :</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:DropDownList ID="parcel07" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem>Category :</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="clearfix">
            <br class="hidden-xs" />
        </div>
        <div class="form-group">
            <div class="text-muted">
                Diurutkan :
            </div>
            <asp:DropDownList ID="order" runat="server" CssClass="form-control" Width="150">
                <asp:ListItem Value="Nama">Nama</asp:ListItem>
                <asp:ListItem Value="KodeParcel">Kode Parcel</asp:ListItem>
                <asp:ListItem Value="ID">ID</asp:ListItem>
            </asp:DropDownList>
            <div class="text-00 visible-xs">&nbsp;</div>
            <asp:DropDownList ID="orderby" runat="server" CssClass="form-control" Width="100">
                <asp:ListItem>ASC</asp:ListItem>
                <asp:ListItem>DESC</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
                <div class="text-muted">
                    Substorage
                </div>
                <asp:DropDownList ID="substorage" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Substorage</asp:ListItem>
                    <asp:ListItem Value="1">Jual</asp:ListItem>
                    <asp:ListItem Value="2">Resell</asp:ListItem>
                    <asp:ListItem Value="3">Trade In</asp:ListItem>
                    <asp:ListItem Value="4">Titipan Customer</asp:ListItem>
                    <asp:ListItem Value="5">Reparasi</asp:ListItem>
                    <asp:ListItem Value="6">Lebur</asp:ListItem>
                    <asp:ListItem Value="95">Ongoing Lebur</asp:ListItem>
                    <asp:ListItem Value="96">Broken</asp:ListItem>
                    <asp:ListItem Value="97">Outgoing - New / Package</asp:ListItem>
                    <asp:ListItem Value="98">Outgoing - Approved / On The Way</asp:ListItem>
                    <asp:ListItem Value="99">Lost / Trouble</asp:ListItem>
                </asp:DropDownList>
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
        <asp:Table ID="tb" runat="server" CssClass="table-list">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Nama</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Shape</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Colour</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Clarity</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Cutting</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Brand</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Total Butir</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Total Carat</asp:TableHeaderCell>

            </asp:TableHeaderRow>
        </asp:Table>
    </div>
</asp:Content>
