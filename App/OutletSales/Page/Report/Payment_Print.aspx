<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="Payment_Print.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="130">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Total Paid</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">MDR</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

