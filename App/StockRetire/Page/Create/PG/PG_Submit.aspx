<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="PG_Submit.aspx.cs" Inherits="Page_Approval_PG_PricingSubmit" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Plain Gold
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
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
        <div class="button-left">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-check"></i>
                Submit
            </button>
        </div>
    </form>
</asp:Content>

