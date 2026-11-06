<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Repair_WorkAdd.aspx.cs" Inherits="Page_Register_DataAgent" %>

<%@ Register Src="MenuRepair.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Repair Item
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
                <a id="cancel" runat="server" class="btn btn-default" href="Repair_Data.aspx">
                    <i class="icon-remove"></i>
                    Cancel
                </a>
            </div>
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
					<asp:tableheadercell width="300">Item</asp:tableheadercell>
                    <asp:tableheadercell width="100">Price</asp:tableheadercell>
				</asp:tableheaderrow>
            </asp:Table>
        </div>
    </form>
</asp:Content>

