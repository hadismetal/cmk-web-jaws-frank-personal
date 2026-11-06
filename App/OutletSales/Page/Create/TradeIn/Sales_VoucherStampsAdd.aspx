<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Sales_VoucherStampsAdd.aspx.cs" inherits="Sales_VoucherStampsAdd" %>

<asp:content contentplaceholderid="h1" runat="server">
	Trade In
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server" class="form-inline">
		<div class="row">
            <div class="col-sm-3">
                
		        <div class="form-group">
			        <div class="text-muted">
				        Voucher Code (Discount) :
			        </div>
			        <asp:textbox id="kw" runat="server" cssclass="form-control" width="250" placeholder="..." />
		        </div>
        
		        <div class="form-group">
			        <div class="hidden-xs">&nbsp;</div>
			        <div>
				        <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					        <i class="icon-search"></i>
					        Display
				        </button>
				        <a id="cancel" runat="server" class="btn btn-default" href="TradeIn_Data.aspx">
					        <i class="icon-remove"></i>
					        Cancel
				        </a>
			        </div>
		        </div>

            </div>
            <div class="col-sm-3">
                <span class="text-muted">Voucher will be applied to :</span><br />
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div class="row">
                            
                            <div class="col-sm-9">
                                <table>
                                    <tr>
                                        <td><span runat="server" id="span_plu_type" /></td>
                                    </tr>
                                    <tr>
                                        <td><span runat="server" class="text-bold" id="span_plu_number" /></td>
                                    </tr>
                                    <tr>
                                        <td><span runat="server" class="text-muted" id="span_plu_item" /></td>
                                    </tr>
                    
                                </table>
                            </div>
                            <div class="col-sm-3">
                                <asp:Literal runat="server" ID="lit_img" />
                            </div>
                        </div>
                    </div>
                </div>
                
            </div>
            <div class="col-sm-6">
                <div class="alert alert-warning" role="alert">
                    <b><i>Notes :</i></b>
                    <ul>
                        <li><i>Kode Voucher sifatnya <b>case-sensitive</b>, mohon perhatikan huruf kapital atau huruf kecil.</i></li>
                        <li><i>General Discount card <b>TIDAK</b> dapat digunakan untuk pembelian Logam Mulia, Perhiasan Emas, Berlian ukuran 0,3 carat ke atas bersertifikat internasional.</i></li>
                        <li><i>Discount Birthday voucher dapat digunakan untuk pembelian seluruh produk Diamond Jewellery termasuk Branded Diamond (kecuali Logam Mulia, Perhiasan Emas).</i></li>
                        <li><i>Untuk barang jenis Plain Gold / Gold Jewellery hanya dapat menggunakan voucher discount jenis 'MEMBERSHIP REDEMPTION'.</i></li>
                    </ul>

                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <asp:Literal runat="server" ID="lit_msg" />
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <div class="alert alert-info" runat="server" id="div_panel_voucher" visible="false">
                    <div class="row">
                        <div class="col-xs-1 col-sm-1 col-md-1">
                            <asp:Literal runat="server" ID="lit_voucher_link" />
                        </div>
                        <div class="col-xs-11 col-sm-11 col-md-11">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div class="col-xs-6 col-sm-4 col-md-3">
                                    <span class="text-muted">Voucher Code</span><br />
                                    <span class="text-md" style="font-weight: bold;" id="spn_voucher_nomorkode" runat="server" />
                                </div>
                                <div class="col-xs-6 col-sm-4 col-md-3">
                                    <span class="text-muted">Nominal</span><br />
                                    <span class="text-md" style="font-weight: bold;" id="spn_voucher_nominal" runat="server" />
                                
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-12">
                            <hr style="margin-top:10px; margin-bottom:10px;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-sm-6 col-md-5">
                            <asp:Literal runat="server" ID="lit_voucher_info_left" />
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-5">
                            <asp:Literal runat="server" ID="lit_voucher_info_right" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
		
	</form>
</asp:content>

