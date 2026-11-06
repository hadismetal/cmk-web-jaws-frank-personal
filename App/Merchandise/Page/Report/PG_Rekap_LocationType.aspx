<%@ Page Language="C#" masterpagefile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PG_Rekap_LocationType.aspx.cs" Inherits="Page_Report_LD_Rekap_LocationType" %>

<asp:content contentplaceholderid="h1" runat="server">
	Plain Gold Rekap
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <form id="Form1" runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Location Type :
			</div>
			<asp:dropdownlist id="locationtype" runat="server" cssclass="form-control" width="200">
				<asp:listitem Value="0">Warehouse</asp:listitem>
                <asp:listitem Value="1">Outlet</asp:listitem>
                <asp:listitem Value="2">Exhibition</asp:listitem>
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
