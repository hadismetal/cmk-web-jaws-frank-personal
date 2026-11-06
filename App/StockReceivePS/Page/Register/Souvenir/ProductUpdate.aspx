<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="ProductUpdate.aspx.cs" inherits="Page_Register_DJ_ProductUpdate" %>

<%@ register src="Menu.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Souvenir
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	
	<form runat="server">
		<table class="table-form">
            <tr>
                <td>Qty</td>
                <td>:</td>
                <td>
                    <div class="input-group">
                        <asp:TextBox ID="qty" runat="server" CssClass="form-control text-lg text-right" Width="300" />
                        
                    </div>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="qty_RF" runat="server" />
                    <span id="qty_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
            </tr>
            <tr>
                <td>Harga</td>
                <td>:</td>
                <td>
                    <div class="input-group">
                        <asp:TextBox ID="harga" runat="server" CssClass="form-control text-lg text-right" Width="300" />
                        <span class="input-group-addon">IDR</span>
                    </div>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="harga_RF" runat="server" />
                    <span id="harga_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
            </tr>
        </table>
        <hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<a id="cancel" runat="server" class="btn btn-default" onserverclick="cancel_Click">
				<i class="icon-remove"></i>
				Cancel
			</a>
			
		</div>
	</form>
</asp:content>

