<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RepairInvoice_Data.aspx.cs" Inherits="Page_Register_Repair_Item" %>

<%@ Register Src="MenuRepairInvoice.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Repair Invoice
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <hr />
    <form id="Form1" runat="server">
        <a id="add" runat="server" class="btn btn-success" href="RepairInvoice_WorkAdd.aspx">
            <i class="icon-plus"></i>
            Add Item
        </a>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="50"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="300">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Price</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Description</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
        </div>
    </form>
    <script type="text/javascript">
        function goDelete(fooid, footid) {
            txtConfirm =
				"Lanjutkan Proses DELETE ?\n\n" +
				"Penghapusan data / transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";

            if (confirm(txtConfirm))
                location.href = 'RepairInvoice_WorkDelete.aspx?id=' + fooid + '&iid=' + footid;
        }
    </script>
</asp:Content>

