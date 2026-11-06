<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="Template_1B.aspx.cs" Inherits="Page_DJ"%>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">

        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list table-striped text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="100">Stone</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Persen</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

