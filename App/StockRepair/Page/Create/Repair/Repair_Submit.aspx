<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Repair_Submit.aspx.cs" inherits="Page_Register_Repair_Submit2" %>

<%@ register src="MenuRepair.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Repair Invoice
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <div class="file-subheader hidden-sm hidden-xs">
		repair information
	</div>
	<div class="row">
        <div class="col-xs-12 col-sm-3">
			<asp:Image ID="imgpicturerepair" runat="server" CssClass="img-thumbnail" Width="300" />
		</div>
        <div class="col-xs-12 col-sm-3">
            <table class="table-file">
                <tr>
                    <td class="text-label">PLU</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_plu" /></td>
                </tr>
                <tr>
                    <td class="text-label">Tanggal</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_tgl" /></td>
                </tr>
                <tr>
                    <td class="text-label">Tanggal Selesai</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_tgl2" /></td>
                </tr>
                <tr>
                    <td class="text-label">Keterangan</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_keterangan" /></td>
                </tr>
            </table>
        </div>
        <div class="col-xs-12 col-sm-4">
            <table class="table-file">
                <tr>
                    <td class="text-label">Gross Weight</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td class="pull-right">
                        <span class="text-lg" id="grossweight" runat="server" />
                    </td>
                    <td><small class="text-silver">Gram</small></td>
                </tr>
                <%--<tr>
                    <td class="text-label">Kadar Logam</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td class="pull-right">
                        <span class="text-lg" id="kadarlogam" runat="server" />
                    </td>
                    <td><small class="text-silver">%</small></td>
                </tr>--%>
                <tr>
                    <td class="text-label">Diameter</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td class="pull-right">
                        <span class="text-lg" id="diameter" runat="server" />
                    </td>
                    <td><small class="text-silver"></small></td>
                </tr>
                <tr>
                    <td class="text-label">Panjang</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td class="pull-right">
                        <span class="text-lg" id="panjang" runat="server" />
                    </td>
                    <td><small class="text-silver"></small></td>
                </tr>
                <tr>
                    <td class="text-label">Lebar</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td class="pull-right">
                        <span class="text-lg" id="lebar" runat="server" />
				        </td>
                    <td><small class="text-silver"></small></td>
                </tr>
                <tr>
                    <td class="text-label">Ring Size</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td class="pull-right">
                        <span class="text-lg" id="ringsize" runat="server" />
				        </td>
                    <td><small class="text-silver"></small></td>
                </tr>
            </table>
        </div>
        
    </div>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		product characteristic
	</div>
	<div class="row">
        <div class="col-xs-12 col-sm-3">
			<asp:Image ID="imgpicturebefore" runat="server" CssClass="img-thumbnail" Width="300" />
		</div>
		<div class="col-xs-12 col-sm-3">
			<table class="table-file">
				<tr>
					<td class="text-label">Product Item</td>
					<td>:</td>
					<td id="productitem" runat="server" />
				</tr>
			</table>
		</div>
        <div class="col-xs-12 col-sm-4">
			
		</div>
	</div>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		stone
	</div>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="200">Stone</asp:tableheadercell>
                <asp:tableheadercell width="100">Terpasang</asp:tableheadercell>
                <asp:tableheadercell width="70" cssclass="text-center">P</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">L</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">T</asp:tableheadercell>
				<asp:tableheadercell width="110" cssclass="text-right">Carat / Butir</asp:tableheadercell>
				<asp:tableheadercell width="100" cssclass="text-center">Total Butir</asp:tableheadercell>
				<asp:tableheadercell width="140" cssclass="text-center">Total Carat</asp:tableheadercell>
                <asp:tableheadercell width="250">Keterangan</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<hr />
    <div class="file-subheader hidden-sm hidden-xs">
		repair list
	</div>
	<div class="table-responsive">
		<asp:table id="tb2" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="300">Item</asp:tableheadercell>
                <asp:tableheadercell width="150">Biaya</asp:tableheadercell>
                <asp:tableheadercell width="250">Keterangan</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<hr />
    <div class="form-group">
        <div class="text-md text-grandtotal">Jenis Item Repair : <span id="repairproductitem" runat="server"></span></div>
        <div class="text-lg text-grandtotal">Estimasi Harga : <span id="estimasi" runat="server"></span></div>
    </div>
    <hr />
    <asp:Literal runat="server" ID="lit_msg" />
	<form runat="server">
		<div class="button-left">
			<button id="save" runat="server" class="btn btn-success" onserverclick="save_Click" style="display:none">
				<i class="icon-check"></i>
				Finish
			</button>
			<button id="save1" runat="server" class="btn btn-success" onserverclick="save1_Click">
				<i class="icon-check"></i>
				Submit To Doc Repair
			</button>
		</div>
	</form>
</asp:content>

