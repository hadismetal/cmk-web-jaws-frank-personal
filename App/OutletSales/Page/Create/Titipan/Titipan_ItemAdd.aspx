<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Titipan_ItemAdd.aspx.cs" inherits="Page_Register_Titipan" %>

<%@ register src="MenuTitipan.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content ID="Content1" contentplaceholderid="h1" runat="server">
	Titipan
</asp:content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content ID="Content3" contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form id="Form1" runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Search :
			</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
		</div>
		<div class="form-group">
			<div class="text-muted">
				Search By :
			</div>
			<asp:dropdownlist id="searchby" runat="server" cssclass="form-control" width="200">
				<asp:listitem Value="1">Sales Order</asp:listitem>
                <asp:listitem Value="2">PLU</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<%--<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
                &nbsp;--%>
                <button id="Button1" runat="server" class="btn btn-primary" onserverclick="AddPLU">
					<i class="icon-search"></i>
					PLU Titipan
				</button>
			</div>
		</div>
		<div class="table-responsive">
            <h3>Diamond Jewellery</h3>
			<asp:table id="tb" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
                    <asp:tableheadercell width="150">Image</asp:tableheadercell>
					<asp:tableheadercell width="150">Sales Order</asp:tableheadercell>
					<asp:tableheadercell width="150">PLU</asp:tableheadercell>
                    <asp:tableheadercell width="150">Product Item</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
</asp:content>

