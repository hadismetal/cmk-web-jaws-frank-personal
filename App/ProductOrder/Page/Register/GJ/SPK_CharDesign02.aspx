<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="SPK_CharDesign02.aspx.cs" inherits="Page_Register_SPK_CharDesign02" %>

<%@ register src="MenuSPK.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Gold Jewellery
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
				<div class="form-group width-300">
					<label>
						Naming Product :
					</label>
					<asp:textbox id="naming" runat="server" cssclass="form-control" maxlength="50" />
				</div>
				<div class="form-group cb-single">
					<label>
						Keterangan :
					</label>
					<br />
					<asp:checkbox id="setitem" runat="server" />
					<label for="content_setitem">
						<strong>Set Item</strong>
					</label>
					<br />
					<asp:checkbox id="special" runat="server" />
					<label for="content_special">
						<strong>Special Design</strong>
					</label>
				</div>
				<div class="form-group width-300">
					<label>
						Grafir :
					</label>
					<asp:textbox id="grafir" runat="server" cssclass="form-control" maxlength="38" />
					<i class="text-muted">Max. 38 karakter</i>
				</div>
			</div>
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

