<%@ page language="C#" masterpagefile="~/MasterPageExport.master" autoeventwireup="true" codefile="ProductOrder_Export.aspx.cs" inherits="Page_Summary" %>

<asp:content contentplaceholderid="content" runat="server">1
    <div id="form_area" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30" />
                    <asp:TableHeaderCell Width="120">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">No. Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Product</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="75">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="75">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="75">Designer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">T.B. Emas <br /><span class="text-silver">gr</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">T.H. Emas <br /> <span class="text-silver">USD</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">T.B. Stone <br /> <span class="text-silver">Carat</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">T.H. Stone <br /> <span class="text-silver">USD</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Estimasi Harga <br /> <span class="text-silver">IDR</span></asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

