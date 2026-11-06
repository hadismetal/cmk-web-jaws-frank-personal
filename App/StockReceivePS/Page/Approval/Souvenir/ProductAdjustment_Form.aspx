<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProductAdjustment_Form.aspx.cs" Inherits="Page_Register_Souvenir_Form" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Souvenir Adjustment
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
            Souvenir
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Souvenir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass="text-center">Kode</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70" CssClass="text-center">Qty</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <hr />
        <div class="button-left">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-check"></i>
                Approve
            </button>
            <button id="Button1" runat="server" class="btn btn-danger" onserverclick="reject_Click">
                <i class="icon-check"></i>
                Reject
            </button>
        </div>
    </form>
</asp:Content>

