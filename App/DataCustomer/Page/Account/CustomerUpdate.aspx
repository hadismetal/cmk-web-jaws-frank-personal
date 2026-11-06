<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="CustomerUpdate.aspx.cs" Inherits="Page_Account_CustomerUpdate" %>

<%@ Register Src="MenuCustomer.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Customer
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    <asp:Literal ID="h1_st" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_id" runat="server">
    ID:<asp:Literal ID="h1_id" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <%--<script>
        $(document).ready(function () {
            $('#<%=tgl.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
            $('#<%=tgllahir.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
        });
    </script>--%>
    <uc1:Menu ID="menu" runat="server" />
    <form runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="col-xs-12 col-sm-6">
                    Brand<br />
                    <asp:TextBox ID="brand" runat="server" CssClass="form-control text-bold" MaxLength="50" Enabled="false" />
                </div>
                <div class="col-xs-12 col-sm-6">
                    Kode Customer<br />
                    <asp:TextBox ID="kode" runat="server" CssClass="form-control text-bold" MaxLength="50" Enabled="false" />
                </div>
                <div class="col-xs-12 col-sm-12">
                    Nama<br />
                    <asp:RequiredFieldValidator ID="nama_RF" runat="server" />
                    <asp:TextBox ID="nama" runat="server" CssClass="form-control text-bold" MaxLength="50" />
                </div>
                <div class="col-xs-12 col-sm-12">
                    Tgl<br />
                    <asp:TextBox ID="tgl" runat="server" CssClass="form-control text-bold" MaxLength="50" Width="120" Enabled="false" style="text-align:center;" />
                </div>
                <div class="col-xs-12 col-sm-12">
                    Keterangan<br />
                    <asp:TextBox ID="keterangan" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" style="resize: none;" />
                    
                </div>

            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <asp:Literal runat="server" ID="lit_cmk_status" />
                <div class="alert alert-warning">
                    <i>Notes :</i>
                    <ul>
                        <li><i>Nama customer hanya dapat dirubah maksimal di hari yang sama ketika customer tersebut pertama kali melakukan transaksi</i></li>
                        <li><i>Nama customer hanya boleh berupa huruf dan spasi</i></li>
                        <li><i>Jika ditemukan Kode Customer yang sama namun Nama Customer berbeda silahkan konfirmasi ke bagian IT dan CRM</i></li>
                        <li><i>Jika data Kode Customer tidak ditemukan silahkan hubungi ke bagian IT</i></li>
                    </ul>
                </div>
            </div>
        </div>
        
        <div><asp:Literal runat="server" ID="lit_msg" /></div>
        
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-ok"></i>
                Save
            </button>
        </div>
    </form>
</asp:Content>
