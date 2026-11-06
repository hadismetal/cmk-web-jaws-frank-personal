<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="MDR_Export.aspx.cs" Inherits="Page_Summary" %>


<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="130">EDC</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Bank</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Nominal</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

