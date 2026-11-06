<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="Stone1B_Update.aspx.cs" Inherits="Page_Account_Stone1BUpdate" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Rapaport
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    <asp:Literal ID="h1_st" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_id" runat="server">
    ID:<asp:Literal ID="h1_id" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <script>
        $(document).ready(function () {
            $('#<%=tgl.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
        });
    </script>

    <form runat="server">
        <table class="table-form">
            <tr>
                <td>Kode PLU</td>
                <td>:</td>
                <td id="plu" runat="server"></td>
            </tr>
            <tr>
                <td>Parcel</td>
                <td>:</td>
                <td id="parcel" runat="server"></td>
            </tr>
        </table>
        <hr />
        <div class="form-group width-300">

            <label>
                GIA :
            </label>
            <asp:RequiredFieldValidator ID="gia_RF" runat="server" />
            <asp:TextBox ID="gia" runat="server" CssClass="form-control text-bold" MaxLength="50" />
        </div>
        <div class="form-group width-300">
            <label>
                Tanggal Expired :
            </label>
            <asp:TextBox ID="tgl" runat="server" CssClass="form-control text-center" Width="110" />
        </div>
        <hr />
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-ok"></i>
                Save
            </button>
        </div>
    </form>
</asp:Content>
