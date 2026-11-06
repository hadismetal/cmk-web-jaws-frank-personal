<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="Report_EDC_Export.aspx.cs" Inherits="Page_Report_Bank_Report_EDC_Export" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    
    <div id="form_area" runat="server">
        
        <div class="table table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow CssClass="table-primary">
                <asp:TableHeaderCell>No.</asp:TableHeaderCell>
                <asp:TableHeaderCell>SO Number</asp:TableHeaderCell>
				<asp:TableHeaderCell>Lokasi</asp:TableHeaderCell>
				<asp:TableHeaderCell>Tgl SO</asp:TableHeaderCell>
                <asp:TableHeaderCell>Approval Code</asp:TableHeaderCell>
				<asp:TableHeaderCell>Nominal SO</asp:TableHeaderCell>
				<asp:TableHeaderCell>EDC Bank</asp:TableHeaderCell>
				<asp:TableHeaderCell Visible="false">Nominal Gesek</asp:TableHeaderCell>
				<asp:TableHeaderCell>Nominal Cair</asp:TableHeaderCell>
				<asp:TableHeaderCell>Nominal MDR</asp:TableHeaderCell>
                <asp:TableHeaderCell>Status</asp:TableHeaderCell>
				<asp:TableHeaderCell>Credit Card Number</asp:TableHeaderCell>
				<asp:TableHeaderCell>Credit Card Name</asp:TableHeaderCell>
                <asp:TableHeaderCell>Customer</asp:TableHeaderCell>
				<asp:TableHeaderCell>Operator</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

