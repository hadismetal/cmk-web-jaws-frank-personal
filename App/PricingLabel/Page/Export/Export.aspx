<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Export.aspx.cs" Inherits="Page_Export" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Export Product
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server" class="form-inline">
        <div id="err" runat="server" visible="false">
        </div>
        <div class="form-group">
            <div class="text-muted">
                &nbsp;
            </div>
            <asp:DropDownList ID="cabang" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Cabang :</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                &nbsp;
            </div>
            <asp:DropDownList ID="clarity" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Claritry :</asp:ListItem>
                <asp:ListItem Value="1">VS</asp:ListItem>
                <asp:ListItem Value="2">VVS</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                &nbsp;
            </div>
            <asp:DropDownList ID="productitem" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Product Item</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="clearfix">
            <br class="hidden-xs" />
        </div>
        <div class="text-muted">
            No. PLU :
        </div>
        <div class="form-group">
            <asp:TextBox ID="from" CssClass="form-control" runat="server" placeholder="Start" />
        </div>
        <div class="form-group">
            <asp:TextBox ID="to" CssClass="form-control" runat="server" placeholder="End" />
        </div>

        <div class="clearfix">
            <br class="hidden-xs" />
        </div>
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="go" runat="server" class="btn btn-primary" onserverclick="exportDJ">
                    <i class="icon-search"></i>
                    Diamond Jewellery
                </button>
                &nbsp;
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="exportPG">
                        <i class="icon-search"></i>
                        Plain Gold
                    </button>
                &nbsp;
                    <button id="Button2" runat="server" class="btn btn-primary" onserverclick="exportLD">
                        <i class="icon-search"></i>
                        Loose Diamond
                    </button>
                &nbsp;
                    <button id="Button3" runat="server" class="btn btn-primary" onserverclick="exportGJ">
                        <i class="icon-search"></i>
                        Gold Jewellery
                    </button>
            </div>
        </div>
    </form>

</asp:Content>

