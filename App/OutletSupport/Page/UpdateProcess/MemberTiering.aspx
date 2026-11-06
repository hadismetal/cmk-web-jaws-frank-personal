<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="MemberTiering.aspx.cs" inherits="Page_UpdateProcess_MU" %>

<asp:content contentplaceholderid="h1" runat="server">
	Member Tiering
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server">
		<table class="table-form">
            <tr id="err_tr" runat="server">
                <td>
                    <asp:Literal ID="err" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
				<td colspan="2">
                    Tipe Customer
						&nbsp;&nbsp;<asp:Literal runat="server" ID="tipecustomer"></asp:Literal>
				</td>
			</tr>
			<tr>
				<td colspan="2">Rupiah :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="rupiah" runat="server" cssclass="form-control text-lg text-center" width="180" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="rupiah_RF" runat="server" />
					<span id="rupiah_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td colspan="2">Discount :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="discount" runat="server" cssclass="form-control text-lg text-center" width="180" />
						<span class="input-group-addon">%&nbsp;&nbsp;&nbsp;</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="discount_RF" runat="server" />
					<span id="discount_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td colspan="2">Birthday Surprise :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="birthday" runat="server" cssclass="form-control text-lg text-center" width="180" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="birthday_RF" runat="server" />
					<span id="birthday_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
		</table>
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-ok"></i>
				Save
			</button>
			<a href="javascript:closeWindow();" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:content>
