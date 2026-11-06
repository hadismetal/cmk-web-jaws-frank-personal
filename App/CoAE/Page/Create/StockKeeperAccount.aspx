<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StockKeeperAccount.aspx.cs" Inherits="Page_Create_StockKeeperAccount" %>

<%@ Register Src="MenuStockKeeper.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Stock Keeper
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Create New
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <table class="table-file">
        <tr>
            <td class="text-label">Market Brand</td>
            <td>:</td>
            <td id="brand" runat="server" class="text-bold" />
        </tr>
        <tr>
            <td class="text-label"><asp:Literal ID="tipelokasi" runat="server" /></td>
            <td>:</td>
            <td id="lokasi" runat="server" class="text-bold" />
        </tr>
    </table>
    <hr />
    <form runat="server">
        <div class="form-group width-300">
            <label>
                Username :
            </label>
            <asp:DropDownList ID="username" runat="server" CssClass="form-control text-bold" Width="275">
                <asp:ListItem Value="0">Username</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group width-300">
            <label>
                Group :
            </label>
            <asp:DropDownList ID="groupsk" runat="server" CssClass="form-control text-bold" Width="275">
                <asp:ListItem Value="0">Group</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group width-300">
            <label>
                Nama :
            </label>
            <asp:RequiredFieldValidator ID="nama_RF" runat="server" />
            <asp:TextBox ID="nama" runat="server" CssClass="form-control text-bold" MaxLength="50" />
        </div>
        <div class="form-group width-500">
            <label>
                Keterangan :
            </label>
            <asp:TextBox ID="keterangan" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
        </div>
        <table class="table-form">
            <tr>
                <td>Tanggal</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tgl" runat="server" CssClass="form-control text-center" Width="110" />
                    <script>
                        $(document).ready(function () { $('#content_tgl').datepicker({ format: 'dd-mm-yyyy' }); });
                    </script>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="tgl_RF" runat="server" />
                    <span id="tgl_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
            </tr>
        </table>
        <hr />
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
            <a href="StockKeeper_LocationType.aspx" class="btn btn-default">
                <i class="icon-remove"></i>
                Cancel
            </a>
        </div>
    </form>
</asp:Content>

