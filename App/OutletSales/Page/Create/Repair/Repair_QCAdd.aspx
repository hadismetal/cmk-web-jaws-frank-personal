<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Repair_QCAdd.aspx.cs" Inherits="Page_Register_DataAgent" %>

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
                QC Number :
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
                <a id="cancel" runat="server" class="btn btn-default" href="Repair_Form.aspx">
                    <i class="icon-remove"></i>
                    Cancel
                </a>
            </div>
        </div>
        <div>
            <asp:Literal runat="server" ID="lit_message" />
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="50">No.</asp:tableheadercell>
					<asp:tableheadercell width="150">QC</asp:tableheadercell>
                    <asp:tableheadercell width="100">PLU</asp:tableheadercell>
					<asp:tableheadercell width="180">User</asp:tableheadercell>
                    <asp:tableheadercell width="180">Tanggal</asp:tableheadercell>
                    <asp:tableheadercell width="150">Image</asp:tableheadercell>
				</asp:tableheaderrow>
            </asp:Table>
        </div>
    </form>
</asp:Content>

