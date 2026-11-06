<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Stone2.aspx.cs" Inherits="Page_Account_Stone" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Colour Diamond
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
            <div class="text-muted">
                Tipe Lokasi
            </div>
            <asp:DropDownList ID="tipelokasi" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasi" AutoPostBack="true">
                <asp:ListItem Value="-1">Tipe Lokasi</asp:ListItem>
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
                <asp:ListItem Value="-1">Lokasi</asp:ListItem>
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
            
        </asp:Table>
    </div>
    <asp:Literal ID="paging_text" runat="server" />
</asp:Content>

