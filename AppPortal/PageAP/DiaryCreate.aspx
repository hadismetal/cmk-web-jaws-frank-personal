<%@ page language="C#" masterpagefile="~/MasterAP.master" autoeventwireup="true" codefile="DiaryCreate.aspx.cs" inherits="AP_DiaryCreate" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diary
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server">
		<div class="form-group width-700">
			<label>
				Input Catatan :
			</label>
			<asp:requiredfieldvalidator id="catatan_RF" runat="server" />
			<asp:textbox id="catatan" runat="server" cssclass="form-control" textmode="MultiLine" rows="10" />
		</div>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-ok"></i>
				Save
			</button>
			<a id="back" runat="server" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:content>

