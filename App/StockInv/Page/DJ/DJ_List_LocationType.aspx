<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DJ_List_LocationType.aspx.cs" inherits="Page_DJ" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewellery
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <form id="Form1" runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Location Type :
			</div>
			<asp:dropdownlist id="locationtype" runat="server" cssclass="form-control" width="200">
				<asp:listitem Value="1">Warehouse</asp:listitem>
                <asp:listitem Value="2">Outlet</asp:listitem>
                <asp:listitem Value="3">Exhibition</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
			</div>
		</div>
	</form>
</asp:content>

