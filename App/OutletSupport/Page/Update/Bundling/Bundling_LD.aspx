<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Bundling_LD.aspx.cs" Inherits="Page_Create_ProductAddr" %>

<%@ Register Src="MenuPromo.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Promo Bundling
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Update
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <hr />
    <form runat="server">
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
                <i class="icon-ok"></i>
                Save
            </button>
            
        </div>
    </form>
</asp:Content>

