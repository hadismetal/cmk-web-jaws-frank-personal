<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_StoneAdd.aspx.cs" Inherits="Page_Register_DJ_StoneAdd" %>

<%@ Register Src="MenuDJ.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Diamond Jewelry
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <p>
        <span class="text-muted">Nomor :</span><br />
        <span class="text-lg" id="header_nomor" runat="server" />
    </p>
    <hr />
    <form id="Form1" runat="server" class="form-inline">
        <div class="form-group cb-multi">
            <asp:RadioButtonList ID="stoneindex" runat="server" RepeatColumns="2" AutoPostBack="true" OnSelectedIndexChanged="StoneChange" />
        </div>
        <hr />
        <div id="stone1a" runat="server" visible="false">
            <div class="text-muted">
                Parcel :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel101a" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Shape :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel102a" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Size :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel103a" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Colour :</asp:ListItem>
                    <asp:ListItem Value="">F</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel104a" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Clarity :</asp:ListItem>
                    <asp:ListItem Value="1">VVS</asp:ListItem>
                    <asp:ListItem Value="2">VS</asp:ListItem>
                    <asp:ListItem Value="3">RED</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div id="stone1b" runat="server" visible="false">
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
        <div id="stone2" runat="server" visible="false">
            <div class="text-muted">
                Parcel :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel201" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Shape :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel202" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Size :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel203" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Colour :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel204" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Clarity :</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div id="stone3" runat="server" visible="false">
            <div class="text-muted">
                Parcel :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel301" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Shape :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel302" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Size :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel303" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Colour :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel304" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Clarity :</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div id="stone4" runat="server" visible="false">
            <div class="text-muted">
                Parcel :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel401" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Shape :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel402" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Size :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel403" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Colour :</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div id="stone5" runat="server" visible="false">
            <div class="text-muted">
                Parcel :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel501" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Shape :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel502" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Size :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel503" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Colour :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel504" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Clarity :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="clearfix"></div>
            <div class="form-separator hidden-xs"></div>
            <div class="form-group">
                <asp:DropDownList ID="parcel505" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Cutting :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel506" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Brand :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel507" runat="server" CssClass="form-control" Width="175">
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
                <a id="cancel" runat="server" class="btn btn-default" href="DJ_Stone.aspx">
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
                    <asp:TableHeaderCell Width="400">Nama</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </form>
</asp:Content>

