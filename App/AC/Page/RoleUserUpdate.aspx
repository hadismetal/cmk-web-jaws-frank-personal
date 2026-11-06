<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="RoleUserUpdate.aspx.cs" inherits="Page_RoleUserUpdate" %>

<asp:content contentplaceholderid="h1" runat="server">
	Role Settings
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <style type="text/css">
        table.table-list tr td {
            text-align:center;
            vertical-align:middle;
        }
    </style>
	<div class="row">
		<div class="col-md-3">
			<p class="text-nowrap">
				<span class="text-warning" id="header_group" runat="server" />
				<br />
				<span class="text-lg" id="header_module" runat="server" />
			</p>
		</div>
		<div class="col-md-3">
			<p>
				<span class="text-muted">User Account :</span><br />
				<span class="text-lg" id="header_user" runat="server" />
			</p>
		</div>
	</div>
	<hr />
	<form runat="server">
		<script type="text/javascript">
			function roleCheck(foo) {
				document.getElementById("content_role01").checked = foo.checked;
				document.getElementById("content_role02").checked = foo.checked;
				document.getElementById("content_role21").checked = foo.checked;
				document.getElementById("content_role22").checked = foo.checked;
			}
		</script>
		<div class="table-responsive">
			<table class="table-list">
				<tr>
					<th style="width: 30px;">
						<input type="checkbox" onclick="roleCheck(this);" />
					</th>
					<th style="width: 150px;">Role</th>
					<th style="width: 600px;">Keterangan</th>
				</tr>
				<tr class="vertical-top">
					<td>
						<asp:checkbox id="role01" runat="server" />
					</td>
					<td>
						<label for="content_role01" class="text-addheight">
							<strong>Manager</strong>
						</label>
					</td>
					<td class="text-muted" style="text-align:left; vertical-align:middle;">
						<p>Fungsi pengawasan proses bisnis.</p>
						<p>Berhak untuk melakukan setup master, mekanisme approval dan perubahan data / transaksi termasuk penghapusan.</p>
					</td>
				</tr>
				<tr class="vertical-top">
					<td>
						<asp:checkbox id="role02" runat="server" />
					</td>
					<td>
						<label for="content_role02" class="text-addheight">
							<strong>Operator</strong>
						</label>
					</td>
					<td class="text-muted" style="text-align:left; vertical-align:middle;">
						<p>Fungsi administratif umum.</p>
						<p>Bisa melakukan proses input data / transaksi.</p>
					</td>
				</tr>
				<tr class="vertical-top">
					<td>
						<asp:checkbox id="role21" runat="server" />
					</td>
					<td>
						<label for="content_role21" class="text-addheight">
							<strong>Download File</strong>
						</label>
					</td>
					<td class="text-muted" style="text-align:left; vertical-align:middle;">
						<p>Bisa mengambil dan menyimpan file-file digital dari sistem ke perangkat pribadi.</p>
					</td>
				</tr>
				<tr class="vertical-top">
					<td>
						<asp:checkbox id="role22" runat="server" />
					</td>
					<td>
						<label for="content_role22" class="text-addheight">
							<strong>Download Excel</strong>
						</label>
					</td>
					<td class="text-muted" style="text-align:left; vertical-align:middle;">
						<p>Bisa membuka laporan-laporan yang tersedia di sistem dalam format Microsoft Excel.</p>
					</td>
				</tr>
			</table>
		</div>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-ok"></i>
				Save
			</button>
			<a href="javascript:closeWindow();" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:content>

