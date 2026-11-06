<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sales_DataAgent.aspx.cs" Inherits="Page_Register_DataAgent" %>

<%@ Register Src="MenuSales.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Souvenir
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <p>
        <span class="text-muted">Nomor :</span><br />
        <span class="text-lg" id="header_nomor" runat="server" />
    </p>
    <asp:Literal ID="idbrand" runat="server" Visible="false" />
    <hr />
    <form runat="server" class="form-inline">
        <div class="form-group">
            <div class="text-muted">
                Search :
            </div>
            <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
        </div>
        <%--div class="form-group">
            <div class="text-muted">
                Market Brand :
            </div>
            <asp:DropDownList ID="brand" runat="server" CssClass="form-control" Width="200">
                <asp:ListItem Value="0">Semua</asp:ListItem>
            </asp:DropDownList>
        </div--%>
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                    <i class="icon-search"></i>
                    Display
                </button>
                <a id="cancel" runat="server" class="btn btn-default" href="Sales_Data01.aspx">
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
                    <asp:TableHeaderCell Width="200">JPJR</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Email</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200" CssClass="text-center">Brand</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </form>
</asp:Content>

