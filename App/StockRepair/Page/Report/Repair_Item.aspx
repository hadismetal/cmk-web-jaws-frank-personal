<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Repair_Item.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Repair Invoice
</asp:Content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Report (PLU)
</asp:content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <p>
        <span class="text-muted">PLU :</span><br />
        <span class="text-lg" id="header_plu" runat="server" />
    </p>
    <hr />
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <%--<asp:TableHeaderCell Width="100" />--%>
                        <asp:TableHeaderCell Width="40" />
                        <asp:TableHeaderCell Width="140">No. Dokumen Repair</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Tanggal</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Operator</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>

