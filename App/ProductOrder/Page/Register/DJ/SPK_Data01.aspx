<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="SPK_Data01.aspx.cs" inherits="Page_Register_SPK_Data01" %>

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
		
		<table class="table-form">
            <tr>
				<td class="fixed-150">Request Location</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="SPK_DataOutlet.aspx" class="btn btn-info" id="search_outlet" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="requestoutletnama" runat="server" cssclass="form-control" Enabled="false" maxlength="50" width="400" />
                        &nbsp &nbsp
                        <%-- <a href="SPK_ClearOutlet.aspx" class="btn btn-danger" id="clear_outlet" runat="server" visible="false">
                                <i class="glyphicon glyphicon-remove"></i>
                            </a>--%>
                         <asp:Button ID="clearoutlet" class="btn btn-danger"  runat="server" visible="false" OnClick="clearoutlet_Click" Text="X"/>
					</div>
				</td>
			</tr>
			<tr>
				<td class="fixed-150">Request Customer</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="SPK_DataCustomer.aspx" class="btn btn-info" id="search_customer" runat="server">
								<i class="icon-search"></i>
							</a>
                            
						</div>
						<asp:textbox id="requestcustomernama" runat="server" cssclass="form-control" Enabled="false" maxlength="50" width="400" />
                         &nbsp &nbsp
                       <%-- <a href="SPK_ClearCustomer.aspx" class="btn btn-danger" id="clear_customer" runat="server" visible="false">
                                <i class="glyphicon glyphicon-remove"></i>
                            </a>--%>
                        <asp:Button ID="clearcustomer" class="btn btn-danger"  runat="server" visible="false" OnClick="clearcustomer_Click" Text="X"/>
					</div>
				</td>
			</tr>

            <tr>
				<td class="fixed-150">No. PO</td>
				<td>:</td>
				<td>
                    <div class="input-group">
						<div class="input-group-btn">
							<a href="SPK_DataPO.aspx" class="btn btn-info" id="search_po" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="nopo" runat="server" cssclass="form-control" Enabled="false" maxlength="50" width="400" />
                         &nbsp &nbsp
                       <%-- <a href="SPK_ClearPO.aspx" class="btn btn-danger" id="clear_po" runat="server" visible="false">
                                <i class="glyphicon glyphicon-remove"></i>
                            </a>--%>

                        <asp:Button ID="clearpo" class="btn btn-danger"  runat="server" visible="false" OnClick="clearpo_Click" Text="X" />
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

