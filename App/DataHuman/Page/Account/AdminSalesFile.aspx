<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="AdminSalesFile.aspx.cs" Inherits="Page_Account_AdminSalesFile" %>

<%@ Register Src="MenuAdminSales.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    AdminSales
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    <asp:Literal ID="h1_st" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_id" runat="server">
    ID:<asp:Literal ID="h1_id" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <div class="row">
        <div class="col-md-10">
            <div class="file-subheader hidden-sm hidden-xs">
                master
            </div>
            <table class="table-file">
                <tr>
                    <td class="text-label">Brand</td>
                    <td>:</td>
                    <td id="brand" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">
                        <asp:Literal ID="tipelokasi" runat="server" /></td>
                    <td>:</td>
                    <td id="lokasi" runat="server" />
                </tr>
            </table>
            <hr />
            <div class="file-subheader hidden-sm hidden-xs">
                account
            </div>
            <table class="table-file">
                <tr>
                    <td class="text-label">Keterangan</td>
                    <td>:</td>
                    <td id="keterangan" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Tanggal</td>
                    <td>:</td>
                    <td id="tgl" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Status</td>
                    <td>:</td>
                    <td id="disable" runat="server" />
                </tr>
            </table>
            <hr />
            <div class="file-subheader hidden-sm hidden-xs">
                address
            </div>
            <table class="table-file">
                <tr>
                    <td class="text-label">Alamat Domisili</td>
                    <td>:</td>
                    <td id="addralamat" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">No. Telepon</td>
                    <td>:</td>
                    <td id="addrnotelp" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">No. Fax</td>
                    <td>:</td>
                    <td id="addrnofax" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Alamat Email</td>
                    <td>:</td>
                    <td id="addremail" runat="server" />
                </tr>
            </table>
        </div>
        <div class="col-md-2">
            <hr class="visible-sm visible-xs" />
            <asp:Literal ID="ap" runat="server" />
        </div>
    </div>
</asp:Content>

