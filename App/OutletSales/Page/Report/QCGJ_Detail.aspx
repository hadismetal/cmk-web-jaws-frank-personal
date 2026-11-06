<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="QCGJ_Detail.aspx.cs" Inherits="Page_Approval_GJ_PricingSubmit" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Gold Jewellery
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    QC Detail
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server">
        <p>
            <span class="text-muted">Nomor :</span><br />
            <span class="text-lg" id="header_nomor" runat="server" />
        </p>
        <hr />
        <table class="table-file" width="500">
            <tr>
                <td class="text-muted">Petugas QC</td>
                <td>
                    <asp:Literal ID="petugasqc" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Blemish untuk > 0,3</td>
                <td>
                    <asp:Literal ID="blemish" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Kecocokan Jumlah Butir</td>
                <td>
                    <asp:Literal ID="jlhbutir" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Kecocokan Sertifikat</td>
                <td>
                    <asp:Literal ID="sertifikat" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Grafir Total Carat - Non CZ</td>
                <td>
                    <asp:Literal ID="noncz" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Grafir Total Carat - CZ</td>
                <td>
                    <asp:Literal ID="cz" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Grafir Logo</td>
                <td>
                    <asp:Literal ID="logo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Grafir Kadar Emas</td>
                <td>
                    <asp:Literal ID="kadaremas" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Frame Color</td>
                <td>
                    <asp:Literal ID="framecolor" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Grafir Merk</td>
                <td>
                    <asp:Literal ID="merk" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Fisik Barang Keseluruhan</td>
                <td>
                    <asp:Literal ID="fisik" runat="server" />
                </td>
            </tr>

        </table>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            frame
        </div>
        <table class="table-file">
            <tr>
                <td class="text-label">Netto Weight</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150" id="netto" runat="server" />
                
                <td>
                    <asp:Literal ID="netweight" runat="server" />
                </td>
                <td><small class="text-silver">Gram</small></td>
            </tr>

            <tr>
                <td class="text-label">Kadar Tukaran</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150" id="kadar" runat="server" />
                <td><small class="text-silver">%</small></td>
            </tr>
        </table>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            stone
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-report">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Stone</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tgl.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-center">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-center">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200" CssClass="text-center">Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            price
        </div>
        <div class="row">

            <div class="col-md-5">
                <p>
                    <span class="text-muted">Final Price (IDR) :</span><br />
                    <span class="text-lg text-orange" id="hargaidr" runat="server" />
                    <small class="text-silver">IDR</small><br />
                </p>
            </div>
        </div>
    </form>
</asp:Content>

