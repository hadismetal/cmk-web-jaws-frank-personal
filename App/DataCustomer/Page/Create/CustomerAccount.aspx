<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CustomerAccount.aspx.cs" Inherits="Page_Create_CustomerAccount" %>

<%@ Register Src="MenuCustomer.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Customer
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Create New
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <script>
        $(document).ready(function () {
            $('#<%=tgl.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
            $('#<%=tgllahir.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
        });
    </script>
    <uc1:Menu ID="menu" runat="server" />
    <table class="table-file">
        <tr>
            <td class="text-label">Market Brand</td>
            <td>:</td>
            <td id="brand" runat="server" class="text-bold" />
        </tr>
    </table>
    <hr />
    <form runat="server">
        <div class="form-group width-300">
            <label>
                Nama :
            </label>
            <asp:RequiredFieldValidator ID="nama_RF" runat="server" />
            <asp:TextBox ID="nama" runat="server" CssClass="form-control text-bold" MaxLength="50" />
        </div>
        <div class="form-group width-300">
            <label>
                Location :
            </label>
            <asp:dropdownlist id="location" runat="server" cssclass="form-control text-bold" width="275" />
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
                    
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="tgl_RF" runat="server" />
                    <span id="tgl_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
            </tr>
            <tr>
                <td>Tanggal Lahir</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tgllahir" runat="server" CssClass="form-control text-center" Width="110" />
                    
                </td>
                <td></td>
            </tr>
            <tr>
                <td class="text-label">Agama</td>
                <td>:</td>
                <td runat="server" class="text-bold">
                    <asp:DropDownList ID="agama" runat="server" CssClass="form-control" Width="200">
                        <asp:ListItem Value="1">Islam</asp:ListItem>
                        <asp:ListItem Value="2">Kristen</asp:ListItem>
                        <asp:ListItem Value="3">Katolik</asp:ListItem>
                        <asp:ListItem Value="4">Buddha</asp:ListItem>
                        <asp:ListItem Value="5">Hindu</asp:ListItem>
                        <asp:ListItem Value="6">Khonghucu</asp:ListItem>
                        <asp:ListItem Value="7" Selected>Lainnya</asp:ListItem>
                    </asp:DropDownList>

                </td>
            </tr>
        </table>
        <hr />
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
            <a href="Customer.aspx" class="btn btn-default">
                <i class="icon-remove"></i>
                Cancel
            </a>
        </div>
    </form>
</asp:Content>

