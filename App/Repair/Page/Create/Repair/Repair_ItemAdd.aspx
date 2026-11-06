<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Repair_ItemAdd.aspx.cs" Inherits="Page_Register_DataAgent" %>

<%@ Register Src="MenuRepair.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Repair Order
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <asp:Literal ID="idbrand" runat="server" Visible="false" />
    <hr />
    <form runat="server" class="form-inline">
        <div class="form-group">
            <div class="text-muted">
                Search :
            </div>
            <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
            <asp:TextBox ID="inputoldcust" runat="server" CssClass="form-control" placeholder="Input Old Customer Code..." Width="350"/>
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
                <a id="cancel" runat="server" class="btn btn-default" href="Repair_Form.aspx">
                    <i class="icon-remove"></i>
                    Cancel
                </a>
				<a id="addPLUPG" href="Repair_ItemAddPLUPG.aspx" class="btn btn-primary"></a>
                <a id="addPLULD" href="Repair_ItemAddPLULD.aspx" class="btn btn-primary"></a>
            </div>
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
					<asp:tableheadercell width="200">Kode PLU</asp:tableheadercell>
					<asp:tableheadercell width="200">Product Item</asp:tableheadercell>
                    <asp:tableheadercell width="200">Harga</asp:tableheadercell>
                    <asp:tableheadercell width="200">Image</asp:tableheadercell>
				</asp:tableheaderrow>
            </asp:Table>
        </div>
    </form>
</asp:Content>

