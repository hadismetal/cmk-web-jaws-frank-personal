<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="GJ_CharProduct01.aspx.cs" inherits="Page_Register_GJ_CharProduct01" %>

<%@ register src="MenuGJ.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Gold Jewellery
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update
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
			<div class="col-md-6">
				<table class="table-form table-form-nomargin">
					<tr>
						<td class="fixed-150">Product Item</td>
						<td>:</td>
						<td>
							<asp:dropdownlist id="productitem" runat="server" cssclass="form-control" width="275" />
						</td>
						<td>
							<span id="productitem_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
						</td>
					</tr>
					<tr>
						<td class="fixed-150">Product Category</td>
						<td>:</td>
						<td>
							<asp:dropdownlist id="productcategory" runat="server" cssclass="form-control" width="275"
								autopostback="true" onselectedindexchanged="fillDist" />
						</td>
						<td>
							<span id="productcategory_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
						</td>
					</tr>
					<tr>
						<td class="fixed-150">Product Level</td>
						<td>:</td>
						<td>
							<asp:dropdownlist id="productlevel" runat="server" cssclass="form-control" width="275"
								autopostback="true" onselectedindexchanged="fillDist" />
						</td>
						<td>
							<span id="productlevel_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
						</td>
					</tr>
					<tr>
						<td class="fixed-150">Stone Distribution</td>
						<td>:</td>
						<td>
							<asp:dropdownlist id="stonedist" runat="server" cssclass="form-control" width="275" />
						</td>
						<td>
							<span id="stonedist_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
						</td>
					</tr>
				</table>
			</div>
			<br class="visible-sm visible-xs" />
			<div class="col-md-6">
				<table class="table-form table-form-nomargin">
					<tr>
						<td class="fixed-150">Frame Material</td>
						<td>:</td>
						<td>
							<asp:dropdownlist id="framematerial" runat="server" cssclass="form-control" width="275" />
						</td>
						<td>
							<span id="framematerial_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
						</td>
					</tr>
					<tr>
						<td class="fixed-150">Frame Finishing</td>
						<td>:</td>
						<td>
							<asp:dropdownlist id="framefinishing" runat="server" cssclass="form-control" width="275" />
						</td>
						<td>
							<span id="framefinishing_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
						</td>
					</tr>
					<tr>
						<td class="fixed-150">Frame Color</td>
						<td>:</td>
						<td>
							<asp:dropdownlist id="framecolor" runat="server" cssclass="form-control" width="275" />
						</td>
						<td>
							<span id="framecolor_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
						</td>
					</tr>
					<tr>
						<td class="fixed-150">Construction Process</td>
						<td>:</td>
						<td>
							<asp:dropdownlist id="processcons" runat="server" cssclass="form-control" width="275" />
						</td>
						<td>
							<span id="processcons_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
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
</asp:content>

