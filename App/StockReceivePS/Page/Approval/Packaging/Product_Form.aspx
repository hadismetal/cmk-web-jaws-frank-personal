<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Product_Form.aspx.cs" Inherits="Page_Register_Packaging_Form" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Packaging
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server">
        <div class="row">
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">No. PO</td>
                        <td>:</td>
                        <td id="nopo" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">No. DO</td>
                        <td>:</td>
                        <td id="nodo" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Tanggal</td>
                        <td>:</td>
                        <td id="tgl" runat="server" />
                    </tr>
                </table>
            </div>
            <br class="visible-sm visible-xs" />
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Keterangan</td>
                        <td>:</td>
                        <td id="keterangan" runat="server" />
                    </tr>
                </table>
            </div>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            Packaging
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Packaging</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass="text-center">Kode</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass="text-center">Qty Receive</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass="text-center">Qty Rusak</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass="text-center">Qty Hilang</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70" CssClass="text-center">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass="text-center">Harga</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <hr />
        <div class="button-left">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-check"></i>
                Approve
            </button>
        </div>
    </form>
</asp:Content>

