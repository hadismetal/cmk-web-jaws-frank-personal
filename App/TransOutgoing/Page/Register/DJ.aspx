<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ.aspx.cs" Inherits="DJ" %>

<%@ Register Src="MenuDJ.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewelry
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <form runat="server" class="form-inline">
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
                Storage Type (From) :
            </div>
            <asp:DropDownList ID="ddl_from" runat="server" CssClass="form-control" Width="200">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Search (From) :
            </div>
            <asp:TextBox runat="server" ID="txt_from" CssClass="form-control" Width="200" />
        </div>
        <div class="form-group">
            <div class="text-muted">
                Storage Type (To) :
            </div>
            <asp:DropDownList ID="ddl_to" runat="server" CssClass="form-control" Width="200">
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <div class="text-muted">
                Search (To) :
            </div>
            <asp:TextBox runat="server" ID="txt_to" CssClass="form-control" Width="200" />
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
            <asp:Literal runat="server" ID="lit_valid" />
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90" />
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">From
                        <asp:Literal runat="server" ID="lit_TypeFrom" /></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">To
                        <asp:Literal runat="server" ID="lit_TypeTo" /></asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </form>
</asp:Content>

