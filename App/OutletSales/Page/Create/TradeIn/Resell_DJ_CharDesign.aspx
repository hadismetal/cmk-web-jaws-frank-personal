<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Resell_DJ_CharDesign.aspx.cs" inherits="Page_Register_DJ_CharDesign02" %>

<%@ register src="MenuDJ.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewellery
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Nomor :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<hr />
	<form runat="server">
		<div class="row">
			
			<div class="col-md-4">
				<div class="form-group cb-multi">
					<p>
						Finishing :
					</p>
					<p>
						<a href="javascript:checkCtrl('content_cb','true')">Check</a>&nbsp;
						<a href="javascript:checkCtrl('content_cb','false')">Uncheck</a>
					</p>
					<asp:checkboxlist id="cb" runat="server" />
				</div>
			</div>
		</div>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
		</div>
	</form>
</asp:content>

