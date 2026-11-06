<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJDO_DataDesigner.aspx.cs" Inherits="Page_Register_DJDO_DJDO_DataDesigner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" Runat="Server">
    Diamond Jewelry
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" Runat="Server">
    Register
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" Runat="Server">
    <p>
		<span class="text-muted">Nomor :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<asp:literal id="idbrand" runat="server" visible="false" />
	<hr />
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
				<a id="cancel" runat="server" class="btn btn-default" href="DJDO_Detail.aspx">
					<i class="icon-remove"></i>
					Cancel
				</a>
			</div>
		</div>
		<div class="table-responsive">
			<asp:table id="tb" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
					<asp:tableheadercell width="250">Designer</asp:tableheadercell>
					<asp:tableheadercell width="250">Email</asp:tableheadercell>
					<asp:tableheadercell width="75" cssclass="text-center">Brand</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
</asp:Content>

