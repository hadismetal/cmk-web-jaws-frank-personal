<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sales_Data.aspx.cs" Inherits="Page_Register_Sales_Item" %>

<%@ Register Src="MenuSales.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Souvenir
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <p>
        <span class="text-muted">Nomor :</span><br />
        <span class="text-lg" id="header_nomor" runat="server" />
    </p>
    <hr />
    <form id="Form1" runat="server">
       <div class="table-responsive">
            <asp:Table ID="tb3" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="180">
                        <a id="addsv" runat="server" class="btn btn-success" href="Sales_SouvenirAdd.aspx">
                            <i class="icon-plus"></i>
                            Add Souvenir
                        </a>
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Qty</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" OnClick="if ( ! UserConfirmation()) return false;" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Finish
            </button>
        </div>
    </form>
    <script type="text/javascript">
        function goDelete(fooid, fosi, footid) {
            txtConfirm =
				"Lanjutkan Proses DELETE ?\n\n" +
				"Penghapusan data / transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";

            if (confirm(txtConfirm))
                location.href = 'Sales_ItemDelete.aspx?id=' + fooid + '&itemindex=' + fosi + '&iid=' + footid;
        }
        function UserConfirmation() {
            txtConfirm =
				"Lanjutkan Proses SALES ORDER ?\n\n" +
				"transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";
            if (confirm(txtConfirm))
                return true;
            else
                return false;
        }
    </script>
</asp:Content>

