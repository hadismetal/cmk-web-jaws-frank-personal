<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Repair_CharProduct02.aspx.cs" inherits="Page_Register_Repair_CharProduct02" %>

<%@ register src="MenuRepair.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content ID="Content1" contentplaceholderid="h1" runat="server">
	Repair
</asp:content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content ID="Content3" contentplaceholderid="content" runat="server">
    <uc1:menu id="menu" runat="server" />
	<div class="row">
        <div class="col-xs-12 col-sm-4">
            <table class="table-file">
                <tr>
                    <td>Customer</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_customer" /></td>
                </tr>
                <tr>
                    <td>JP/JR</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_jpjr" /></td>
                </tr>
                <tr>
                    <td>PLU</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_plu" /></td>
                </tr>
                <tr>
                    <td>Tanggal</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_tgl" /></td>
                </tr>
                <tr>
                    <td>Tanggal Selesai</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_tgl2" /></td>
                </tr>
                <tr>
                    <td>Keterangan</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_keterangan" /></td>
                </tr>
            </table>
        </div>
        <div class="col-xs-12 col-sm-4">
            Repair Item :<br />
            <asp:Table runat="server" ID="tbl_repairitem" BorderStyle="None" />
        </div>
    </div>
	<hr />
	<form id="Form1" runat="server">
        <table class="table-form text-right" style="margin-top:10px;">
            <tr>
                <th class="text-center" style="border:solid 1px darkcyan;">Keterangan</th>
                <th></th>
                <th class="text-center" style="border:solid 1px darkcyan;">Current</th>
                <th></th>
                <th class="text-center" style="border:solid 1px darkcyan;">Register</th>
            </tr>
            <tr>
                <td>Gross Weight</td>
                <td>&nbsp;&nbsp;</td>
                <td>
                    <div class="input-group">
						<asp:textbox id="grossweightold" runat="server" cssclass="form-control text-lg text-center" width="125" Enabled="false" />
						<span class="input-group-addon">Gram</span>
					</div>
                </td>
                <td><span class="glyphicon glyphicon-arrow-right" aria-hidden="true"></span></td>
                <td>
                    <div class="input-group">
						<asp:textbox id="grossweight" runat="server" cssclass="form-control text-lg text-center" width="125" Enabled="true" />
						<span class="input-group-addon">Gram</span>
					</div>
                </td>
                <td>
                    <span id="grossweight_ERR" runat="server" class="text-validator" visible="false">Format</span>
					<span id="grossweight_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
					<asp:requiredfieldvalidator id="grossweight_RF" runat="server" />
                </td>
            </tr>
            <%--<tr>
                <td>Kadar Logam :&nbsp;</td>
                <td>
                    <div class="input-group">
						<asp:textbox id="kadarlogamold" runat="server" cssclass="form-control text-lg text-center" width="125" Enabled="false" />
						<span class="input-group-addon">%</span>
					</div>
                </td>
                <td><span class="glyphicon glyphicon-arrow-right" aria-hidden="true"></span></td>
                <td>
                    <div class="input-group">
						<asp:textbox id="kadarlogam" runat="server" cssclass="form-control text-lg text-center" width="125" />
						<span class="input-group-addon">%</span>
					</div>
                </td>
                <td>
                    <asp:requiredfieldvalidator id="kadarlogam_RF" runat="server" />
					<span id="kadarlogam_ERR" runat="server" class="text-validator" visible="false">Format</span>
					<span id="kadarlogam_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
                </td>
            </tr>--%>
            <%--<tr>
                <td>Total Butir :&nbsp;</td>
                <td>
                    <div class="input-group">
						<asp:textbox id="stoneqtyold" runat="server" cssclass="form-control text-lg text-center" width="125" Enabled="false" />
						<span class="input-group-addon">&nbsp;&nbsp;&nbsp;</span>
					</div>
                </td>
                <td><span class="glyphicon glyphicon-arrow-right" aria-hidden="true"></span></td>
                <td>
                    <div class="input-group">
						<asp:textbox id="stoneqty" runat="server" cssclass="form-control text-lg text-center" width="125" />
						<span class="input-group-addon">&nbsp;&nbsp;&nbsp;</span>
					</div>
                </td>
                <td>
                    <asp:requiredfieldvalidator id="stoneqty_RF" runat="server" />
                </td>
            </tr>--%>
            <tr>
                <td>Diameter</td>
                <td>&nbsp;&nbsp;</td>
                <td>
                    <div class="input-group">
						<asp:textbox id="dimensidold" Text="0" runat="server" cssclass="form-control text-lg text-center" width="125" Enabled="false" />
						<%--span class="input-group-addon">MM</--%>
					</div>
                </td>
                <td><span class="glyphicon glyphicon-arrow-right" aria-hidden="true"></span></td>
                <td>
                    <div class="input-group">
						<asp:textbox id="dimensid" Text="0" runat="server" cssclass="form-control text-lg text-center" width="125" />
						<%--span class="input-group-addon">MM</--%>
					</div>
                </td>
                <td>
                    <asp:requiredfieldvalidator id="dimensid_RF" runat="server" />
					<span id="dimensid_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
            </tr>
            <tr>
                <td>Panjang</td>
                <td>&nbsp;&nbsp;</td>
                <td>
                    <div class="input-group">
						<asp:textbox id="dimensipold" Text="0" runat="server" cssclass="form-control text-lg text-center" width="125" Enabled="false" />
						<%--span class="input-group-addon">MM</--%>
					</div>
                </td>
                <td><span class="glyphicon glyphicon-arrow-right" aria-hidden="true"></span></td>
                <td>
                    <div class="input-group">
						<asp:textbox id="dimensip" Text="0" runat="server" cssclass="form-control text-lg text-center" width="125" />
						<%--span class="input-group-addon">MM</--%>
					</div>
                </td>
                <td>
                    <asp:requiredfieldvalidator id="dimensip_RF" runat="server" />
					<span id="dimensip_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
            </tr>
            <tr>
                <td>Lebar</td>
                <td>&nbsp;&nbsp;</td>
                <td>
                    <div class="input-group">
						<asp:textbox id="dimensilold" Text="0" runat="server" cssclass="form-control text-lg text-center" width="125" Enabled="false" />
						<%--span class="input-group-addon">MM</--%>
					</div>
                </td>
                <td><span class="glyphicon glyphicon-arrow-right" aria-hidden="true"></span></td>
                <td>
                    <div class="input-group">
						<asp:textbox id="dimensil" Text="0" runat="server" cssclass="form-control text-lg text-center" width="125" />
						<%--span class="input-group-addon">MM</--%>
					</div>
                </td>
                <td>
                    <asp:requiredfieldvalidator id="dimensil_RF" runat="server" />
					<span id="dimensil_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
            </tr>
            <tr>
                <td>Ring Size</td>
                <td>&nbsp;&nbsp;</td>
                <td>
                    <div class="input-group">
						<asp:textbox id="dimensirold" Text="0" runat="server" cssclass="form-control text-lg text-center" width="125" Enabled="false" />
						<%--span class="input-group-addon">MM</--%>
					</div>
                </td>
                <td><span class="glyphicon glyphicon-arrow-right" aria-hidden="true"></span></td>
                <td>
                    <div class="input-group">
						<asp:textbox id="dimensir" Text="0" runat="server" cssclass="form-control text-lg text-center" width="125" />
						<%--span class="input-group-addon">MM</--%>
					</div>
                </td>
                <td>
                    <span id="dimensir_ERR" runat="server" class="text-validator" visible="false">Format</span>
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

