<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="Payment_Export.aspx.cs" Inherits="Page_Report_Payment_Export" %>

<asp:Content ID="Content1"  ContentPlaceHolderID ="content" runat="server">
    
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="130">Payment Type</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" HorizontalAlign="Right">Total Paid</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">MDR</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
 </asp:Content>
