<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MDR.aspx.cs" Inherits="Page_Approval_Product" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    MDR Rate
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Report
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">

            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="170">EDC</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="170">Jenis Kartu</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Program</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Rate</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130" HorizontalAlign="Center" Style="margin-top: 1em; margin-left:1em; margin-right:1em; text-align: center;">Action</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>
