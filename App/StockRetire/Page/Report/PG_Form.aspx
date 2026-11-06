<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="PG_Form.aspx.cs" Inherits="Page_Approval_PG_Retirement" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Plain Gold
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Report
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
                <td class="text-muted">Petugas Lebur</td>
                <td>
                    <asp:Literal ID="petugaslebur" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Keterangan</td>
                <td>
                    <asp:Literal ID="keterangan" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Alasan</td>
                <td>
                    <asp:Literal ID="alasan" runat="server" />
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
                <td><small class="text-silver">Gram</small></td>
            </tr>
            <tr>
                <td class="text-label">Susut Bongkar</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150">
                    <asp:Literal ID="susutbongkar" runat="server" />
                </td>
                <td><small class="text-silver">Gram</small></td>
            </tr>
            <tr>
                <td class="text-label">Berat Setelah Bongkar</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150">
                    <asp:Literal ID="nettobongkar" runat="server" />
                </td>
                <td><small class="text-silver">Gram</small></td>
            </tr>
            <tr>
                <td class="text-label">Susut Lebur</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150">
                    <asp:Literal ID="susutlebur" runat="server" />
                </td>
                <td><small class="text-silver">Gram</small></td>
            </tr>
            <tr>
                <td class="text-label">Berat Akhir</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150">
                    <asp:Literal ID="beratakhir" runat="server" />
                </td>
                <td><small class="text-silver">Gram</small></td>
            </tr>
            <tr>
                <td class="text-label">Kadar Tukaran</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150" id="kadar" runat="server" />
                <td><small class="text-silver">%</small></td>
            </tr>
            <tr>
                <td class="text-label">Frame Color</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150" id="frame" runat="server" />
                <td></td>
            </tr>
        </table>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            picture
        </div>
        <asp:Image ID="imgpicture" runat="server" CssClass="img-thumbnail" Width="500" />
        <hr />
        
        <%--div class="button-left">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-check"></i>
                Approve
            </button>
            <button id="reject" runat="server" class="btn btn-danger" onserverclick="reject_Click">
                <i class="icon-trash"></i>
                Reject
            </button>
        </--%>
    </form>
</asp:Content>

