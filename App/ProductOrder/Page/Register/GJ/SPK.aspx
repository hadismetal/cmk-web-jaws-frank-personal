<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SPK.aspx.cs" Inherits="Page_Register_Design" %>

<%@ Register Src="MenuSPK.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Gold Jewellery
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <div id="spk_ERR" runat="server" visible="false">
		<i class="icon-warning-sign icon-5x pull-left text-red"></i>
		<h4>No. Ref Invalid</h4>
		<p class="text-muted">
			Transaksi tidak bisa di-input ke dalam sistem.<br />
			No. Referensi tidak ditemukan
		</p>
		<div class="clearfix"></div>
		<hr />
	</div>
    <form id="Form1" runat="server">
        <%--<table class="table-form">
            <tr>
                <td class="fixed-150">Tipe</td>
                <td>:</td>
                <td>
                    <asp:RadioButtonList ID="SPK" runat="server">
                        <asp:ListItem Text="New Order" Value="1" Selected />
                        <asp:ListItem Text="Repeat Order" Value="2" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            
        </table>--%>
        <div>
            <label>Tipe SPK :</label><br />
            <div class="checkbox"><asp:RadioButton runat="server" GroupName="rdo_spk" ID="rdo_no" Text=" New Order" Checked="true" /></div>
            <div class="checkbox"><asp:RadioButton runat="server" GroupName="rdo_spk" ID="rdo_ro" Text=" Repeat Order" /></div>
        </div>
        
        <div>
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="go" runat="server" class="btn btn-primary" onserverclick="getData">
                    <i class="icon-arrow-right"></i>
                    Next
                </button>
            </div>
        </div>

    </form>
</asp:Content>

