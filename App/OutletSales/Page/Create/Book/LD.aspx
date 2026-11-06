<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LD.aspx.cs" Inherits="Page_Register_Titipan_StoneAdd" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Loose Diamond
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Booking
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form runat="server" class="form-inline">
        <div id="stone1b" runat="server">
            <div class="text-muted">
                Parcel :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel101b" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Shape :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel102b" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Size :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel103b" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Colour :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel104b" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Clarity :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="clearfix"></div>
            <div class="form-separator hidden-xs"></div>
            <div class="form-group">
                <asp:DropDownList ID="parcel105b" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Cutting :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel106b" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Brand :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel107b" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Category :</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                    <i class="icon-search"></i>
                    Display
                </button>
                <a id="cancel" runat="server" class="btn btn-default" href="Titipan_Stone.aspx">
                    <i class="icon-remove"></i>
                    Cancel
                </a>
            </div>
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90" />
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Shape</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Size</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Colour</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Clarity</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
            
        </div>
    </form>
</asp:Content>

