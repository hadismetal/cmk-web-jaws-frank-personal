<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="LD_CharDesign02.aspx.cs" inherits="Page_Register_LD_CharDesign02" %>

<%@ register src="MenuLD.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Loose Diamond
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

