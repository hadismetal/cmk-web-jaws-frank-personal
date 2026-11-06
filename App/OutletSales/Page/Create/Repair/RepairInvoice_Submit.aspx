<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RepairInvoice_Submit.aspx.cs" Inherits="Page_Register_Repair_Submit" %>

<%@ Register Src="MenuRepairInvoice.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Repair Invoice
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <hr />
    <form runat="server">
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
            <div class="col-md-5">
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
            <br class="visible-sm visible-xs" />
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            stone
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Stone</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Terpasang</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110" CssClass="text-right">Carat / Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass="text-center">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" CssClass="text-center">Total Carat</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            List Repair
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb2" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="300">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Biaya</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        
        <hr />
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
            <%--<div class="text-lg text-grandtotal">Total Harga Repair : <span id="estimasi" runat="server"></span></div>--%>
        </div>
        <hr />
        <div class="row">
            
            <div class="col-md-5">
                <table class="table-file table-file-nomargin">
                    <tr>
                        <td class="text-label">Discount</td>
                        <td>:</td>
                        <td class="fixed-75 text-right text-lg text-warning">
                            <asp:TextBox ID="discount" CssClass="form-control input-sm text-right" runat="server" Width="100">0</asp:TextBox>
                        </td>
                        <td class="text-silver"><small>Rp</small></td>
                    </tr>
                </table>
            </div>
        </div>
        <hr />

        <asp:Literal runat="server" ID="lit_msg" />
        <div class="button-left">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-check"></i>
                Finish
            </button>
        </div>
    </form>
</asp:Content>

