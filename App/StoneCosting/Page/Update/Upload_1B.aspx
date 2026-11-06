<%@ page language="C#" masterpagefile="~/MasterFile.master" AutoEventWireup="true" codefile="Upload_1B.aspx.cs" inherits="Page_Update_Process_1B" %>

<asp:content contentplaceholderid="h1" runat="server">
    Diamond Rapaport
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
    Update Process
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    
	<form runat="server" id="form">
		<table class="table-form">
			<tr>
				<td>
					<div class="input-group">
						 <asp:FileUpload ID="FileUpload1" runat="server" class="form-control"/>
					</div>
				</td>
				<td>
					<asp:Button ID="btnUpload" runat="server" Text="Upload" class="btn btn-primary" OnClick="uploadData" />
				</td>
			</tr>
			<tr>
				<td><asp:Label runat="server" ID="error" Visible="false" ForeColor="Red"></asp:Label></td>
			</tr>
		</table>
		<hr />
		<asp:Button ID="cancel" class="btn" runat="server" OnClick="cancel_Click" Text="Cancel"/>
	</form>
</asp:content>
