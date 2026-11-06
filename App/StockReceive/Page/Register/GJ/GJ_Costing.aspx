<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="GJ_Costing.aspx.cs" inherits="Page_Register_GJ_Costing" %>

<%@ register src="MenuGJ.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Gold Jewellery
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
			<tr id="unitprice_Row" runat="server">
				<td>Unit Price</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="unitprice" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="unitprice_RF" runat="server" />
					<span id="unitprice_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <%--tr>
				<td>Frame</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="frame" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="frame_RF" runat="server" />
					<span id="frame_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</--%>
			<tr>
				<td>Mounting NO</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="mountingno" Text="0" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="mountingno_RF" runat="server" />
					<span id="mountingno_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td>Mounting RO</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="mountingro" Text="0" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="mountingro_RF" runat="server" />
					<span id="mountingro_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td>Pembagi</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="pembagi" runat="server" cssclass="form-control text-lg text-right" width="175" Enabled="false" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="pembagi_RF" runat="server" />
					<span id="pembagi_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
			<tr>
				<td>Stone Setting</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="setting" runat="server" Text="0" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="setting_RF" runat="server" />
					<span id="setting_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <%--tr>
				<td>Finishing</td>
				<td>:</td>
				<td id="finishing_td" runat="server">
					<div class="input-group">
						<asp:textbox id="finishing" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">IDR</span>
                        
					</div>
                    
                    <div class="input-group">
						<asp:Panel ID="finishingfield" runat="server" />
                        
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="finishing_RF" runat="server" />
					<span id="finishing_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</--%>
            <asp:TableRow ID="finishing_tr" runat="server" />
            <tr>
				<td>Ongkos Lain-lain <br /><small class="text-muted">COGS</small></td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="ongkoslaincogs" runat="server" Text="0" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="ongkoslaincogs_RF" runat="server" />
					<span id="ongkoslaincogs_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td>Ongkos Lain-lain</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="ongkoslain" runat="server" Text="0" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="ongkoslain_RF" runat="server" />
					<span id="ongkoslain_ERR" runat="server" class="text-validator" visible="false">Format</span>
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

