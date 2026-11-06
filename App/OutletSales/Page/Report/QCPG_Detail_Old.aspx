<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="QCPG_Detail.aspx.cs" Inherits="Page_Approval_PG_PricingSubmit" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Plain Gold
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
                <td class="text-muted">Grafir Logo</td>
                <td>
                    <asp:Literal ID="logo" runat="server" />
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
    </form>
</asp:Content>

