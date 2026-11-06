<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Summary.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Summary
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form runat="server">
        <div id="div_setup" runat="server" class="button-left">
            <p>
                <a class="btn btn-primary" href="javascript:goSetup()">
                    <i class="icon-fixed-width icon-wrench"></i>
                    Setup Database
                </a>
                &nbsp;
            <button id="go" runat="server" class="btn btn-primary" onserverclick="Sync">
                <i class="icon-fixed-width icon-wrench"></i>
                Sync
            </button>
                &nbsp;
            <button id="Button1" runat="server" class="btn btn-primary" onserverclick="SyncFrank">
                <i class="icon-fixed-width icon-wrench"></i>
                Sync Frank n Co
            </button>
                &nbsp;
            <button id="Button2" runat="server" class="btn btn-primary" onserverclick="SyncPalace">
                <i class="icon-fixed-width icon-wrench"></i>
                Sync Palace
            </button>
            </p>
            <hr />
        </div>
    </form>
    <script type="text/javascript">
        function goSetup() {
            txtConfirm =
				"Lanjutkan Proses SETUP ?\n\n" +
				"Apabila proses memakan waktu cukup lama :\n" +
				"Harap layar browser jangan ditutup sampai proses selesai.";

            if (confirm(txtConfirm))
                location.href = 'Setup.aspx';
        }
    </script>
    <div class="table-responsive">
        <asp:Table ID="tb" runat="server" CssClass="table-list">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="200">Stone</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150" CssClass="text-center">Normal</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150" CssClass="text-center">Pending Update</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150" CssClass="text-center">Pending Approval</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150" CssClass="text-center">Invalid Pricing</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
</asp:Content>

