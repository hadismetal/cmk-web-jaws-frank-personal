<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="ServicePointExport.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm" 
                style="border-collapse:collapse;">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell RowSpan="2" Width="40" 
                        Style="border:1px solid #000;">No</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="250" 
                        Style="border:1px solid #000;">Tgl Terima</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="300" 
                        Style="border:1px solid #000;">Perajin</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="100" 
                        Style="border:1px solid #000;">NIK</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="250" 
                        Style="border:1px solid #000;">No BON</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="300" 
                        Style="border:1px solid #000;">Outlet Asal</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="300" 
                        Style="border:1px solid #000;">Service Point</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="250" 
                        Style="border:1px solid #000;">Jenis Barang</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="250" 
                        Style="border:1px solid #000;">Kode PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="250" 
                        Style="border:1px solid #000;">Instruksi 1</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="250" 
                        Style="border:1px solid #000;">Instruksi 2</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="250" 
                        Style="border:1px solid #000;">Ket Pekerjaan</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="100" 
                        Style="border:1px solid #000;">Frame Color</asp:TableHeaderCell>
                    <asp:TableHeaderCell ColumnSpan="2" Width="400" 
                        Style="border:1px solid #000;">Awal Rep</asp:TableHeaderCell>
                    <asp:TableHeaderCell ColumnSpan="1" Width="200" 
                        Style="border:1px solid #000;">Akhir Rep</asp:TableHeaderCell>
                    <asp:TableHeaderCell ColumnSpan="2" Width="400" 
                        Style="border:1px solid #000;">Akhir Poles BRJ</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="300" 
                        Style="border:1px solid #000;">Tgl Selesai</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="300" 
                        Style="border:1px solid #000;">Tgl Kirim</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableHeaderCell style="text-align: center;width:200px;border:1px solid #000;">Size</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:200px;border:1px solid #000;">Berat</asp:TableHeaderCell>
                    
                    <asp:TableHeaderCell style="text-align: center;width:200px;border:1px solid #000;">Berat</asp:TableHeaderCell>

                    <asp:TableHeaderCell style="text-align: center;width:200px;border:1px solid #000;">Size</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:200px;border:1px solid #000;">Berat</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

