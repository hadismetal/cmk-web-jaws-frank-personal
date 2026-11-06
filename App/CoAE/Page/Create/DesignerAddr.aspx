<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DesignerAddr.aspx.cs" inherits="Page_Create_DesignerAddr" %>

<%@ register src="MenuDesigner.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Designer
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Nama :</span><br />
		<span class="text-lg" id="header_nama" runat="server" />
	</p>
	<hr />
	<form runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-6">
                
		        <table class="table-form">
			        <tr>
				        <td>No. Telepon</td>
				        <td>:</td>
				        <td>
					        <asp:textbox id="addrnotelp" runat="server" cssclass="form-control" width="300" />
				        </td>
			        </tr>
			        <tr>
				        <td>No. Fax</td>
				        <td>:</td>
				        <td>
					        <asp:textbox id="addrnofax" runat="server" cssclass="form-control" width="300" />
				        </td>
			        </tr>
			        <tr>
				        <td>Alamat Email</td>
				        <td>:</td>
				        <td>
					        <asp:textbox id="addremail" runat="server" cssclass="form-control" width="300" />
				        </td>
			        </tr>
		        </table>
            </div>
            <div class="col-xs-12 col-sm-6">
                <div class="form-group width-500">
			        <label>
				        Alamat Domisili :
			        </label>
			        <asp:textbox id="addralamat" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
		        </div>
            </div>
        </div>
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
		</div>
	</form>
</asp:content>

