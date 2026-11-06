<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Design_Data01.aspx.cs" inherits="Page_Register_Design_Data01" %>

<%@ register src="MenuDesign.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewellery
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
		
		<table class="table-form">
			
			<tr>
				<td class="fixed-150">Request Outlet</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="Design_DataOutlet.aspx" class="btn btn-info" id="search_outlet" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="requestoutletnama" runat="server" cssclass="form-control" maxlength="50" width="300" Enabled="false" />
                          &nbsp &nbsp
                         <asp:Button ID="clearoutlet" class="btn btn-danger" Visible ="false"  runat="server" Text="x" OnClick ="clearoutlet_Click"/>
					</div>
				</td>
			</tr>
            <tr>
				<td class="fixed-150">Request Customer</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="Design_DataCustomer.aspx" class="btn btn-info" id="search_customer" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="requestcustomernama" runat="server" Enabled="false" cssclass="form-control" maxlength="50" width="300" />
                        &nbsp &nbsp
                         <asp:Button ID="clearcustomer" class="btn btn-danger"  Visible ="false"  runat="server" Text="x" OnClick="clearcustomer_Click"/>
					</div>
				</td>
			</tr>
		</table>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
		</div>
	</form>
</asp:content>

