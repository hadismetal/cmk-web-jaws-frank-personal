<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="PG_Form.aspx.cs" Inherits="Page_Approval_PG_Retirement" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Plain Gold
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Jawaban Lebur
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
                    <asp:TextBox ID="petugaslebur" runat="server" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Keterangan</td>
                <td>
                    <asp:TextBox ID="keterangan" runat="server" CssClass="form-control" TextMode="MultiLine" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Alasan</td>
                <td>
                    <asp:TextBox ID="alasan" runat="server" CssClass="form-control" TextMode="MultiLine" />
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
                    <asp:TextBox ID="susutbongkar" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CalcSusutBongkar" />
                </td>
                <td><small class="text-silver">Gram</small></td>
            </tr>
            <tr>
                <td class="text-label">Berat Setelah Bongkar</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150">
                    <asp:TextBox ID="nettobongkar" ReadOnly="true" runat="server" CssClass="form-control" />
                </td>
                <td><small class="text-silver">Gram</small></td>
            </tr>
            <tr>
                <td class="text-label">Susut Lebur</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150">
                    <asp:TextBox ID="susutlebur" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="CalcSusutLebur" />
                </td>
                <td><small class="text-silver">Gram</small></td>
            </tr>
            <tr>
                <td class="text-label">Berat Akhir</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150">
                    <asp:TextBox ID="beratakhir" ReadOnly="true" runat="server" CssClass="form-control" />
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
        
        <div class="button-left">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-check"></i>
                Save
            </button>
        </div>
    </form>
</asp:Content>

