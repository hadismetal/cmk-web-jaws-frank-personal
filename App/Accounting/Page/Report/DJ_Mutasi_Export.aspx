<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="DJ_Mutasi_Export.aspx.cs" Inherits="Page_Report_DJ_Mutasi_Export" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <asp:Table ID="Table1" runat="server" CssClass="table-list text-sm">
            
        </asp:Table>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table table-bordered">
                <asp:TableRow>
                    <asp:TableHeaderCell Width="250" RowSpan="3" BorderWidth="1" BorderColor="Gray"><asp:Label runat="server" ID="descHeader">Deskripsi</asp:Label></asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" Width="200" ColumnSpan="6" RowSpan="2" BorderWidth="1" BorderColor="Gray">Saldo Awal</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" Width="800" ColumnSpan="24" BorderWidth="1" BorderColor="Gray">Penerimaan</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" Width="1000" ColumnSpan="30" BorderWidth="1" BorderColor="Gray">Pengeluaran</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" Width="200" ColumnSpan="6" RowSpan="2" BorderWidth="1" BorderColor="Gray">Akhir Mutasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" Width="200" ColumnSpan="6" RowSpan="2" BorderWidth="1" BorderColor="Gray">Akhir Opname</asp:TableHeaderCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell style="text-align: center" ColumnSpan="6" BorderWidth="1" BorderColor="Gray">Receiving</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" ColumnSpan="6" BorderWidth="1" BorderColor="Gray">Incoming</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" ColumnSpan="6" BorderWidth="1" BorderColor="Gray">Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" ColumnSpan="6" BorderWidth="1" BorderColor="Gray">Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" ColumnSpan="6" BorderWidth="1" BorderColor="Gray">Outgoing</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" ColumnSpan="6" BorderWidth="1" BorderColor="Gray">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" ColumnSpan="6" BorderWidth="1" BorderColor="Gray">Gift</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" ColumnSpan="6" BorderWidth="1" BorderColor="Gray">Return</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" ColumnSpan="6" BorderWidth="1" BorderColor="Gray">Lebur</asp:TableHeaderCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga M</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>