<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Sales_FormVoid.aspx.cs" inherits="Page_Register_Sales_Form" %>

<asp:content contentplaceholderid="h1" runat="server">
	Sales Order
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Void
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p>
		<span class="text-muted">Sales Order :</span><br />
		<span class="text-bold" id="so" runat="server" />
	</p>
	<hr />
	<form runat="server">
        <div class="form-group width-500">
			<label>
				Keterangan Void :
			</label>
			<asp:textbox id="keterangan" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
		</div>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-danger" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Void
			</button>
		</div>
	</form>
</asp:content>
