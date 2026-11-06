<%@ page language="C#" masterpagefile="~/MasterPageExport.master" autoeventwireup="true" codefile="LD_ListExport_BU.aspx.cs" inherits="Page_LD" %>


<asp:content contentplaceholderid="content" runat="server">

    <div id="form_area" runat="server">
        
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                    
                    <asp:TableHeaderCell Width="80">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Modal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Supplier</asp:TableHeaderCell>
                    
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

