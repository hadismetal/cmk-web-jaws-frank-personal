<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LD.aspx.cs" Inherits="LD" %>

<%@ Register Src="~/Page/Register/MenuLD.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Loose Diamond
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <form runat="server" class="form-inline">
        <div class="form-group">
            <div class="text-muted">
                Market Brand (From):
            </div>
            <asp:DropDownList ID="brandFrom" runat="server" CssClass="form-control" Width="200"  OnSelectedIndexChanged="ddl_from_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <br />
        <div class="form-group">
            <div class="text-muted">
                Storage Type (From) :
            </div>
            <asp:DropDownList ID="ddl_from" runat="server" CssClass="form-control" Width="200" OnSelectedIndexChanged="ddl_from_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Location (From) :
            </div>
            <asp:DropDownList ID="ddl_locFrom" runat="server" CssClass="form-control" Width="200">
            </asp:DropDownList>
        </div>
        <br />
        <div class="form-group">
            <div class="text-muted">
                Market Brand (To):
            </div>
            <asp:DropDownList ID="brandTo" runat="server" CssClass="form-control" Width="200" OnSelectedIndexChanged="ddl_to_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="7" Selected="True">FRANK & CO (F)</asp:ListItem>
                <asp:ListItem Value="3" Selected="True">MONDIAL (M)</asp:ListItem>
                <asp:ListItem Value="4" Selected="True">MISS MONDIAL (S)</asp:ListItem>
                <asp:ListItem Value="5" Selected="True">THE PALACE (P)</asp:ListItem>
            </asp:DropDownList>
        </div>
        <br />
        <div class="form-group">
            <div class="text-muted">
                Storage Type (To) :
            </div>
            <asp:DropDownList ID="ddl_to" runat="server" CssClass="form-control" Width="200" OnSelectedIndexChanged="ddl_to_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <div class="text-muted">
                Location (To) :
            </div>
            <asp:DropDownList ID="ddl_locTo" runat="server" CssClass="form-control" Width="200">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                    Next
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

