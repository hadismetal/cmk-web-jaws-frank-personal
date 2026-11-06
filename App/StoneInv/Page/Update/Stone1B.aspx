<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Stone1B.aspx.cs" Inherits="Page_Account_Stone" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Rapaport
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Report
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form runat="server" class="form-inline">
        <div class="form-group">
            <div class="text-muted">
                Search :
            </div>
            <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
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
                <asp:ListItem Value="Nomor">PLU</asp:ListItem>
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
                &nbsp;
            </div>
            <asp:DropDownList ID="supplier" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem>Supplier</asp:ListItem>
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
    </form>
    <div class="table-responsive">
        <asp:Table ID="tb" runat="server" CssClass="table-list">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="120"></asp:TableHeaderCell>
                <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">PLU</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">GIA</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">GIA Expired</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Tipe Lokasi</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="200">Lokasi</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Nama</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Shape</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Colour</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Clarity</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Cutting</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Brand</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="90">Butir</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="90">Carat</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="90">Harga Per Karat USD</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="90">Total Harga USD</asp:TableHeaderCell>
                
                <asp:TableHeaderCell Width="120">Tanggal Penerimaan</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Usia Stock <span class="text-sm text-silver">hari</span></asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
    <asp:Literal ID="paging_text" runat="server" />
</asp:Content>

