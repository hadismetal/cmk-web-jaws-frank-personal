<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="CustomerUpdateAddInfo.aspx.cs" inherits="Page_Account_CustomerUpdateAddr" %>

<%@ register src="MenuCustomer.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Customer
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	<asp:literal id="h1_st" runat="server" />
</asp:content>
<asp:content contentplaceholderid="h1_id" runat="server">
	ID:<asp:literal id="h1_id" runat="server" />
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <script>
        $(document).ready(function () {
            $('#<%=tgllahirpasangan.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
            $('#<%=weddinganniversarry.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
        });
    </script>
	<uc1:menu id="menu" runat="server" />
	<form runat="server">
		<table class="table-form">
            <tr>
				<td>No. Member</td>
				<td>:</td>
				<td>
					<asp:textbox id="nomember" runat="server" cssclass="form-control" width="300" />
				</td>
			</tr>
            <tr>
				<td>No. NPWP</td>
				<td>:</td>
				<td>
					<asp:textbox id="nonpwp" runat="server" cssclass="form-control" width="300" />
				</td>
			</tr>
            <tr>
				<td>Tipe Customer</td>
				<td>:</td>
				<td>
					<asp:DropDownList ID="ddltipecustomer" cssclass="form-control" runat="server" />
				</td>
			</tr>
			<tr>
				<td>Nama Pasangan</td>
				<td>:</td>
				<td>
					<asp:textbox id="namapasangan" runat="server" cssclass="form-control" width="300" />
				</td>
			</tr>
            <tr>
                <td>Tanggal Lahir Pasangan</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tgllahirpasangan" runat="server" CssClass="form-control text-center" Width="110" />
                    
                </td>
            </tr>
            <tr>
                <td>Wedding Anniversarry</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="weddinganniversarry" runat="server" CssClass="form-control text-center" Width="110" />
                    
                </td>
            </tr>
		</table>
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-ok"></i>
				Save
			</button>
		</div>
	</form>
</asp:content>

