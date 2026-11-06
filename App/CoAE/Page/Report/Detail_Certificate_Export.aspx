<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="Detail_Certificate_Export.aspx.cs" Inherits="Page_Report_Detail_Certificate_Export" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="h1_st" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
     <div id="form_area" runat="server">
        <!-- -->
         <div class="table-responsive">
                          <asp:Table ID="tb" runat="server" CssClass="table-list">
                                    <asp:TableHeaderRow>
                                        <asp:TableHeaderCell Width="150">No PLU</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="260">Item</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="1275">URLs</asp:TableHeaderCell>
                                    </asp:TableHeaderRow>
                            </asp:Table>
                    </div>                   
    </div>
</asp:Content>

