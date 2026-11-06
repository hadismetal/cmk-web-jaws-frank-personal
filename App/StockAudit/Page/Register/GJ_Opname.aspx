<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GJ_Opname.aspx.cs" Inherits="Page_GJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Gold Jewellery Stock Audit
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            
            <div class="form-group">
                <asp:TextBox ID="nomeja" CssClass="form-control" runat="server" placeholder="No Meja" />
            </div>
            <div class="form-group">
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="Input PLU" />
            </div>
            <div class="form-group">
                <div>
                    <button id="save" runat="server" class="btn btn-primary" onserverclick="Add">
                        <i class="icon-search"></i>
                        Add
                    </button>
                </div>
            </div>
            <div class="form-group">
                <span id="err" class="text-danger" runat="server" />
            </div>
            
            <div class="table-responsive">
                <table class="table-form" cssclass="table-list text-sm" height="200">
                    <tr>
                        <td>Product Picture</td>
                        <td>:</td>
                        <td>
                            <asp:Image ID="imgpicture" runat="server" CssClass="img-thumbnail" Width="200" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="80">No. PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Harga</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="60">G. Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="60">N. Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="60">Butir</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="60">Carat</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Gambar Tidak Ada</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Gambar Tidak Sesuai</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Bandrol Tidak Sesuai</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">No. Meja</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">Keterangan</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Operator</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Tanggal</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
            
            <div class="form-group">
                <asp:Table ID="tb2" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Qty</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
            <div class="form-group">
                <asp:Table ID="tb3" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="120">Meja</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Qty</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
            <div style="clear: both; font-size: 1px;">&nbsp;</div>
            <div class="form-group">
                <div>
                    <button id="Button2" runat="server" class="btn btn-primary btn-danger" OnClick="if ( ! UserConfirmationClose()) return false;">
                        <i class="icon-cross"></i>
                        Close
                    </button>
                </div>
            </div>
            <div class="form-group">
                <div>
                    <button id="Button1" runat="server" class="btn btn-primary" OnClick="if ( ! UserConfirmation()) return false;" onserverclick="Finish">
                        <i class="icon-search"></i>
                        Finish Opname
                    </button>
                </div>
            </div>
        </form>
    </div>
    <script type="text/javascript">
        function UserConfirmationClose() {
            txtConfirm =
				"Tutup Layar Opname ?\n\n" +
				"transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";
            if (confirm(txtConfirm))
                window.close();
            else
                return false;
        }
    </script>
    <script type="text/javascript">
        function UserConfirmation() {
            txtConfirm =
				"Selesaikan Proses Opname ?\n\n" +
				"transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";
            if (confirm(txtConfirm))
                return true;
            else
                return false;
        }
    </script>
</asp:Content>

