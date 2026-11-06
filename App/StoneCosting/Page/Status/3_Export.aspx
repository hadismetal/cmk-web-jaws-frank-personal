<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="3_Export.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">

        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list table-striped text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Nama</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="175">Approval</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-right"><small>USD</small> Harga</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

