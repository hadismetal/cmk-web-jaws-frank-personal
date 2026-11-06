<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="LD_Submit.aspx.cs" Inherits="Page_Approval_LD_PricingSubmit" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Loose Diamond
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    QC
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
                    <asp:TextBox ID="petugasqc" runat="server" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Keterangan</td>
                <td>
                    <asp:TextBox ID="keterangan" runat="server" TextMode ="MultiLine" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Blemish untuk > 0,3</td>
                <td>
                    <asp:TextBox ID="blemish" runat="server" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Kecocokan Jumlah Butir</td>
                <td>
                    <asp:TextBox ID="jlhbutir" runat="server" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Kecocokan Sertifikat</td>
                <td>
                    <asp:TextBox ID="sertifikat" runat="server" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Grafir Total Carat - Non CZ</td>
                <td>
                    <asp:TextBox ID="noncz" runat="server" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Grafir Total Carat - CZ</td>
                <td>
                    <asp:TextBox ID="cz" runat="server" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Grafir Logo</td>
                <td>
                    <asp:TextBox ID="logo" runat="server" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Grafir Kadar Emas</td>
                <td>
                    <asp:TextBox ID="kadaremas" runat="server" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Frame Color</td>
                <td>
                    <asp:TextBox ID="framecolor" runat="server" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Grafir Merk</td>
                <td>
                    <asp:TextBox ID="merk" runat="server" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Fisik Barang Keseluruhan</td>
                <td>
                    <asp:TextBox ID="fisik" runat="server" CssClass="form-control" />
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
                    <asp:TextBox ID="netweight" runat="server" CssClass="form-control" />
                </td>
                <td><small class="text-silver">Gram</small></td>
            </tr>

            <tr>
                <td class="text-label">Kadar Logam</td>
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
        <hr />

        <div class="button-left">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-check"></i>
                Submit
            </button>
        </div>
    </form>
</asp:Content>

