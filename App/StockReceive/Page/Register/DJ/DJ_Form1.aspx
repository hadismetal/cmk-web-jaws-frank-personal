<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DJ_Form1.aspx.cs" inherits="Page_Register_DJ_Form" %>


<asp:content contentplaceholderid="content" runat="server">
	
	<form runat="server">
		
		<div class="form-group width-500">
			<label>
				Query :
			</label>
			<asp:textbox id="keterangan" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
		</div>
		<div class="form-group width-500">
			<label>
				result :
			</label>
			<asp:textbox id="result" runat="server" cssclass="form-control" textmode="MultiLine" />
		</div>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Execute
			</button>
		</div>
	</form>
</asp:content>
