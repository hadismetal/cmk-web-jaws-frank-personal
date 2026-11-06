<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Repair_Form.aspx.cs" inherits="Page_Register_Repair_Form" %>

<%@ register src="MenuRepair.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Repair Order
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Location</span><br />
		<span class="text-bold" id="warehouse" runat="server" />
	</p>
	<hr />
	<form runat="server">			
			<div class="row">
				<div class="form-group ">
					<div class="col-md-1">
						<label for="customerlist" class="text-muted">Customer  :</label>
					</div>
					<div class="col-md-4 col-xs-12">
						<div class="input-group" id="customerlist">
							<span class="input-group-text" id="basic-addon1">
								<a href="Repair_DataCustomer.aspx" class="btn btn-primary " id="search_customer" runat="server">
									<i class="icon-search"></i>
								</a>
							</span>					
							<asp:textbox id="customer" Enabled="false" runat="server" cssclass="form-control" maxlength="50" width="200" />
						</div>
					</div>
				</div>
				<div class="form-group col-md-12">
					<div class="col-md-1">
						<label for="JPJR" class="text-muted">JP/JR  :</label>
					</div>
					<div class="col-md-4 col-xs-12">
						<div class="input-group" id="JPJR">
							<span class="input-group-text" id="basic-addon1">
								<a href="Repair_DataAgent.aspx" class="btn btn-primary" id="search_agent" runat="server">
									<i class="icon-search"></i>
								</a>
							</span>					
							<asp:textbox id="agent" Enabled="false" runat="server" cssclass="form-control" maxlength="50" width="200" />
						</div>
					</div>
				</div>
				<div class="form-group col-md-12">
					<div class="col-md-1">
						<label for="PLU" class="text-muted">PLU  :</label>
					</div>
					<div class="col-md-4 col-xs-12">
						<div class="input-group" id="PLU">
							<span class="input-group-text" id="basic-addon1">
								<a href="Repair_ItemAdd.aspx" class="btn btn-primary" id="search_plu" runat="server">
									<i class="icon-search"></i>
								</a>
							</span>					
							<asp:textbox id="plu" Enabled="false" runat="server" cssclass="form-control" maxlength="50" width="200" />
						</div>
					</div>
				</div>
				<div class="form-group ">
					<div class="col-md-1">
						<label for="QC" class="text-muted">QC  :</label>
					</div>
					<div class="col-md-4 col-xs-12">
						<div class="input-group" id="QC">
							<span class="input-group-text" id="basic-addon1">
								<a href="Repair_QCAdd.aspx" class="btn btn-primary" id="search_qc" runat="server">
									<i class="icon-search"></i>
								</a>
							</span>					
							<asp:textbox id="qc" Enabled="false" runat="server" cssclass="form-control" maxlength="50" width="200" />
						</div>
					</div>
				</div>
				<div class="form-group col-md-12">
					<div class="col-md-1">
						<label for="Tanggal" class="text-muted">Tanggal  :</label>
					</div>
					<div class="col-md-4 col-xs-12">
						<asp:textbox id="tgl" runat="server" cssclass="form-control text-center" width="470" />						
						<script>
							$(document).ready(function () { $('#content_tgl').datepicker({ format: 'dd-mm-yyyy' }); });
						</script>
						<asp:requiredfieldvalidator id="tgl_RF" runat="server" />
						<span id="tgl_ERR" runat="server" class="text-validator" visible="false">Format</span>
					</div>
				</div>
				<div class="form-group col-md-12">
					<div class="col-md-1">
						<label for="TanggalSelesai" class="text-muted">Tanggal Selesai  :</label>
					</div>
					<div class="col-md-4 col-xs-12">											
						<asp:textbox id="tglselesai" runat="server" cssclass="form-control text-center" width="470" />						
						<script>
							$(document).ready(function () { $('#content_tglselesai').datepicker({ format: 'dd-mm-yyyy' }); });
						</script>
						<asp:requiredfieldvalidator id="tglselesai_RF" runat="server" />
						<span id="tglselesai_ERR" runat="server" class="text-validator" visible="false">Format</span>
					</div>
				</div>
				<div class="form-group col-md-12">
					<div class="col-md-1">
						<label for="QC" class="text-muted">Keterangan :</label>
					</div>
					<div class="col-md-4 col-xs-12">											
						<asp:textbox id="keterangan" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />					
					</div>
				</div>
	
			</div>		
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<a href="Repair.aspx" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:content>
