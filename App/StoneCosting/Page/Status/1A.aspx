<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="1A.aspx.cs" Inherits="Page_Status_1A" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Reguler
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Status
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
                <asp:ListItem Value="0">Shape :</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:DropDownList ID="parcel02" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Size :</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:DropDownList ID="parcel03" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Colour :</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:DropDownList ID="parcel04" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Clarity :</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="clearfix">
            <br class="hidden-xs" />
        </div>
        <div class="form-group">
            <div class="text-muted">
                Status :
            </div>
            <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="3">Semua</asp:ListItem>
            </asp:DropDownList>
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
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                    <i class="icon-search"></i>
                    Display
                </button>
            </div>
        </div>
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="Button3" runat="server" class="btn btn-primary" onserverclick="Export">
                    <i class="icon-download-alt"></i>
                    Export
                </button>
            </div>
        </div>
    </form>
    <div class="table-responsive">
        <asp:Table ID="tb" runat="server" CssClass="table-list">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="90" />
                <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="250">Nama</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="175">Approval</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="130" CssClass="text-right"><small>USD</small> Harga</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
</asp:Content>

