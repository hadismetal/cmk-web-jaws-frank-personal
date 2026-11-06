<%@ page language="C#" masterpagefile="~/MasterAP.master" autoeventwireup="true" codefile="AttachCreate.aspx.cs" inherits="AP_AttachCreate" %>

<asp:content contentplaceholderid="h1" runat="server">
	Attachment
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server">
		<p class="text-muted">
			Max. Size : 
			<span id="kb" runat="server" class="text-bold text-danger" />
		</p>
		<table class="table-form">
			<tr>
				<td>Upload File</td>
				<td>:</td>
				<td>
					<asp:fileupload id="file_1" runat="server" cssclass="form-control" />
				</td>
				<td>Nama File</td>
				<td>:</td>
				<td>
					<asp:textbox id="nama_1" runat="server" cssclass="form-control" width="200" />
				</td>
			</tr>
			<tr>
				<td>Upload File</td>
				<td>:</td>
				<td>
					<asp:fileupload id="file_2" runat="server" cssclass="form-control" />
				</td>
				<td>Nama File</td>
				<td>:</td>
				<td>
					<asp:textbox id="nama_2" runat="server" cssclass="form-control" />
				</td>
			</tr>
			<tr>
				<td>Upload File</td>
				<td>:</td>
				<td>
					<asp:fileupload id="file_3" runat="server" cssclass="form-control" />
				</td>
				<td>Nama File</td>
				<td>:</td>
				<td>
					<asp:textbox id="nama_3" runat="server" cssclass="form-control" />
				</td>
			</tr>
			<tr>
				<td>Upload File</td>
				<td>:</td>
				<td>
					<asp:fileupload id="file_4" runat="server" cssclass="form-control" />
				</td>
				<td>Nama File</td>
				<td>:</td>
				<td>
					<asp:textbox id="nama_4" runat="server" cssclass="form-control" />
				</td>
			</tr>
			<tr>
				<td>Upload File</td>
				<td>:</td>
				<td>
					<asp:fileupload id="file_5" runat="server" cssclass="form-control" />
				</td>
				<td>Nama File</td>
				<td>:</td>
				<td>
					<asp:textbox id="nama_5" runat="server" cssclass="form-control" />
				</td>
			</tr>
		</table>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-ok"></i>
				Save
			</button>
			<a id="back" runat="server" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:content>

