<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Summary.aspx.cs" inherits="Page_Summary" %>

<asp:content contentplaceholderid="h1" runat="server">
	Summary
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <h1>All</h1>
            <div class="table-responsive">
                <asp:Table ID="tb5" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="180"></asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">ID</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Tipe Lokasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Nama Lokasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Waktu Mulai</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Operator</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Total Item Sistem</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Total Item Fisik</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
            <h1>Diamond Jewellery</h1>
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="180"></asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">ID</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Tipe Lokasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Nama Lokasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Waktu Mulai</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Operator</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Total Item Sistem</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Total Item Fisik</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
            <h1>Gold Jewellery</h1>
            <div class="table-responsive">
                <asp:Table ID="tb2" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="180"></asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">ID</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Tipe Lokasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Nama Lokasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Waktu Mulai</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Operator</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Total Item Sistem</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Total Item Fisik</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
            <h1>Plain Gold</h1>
            <div class="table-responsive">
                <asp:Table ID="tb3" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="180"></asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">ID</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Tipe Lokasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Nama Lokasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Waktu Mulai</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Operator</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Total Item Sistem</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Total Item Fisik</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
            <h1>Loose Diamond</h1>
            <div class="table-responsive">
                <asp:Table ID="tb4" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="180"></asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">ID</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Tipe Lokasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Nama Lokasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Waktu Mulai</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Operator</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Total Item Sistem</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Total Item Fisik</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:content>

