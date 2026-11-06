<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="Design_Print.aspx.cs" Inherits="Page_Register_Design_Design" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div class="row">

        <table class="table-file">
            <tr>
                <td>
                    <asp:Literal ID="customernama" runat="server" /></td>
                <td>&nbsp;</td>
                <td>Code :
                    <asp:Literal ID="Kode" runat="server" />
                    <br /><br />
                </td>
            </tr>
            <tr>
                <td class="text-label">
                    <asp:Image ID="imgpicture" runat="server" CssClass="img-thumbnail" Width="500" /></td>
                <td>&nbsp;</td>
                <td width="400">


                    <b>Tanggal</b> :
                    <br />
                    <asp:Literal ID="tgl" runat="server" />
                    <br /><br />
                    <asp:Table runat="server" CssClass="table table-striped" ID="tbl_List_Parcel" />
                    <br /><br />
                    <b>Estimasi Harga Sementara</b> : 
                    <asp:Literal ID="estimasi_harga" runat="server" /><br />
                </td>
            </tr>
        </table>
    </div>
    <hr />
    <div class="row">
        <table class="table-file">
            <tr>
                <td width="500">
                    <b>Approval</b>:
                    
                        <asp:Literal ID="ApprovedBy" runat="server" />
                </td>
                <td>&nbsp;</td>
                <td width="400">
                    <b>Nama Designer</b> :
                    
                        <asp:Literal ID="NamaDesigner" runat="server" />
                    <br />
                    <br />
                    <b>Store</b> :
                    
                        <asp:Literal ID="Toko" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

