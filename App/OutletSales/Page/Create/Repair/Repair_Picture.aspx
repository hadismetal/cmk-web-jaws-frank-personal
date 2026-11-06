<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Repair_Picture.aspx.cs" inherits="Page_Register_Repair_Picture" %>

<%@ register src="MenuRepair.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Repair
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
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
    </div>
	<hr />
	<form runat="server">
		<p class="text-muted">
			Max. Size : 
			<span id="kb" runat="server" class="text-bold text-danger" />
		</p>
		<table class="table-form">
			<tr>
				<td>Product Picture</td>
				<td>:</td>
				<td>
					<asp:fileupload id="imgpicture" runat="server" cssclass="form-control" />
				</td>
				<td>
					<span id="imgpicture_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
				<td id="imgpicture_format" runat="server" class="text-muted text-sm" />
			</tr>
		</table>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<span id="dir_ERR" runat="server" class="text-validator" visible="false">Folder Error : </span>
		</div>
	</form>
</asp:content>

