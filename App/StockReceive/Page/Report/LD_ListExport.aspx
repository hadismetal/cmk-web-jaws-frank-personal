<%@ page language="C#" masterpagefile="~/MasterPageExport.master" autoeventwireup="true" codefile="LD_ListExport.aspx.cs" inherits="Page_LD" %>


<asp:content contentplaceholderid="content" runat="server">

    <div id="form_area" runat="server">
        
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">PLU</asp:TableHeaderCell>
                     <asp:TableHeaderCell Width="100">PLU Lama</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Parcel</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">No. GIA</asp:TableHeaderCell>     
                    <asp:TableHeaderCell Width="100">Nama Supplier</asp:TableHeaderCell>                 
                    <asp:TableHeaderCell Width="100" CssClass ="text-right">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass ="text-right">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass ="text-right">Harga Total M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass ="text-right">Harga Total R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass ="text-right">Rappaport</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

