<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CZ.aspx.cs" Inherits="Page_Register_CZ_CZ" %>

<%@ register src="MenuCZ.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Cubic Zirconia
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Search Delivery Order :
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
			</div>
		</div>

        <div class="table-responsive">
		        <asp:table id="tb1" runat="server" cssclass="table-list">
                    <asp:tableheaderrow>
                        <asp:tableheadercell width="100" cssclass="text-center">Action</asp:tableheadercell>
                        <asp:tableheadercell width="50" cssclass="text-center">No</asp:tableheadercell>
                        <asp:tableheadercell width="150" cssclass="text-center">Delivery Order Number</asp:tableheadercell>
                        <asp:TableHeaderCell Width="100" CssClass="text-center">Approval by</asp:TableHeaderCell>
                        <asp:tableheadercell width="150" cssclass="text-center">Operator Tanggal</asp:tableheadercell>
                        <asp:TableHeaderCell Width="100" CssClass="text-center">Qty</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100" CssClass="text-center">Weight Total</asp:TableHeaderCell>
                        
                        
			        </asp:tableheaderrow>
		        </asp:table>
	        </div>
	</form>
</asp:content>
