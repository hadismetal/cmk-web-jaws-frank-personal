<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DJDO.aspx.cs" inherits="Page_Register_DJDO" %>

<%@ register src="MenuDJDO.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewellery (DO)
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    
    <script>
        $(document).ready(function () {
            $("select").select2({ width: '100%' });
        });

    </script>
    <style type="text/css">
        .form-group .select2-container {
              position: relative;
              z-index: 2;
              float: left;
              width: 100%;
              margin-bottom: 0;
              display: table;
              table-layout: fixed;
            }
    </style>

	<%--<uc1:menu id="menu" runat="server" />--%>
	<asp:literal id="idbrand" runat="server" visible="false" />
	<form runat="server" class="form-inline">
		
        <div class="row">
            <div class="col-xs-12 col-sm-4">
                <div class="form-group" runat="server" id="div_search1">
			        <div class="text-muted">
				        Search (No. DO) :
			        </div>
			        <asp:textbox id="kw" runat="server" cssclass="form-control" width="200" placeholder="Input Keyword..." />
		        </div>
		        <div class="form-group" runat="server" id="div_search2">
			        <div class="hidden-xs">&nbsp;</div>
			        <div>
				        <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					        <i class="icon-search"></i>
					        Display
				        </button>
						<button id="btn_retake1" runat="server" class="btn btn-primary" onserverclick="retakestonedata1_Click" >
							<i class="icon-refresh"></i>
						</button>
			        </div>
		        </div>
            </div>
            <div class="col-xs-12 col-sm-8">
                <asp:Literal runat="server" ID="lit_invalid_msg" />
                <div class="row" id="message_ERR" runat="server" visible="false">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                    <div>
		                <i class="icon-warning-sign icon-5x pull-left text-red"></i>
		                <h4>Invalid DO Number</h4>
		                <p class="text-muted">
			                Nomor DO tidak ditemukan dalam sistem.<br />
			                Status : <span id="sebab" runat="server" class="text-bold text-red" />
		                </p>
		                <div class="clearfix"></div>
		                <hr />
	                </div>
                    </div>
                </div>
            </div>
        </div>
        <div runat="server" id="div_process">
            <asp:Literal runat="server" ID="lit_process" />
            
        </div>
        <div runat="server" id="div_info">
            
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <div class="col-xs-12 col-sm-3 col-md-2">
                        <span class="text-muted">DO Number</span><br />
                        <span class="text-lg" id="do_number" runat="server" />
                        
                    </div>
                    <div class="col-xs-12 col-sm-3 col-md-2">
                        <span class="text-muted">BRJ Qty</span><br />
                        <span class="text-lg" id="do_brjqty" runat="server" />
                    </div>
                    <div class="col-xs-12 col-sm-3 col-md-2">
                        <span class="text-muted">Gross Weight</span><br />
                        <span class="text-lg" id="do_grossweight" runat="server" />
                    </div>
                    <div class="col-xs-12 col-sm-3 col-md-2">
                        <span class="text-muted">Stone Qty</span><br />
                        <span class="text-lg" id="do_stoneqty" runat="server" />
                    </div>
                    <div class="col-xs-12 col-sm-3 col-md-2">
                        <span class="text-muted">Stone Carat</span><br />
                        <span class="text-lg" id="do_stonecarat" runat="server" />
                    </div>
                    <div class="col-xs-12 col-sm-3 col-md-2" runat="server" id="div_warehouse_info">
                        <span class="text-muted">Warehouse</span><br />
                        <span class="text-lg" id="do_warehouse" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <hr />
	    <asp:literal id="lit_msg_search_do" runat="server" visible="false" />
        <div runat="server" id="div1">
            <div class="row" id="row2" runat="server">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <div class="alert alert-warning" role="alert">
                        Dengan memilih lokasi Warehouse, transaksi penerimaan BRJ DO akan tersimpan secara <b>permanen</b> terhadap Brand SBU pada lokasi Warehouse. <br /><b>Pastikan kembali cetakan (hardcopy) nomor DO sudah sesuai dengan SBU pada aplikasi JAWS yang akan anda proses !!!</b>
                        <br />
                        <br />
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group" runat="server" id="div_warehouse1">
			                        <div class="text-muted">
				                        Warehouse :
			                        </div>
			                        <asp:dropdownlist id="ddl_warehouse" runat="server" cssclass="select2" width="275" />
		                        </div>
                                <div class="form-group" runat="server" id="div_warehouse2">
			                        <div class="hidden-xs">&nbsp;</div>
			                        <div>
				                        <button id="btn_submit" runat="server" class="btn btn-success" onserverclick="submit_Click">
					                        <i class="icon-ok"></i>
					                        Submit
				                        </button>
				                        <button id="btn_cancel" runat="server" class="btn btn-danger" onserverclick="cancel_Click">
					                        <i class="icon-remove"></i>
					                        Cancel
				                        </button>
			                        </div>
		                        </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="table-responsive">
			            <asp:table id="tb2" runat="server" cssclass="table-list" Visible="false">
				            <asp:tableheaderrow>
					            <asp:tableheadercell width="40">No.</asp:tableheadercell>
					            <asp:tableheadercell width="150">Kode Validasi</asp:tableheadercell>
					            <asp:tableheadercell width="150">Kode Barang</asp:tableheadercell>
					            <asp:tableheadercell width="150">Kode Bagian</asp:tableheadercell>
					            <asp:tableheadercell width="60">Gross Weight</asp:tableheadercell>
					            <asp:tableheadercell width="60">Stone Qty</asp:tableheadercell>
					            <asp:tableheadercell width="60">Stone Carat</asp:tableheadercell>
					            <asp:tableheadercell width="150">No. SPK</asp:tableheadercell>
				            </asp:tableheaderrow>
			            </asp:table>
		            </div>
                    
                </div>
            
            </div>
        </div>
        <div runat="server" id="div2">
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <div class="table-responsive">
			            <asp:table id="tb3" runat="server" cssclass="table-list" Visible="false" style="table-layout:auto;">
                            <asp:tableheaderrow>
					            <asp:tableheadercell RowSpan="2" width="40">No.</asp:tableheadercell>
					            <asp:tableheadercell ColumnSpan="6" width="480">BRJ</asp:tableheadercell>
					            <asp:tableheadercell ColumnSpan="4" width="380">SPK</asp:tableheadercell>
					            <asp:tableheadercell ColumnSpan="3" width="350">Receive</asp:tableheadercell>

                            </asp:tableheaderrow>
				            <asp:tableheaderrow>
					            <asp:tableheadercell width="120">Kode Barang</asp:tableheadercell>
					            <asp:tableheadercell width="100">Kode Validasi</asp:tableheadercell>
					            <asp:tableheadercell width="80">Kode Bagian</asp:tableheadercell>
					            <asp:tableheadercell width="60">Gross Weight</asp:tableheadercell>
					            <asp:tableheadercell width="60">Stone Qty</asp:tableheadercell>
					            <asp:tableheadercell width="60">Stone Carat</asp:tableheadercell>
					            <asp:tableheadercell runat="server" ID="thc_spk" width="200">Nomor SPK</asp:tableheadercell>
					            <asp:tableheadercell width="60">Gross Weight</asp:tableheadercell>
					            <asp:tableheadercell width="60">Stone Qty</asp:tableheadercell>
					            <asp:tableheadercell width="60">Stone Carat</asp:tableheadercell>
					            <asp:tableheadercell width="88">View</asp:tableheadercell>
					            <asp:tableheadercell width="150">Operator</asp:tableheadercell>
					            <asp:tableheadercell width="150">Approval</asp:tableheadercell>
				            </asp:tableheaderrow>
			            </asp:table>
		            </div>
                    <div class="form-group" runat="server" id="div3">
			            <div class="hidden-xs">&nbsp;</div>
                        <div>
                            <asp:Literal runat="server" ID="lit_msg_SPK" />
                            <asp:Literal runat="server" ID="lit_msg_SPK_multiple" />
				            <button id="btn_submit_SPK" runat="server" class="btn btn-success" onserverclick="submit_SPK_Click" disabled="disabled">
					            <i class="icon-ok"></i>
					            Submit All SPK
				            </button>
			            </div>
		            </div>
                </div>
            
            </div>
        </div>
        <div>
            <asp:Literal runat="server" ID="lit_data" />
        </div>
        <div runat="server" id="btn_cancel_invalid" visible="false">
                <br />
            <button id="btn_cancel2" runat="server" class="btn btn-default" onserverclick="cancel_Click">
				<i class="icon-arrow-left"></i>
				Back
			</button>
        </div>
        <div>
            <div runat="server" id="div_panel_info" visible="false">
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="row">
                            <div class="col-xs-1 col-sm-1 col-md-1">
                                <asp:Literal runat="server" ID="lit_info_link" />
                            </div>
                            <div class="col-xs-11 col-sm-11 col-md-11">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <div class="col-xs-6 col-sm-6 col-md-5">
                                        <span class="text-muted" id="spn_info_head_1_label" runat="server" /><br />
                                        <span class="text-md" style="font-weight: bold;" id="spn_info_head_1_value" runat="server" />
                                    </div>
                                    <div class="col-xs-6 col-sm-6 col-md-5">
                                        <span class="text-muted" id="spn_info_head_2_label" runat="server" /><br />
                                        <span class="text-md" style="font-weight: bold;" id="spn_info_head_2_value" runat="server" />
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
                            <div class="col-xs-6 col-sm-6 col-md-6">
                                <asp:Literal runat="server" ID="lit_info_left" />
                            </div>
                            <div class="col-xs-6 col-sm-6 col-md-6">
                                <asp:Literal runat="server" ID="lit_info_right" />
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="row">
                            <div class="col-xs-1 col-sm-1 col-md-1">
                                <button id="btn_retake" runat="server" class="btn btn-primary" onserverclick="retakestonedata_Click" >
					                <i class="icon-refresh"></i>
				                </button>
                            </div>
                            <div class="col-xs-11 col-sm-11 col-md-11">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <div class="col-xs-6 col-sm-6 col-md-5">
                                        <span class="text-muted" id="spn_info_head_3_label" runat="server" /><br />
                                        <span class="text-md" style="font-weight: bold;" id="spn_info_head_3_value" runat="server" />
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
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <asp:Literal runat="server" ID="lit_info_stone_left" />
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <asp:Literal runat="server" ID="lit_info_stone_right" />
                            </div>
                        </div>
                    </div>
                </div>
                
            </div>
        </div>
		
        <div class="table-responsive" runat="server" id="div_stone_table">
			<asp:table id="tb_stone" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
					<asp:tableheadercell width="150">Stone Name</asp:tableheadercell>
					<asp:tableheadercell width="60">Stone Carat</asp:tableheadercell>
					<asp:tableheadercell width="100">No. GIA</asp:tableheadercell>
					<asp:tableheadercell width="120">Kode Barang</asp:tableheadercell>
					<asp:tableheadercell width="100">Kode Validasi</asp:tableheadercell>
					<asp:tableheadercell width="80">Kode Bagian</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
		</div>
        <div>
            <asp:Literal runat="server" ID="lit_message_status" />
            <asp:Literal runat="server" ID="lit_test_msg" />
        </div>
	</form>
</asp:content>

