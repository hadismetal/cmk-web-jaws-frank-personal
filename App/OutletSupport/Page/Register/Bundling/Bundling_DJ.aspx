<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Bundling_DJ.aspx.cs" Inherits="Page_Create_ProductAddr" %>

<%@ Register Src="MenuPromo.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Promo Bundling
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Create New
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <hr />
    <form runat="server">
        <div class="form-group width-500">
            <div class="row">
                <div class="col-xs-7">
                    <label>
                        Product Category :
                    </label>
                    <asp:DropDownList CssClass="form-control" ID="productcategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="category_SelectedIndexChanged">
                        <asp:ListItem Value="0"> All</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="form-group width-500">
            <div class="row">
                <div class="col-xs-7">
                    <label>
                        Product Level :
                    </label>
                    <asp:DropDownList CssClass="form-control" ID="productlevel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="category_SelectedIndexChanged">
                        <asp:ListItem Value="0"> All</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="form-group width-500">
            <div class="row">
                <div class="col-xs-7">
                    <label>
                        Product Item :
                    </label>
                    <asp:DropDownList CssClass="form-control" ID="productitem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="category_SelectedIndexChanged">
                        <asp:ListItem Value="0"> All</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        
        <div class="form-group width-500">
            <div class="row">
                <div class="col-xs-7">
                    <label>
                        Product :
                        <asp:CheckBox ID="all" runat="server" Text="All" />
                    </label>
                    <asp:ListBox CssClass="form-control" runat="server" ID="prod" Height="400" Width="400" SelectionMode="Multiple"></asp:ListBox>

                </div>
            </div>
        </div>

        <hr />
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next
            </button>
            <button id="skip" runat="server" class="btn btn-success" onserverclick="skip_Click">
                <i class="icon-remove"></i>
                Skip
            </button>
        </div>
    </form>
</asp:Content>

