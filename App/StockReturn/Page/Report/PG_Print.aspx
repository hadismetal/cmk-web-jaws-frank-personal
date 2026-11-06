<%@ Page Language="C#" masterpagefile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="PG_Print.aspx.cs" Inherits="Page_Report_PG_List" %>

<asp:content contentplaceholderid="content" runat="server">
    <style>
    @media print {
        @page {
            size: landscape;
        }
    }
</style>

    <div class="row">
        <div class="col-md-12">
            <h2>Stock Return PG</h2>
        </div>
    </div>
    <div class="row">
        <div class="col-md-11 text-right" id="Dari" runat="server"></div>
    </div>  
    <div class="row">
        <div class="col-md-11 text-right" id="Sampai" runat="server"></div>
    </div>
    <div id="form_area" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="125">No. Dokumen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Images</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">No. PLU</asp:TableHeaderCell>                                        
                    <asp:TableHeaderCell Width="100">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Kadar Logam</asp:TableHeaderCell>                    
                    <asp:TableHeaderCell Width="100">Frame Color</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Resell Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>
