<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="SPK_CharDesign01.aspx.cs" inherits="Page_Register_SPK_CharDesign01" %>

<%@ Register Src="MenuSPK.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="HeaderSPK.ascx" TagName="Header" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <uc2:Header ID="header" runat="server" />
    <uc1:Menu ID="menu" runat="server" />

	<form runat="server">
		<div class="row">
			<div class="col-md-6">
				<table class="table-form table-form-nomargin">
					<tr>
						<td class="fixed-150">Design Category</td>
						<td>:</td>
						<td>
							<asp:dropdownlist id="designcategory" runat="server" cssclass="form-control" width="275" />
						</td>
						<td>
							<span id="designcategory_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
						</td>
					</tr>
					<tr>
						<td class="fixed-150">Design Concept</td>
						<td>:</td>
						<td>
							<asp:dropdownlist id="designconcept" runat="server" cssclass="form-control" width="275" />
						</td>
						<td>
							<span id="designconcept_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
						</td>
					</tr>
					<tr>
						<td class="fixed-150">Design Process</td>
						<td>:</td>
						<td>
							<asp:dropdownlist id="designprocess" runat="server" cssclass="form-control" width="275" />
						</td>
						<td>
							<span id="designprocess_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
						</td>
					</tr>
				</table>
			</div>
			<br class="visible-sm visible-xs" />
			<div class="col-md-6">
				<table class="table-form table-form-nomargin">
					<tr>
						<td class="fixed-150">Target Age</td>
						<td>:</td>
						<td>
							<asp:dropdownlist id="targetage" runat="server" cssclass="form-control" width="275" />
						</td>
						<td>
							<span id="targetage_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
						</td>
					</tr>
					<tr>
						<td class="fixed-150">Target Gender</td>
						<td>:</td>
						<td>
							<asp:dropdownlist id="targetgender" runat="server" cssclass="form-control" width="275" />
						</td>
						<td>
							<span id="targetgender_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
						</td>
					</tr>
				</table>
			</div>
		</div>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
		</div>
	</form>
</asp:Content>

