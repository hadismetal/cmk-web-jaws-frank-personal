<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="SPK_Form.aspx.cs" inherits="Page_Register_DJ_Form" %>

<%@ register src="MenuSPK.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Gold Jewellery
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<div class="row">
        <div class="col-xs-12 col-sm-2">
            <span class="text-muted">No. Ref :</span><br />
		    <span class="text-bold" id="noref" runat="server" />
        </div>
        <div class="col-xs-12 col-sm-2">
            <span class="text-muted">Designer :</span><br />
		    <span class="text-bold" id="designer" runat="server" />
        </div>
    </div>
	<hr />
	<form runat="server">
		<div class="form-group">
            <label>
				SPK Brand :
			</label>
            <asp:DropDownList ID="spkbrand" runat="server" CssClass="form-control" Width="175">
                
            </asp:DropDownList>
        </div>
		<div class="form-group width-500">
			<label>
				Keterangan :
			</label>
			<asp:textbox id="keterangan" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
		</div>
		<table class="table-form">
			<tr>
				<td>Tanggal Jatuh Tempo Rangka</td>
				<td>:</td>
				<td>
					<asp:textbox id="tglrangka" runat="server" cssclass="form-control text-center" width="120" />
					<script>
					    $(document).ready(function () { $('#<%=tglrangka.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
					</script>
				</td>
				<td>
					<asp:requiredfieldvalidator id="tglrangka_RF" runat="server" />
					<span id="tglrangka_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td>Tanggal Jatuh Tempo Setting</td>
				<td>:</td>
				<td>
					<asp:textbox id="tglsetting" runat="server" cssclass="form-control text-center" width="120" />
					<script>
					    $(document).ready(function () { $('#<%=tglsetting.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
					</script>
				</td>
				<td>
					<asp:requiredfieldvalidator id="tglsetting_RF" runat="server" />
					<span id="tglsetting_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td>Tanggal Jatuh Tempo DJ</td>
				<td>:</td>
				<td>
					<asp:textbox id="tgldj" runat="server" cssclass="form-control text-center" width="120" />
					<script>
					    $(document).ready(function () { $('#<%=tgldj.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
					</script>
				</td>
				<td>
					<asp:requiredfieldvalidator id="tgldj_RF" runat="server" />
					<span id="tgldj_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td>Tanggal Jatuh Tempo Konsumen</td>
				<td>:</td>
				<td>
					<asp:textbox id="tglkonsumen" runat="server" cssclass="form-control text-center" width="120" />
					<script>
					    $(document).ready(function () { $('#<%=tglkonsumen.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
					</script>
				</td>
				<td>
					<asp:requiredfieldvalidator id="tglkonsumen_RF" runat="server" />
					<span id="tglkonsumen_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td>No. Invoice</td>
				<td>:</td>
				<td>
					<asp:textbox id="noinvoice" runat="server" cssclass="form-control text-center" width="200" />
				</td>
				<td>
					&nbsp;
				</td>
			</tr>
            <tr>
				<td>No. PO</td>
				<td>:</td>
				<td>
					<asp:textbox id="nopo" runat="server" cssclass="form-control text-center" width="200" />
				</td>
				<td>
					&nbsp;
				</td>
			</tr>
            <tr>
				<td>No. Titipan</td>
				<td>:</td>
				<td>
					<asp:textbox id="notitipan" runat="server" cssclass="form-control text-center" width="200" />
				</td>
				<td>
					&nbsp;
				</td>
			</tr>
            <tr>
				<td>Kode Karet Lama</td>
				<td>:</td>
				<td>
					<asp:textbox id="kodekaretlama" runat="server" cssclass="form-control text-center" width="200" />
				</td>
				<td>
					&nbsp;
				</td>
			</tr>
            <tr>
				<td>Jumlah Copy</td>
				<td>:</td>
				<td>
					<asp:textbox id="jlhcopy" Text="1" runat="server" cssclass="form-control text-center" width="80" />
				</td>
				<td>
					<asp:requiredfieldvalidator id="jlhcopy_RF" runat="server" />
					<span id="jlhcopy_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
		</table>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<a href="DJ.aspx" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:content>
