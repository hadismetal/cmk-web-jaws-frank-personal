<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Repair_Submit.aspx.cs" inherits="Page_Register_Repair_Submit" %>

<%@ register src="MenuRepair.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content ID="Content1" contentplaceholderid="h1" runat="server">
	Repair
</asp:content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content ID="Content3" contentplaceholderid="content" runat="server">
    <uc1:menu id="menu" runat="server" />
    <hr />
	<div class="file-subheader hidden-sm hidden-xs">
		repair information 1
	</div>
	<div class="row">
        <div class="col-xs-12 col-sm-2">
            <span class="text-muted">Original :</span>
            <asp:Image ID="imgpicturebefore" runat="server" CssClass="img-thumbnail" Width="300" />
        </div>
        <div class="col-xs-12 col-sm-2">
            <span class="text-muted">Repair :</span>
			<asp:Image ID="imgpicturerepair" runat="server" CssClass="img-thumbnail" Width="300" />
		</div>
        <div class="col-xs-12 col-sm-2">
            <p>
				<span class="text-muted">PLU :</span><br />
				<span class="text-lg" id="span_plu" runat="server" />
				<small class="text-silver"></small>
			</p>
            <p>
				<span class="text-muted">Product Item :</span><br />
				<span class="text-lg" id="span_productitem" runat="server" />
				<small class="text-silver"></small>
			</p>
        </div>
        <div class="col-xs-12 col-sm-4">
            <div class="col-xs-12">
                <p>
				    <span class="text-muted">Gross Weight :</span><br />
				    <span class="text-lg" id="span_grossweight" runat="server" />
				    <small class="text-silver">Gram</small>
			    </p>
            </div>
            <div class="col-xs-12 col-sm-6">
                <p>
				    <span class="text-muted">Diameter :</span><br />
				    <span class="text-lg" id="span_diameter" runat="server" />
				    <small class="text-silver">MM</small>
			    </p>
                <p>
				    <span class="text-muted">Panjang :</span><br />
				    <span class="text-lg" id="span_panjang" runat="server" />
				    <small class="text-silver">MM</small>
			    </p>
            </div>
            <div class="col-xs-12 col-sm-6">
                <p>
				    <span class="text-muted">Lebar :</span><br />
				    <span class="text-lg" id="span_lebar" runat="server" />
				    <small class="text-silver">MM</small>
			    </p>
                <p>
				    <span class="text-muted">Ring Size :</span><br />
				    <span class="text-lg" id="span_ringsize" runat="server" />
				    <small class="text-silver"></small>
			    </p>
            </div>
                
        </div>
        
        
    </div>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		repair information 2
	</div>
	<div class="row">
        <div class="col-xs-12 col-sm-5">
			<table class="table-file">
                <tr>
                    <td class="text-label">Customer</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_customer" /></td>
                </tr>
                <tr>
                    <td class="text-label">JP/JR</td>
                    <td>&nbsp;:&nbsp;</td>
                    <td><asp:Literal runat="server" ID="lit_jpjr" /></td>
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
		<div class="col-xs-12 col-sm-5">
			
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
    <div class="file-subheader hidden-sm hidden-xs">
		price detail
	</div>
    <div class="form-group">
        <table class="table-file">
            <tr>
                <td>Emas</td>
                <td>&nbsp;:&nbsp;</td>
                <td><asp:Literal runat="server" ID="lit_pricetotal_emas" /></td>
            </tr>
            <tr>
                <td>Batu</td>
                <td>&nbsp;:&nbsp;</td>
                <td><asp:Literal runat="server" ID="lit_pricetotal_batu" /></td>
            </tr>
            <tr>
                <td>Jasa</td>
                <td>&nbsp;:&nbsp;</td>
                <td><asp:Literal runat="server" ID="lit_pricetotal_jasa" /></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td><div class="text-lg text-grandtotal"><asp:Literal runat="server" ID="lit_pricetotal" /></div></td>
            </tr>
        </table>
        <%--<div class="text-md text-grandtotal">Jenis Item Repair : <span id="repairproductitem" runat="server"></span></div>--%>
        <%--<div class="text-lg text-grandtotal">Estimasi Harga : <span id="estimasi" runat="server"></span></div>--%>
    </div>
    <hr />
    <asp:Literal runat="server" ID="lit_msg" />
	<form id="Form1" runat="server">
		<div class="button-left">
			<button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
				<i class="icon-check"></i>
				Finish
			</button>
		</div>
	</form>
</asp:content>

