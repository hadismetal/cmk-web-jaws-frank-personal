<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="DesignerUpdate.aspx.cs" Inherits="Page_Account_DesignerUpdate" %>

<%@ Register Src="MenuDesigner.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Designer
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    <asp:Literal ID="h1_st" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_id" runat="server">
    ID:<asp:Literal ID="h1_id" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <form runat="server">
        <table class="table-form">
            <tr>
                <td>Group</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="group" runat="server" CssClass="form-control text-bold" Width="275" />
                </td>
            </tr>
            <tr>
                <td>Market Brand</td>
                <td>:</td>
                <td>
                    <asp:DropDownList ID="brand" runat="server" CssClass="form-control text-bold" Width="275" Enabled="false" />
                </td>
            </tr>
        </table>
        <hr />
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
        <div class="form-group cb-single">
            <div id="div_draft" runat="server">
                <label for="content_disable">
                    <strong class="text-draft">Status : Draft</strong>
                    <small>Data tidak dimunculkan di dalam sistem</small>
                </label>
            </div>
            <div id="div_disable" runat="server">
                <asp:CheckBox ID="disable" runat="server" />
                <label for="content_disable">
                    <strong class="text-warning">Status : Disable</strong>
                    <small>Data tidak dimunculkan di dalam sistem</small>
                </label>
            </div>
        </div>
        <hr />
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-ok"></i>
                Save
            </button>
            <asp:Button ID="del" runat="server" CssClass="btn btn-danger" Text="Delete" Width="80" OnClick="del_Click" />
            <span id="nodel" runat="server" class="text-validator" visible="false">Disable Delete : File Sudah Digunakan</span>
        </div>
    </form>
</asp:Content>
