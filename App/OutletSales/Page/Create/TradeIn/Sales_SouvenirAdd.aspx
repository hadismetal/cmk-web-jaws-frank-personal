<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Sales_SouvenirAdd.aspx.cs" inherits="Page_Register_Sales_ItemAdd" %>

<asp:content contentplaceholderid="h1" runat="server">
	Souvenir
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Search :
			</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
		</div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
				<a id="cancel" runat="server" class="btn btn-default" href="TradeIn_Data.aspx">
					<i class="icon-remove"></i>
					Cancel
				</a>
			</div>
		</div>
		<div style="padding-top:15px;">
			<div class="table-responsive">
				<asp:Table ID="tb_souv_venir" runat="server" CssClass="table table-bordered">
					<asp:TableHeaderRow style="background-color:grey">
						<%--<asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>--%>
						<asp:TableHeaderCell Width="90">Action</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="130">Kode</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="130">Nama</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="100">Total Stock</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="100">Total Out For Sales</asp:TableHeaderCell>
					</asp:TableHeaderRow>
				</asp:Table>
			</div>
        </div>
		<div style="padding-top:15px;">
			<button id="Button1" runat="server" class="btn btn-primary" onserverclick="savesouvenir_Click">
				<i class="icon-ok"></i>
				Save
			</button>
        </div>
	</form>
</asp:content>

