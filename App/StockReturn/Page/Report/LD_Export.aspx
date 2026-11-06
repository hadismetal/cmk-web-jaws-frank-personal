<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="LD_Export.aspx.cs" Inherits="Page_LD" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="30">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">No. Dokumen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Parcel</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Tanggal</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="140">Nama Supplier</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">No. GIA</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100" CssClass="text-right">Butir</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100" CssClass="text-right">Carat</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120" CssClass="text-right">Harga Total M</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120" CssClass="text-right">Harga Total R</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120" CssClass="text-right">Rappaport</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">Keterangan</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>


