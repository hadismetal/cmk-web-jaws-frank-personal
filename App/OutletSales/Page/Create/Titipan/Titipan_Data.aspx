<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Titipan_Data.aspx.cs" Inherits="Page_Register_Titipan_Item" %>

<%@ Register Src="MenuTitipan.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Titipan
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <hr />
    <div class="alert alert-warning" role="alert">
        1 dokumen titipan hanya untuk 1 barang titipan
    </div>
    
    <form id="Form1" runat="server">
        <div class="table-responsive">

            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="180">
                        <a id="add" runat="server" class="btn btn-success" href="Titipan_ItemAdd.aspx">
                            <i class="icon-plus"></i>
                            Add Item
                        </a>
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180">Dokumen QC</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
            <span class="text-danger" id="err" runat="server"></span>
        </div>
    </form>
    <script type="text/javascript">
        function goDelete(fooid, fosi, footid) {
            txtConfirm =
				"Lanjutkan Proses DELETE ?\n\n" +
				"Penghapusan data / transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";

            if (confirm(txtConfirm))
                location.href = 'Titipan_ItemDelete.aspx?id=' + fooid + '&itemindex=' + fosi + '&iid=' + footid;
        }
    </script>
</asp:Content>

