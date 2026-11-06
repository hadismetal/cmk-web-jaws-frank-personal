<%@ page language="C#" masterpagefile="~/MasterPageExport.master" autoeventwireup="true" codefile="LD_ListExport.aspx.cs" inherits="Page_LD" %>


<asp:content contentplaceholderid="content" runat="server">

    <div id="form_area" runat="server">
        
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Parcel</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">GIA</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">H. Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Status</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

