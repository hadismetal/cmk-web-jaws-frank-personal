<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RepairInvoice_DataRepair.aspx.cs" Inherits="Page_Register_DocRepair" %>

<%@ Register Src="MenuRepairInvoice.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Repair Invoice
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <asp:Literal ID="idbrand" runat="server" Visible="false" />
    <hr />
    <form runat="server" class="form-inline">
        <div id="customerlist" runat="server">
            <div class="form-group">
                <div class="text-muted">
                    Search :
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                        <i class="icon-search"></i>
                        Display
                    </button>
                    <a id="cancel" runat="server" class="btn btn-default" href="Repair_Data01.aspx">
                        <i class="icon-remove"></i>
                        Cancel
                    </a>
                </div>
            </div>
            <br /><br />
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="90" />
                        <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">Nomor Dokumen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">Tgl</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">Nama Customer</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">Nama JR</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">Tipe Lokasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">Lokasi</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
            <hr />
        </div>
    </form>
</asp:Content>

