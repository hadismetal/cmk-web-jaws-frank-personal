<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="DesignerUpdate.aspx.cs" inherits="Page_Group_DesignerUpdate" %>

<asp:content contentplaceholderid="h1" runat="server">
	Designer Group
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	<asp:literal id="h1_st" runat="server" />
</asp:content>
<asp:content contentplaceholderid="h1_id" runat="server">
	ID:<asp:literal id="h1_id" runat="server" />
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div class="row">
		<div class="col-md-10">
			<form runat="server">
				<div class="form-group width-500">
					<div class="row">
						<div class="col-xs-7">
							<label>
								Nama :
							</label>
							<asp:requiredfieldvalidator id="nama_RF" runat="server" />
							<asp:textbox id="nama" runat="server" cssclass="form-control text-bold" maxlength="50" />
						</div>
						<div class="col-xs-3">
							<label>
								Kode :
							</label>
							<asp:requiredfieldvalidator id="namakode_RF" runat="server" />
							<asp:textbox id="namakode" runat="server" cssclass="form-control text-bold text-center" maxlength="20" />
						</div>
					</div>
				</div>
				<div class="form-group width-500">
					<label>
						Keterangan :
					</label>
					<asp:textbox id="keterangan" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
				</div>
				<table class="table-form">
					<tr>
						<td>SN</td>
						<td>:</td>
						<td>
							<asp:textbox id="sn" runat="server" cssclass="form-control text-lg text-center" width="75" />
						</td>
						<td>
							<asp:requiredfieldvalidator id="sn_RF" runat="server" />
							<span id="sn_ERR" runat="server" class="text-validator" visible="false">Format</span>
						</td>
					</tr>
				</table>
				<div class="form-group cb-single">
					<asp:checkbox id="disable" runat="server" />
					<label for="content_disable">
						<strong class="text-warning">Status : Disable</strong>
						<small>Data tidak dimunculkan di dalam sistem</small>
					</label>
				</div>
				<hr />
				<div class="button-inline">
					<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
						<i class="icon-ok"></i>
						Save
					</button>
					<asp:button id="del" runat="server" cssclass="btn btn-danger" text="Delete" width="80" onclick="del_Click" />
					<span id="nodel" runat="server" class="text-validator" visible="false">Disable Delete : File Sudah Digunakan</span>
				</div>
			</form>
		</div>
		<div class="col-md-2">
			<hr class="visible-sm visible-xs" />
			<asp:literal id="ap" runat="server" />
		</div>
	</div>
</asp:content>
