<%@ Page Language="C#" SmartNavigation="true" MaintainScrollPositionOnPostback="true" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Payment_ItemList.aspx.cs" Inherits="Page_Register_Payment_ItemList" %>

<%@ Register Src="~/UC/PaymentSummary.ascx" TagName="paymentSummary" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Sales Order
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Payment
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
        #toast-container.toast-top-center {
            top: 50% !important;
            left: 50% !important;
            transform: translate(-50%, -50%) !important;
            right: auto !important;
            bottom: auto !important;
        }

        #toast-container .toast {
            font-size: 22px;
            padding: 30px 45px;
            min-width: 480px;
            text-align: center;
            border-radius: 12px;
        }

        #toast-container .toast-message {
            font-weight: 600;
            line-height: 1.5;
        }
    </style>
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

        .popover-content {
            font-size: 12px;
        }

        table.table-list td {
            padding: 5px;
        }

        .overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
            z-index: 9999;
            display: none;
            align-items: center;
            justify-content: center;
            color: white;
            font-size: 24px;
        }

        .spinner {
            border: 6px solid #f3f3f3;
            border-top: 6px solid #3498db;
            border-radius: 50%;
            width: 50px;
            height: 50px;
            animation: spin 1s linear infinite;
        }

        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
    </style>
    <div id="overlay" class="overlay">
        <div class="spinner"></div>
        <div style="margin-top: 10px;">Loading...</div>
    </div>
	<div class="modal" role="dialog" id="modal-example" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Warning</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true"></span>
                    </button>
                </div>
            <div class="modal-body text-center">
                <h2><strong>E-receipt akan dikirimkan ke customer.</strong></h2>
            </div>
            <div class="modal-footer">
                <button class="btn btn-success" runat="server" onserverclick="save_Click" onclick="issentreceipt();" >Lanjutkan</button>
                <button type="button" class="btn btn-danger" data-dismiss="modal">Ubah</button>
                <script type="text/javascript">
                    function issentreceipt() {
						document.getElementById("overlay").style.display = "flex";
                        $('#modal-example').modal('hide');
                    }
                </script>
            </div>
        </div>
      </div>
    </div>

    <div class="modal fade" role="dialog" id="modal-submit" data-backdrop="static">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Warning</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true"></span>
                    </button>
                </div>
            <div class="modal-body text-center">
                <strong>Pastikan data yang diinput sudah sesuai. Klik / pilih Yakin untuk melanjutkan transaksi.</strong>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-danger" data-dismiss="modal" onclick="iscancelsubmit();">Tidak Yakin</button>
                <button type="button" class="btn btn-success" runat="server" onserverclick="save_Click" onclick="issubmit();" >Yakin</button>
                <script type="text/javascript">
                    function issubmit() {
						document.getElementById("overlay").style.display = "flex";
                        $('#modal-submit').modal('hide');
                    }

                    function iscancelsubmit() {
                        $('#modal-submit').modal('hide');
                    }
                </script>
            </div>
        </div>
      </div>
    </div>

    <form id="Form1" runat="server" onkeydown="return event.key != 'Enter';" class="form-inline">
		<div class="modal" role="dialog" id="modal-ngga-modal" data-backdrop="static">
			<div class="modal-dialog" role="document">
				<div class="modal-content">
					<div class="modal-header">
						<h5 class="modal-title">Alasan E-receipt tidak dikirimkan.</h5>
						<button type="button" class="close" data-dismiss="modal" aria-label="Close">
							<span aria-hidden="true"></span>
						</button>
					</div>
				<div class="modal-body text-center">
					<div class="form-group">
						<div class="col-md-4" id="div_receipt_cuy_1">
							<div>
								<asp:DropDownList runat="server" ID="ddl_erecipt_cuy" onchange="onchangeddlbercuy(this);">
									<asp:ListItem Value="2">Gift</asp:ListItem>
									<asp:ListItem Value="1">Incognito</asp:ListItem>
									<asp:ListItem Value="3">Others</asp:ListItem>
								</asp:DropDownList>
							</div>
						</div>
						<div class="col-md-8" id="div_receipt_cuy_2">
							<asp:TextBox TextMode="MultiLine" runat="server" ID="ereceipt_light" CssClass="form-control" />
						</div>
					</div>
				</div>
				<div class="modal-footer">
					<button class="btn btn-success" runat="server" onserverclick="save_Click" onclick="if (!onconfirmereceipt()) return false;" >Lanjutkan</button>
					<button type="button" class="btn btn-danger" data-dismiss="modal">Ubah</button>
					<script type="text/javascript">
						$(window).load(function(){
							$('#content_ereceipt_light').css('display','none');
						});
						
						function isnotsentreceipt() {
							$('#modal-ngga-modal').modal('hide');
						}
						
						function onchangeddlbercuy(e){
							if(e.value == '2' || e.value == '1')
							{
								$('#content_ereceipt_light').css('display','none');
							}
							else
							{
								$('#content_ereceipt_light').css('display','');
							}
						}
						
						function onconfirmereceipt() {
							var cuy = $('#content_ddl_erecipt_cuy :selected').val();
							var cuy_banget = $('#content_ereceipt_light').val();
							if (cuy == '3' && cuy_banget == '') {
								alert('Jika pilih Others maka wajib isi keterangan...');
								return false;
							}
							else{
								$('modal-ngga-modal').modal('show');
								return true;
							}
						}
					</script>
				</div>
			</div>
		  </div>
		</div>
        <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script type="text/javascript"> 
            var postbackControl = null;
            var parm = Sys.WebForms.PageRequestManager.getInstance();
            parm.add_beginRequest(BeginRequestHandler);
            parm.add_endRequest(EndRequestHandler);

            function BeginRequestHandler(sender, args) {
                postbackControl = args.get_postBackElement();
                postbackControl.disabled = true;
            }
            function EndRequestHandler(sender, args) {
                postbackControl.disabled = false;
                postbackControl = null;
            }
        </script>
        <div runat="server" id="div_finished" visible="false">
            <div class="row">

                <div class="col-xs-12 col-sm-3 col-md-3">
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="panel panel-default">

                        <div class="panel-body">
                            <asp:Literal runat="server" ID="lit_info_invalid" />
                            <button id="redirect_summary" runat="server" style="width: 100%;" class="btn btn-default" onserverclick="redirect_summary_Click" visible="false">
                                <span class="glyphicon glyphicon-arrow-left" aria-hidden="true"></span>
                                &nbsp;&nbsp;Back to Summary
                            </button>
                            <button id="redirect_payment_list" runat="server" style="width: 100%;" class="btn btn-default" onserverclick="redirect_payment_list_Click" visible="false">
                                <span class="glyphicon glyphicon-arrow-left" aria-hidden="true"></span>
                                &nbsp;&nbsp;Back to Payment List
                            </button>
                        </div>

                    </div>
                </div>
                <div class="col-xs-12 col-sm-3 col-md-3">
                </div>
            </div>
            <div>
                <hr />
            </div>
        </div>

        <div runat="server" id="div_unfinished" visible="false">
            <div class="row">
                <div class="col-xs-12 col-sm-4">
                    <p>
                        <span class="text-muted">Sales Order</span><br />
                        <span class="text-lg" id="header_nomor" runat="server" />
                    </p>
                </div>
                <div class="col-xs-12 col-sm-4" runat="server" id="div_tradein" visible="false">
                    <p>
                        <span class="text-muted">TradeIn</span><br />
                        <span class="text-lg" id="header_tradein" runat="server" />
                    </p>
                </div>
                <div class="col-xs-12 col-sm-4" runat="server" id="div_resell" visible="false">
                    <p>
                        <span class="text-muted">Resell Order</span><br />
                        <span class="text-lg" id="header_resell" runat="server" />
                    </p>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col-xs-12">
                    <div class="panel panel-default">
                        <div class="panel-body">

                            <div class="file-subheader hidden-sm hidden-xs">
                                Form
                            </div>

                            <div class="row">
                                <div class="col-md-5">
                                    <table class="table-file">
                                        <tr>
                                            <td class="text-label">Brand</td>
                                            <td>:</td>
                                            <td id="brand" runat="server" />
                                        </tr>
                                        <tr>
                                            <td class="text-label">Location</td>
                                            <td>:</td>
                                            <td id="location" runat="server" />
                                        </tr>
                                        <tr>
                                            <td class="text-label">Operator</td>
                                            <td>:</td>
                                            <td id="operator_input" runat="server" />
                                        </tr>
                                        <tr>
                                            <td class="text-label">Transaction Date</td>
                                            <td>:</td>
                                            <td id="tgl" runat="server" />
                                        </tr>

                                    </table>
                                </div>
                                <br class="visible-sm visible-xs" />
                                <div class="col-md-5">
                                    <table class="table-file">
                                        <tr>
                                            <td class="text-label">Customer</td>
                                            <td>:</td>
                                            <td id="customer" runat="server" />
                                        </tr>
										<tr>
                                            <td class="text-label">Given To</td>
                                            <td>:</td>
                                            <td id="givento" runat="server" />
                                        </tr>
                                        <tr>
                                            <td class="text-label">Membership</td>
                                            <td>:</td>
                                            <td id="membership" runat="server" />
                                        </tr>
                                        <tr>
                                            <td class="text-label">JP / JR</td>
                                            <td>:</td>
                                            <td id="jpjr" runat="server" />
                                        </tr>
                                        <tr>
                                            <td class="text-label">Description</td>
                                            <td>:</td>
                                            <td id="keterangan" runat="server" />
                                        </tr>
                                    </table>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12" style="width: 115%;">
                    <div class="panel panel-default">
                        <div class="panel-body">


                            <script type="text/javascript">

                                function showhide_item() {
                                    var x = document.getElementById("div_table_item");
                                    if (x.style.display === "none") {
                                        x.style.display = "block";
                                    } else {
                                        x.style.display = "none";
                                    }
                                }

                            </script>

                            <button class="btn btn-xs btn-default pull-left" onclick="showhide_item(); return false;">Show / Hide ...</button>

                            <div class="file-subheader hidden-sm hidden-xs">
                                Data
                            </div>

                            <div class="table-responsive" id="div_table_item" style="display: none;">
                                <asp:Table ID="tb" runat="server" CssClass="table-list" Style="width: 100%;">
                                    <asp:TableHeaderRow>
                                        <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="210">No. Invoice<br />PLU & Item</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="150">Harga</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="100">Discount Normal</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="100">Discount Promo</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="100">Discount Membership</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="100">Discount Voucher</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="100">Discount Bank</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="100">Discount Other</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="100">Discount Round</asp:TableHeaderCell>
										<asp:TableHeaderCell Width="100">Fee Extra Ring Size</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="150">Total Bayar</asp:TableHeaderCell>
                                    </asp:TableHeaderRow>
                                </asp:Table>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <div class="panel panel-default" id="divrowpayment">
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="file-subheader hidden-sm hidden-xs">
                                        Payment
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-6">
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                            <div class="row">

                                                <div class="col-xs-12">
                                                    <div class="alert alert-danger" role="alert">
                                                        <strong>Perhatian !</strong>&nbsp;Segala kesalahan input pembayaran (baik secara nominal ataupun voucher) <strong>TIDAK</strong> dapat dikembalikan. Silahkan gunakan fitur <strong>Void Sales Order</strong> untuk melakukan penginputan kembali dari awal.
                                                    </div>
                                                </div>
                                                <div class="col-xs-12 col-sm-12 col-md-12">
                                                    <style type="text/css">
                                                        .tabcustom {
                                                            padding: 8px 13px;
                                                            font-size: 12px;
                                                        }

                                                            .tabcustom span {
                                                                font-size: 14px;
                                                                padding-left: 3px;
                                                            }

                                                        .padtop13 {
                                                            padding-top: 13px !important;
                                                        }

                                                        ul.nav li a.tabcustom {
                                                            text-align: center;
															max-width: 75px
                                                        }

                                                        h3 span {
                                                            font-size: 20px;
                                                        }
                                                    </style>
                                                    <ul class="nav nav-pills" role="tablist">
                                                        <li role="presentation" class="<%= Active_CreditCard %>"><a class="tabcustom" <%= Href_CreditCard %> aria-controls="creditcard" role="tab" data-toggle="<%= Tab_CreditCard %>"><span class="glyphicon glyphicon-credit-card" aria-hidden="true"></span>
                                                            <br />
                                                            Credit Card</a></li>
                                                        <li role="presentation" class="<%= Active_DebitCard %>"><a class="tabcustom" <%= Href_DebitCard %> aria-controls="debitcard" role="tab" data-toggle="<%= Tab_DebitCard %>"><span class="glyphicon glyphicon-credit-card" aria-hidden="true"></span>
                                                            <br />
                                                            Debit Card</a></li>
                                                        <li role="presentation" class="<%= Active_Cash %>"><a class="tabcustom padtop13" <%= Href_Cash %> aria-controls="cash" role="tab" data-toggle="<%= Tab_Cash %>"><span class="icon-money" style="font-size: 14px; padding-left: 6px;" aria-hidden="true"></span>
                                                            <br />
                                                            Cash</a></li>
                                                        <li role="presentation" class="<%= Active_DownPayment %>"><a class="tabcustom" <%= Href_DownPayment %> aria-controls="creditcard" role="tab" data-toggle="<%= Tab_DownPayment %>"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span>
                                                            <br />
                                                            Down Payment</a></li>
                                                        <li role="presentation" class="<%= Active_Transfer %>"><a class="tabcustom" <%= Href_Transfer %> aria-controls="transfer" role="tab" data-toggle="<%= Tab_Transfer %>"><span class="glyphicon glyphicon-transfer" aria-hidden="true"></span>
                                                            <br />
                                                            Transfer</a></li>
                                                        <li role="presentation" class="<%= Active_Voucher %>"><a class="tabcustom padtop13" <%= Href_Voucher %> aria-controls="voucher" role="tab" data-toggle="<%= Tab_Voucher %>"><span class="icon-ticket" style="font-size: 14px; padding-left: 6px;" aria-hidden="true"></span>
                                                            <br />
                                                            Voucher</a></li>
                                                        <li role="presentation" class="<%= Active_GiftCard %>"><a class="tabcustom" <%= Href_GiftCard %> aria-controls="giftcard" role="tab" data-toggle="<%= Tab_GiftCard %>"><span class="glyphicon glyphicon-credit-card" aria-hidden="true"></span>
                                                            <br />
                                                            Gift Card</a></li>
                                                        <li role="presentation" class="<%= Active_BrideStoryPay %>"><a class="tabcustom" <%= Href_BrideStoryPay %> aria-controls="bridestorypay" role="tab" data-toggle="<%= Tab_BrideStoryPay %>"><span class="glyphicon glyphicon-phone" aria-hidden="true"></span>
                                                            <br />
                                                            Bridestory Pay</a></li>
														<%--<li role="presentation" class="<%= Active_LakuEmas %>"><a class="tabcustom" <%= Href_LakuEmas %> aria-controls="lakuemas" role="tab" data-toggle="<%= Tab_LakuEmas %>"><span class="glyphicon glyphicon-inbox" aria-hidden="true"></span>
                                                            <br />
                                                            LEI</a></li>--%>
														<%--<li role="presentation" class="<%= Active_CashLEI %>"><a class="tabcustom" <%= Href_CashLEI %> aria-controls="cashlei" role="tab" data-toggle="<%= Tab_CashLEI %>"><span class="icon-money" aria-hidden="true"></span>
                                                            <br />
                                                            Cash LEI</a></li>--%>
														<li role="presentation" class="<%= Active_QRIS %>"><a class="tabcustom" <%= Href_QRIS %> aria-controls="qris" role="tab" data-toggle="<%= Tab_QRIS %>"><span class="glyphicon glyphicon-qrcode" aria-hidden="true"></span>
                                                            <br />
                                                            QRIS</a></li>
														<li role="presentation" class="<%= Active_Xendit %>"><a class="tabcustom" <%= Href_Xendit %> aria-controls="qris" role="tab" data-toggle="<%= Tab_Xendit %>"><span class="glyphicon glyphicon-phone" aria-hidden="true"></span>
                                                            <br />
                                                            Xendit</a></li>
														<li role="presentation" class="<%= Active_Ecommerce %>"><a class="tabcustom" <%= Href_Ecommerce %> aria-controls="qris" role="tab" data-toggle="<%= Tab_Ecommerce %>"><span class="glyphicon glyphicon-globe" aria-hidden="true"></span>
                                                            <br />
                                                            E-commerce</a></li>
                                                        <li role="presentation" class="<%= Active_CashA %>"><a class="tabcustom" <%= Href_CashA %> aria-controls="casha" role="tab" data-toggle="<%= Tab_CashA %>"><span class="glyphicon glyphicon-globe" aria-hidden="true"></span>
                                                            <br />
                                                            Cash-A</a></li>
                                                    </ul>
                                                </div>
                                                <div class="col-xs-12">
                                                    <h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;"></h3>
                                                </div>
                                                <div class="col-xs-12 col-sm-12 col-md-12" style="padding-bottom: 15px;">

                                                    <div class="tab-content">
                                                        <div role="tabpanel" class="tab-pane <%= Active_CreditCard %>" id="creditcard">
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="text-center">
                                                                        <h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;">
                                                                            <span class="glyphicon glyphicon-credit-card" aria-hidden="true"></span>&nbsp;&nbsp;Credit Card
                                                                        </h3>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label>Card Holder Name</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_creditcard_cardholdername" Style="text-transform: uppercase;" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label>Card Holder Number</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_creditcard_cardholdernumber" Style="text-transform: uppercase;" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-xs-12 col-sm-6">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label>Approval Code</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_creditcard_approvalcode" />
																				<small style="color:red;display:none" id="lbl_validator_appcode_cc">Appoval Code Must be Filled.</small>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label>EDC</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList runat="server" ID="ddl_creditcard_edc" />
																				<small style="color:red;display:none" id="lbl_validator_ddledc_cc">EDC Must be Filled.</small>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label>Bank Issuer</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList runat="server" ID="ddl_creditcard_bankissuer" />
																				<small style="color:red;display:none" id="lbl_validator_ddlbankissuer_cc">Bank Must be Filled.</small>
                                                                            </div>
                                                                        </div>

                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label>Program Bank</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList runat="server" ID="ddl_creditcard_bankprogram" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-xs-12 col-sm-6">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label>Card Type</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList runat="server" ID="ddl_creditcard_cardtype" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label>Card Category</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList runat="server" ID="ddl_creditcard_cardcategory" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label style="color: blue;">Amount</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox class="form-control" runat="server" ID="txt_creditcard_amount" Style="text-align: center;" />
                                                                                <div class="input-group-addon">IDR</div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <asp:HiddenField runat="server" ID="hf_ecr_cc_flag" Value="0" />
                                                                <asp:HiddenField runat="server" ID="hf_ecr_cc_value" Value="" />
                                                                <div class="col-xs-12" id="div_confirm_creditcard">
                                                                    <button class="btn btn-warning pull-left" onclick="confirm_creditcard(); return false;">Confirm</button>
                                                                </div>
                                                                <div class="col-xs-12" id="div_submit_creditcard" style="display: none;">
                                                                    <button class="btn btn-default pull-left" onclick="confirm_creditcard(); return false;">Cancel</button>
                                                                    <asp:Button runat="server" ID="save_payment_creditcard" OnClick="save_payment_creditcard_Click" OnClientClick="showOverlay();" Text="Add Credit Card" CssClass="btn btn-success pull-right" />
                                                                </div>

                                                            </div>

                                                            <script type="text/javascript">

                                                                function confirm_creditcard() {
                                                                    var x = document.getElementById("div_confirm_creditcard");
																	var appcode = document.getElementById("<%=txt_creditcard_approvalcode.ClientID%>").value;
																	
																	var ddledc = document.getElementById("<%=ddl_creditcard_edc.ClientID%>");
																	var ddledcs = ddledc.options[ddledc.selectedIndex].value;
																	
																	var ddlbank = document.getElementById("<%=ddl_creditcard_bankissuer.ClientID%>");
																	var ddlbanks = ddlbank.options[ddlbank.selectedIndex].value;
																	
																	if ((appcode != "" && appcode.length <= 6) && ddledcs != "0" && ddlbanks != "0") {
																		document.getElementById("lbl_validator_appcode_cc").style.display = "none";
																		document.getElementById("lbl_validator_ddledc_cc").style.display = "none";
																		document.getElementById("lbl_validator_ddlbankissuer_cc").style.display = "none";
																		if (x.style.display === "none") {
																			x.style.display = "block";
																		} else {
																			x.style.display = "none";
																		}
																		var y = document.getElementById("div_submit_creditcard");
																		if (y.style.display === "none") {
																			y.style.display = "block";
																		} else {
																			y.style.display = "none";
																		}
																	}
																	else {
                                                                        if(appcode == ""){
																			document.getElementById("lbl_validator_appcode_cc").innerHTML = "Appoval Code Must be Filled.";
																			document.getElementById("lbl_validator_appcode_cc").style.display = "";
																		}
																		else{
																			if(appcode.length > 6)
																			{
																				document.getElementById("lbl_validator_appcode_cc").innerHTML = "Appoval Code Maximum 6 Character.";
																				document.getElementById("lbl_validator_appcode_cc").style.display = "";
																			}
																			else{
																				document.getElementById("lbl_validator_appcode_cc").style.display = "none";
																			}
																		}
																		
																		if(ddledcs == "0"){
																			document.getElementById("lbl_validator_ddledc_cc").style.display = "";
																		}
																		else{
																			document.getElementById("lbl_validator_ddledc_cc").style.display = "none";
																		}
																		
																		if(ddlbanks == "0"){
																			document.getElementById("lbl_validator_ddlbankissuer_cc").style.display = "";
																		}
																		else{
																			document.getElementById("lbl_validator_ddlbankissuer_cc").style.display = "none";
																		}
                                                                    }
                                                                }

                                                            </script>

                                                        </div>
                                                        <div role="tabpanel" class="tab-pane <%= Active_DebitCard %>" id="debitcard">
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="text-center">
                                                                        <h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;">
                                                                            <span class="glyphicon glyphicon-credit-card" aria-hidden="true"></span>&nbsp;&nbsp;Debit Card
                                                                        </h3>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label>Card Holder Name</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_debitcard_cardholdername" Style="text-transform: uppercase;" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label>Card Holder Number</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_debitcard_cardholdernumber" Style="text-transform: uppercase;" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-xs-12 col-sm-6">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label>Approval Code</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_debitcard_approvalcode" />
																				<small style="color:red;display:none" id="lbl_validator_appcode_debit">Appoval Code Must be Filled.</small>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label>EDC</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList runat="server" ID="ddl_debitcard_edc" />
																				<small style="color:red;display:none" id="lbl_validator_ddledc_debit">EDC Must be Filled.</small>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label>Bank Issuer</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList runat="server" ID="ddl_debitcard_bankissuer" />
																				<small style="color:red;display:none" id="lbl_validator_ddlbankissuer_debit">Bank Must be Filled.</small>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-xs-12 col-sm-6">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label>Card Type</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList runat="server" ID="ddl_debitcard_cardtype" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label>Card Category</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList runat="server" ID="ddl_debitcard_cardcategory" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label style="color: blue;">Amount</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox class="form-control" runat="server" ID="txt_debitcard_amount" Style="text-align: center;" />
                                                                                <div class="input-group-addon">IDR</div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>


                                                            <div class="row">
                                                                <asp:HiddenField runat="server" ID="hf_ecr_dc_flag" Value="0" />
                                                                <asp:HiddenField runat="server" ID="hf_ecr_dc_value" Value="" />
                                                                <div class="col-xs-12" id="div_confirm_debitcard">
                                                                    <button class="btn btn-warning pull-left" onclick="confirm_debitcard(); return false;">Confirm</button>
                                                                </div>
                                                                <div class="col-xs-12" id="div_submit_debitcard" style="display: none;">
                                                                    <button class="btn btn-default pull-left" onclick="confirm_debitcard(); return false;">Cancel</button>
                                                                    <asp:Button runat="server" ID="save_payment_debitcard" OnClick="save_payment_debitcard_Click" OnClientClick="showOverlay();" Text="Add Debit Card" CssClass="btn btn-success pull-right" />
                                                                </div>

                                                            </div>


                                                            <script type="text/javascript">

                                                                function confirm_debitcard() {
                                                                    var x = document.getElementById("div_confirm_debitcard");
																	var appcode = document.getElementById("<%=txt_debitcard_approvalcode.ClientID%>").value;
																	
																	var ddledc = document.getElementById("<%=ddl_debitcard_edc.ClientID%>");
																	var ddledcs = ddledc.options[ddledc.selectedIndex].value;
																	
																	var ddlbank = document.getElementById("<%=ddl_debitcard_bankissuer.ClientID%>");
																	var ddlbanks = ddlbank.options[ddlbank.selectedIndex].value;
																	
                                                                    if ((appcode != "" && appcode.length <= 6) && ddledcs != "0"  && ddlbanks != "0") {
																		document.getElementById("lbl_validator_appcode_debit").style.display = "none";
																		document.getElementById("lbl_validator_ddledc_debit").style.display = "none";
																		document.getElementById("lbl_validator_ddlbankissuer_debit").style.display = "none";
																		if (x.style.display === "none") {
																			x.style.display = "block";
																		} else {
																			x.style.display = "none";
																		}
																		var y = document.getElementById("div_submit_debitcard");
																		if (y.style.display === "none") {
																			y.style.display = "block";
																		} else {
																			y.style.display = "none";
																		}
																	}
                                                                    else {
																		if(appcode == ""){
																			document.getElementById("lbl_validator_appcode_debit").innerHTML = "Appoval Code Must be Filled.";
																			document.getElementById("lbl_validator_appcode_debit").style.display = "";
																		}
																		else{
																			if(appcode.length > 6)
																			{
																				document.getElementById("lbl_validator_appcode_debit").innerHTML = "Appoval Code Maximum 6 Character.";
																				document.getElementById("lbl_validator_appcode_debit").style.display = "";
																			}
																			else{
																				document.getElementById("lbl_validator_appcode_debit").style.display = "none";
																			}
																		}
																		
																		if(ddledcs == "0"){
																			document.getElementById("lbl_validator_ddledc_debit").style.display = "";
																		}
																		else{
																			document.getElementById("lbl_validator_ddledc_debit").style.display = "none";
																		}
																		
																		if(ddlbanks == "0"){
																			document.getElementById("lbl_validator_ddlbankissuer_debit").style.display = "";
																		}
																		else{
																			document.getElementById("lbl_validator_ddlbankissuer_debit").style.display = "none";
																		}
                                                                    }
                                                                }

                                                            </script>

                                                        </div>
                                                        <div role="tabpanel" class="tab-pane <%= Active_Cash %>" id="cash">
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="text-center">
                                                                        <h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;">
                                                                            <span class="icon-money icon-large" aria-hidden="true"></span>&nbsp;&nbsp;Cash
                                                                        </h3>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="alert alert-info" role="alert">
                                                                        <b><i>Notes :</i></b>
                                                                        <ul>
                                                                            <li><i><b>Cash out</b> hanya diberlakukan untuk pembayaran lebih khusus Down Payment.</i></li>
                                                                            <li><i>Pembayaran lebih bayar untuk Voucher Payment (Gift Certificate) <b>tidak dapat melanjutkan transaksi Cash out</b>.</i></li>
                                                                        </ul>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-xs-12 col-sm-6">
                                                                    <div style="text-align: center; padding-top: 30px;">
                                                                        <label class="radio-inline">
                                                                            <asp:RadioButton runat="server" GroupName="rdo_cash" ID="rdo_cash_out" />
                                                                            Cash Out
                                                                        </label>
                                                                        <label class="radio-inline">
                                                                            <asp:RadioButton runat="server" GroupName="rdo_cash" ID="rdo_cash_in" />
                                                                            Cash In
                                                                        </label>
                                                                    </div>
                                                                </div>
                                                                <div class="col-xs-12 col-sm-6">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label style="color: blue;">Amount</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox class="form-control" runat="server" ID="txt_cash_amount" Style="text-align: center;" />
                                                                                <div class="input-group-addon">IDR</div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                            </div>


                                                            <div class="row">

                                                                <div class="col-xs-12" id="div_confirm_cash">
                                                                    <button class="btn btn-warning pull-left" onclick="confirm_cash(); return false;">Confirm</button>
                                                                </div>
                                                                <div class="col-xs-12" id="div_submit_cash" style="display: none;">
                                                                    <button class="btn btn-default pull-left" onclick="confirm_cash(); return false;">Cancel</button>
                                                                    <asp:Button runat="server" ID="save_payment_cash" OnClick="save_payment_cash_Click" OnClientClick="showOverlay();" Text="Add Cash" CssClass="btn btn-success pull-right" />
                                                                </div>

                                                            </div>


                                                            <script type="text/javascript">

                                                                function confirm_cash() {
                                                                    var x = document.getElementById("div_confirm_cash");
                                                                    if (x.style.display === "none") {
                                                                        x.style.display = "block";
                                                                    } else {
                                                                        x.style.display = "none";
                                                                    }
                                                                    var y = document.getElementById("div_submit_cash");
                                                                    if (y.style.display === "none") {
                                                                        y.style.display = "block";
                                                                    } else {
                                                                        y.style.display = "none";
                                                                    }
                                                                }

                                                            </script>
                                                        </div>
                                                        <div role="tabpanel" class="tab-pane <%= Active_DownPayment %>" id="downpayment">
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="text-center">
                                                                        <h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;">
                                                                            <span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span>&nbsp;&nbsp;Down Payment
                                                                        </h3>
                                                                    </div>
                                                                    <div class="alert alert-info" role="alert">
                                                                        <i>Notes :</i>
                                                                        <ul>

                                                                            <li><i>Penggunaan Down Payment harus menggunakan <b>kode customer yang sama</b></i></li>
                                                                            <li><i>Pembayaran dengan menggunakan Down Payment lebih bayar dapat diuangkan melalui <b>Cash Out</b>.</i></li>
                                                                        </ul>

                                                                    </div>

                                                                </div>
                                                            </div>
                                                            <div class="row">

                                                                <div class="col-xs-12" id="div_confirm_downpayment">
                                                                    <button class="btn btn-warning pull-left" onclick="confirm_downpayment(); return false;">Confirm</button>
                                                                </div>
                                                                <div class="col-xs-12" id="div_submit_downpayment" style="display: none;">
                                                                    <button class="btn btn-default pull-left" onclick="confirm_downpayment(); return false;">Cancel</button>
                                                                    <asp:Button runat="server" ID="save_payment_downpayment" OnClick="save_payment_downpayment_Click" OnClientClick="showOverlay();" Text="Add Down Payment" CssClass="btn btn-success pull-right" />
                                                                </div>

                                                            </div>


                                                            <script type="text/javascript">

                                                                function confirm_downpayment() {
                                                                    var x = document.getElementById("div_confirm_downpayment");
                                                                    if (x.style.display === "none") {
                                                                        x.style.display = "block";
                                                                    } else {
                                                                        x.style.display = "none";
                                                                    }
                                                                    var y = document.getElementById("div_submit_downpayment");
                                                                    if (y.style.display === "none") {
                                                                        y.style.display = "block";
                                                                    } else {
                                                                        y.style.display = "none";
                                                                    }
                                                                }

                                                            </script>
                                                        </div>
                                                        <div role="tabpanel" class="tab-pane <%= Active_Transfer %>" id="transfer">
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="text-center">
                                                                        <h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;">
                                                                            <span class="glyphicon glyphicon-transfer" aria-hidden="true"></span>&nbsp;&nbsp;Transfer
                                                                        </h3>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label>Account Name</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_transfer_name" Style="text-transform: uppercase;" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label>Account Number</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_transfer_number" Style="text-transform: uppercase;" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-xs-12 col-sm-6">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label>Bank</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList runat="server" ID="ddl_transfer_bankissuer" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-xs-12 col-sm-6">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label style="color: blue;">Amount</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_transfer_amount" Style="text-align: center;" />
                                                                                <div class="input-group-addon">IDR</div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>


                                                            <div class="row">
                                                                <div class="col-xs-12" id="div_confirm_transfer">
                                                                    <button class="btn btn-warning pull-left" onclick="confirm_transfer(); return false;">Confirm</button>
                                                                </div>
                                                                <div class="col-xs-12" id="div_submit_transfer" style="display: none;">
                                                                    <button class="btn btn-default pull-left" onclick="confirm_transfer(); return false;">Cancel</button>
                                                                    <asp:Button runat="server" ID="save_payment_transfer" OnClick="save_payment_transfer_Click" OnClientClick="showOverlay();" Text="Add Transfer" CssClass="btn btn-success pull-right" />
                                                                </div>

                                                            </div>


                                                            <script type="text/javascript">

                                                                function confirm_transfer() {
                                                                    var x = document.getElementById("div_confirm_transfer");
                                                                    if (x.style.display === "none") {
                                                                        x.style.display = "block";
                                                                    } else {
                                                                        x.style.display = "none";
                                                                    }
                                                                    var y = document.getElementById("div_submit_transfer");
                                                                    if (y.style.display === "none") {
                                                                        y.style.display = "block";
                                                                    } else {
                                                                        y.style.display = "none";
                                                                    }
                                                                }

                                                            </script>


                                                        </div>
                                                        
                                                        <div role="tabpanel" class="tab-pane <%= Active_CashA %>" id="casha">
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="text-center">
                                                                        <h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;">
                                                                            <span class="glyphicon glyphicon-transfer" aria-hidden="true"></span>&nbsp;&nbsp;Cash-A
                                                                        </h3>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12 col-sm-6 col-lg-4" style="padding-bottom: 10px;">
                                                                            <label>Nama Paket & Arisan</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_casha_name" Style="text-transform: uppercase;" />                               
																				<small style="color:red;display:none" id="lbl_validator_txt_name_casha">Nama Paket & Arisan Must be Filled.</small>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12 col-sm-6 col-lg-4" style="padding-bottom: 10px;">
                                                                            <label>Gelombang Arisan / PCS Paket</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_casha_number" Style="text-transform: uppercase;" />
																				<small style="color:red;display:none" id="lbl_validator_txt_number_casha">Gelombang Arisan Must be Filled.</small>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12 col-sm-6 col-lg-4" style="padding-bottom: 10px;">
                                                                            <label>Subsidi</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_casha_approvalcode" Style="text-transform: uppercase;" />
																				<small style="color:red;display:none" id="lbl_validator_txt_appcode_casha">PCS Paket Must be Filled.</small>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-xs-12 col-sm-6">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label style="color: blue;">Amount</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_casha_amount" Style="text-align: center;" />
                                                                                <div class="input-group-addon">IDR</div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>


                                                            <div class="row">
                                                                <div class="col-xs-12" id="div_confirm_casha">
                                                                    <button class="btn btn-warning pull-left" onclick="confirm_casha(); return false;">Confirm</button>
                                                                </div>
                                                                <div class="col-xs-12" id="div_submit_casha" style="display: none;">
                                                                    <button class="btn btn-default pull-left" onclick="confirm_casha('cancel'); return false;">Cancel</button>
                                                                    <asp:Button runat="server" ID="save_payment_casha" OnClick="save_payment_casha_Click" OnClientClick="showOverlay();" Text="Add Cash-A" CssClass="btn btn-success pull-right" />
                                                                </div>

                                                            </div>


                                                            <script type="text/javascript">

                                                                function confirm_casha(type = '') {
                                                                    var x = document.getElementById("div_confirm_casha");

																	var glbarisan = document.getElementById("<%=txt_casha_name.ClientID%>");
																	var namapaket = document.getElementById("<%=txt_casha_number.ClientID%>");
																	var appcode = document.getElementById("<%=txt_casha_approvalcode.ClientID%>");
                                                                    
                                                                    if(type == 'cancel') {
                                                                        appcode.removeAttribute("readonly");
                                                                        namapaket.removeAttribute("readonly");
                                                                        glbarisan.removeAttribute("readonly");
                                                                        
                                                                        if (x.style.display === "none") {
                                                                            x.style.display = "block";
                                                                        } else {
                                                                            x.style.display = "none";
                                                                        }
                                                                        var y = document.getElementById("div_submit_casha");
                                                                        if (y.style.display === "none") {
                                                                            y.style.display = "block";
                                                                        } else {
                                                                            y.style.display = "none";
                                                                        }

                                                                        return false;
                                                                    }
                                                                    
                                                                    let errorcheck = false;

                                                                    if(appcode.value == "") {
                                                                        errorcheck = true;
                                                                        document.getElementById("lbl_validator_txt_appcode_casha").style.display = "";
                                                                    }else{
                                                                        document.getElementById("lbl_validator_txt_appcode_casha").style.display = "none";
                                                                        appcode.setAttribute("readonly", "readonly");
                                                                    }

                                                                    if(glbarisan.value == "") {
                                                                        errorcheck = true;
                                                                        document.getElementById("lbl_validator_txt_name_casha").style.display = "";
                                                                    }else{
                                                                        document.getElementById("lbl_validator_txt_name_casha").style.display = "none";
                                                                        glbarisan.setAttribute("readonly", "readonly");
                                                                    }

                                                                    if(namapaket.value == "") {
                                                                        errorcheck = true;
                                                                        document.getElementById("lbl_validator_txt_number_casha").style.display = "";
                                                                    }else{
                                                                        document.getElementById("lbl_validator_txt_number_casha").style.display = "none";
                                                                        namapaket.setAttribute("readonly", "readonly");
                                                                    }
                                                                    
                                                                    if(errorcheck == false) {
                                                                        if (x.style.display === "none") {
                                                                            x.style.display = "block";
                                                                        } else {
                                                                            x.style.display = "none";
                                                                        }
                                                                        var y = document.getElementById("div_submit_casha");
                                                                        if (y.style.display === "none") {
                                                                            y.style.display = "block";
                                                                        } else {
                                                                            y.style.display = "none";
                                                                        }
                                                                    }
                                                                    
                                                                }

                                                            </script>


                                                        </div>

                                                        <div role="tabpanel" class="tab-pane <%= Active_Voucher %>" id="voucher">
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="text-center">
                                                                        <h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;">
                                                                            <span class="icon-ticket icon-large" aria-hidden="true"></span>&nbsp;&nbsp;Voucher

                                                                        </h3>
                                                                    </div>
                                                                    <div class="alert alert-info" role="alert">
                                                                        <i>Notes :</i>
                                                                        <ul>
                                                                            <li><i>Silahkan dibaca terlebih dahulu Term & Condition yang berada di balik Voucher Payment sebelum melakukan transaksi.</i></li>
                                                                            <li><i>Semua Voucher Payment harus terdaftar di dalam sistem. Jika ditemukan kode voucher yang tidak dapat diverifikasi (tidak terdapat dalam sistem), silahkan dikonfirmasi langsung ke bagian Marketing.</i></li>
                                                                            <li><i>Pembayaran dengan menggunakan single Voucher Payment tidak dapat diuangkan.</i></li>
                                                                        </ul>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">

                                                                <div class="col-xs-12" id="div_confirm_voucher">
                                                                    <button class="btn btn-warning pull-left" onclick="confirm_voucher(); return false;">Confirm</button>
                                                                </div>
                                                                <div class="col-xs-12" id="div_submit_voucher" style="display: none;">
                                                                    <button class="btn btn-default pull-left" onclick="confirm_voucher(); return false;">Cancel</button>
                                                                    <asp:Button runat="server" ID="save_payment_voucher" OnClick="save_payment_voucher_Click" OnClientClick="showOverlay();" Text="Add Voucher" CssClass="btn btn-success pull-right" />
                                                                </div>

                                                            </div>


                                                            <script type="text/javascript">

                                                                function confirm_voucher() {
                                                                    var x = document.getElementById("div_confirm_voucher");
                                                                    if (x.style.display === "none") {
                                                                        x.style.display = "block";
                                                                    } else {
                                                                        x.style.display = "none";
                                                                    }
                                                                    var y = document.getElementById("div_submit_voucher");
                                                                    if (y.style.display === "none") {
                                                                        y.style.display = "block";
                                                                    } else {
                                                                        y.style.display = "none";
                                                                    }
                                                                }

                                                            </script>
                                                        </div>
                                                        <div role="tabpanel" class="tab-pane <%= Active_GiftCard %>" id="giftcard">
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="text-center">
                                                                        <h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;">
                                                                            <span class="glyphicon glyphicon-credit-card" aria-hidden="true"></span>&nbsp;&nbsp;Gift Card
                                                                        </h3>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 15px;">
                                                                            <label>Card Number</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_giftcard_cardholdernumber" Style="text-transform: uppercase;" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 15px;">
                                                                            <label style="color: blue;">Amount</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox class="form-control" runat="server" ID="txt_giftcard_amount" Style="text-align: center;" />
                                                                                <div class="input-group-addon">IDR</div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12 col-sm-12" style="padding-bottom: 15px;">

                                                                            <div id="div_giftcard_tapping" style="display: none;">
                                                                                <div class="panel panel-warning">
                                                                                    <div class="panel-body">
                                                                                        <div id="div_tap_awaiting" style="display: none;">
                                                                                            <div class="col-xs-2 col-sm-4 col-md-3"><span style="font-size: 38px; color: #f0ad4e;" class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span></div>
                                                                                            <div class="col-xs-10 col-sm-8 col-md-9">
                                                                                                <span class="">Please tap the card now !!</span><br />
                                                                                                <span class="text-muted">Status: </span><span class="text-lg text-warning">Awaiting...</span>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>


                                                            <div class="row">
                                                                <div class="col-xs-12" runat="server" id="div_giftcard_confirm">
                                                                    <input type="button" id="btnCancel" name="btnCancel" value="Cancel" onclick="cancelFunc();" class="btn btn-default pull-left" style="display: none;" />
                                                                    <input type="button" id="btnConfirm" name="btnConfirm" value="Confirm" onclick="confirmFunc();" class="btn btn-warning pull-left" />

                                                                </div>
                                                                <div class="col-xs-12" runat="server" id="div_giftcard_submit" visible="false">
                                                                    <asp:Button runat="server" ID="btn_giftcard_cancel" OnClick="btn_giftcard_cancel_Click" Text="Revise" CssClass="btn btn-danger pull-left" />
                                                                    <asp:Button runat="server" ID="save_payment_giftcard" OnClick="save_payment_giftcard_Click" OnClientClick="showOverlay();" Text="Submit" CssClass="btn btn-success pull-right" />
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <script type="text/javascript">
                                                                        function confirmFunc() {
                                                                            var xcardnumber = document.getElementById('content_txt_giftcard_cardholdernumber');
                                                                            var xcardamount = document.getElementById('content_txt_giftcard_amount');
                                                                            var xcarduid = document.getElementById('content_txt_giftcard_cardholderuid');
                                                                            var xdiv1 = document.getElementById('div_giftcard_tapping');
                                                                            var xdiv2 = document.getElementById('div_tap_awaiting');

                                                                            var xbtncancel = document.getElementById('btnCancel');
                                                                            var xbtnconfirm = document.getElementById('btnConfirm');

                                                                            xbtncancel.style.display = 'block';
                                                                            xbtnconfirm.style.display = 'none';

                                                                            xdiv1.style.display = 'block';
                                                                            xdiv2.style.display = 'block';

                                                                            xcardnumber.readOnly = true;
                                                                            xcardamount.readOnly = true;
                                                                            xcarduid.focus();
                                                                        }

                                                                        function cancelFunc() {
                                                                            var xcardnumber = document.getElementById('content_txt_giftcard_cardholdernumber');
                                                                            var xcardamount = document.getElementById('content_txt_giftcard_amount');
                                                                            var xcarduid = document.getElementById('content_txt_giftcard_cardholderuid');
                                                                            var xdiv1 = document.getElementById('div_giftcard_tapping');
                                                                            var xdiv2 = document.getElementById('div_tap_awaiting');

                                                                            var xbtncancel = document.getElementById('btnCancel');
                                                                            var xbtnconfirm = document.getElementById('btnConfirm');

                                                                            xbtncancel.style.display = 'none';
                                                                            xbtnconfirm.style.display = 'block';

                                                                            xdiv1.style.display = 'none';
                                                                            xdiv2.style.display = 'none';

                                                                            xcardnumber.readOnly = false;
                                                                            xcardamount.readOnly = false;
                                                                            xcardnumber.focus();
                                                                        }

                                                                    </script>

                                                                    <div style="width: 1px; opacity: 0;">
                                                                        <asp:TextBox runat="server" OnTextChanged="save_payment_giftcard_Click" ID="txt_giftcard_cardholderuid" Style="color: transparent;" />
                                                                    </div>
                                                                </div>
                                                            </div>


                                                        </div>
                                                        <div role="tabpanel" class="tab-pane <%= Active_BrideStoryPay %>" id="bridestorypay">
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="text-center">
                                                                        <h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;">
                                                                            <span class="glyphicon glyphicon-phone" aria-hidden="true"></span>&nbsp;&nbsp;Bridestory Pay
                                                                        </h3>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label>Account Name</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_bridestorypay_name" Style="text-transform: uppercase;" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label>Account Number</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_bridestorypay_number" Style="text-transform: uppercase;" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-xs-12 col-sm-6">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label>Bank</label>
                                                                            <div class="input-group">
                                                                                <asp:DropDownList runat="server" ID="ddl_bridestorypay_bankissuer" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-xs-12 col-sm-6">
                                                                    <div class="row">

                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label>BridestoryPay Transaction Number</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" MaxLength="16" runat="server" ID="txt_bridestorypay_transactionnumber" Style="text-transform: uppercase;" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="col-xs-12 col-sm-6">
                                                                </div>
                                                                <div class="col-xs-12 col-sm-6">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label style="color: blue;">Amount</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_bridestorypay_amount" Style="text-align: center;" />
                                                                                <div class="input-group-addon">IDR</div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="row">

                                                                <div class="col-xs-12" id="div_confirm_bridestorypay">
                                                                    <button class="btn btn-warning pull-left" onclick="confirm_bridestorypay(); return false;">Confirm</button>
                                                                </div>
                                                                <div class="col-xs-12" id="div_submit_bridestorypay" style="display: none;">
                                                                    <button class="btn btn-default pull-left" onclick="confirm_bridestorypay(); return false;">Cancel</button>
                                                                    <asp:Button runat="server" ID="save_payment_bridestorypay" OnClick="save_payment_bridestorypay_Click" OnClientClick="showOverlay();" Text="Add Bridestory Pay" CssClass="btn btn-success pull-right" />
                                                                </div>

                                                            </div>


                                                            <script type="text/javascript">

                                                                function confirm_bridestorypay() {
                                                                    var x = document.getElementById("div_confirm_bridestorypay");
                                                                    if (x.style.display === "none") {
                                                                        x.style.display = "block";
                                                                    } else {
                                                                        x.style.display = "none";
                                                                    }
                                                                    var y = document.getElementById("div_submit_bridestorypay");
                                                                    if (y.style.display === "none") {
                                                                        y.style.display = "block";
                                                                    } else {
                                                                        y.style.display = "none";
                                                                    }
                                                                }

                                                            </script>
                                                        </div>
														<!--Tambahan By Andri Priyono Cara Pembayaran Laku Emas-->
														<div role="tabpanel" class="tab-pane <%= Active_LakuEmas %>" id="lakuemas">
															<div class="row">
																<div class="col-xs-12">
																	<select id="ddlleipayment" onchange="onselectedleipayment(this);">
																		<option value="0">Pilih Pembayaran LEI</option>
																		<option value="1">Laku Tukar</option>
																		<option value="2">Debit LEI</option>
																		<option value="3">Credit LEI</option>
																		<option value='4'>QRIS LEI</option>
																	</select>
																</div>
															</div>
															<hr/>
															<!--LakuTukarEmas-->
															<div id="divlakutukar" style="display:none">
																<div class="row">
																	<div class="col-xs-12">
																		<div class="text-center">
																			<h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;">
																				<span class="glyphicon glyphicon-inbox" aria-hidden="true"></span>&nbsp;&nbsp;Laku Tukar
																			</h3>
																		</div>
																	</div>
																</div>
																<div class="row">
																	<div class="col-xs-12">
																		<div class="row">
																			<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 15px;">
																				<label>No. Laku Emas</label>
																				<div class="input-group">
																					<asp:TextBox runat="server" class="form-control" ID="txt_no_laku_emas" />
																				</div>
																			</div>
																			<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 15px;">
																				<label>Weight LM (gr)</label>
																				<div class="input-group">
																					<asp:TextBox runat="server" class="form-control" ID="txt_weight_lm" onkeyup="onkeypresstxtrateorweightlm(); return true;" />
																					<small style="color:red">Use ',' for decimal format.</small>
																				</div>
																			</div>
																			<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 15px;">
																				<label>Rate LM (Rp/gr)</label>
																				<div class="input-group">
																					<asp:TextBox runat="server" class="form-control" ID="txt_rate_lm" onkeyup="onkeypresstxtrateorweightlm(); return true;" />
																				</div>
																			</div>
																			<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 15px;">
																				<label>Value (Rp)</label>
																				<div class="input-group">
																					<asp:TextBox runat="server" class="form-control" ID="txt_value_lm" />
																				</div>
																			</div>
																		</div>
																	</div>
																</div>

																<div class="row">
																	<div class="col-xs-12" id="div_confirm_lakuemas">
																		<button type="button" class="btn btn-warning" onclick="confirm_lakuemas()">Confirm</button>
																	</div>
																	<div class="col-xs-12" id="div_submit_lakuemas" style="display: none">
																		<button type="button" class="btn btn-default" onclick="confirm_lakuemas()">Cancel</button>
																		<asp:Button runat="server" CssClass="btn btn-success pull-right" Text="Add Laku Emas" OnClick="save_payment_laku_emas_Click" OnClientClick="showOverlay();" ID="save_payment_laku_emas" />
																	</div>
																</div>
																<script type="text/javascript">
																	function confirm_lakuemas() {
																		var x = document.getElementById("div_confirm_lakuemas");
																		if (x.style.display === "none") {
																			x.style.display = "block";
																		} else {
																			x.style.display = "none";
																		}
																		var y = document.getElementById("div_submit_lakuemas");
																		if (y.style.display === "none") {
																			y.style.display = "block";
																		} else {
																			y.style.display = "none";
																		}
																	}
																	function onkeypresstxtrateorweightlm() {
																		if (x == "" || y == "") {
																			document.getElementById("<%=txt_value_lm.ClientID%>").value = "";
																		}
																		else {
																			var x = document.getElementById("<%=txt_rate_lm.ClientID%>").value.replace(",",".");
																			var y = document.getElementById("<%=txt_weight_lm.ClientID%>").value.replace(",",".");
																			var z = parseFloat(x) * parseFloat(y);
																			document.getElementById("<%=txt_value_lm.ClientID%>").value = Math.round(z);
																		}
																	}
																</script>
															</div>
															<!--DebitLakuEmas-->
															<div id="divdebitlakuemas" style="display:none">
																<div class="row">
																	<div class="col-xs-12">
																		<div class="text-center">
																			<h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;">
																				<span class="glyphicon glyphicon-credit-card" aria-hidden="true"></span>&nbsp;&nbsp;Debit Laku Emas
																			</h3>
																		</div>
																	</div>
																</div>
																<div class="row">
																	<div class="col-xs-12">
																		<div class="row">
																			<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
																				<label>Card Holder Name</label>
																				<div class="input-group" style="width: 100%;">
																					<asp:TextBox CssClass="form-control" runat="server" ID="txt_debitlakuemas_cardholdername" Style="text-transform: uppercase;" />
																				</div>
																			</div>
																			<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
																				<label>Card Holder Number</label>
																				<div class="input-group" style="width: 100%;">
																					<asp:TextBox CssClass="form-control" runat="server" ID="txt_debitlakuemas_cardholdernumber" Style="text-transform: uppercase;" />
																				</div>
																			</div>
																		</div>
																	</div>

																	<div class="col-xs-12 col-sm-6">
																		<div class="row">
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																				<label>Approval Code</label>
																				<div class="input-group" style="width: 100%;">
																					<asp:TextBox CssClass="form-control" runat="server" ID="txt_debitlakuemas_approvalcode" />
																					<small style="color:red;display:none" id="lbl_validator_appcode_debitlakuemas">Appoval Code Must be Filled.</small>
																				</div>
																			</div>
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																				<label>EDC</label>
																				<div class="input-group" style="width: 100%;">
																					<asp:TextBox runat="server" CssClass="form-control" ID="ddl_debitlakuemas_edc" Enabled="false" Text="LAKU EMAS" />
																					<asp:DropDownList runat="server" ID="ddl_edc_bank_debit_laku_emas">
																						<asp:ListItem Value="1">BCA</asp:ListItem>
																						<asp:ListItem Value="2">BNI</asp:ListItem>
																						<asp:ListItem Value="3">BRI</asp:ListItem>
																					</asp:DropDownList>
																				</div>
																			</div>
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																				<label>Bank Issuer</label>
																				<div class="input-group">
																					<asp:DropDownList runat="server" ID="ddl_debitlakuemas_bankissuer" />
																				</div>
																			</div>
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																				<label style="color: blue;">Rate LM</label>
																				<div class="input-group" style="width: 100%;">
																					<asp:TextBox runat="server" CssClass="form-control" ID="txt_debitlakuemas_rate" readonly="true"/>
																					<div class="input-group-addon">IDR</div>
																				</div>
																			</div>
																		</div>
																	</div>
																	<div class="col-xs-12 col-sm-6">
																		<div class="row">
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																				<label>Card Type</label>
																				<div class="input-group">
																					<asp:DropDownList runat="server" ID="ddl_debitlakuemas_cardtype" />
																				</div>
																			</div>
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																				<label>Card Category</label>
																				<div class="input-group">
																					<asp:DropDownList runat="server" ID="ddl_debitlakuemas_cardcategory" />
																				</div>
																			</div>
																		</div>
																	</div>
																	<div class="col-xs-12 col-sm-6" style="display:none">
																		<div class="container img-thumbnail">
																			<label style="font-size:12px"><b>Detail</b></label>
																			<hr style="border-top:solid 1px silver ;margin-top:5px"/>
																			<div class="form-horizontal" runat="server" id="div_debit_lei" style="font-size:12px;font-family:consolas;">
																				<div style="display:none">
																				<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																					<label class="col-xs-4">Weight PG 75%</label>
																					<label class="col-xs-1">:</label>
																					<div class="col-xs-6">
																						<div class="col-xs-8">
																							<asp:Label runat="server" ID="txt_pg_weight" Style="text-align: center;" />
																						</div>&nbsp; gr
																					</div>
																				</div>
																				<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																					<label class="col-xs-4">Value PG 75%</label>
																					<label class="col-xs-1">:</label>
																					<div class="col-xs-6">
																						<div class="col-xs-8">
																							<asp:Label runat="server" ID="txt_pg_amount" Style="text-align: center;" />&nbsp; IDR
																						</div>
																					</div>
																				</div>
																				<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																					<label class="col-xs-4">Weight LM</label>
																					<label class="col-xs-1">:</label>
																					<div class="col-xs-6">
																						<div class="col-xs-8">
																							<asp:Label runat="server" ID="txt_lm_weight" Style="text-align: center;" />
																						</div>&nbsp; gr
																					</div>
																				</div>
																				<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																					<label class="col-xs-4">Value LM</label>
																					<label class="col-xs-1">:</label>
																					<div class="col-xs-6">
																						<div class="col-xs-8">
																							<asp:Label runat="server" ID="txt_lm_amount" Style="text-align: center;" />
																						</div>&nbsp; IDR
																					</div>
																				</div>
																				<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																					<label class="col-xs-4">Ongkos</label>
																					<label class="col-xs-1">:</label>
																					<div class="col-xs-6">
																						<div class="col-xs-8">
																							<asp:Label runat="server" ID="txt_ongkos_amount" Style="text-align: center;" />
																						</div> &nbsp;IDR
																					</div>
																				</div>
																				</div>
																			</div>
																		</div>
																	</div>
																	<br/>
																	<div class="col-sm-6 col-sm-offset-6">
																		<div class="row">
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;display:none">
																				<label style="color: blue;">Value LM Total</label>
																				<div class="input-group">
																					<asp:TextBox class="form-control" runat="server" ID="txt_debitlakuemas_lm_amount" Style="text-align: center;" AutoPostBack="true" OnTextChanged="onChangeAmountDebitLakuEmas" readonly="true" />
																					<div class="input-group-addon">IDR</div>
																				</div>
																			</div>
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;display:none">
																				<label style="color: blue;">Ongkos</label>
																				<div class="input-group">
																					<asp:TextBox class="form-control" runat="server" ID="txt_debitlakuemas_ongkos_amount" Style="text-align: center;" AutoPostBack="true" OnTextChanged="onChangeAmountDebitLakuEmas" />
																					<div class="input-group-addon">IDR</div>
																				</div>
																			</div>
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																				<label style="color: blue;">Amount</label>
																				<div class="input-group">
																					<asp:TextBox class="form-control" runat="server" ID="txt_debitlakuemas_amount" Style="text-align: center;" />
																					<div class="input-group-addon">IDR</div>
																				</div>
																			</div>
																		</div>
																	</div>
																</div>


																<div class="row">
																	<asp:HiddenField runat="server" ID="hf_ecr_dclakuemas_flag" Value="0" />
																	<asp:HiddenField runat="server" ID="hf_ecr_dclakuemas_value" Value="" />
																	<asp:HiddenField runat="server" ID="hf_ecr_paiddclakuemas_value" Value="" />
																	<div class="col-xs-12" id="div_confirm_debitlakuemas">
																		<button class="btn btn-warning pull-left" onclick="confirm_debitlakuemas(); return false;">Confirm</button>
																	</div>
																	<div class="col-xs-12" id="div_submit_debitlakuemas" style="display: none;">
																		<button class="btn btn-default pull-left" onclick="confirm_debitlakuemas(); return false;">Cancel</button>
																		<asp:Button runat="server" ID="save_payment_debitlakuemas" Text="Add Debit Laku Emas" OnClick="save_payment_debitlakuemas_Click" OnClientClick="showOverlay();" CssClass="btn btn-success pull-right" />
																	</div>
																</div>
																<script type="text/javascript">
																	function confirm_debitlakuemas() {
																		var x = document.getElementById("div_confirm_debitlakuemas");
																		var appcode = document.getElementById("<%=txt_debitlakuemas_approvalcode.ClientID%>").value;
																		if (appcode != "") {
																			document.getElementById("lbl_validator_appcode_debitlakuemas").style.display = "none";
																			if (x.style.display === "none") {
																				x.style.display = "block";
																			} else {
																				x.style.display = "none";
																			}
																			var y = document.getElementById("div_submit_debitlakuemas");
																			if (y.style.display === "none") {
																				y.style.display = "block";
																			} else {
																				y.style.display = "none";
																			}
																		}
																		else {
																			document.getElementById("lbl_validator_appcode_debitlakuemas").style.display = "";
																		}
																	}
																</script>
															</div>
															<!--CreditLakuEmas-->
															<div id="divcreditlakuemas" style="display:none">
																<div class="row">
																	<div class="col-xs-12">
																		<div class="row">
																			<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
																				<label>Card Holder Name</label>
																				<div class="input-group" style="width: 100%;">
																					<asp:TextBox CssClass="form-control" runat="server" ID="txt_creditlakuemas_cardholdername" Style="text-transform: uppercase;" />
																				</div>
																			</div>
																			<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
																				<label>Card Holder Number</label>
																				<div class="input-group" style="width: 100%;">
																					<asp:TextBox CssClass="form-control" runat="server" ID="txt_creditlakuemas_cardholdernumber" Style="text-transform: uppercase;" />
																				</div>
																			</div>
																		</div>
																	</div>

																	<div class="col-xs-12 col-sm-6">
																		<div class="row">
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																				<label>Approval Code</label>
																				<div class="input-group" style="width: 100%;">
																					<asp:TextBox CssClass="form-control" runat="server" ID="txt_creditlakuemas_approvalcode" />
																					<small style="color:red;display:none" id="lbl_validator_appcode_creditlakuemas">Appoval Code Must be Filled.</small>
																				</div>
																			</div>
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																				<label>EDC</label>
																				<div class="input-group" style="width: 100%;">
																					<asp:TextBox runat="server" CssClass="form-control" ID="ddl_creditlakuemas_edc" Enabled="false" Text="LAKU EMAS" />
																					<asp:DropDownList runat="server" ID="ddl_edc_bank_credit_laku_emas">
																						<asp:ListItem Value="26">BCA</asp:ListItem>
																						<asp:ListItem Value="27">BNI</asp:ListItem>
																						<asp:ListItem Value="28">BRI</asp:ListItem>
																					</asp:DropDownList>
																				</div>
																			</div>
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																				<label>Bank Issuer</label>
																				<div class="input-group">
																					<asp:DropDownList runat="server" ID="ddl_creditlakuemas_bankissuer" />
																				</div>
																			</div>
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																				<label>Program</label>
																				<div class="input-group">
																					<asp:DropDownList runat="server" ID="ddl_creditlakuemas_bankprogram" />
																				</div>
																			</div>
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																				<label style="color: blue;">Rate LM</label>
																				<div class="input-group" style="width: 100%;">
																					<asp:TextBox runat="server" CssClass="form-control" ID="txt_creditlakuemas_rate" readonly="true"/>
																					<div class="input-group-addon">IDR</div>
																				</div>
																			</div>
																		</div>
																	</div>
																	<div class="col-xs-12 col-sm-6">
																		<div class="row">
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																				<label>Card Type</label>
																				<div class="input-group">
																					<asp:DropDownList runat="server" ID="ddl_creditlakuemas_cardtype" />
																				</div>
																			</div>
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																				<label>Card Category</label>
																				<div class="input-group">
																					<asp:DropDownList runat="server" ID="ddl_creditlakuemas_cardcategory" />
																				</div>
																			</div>
																		</div>
																	</div>
																	<div class="col-xs-12 col-sm-6" style="display:none">
																		<div class="container img-thumbnail">
																			<label style="font-size:12px"><b>Detail</b></label>
																			<hr style="border-top:solid 1px silver ;margin-top:5px"/>
																			<div class="form-horizontal" runat="server" id="div_credit_lei" style="font-size:12px;font-family:consolas">
																			</div>
																		</div>
																	</div>
																	<div class="col-sm-6">
																		<div class="row">
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;display:none">
																				<label style="color: blue;">Value LM Total</label>
																				<div class="input-group">
																					<asp:TextBox class="form-control" runat="server" ID="txt_creditlakuemas_lm_amount" Style="text-align: center;" readonly="true" />
																					<div class="input-group-addon">IDR</div>
																				</div>
																			</div>
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;display:none">
																				<label style="color: blue;">Ongkos</label>
																				<div class="input-group">
																					<asp:TextBox class="form-control" runat="server" ID="txt_creditlakuemas_ongkos_amount" Style="text-align: center;" />
																					<div class="input-group-addon">IDR</div>
																				</div>
																			</div>
																			<div class="form-group col-xs-12" style="padding-bottom: 10px;">
																				<label style="color: blue;">Amount</label>
																				<div class="input-group">
																					<asp:TextBox class="form-control" runat="server" ID="txt_creditlakuemas_amount" Style="text-align: center;" />
																					<div class="input-group-addon">IDR</div>
																				</div>
																			</div>
																		</div>
																	</div>
																</div>


																<div class="row">
																	<asp:HiddenField runat="server" ID="hf_ecr_cclakuemas_flag" Value="0" />
																	<asp:HiddenField runat="server" ID="hf_ecr_cclakuemas_value" Value="" />
																	<asp:HiddenField runat="server" ID="hf_ecr_paidcclakuemas_value" Value="" />
																	<div class="col-xs-12" id="div_confirm_creditlakuemas">
																		<button class="btn btn-warning pull-left" onclick="confirm_creditlakuemas(); return false;">Confirm</button>
																	</div>
																	<div class="col-xs-12" id="div_submit_creditlakuemas" style="display: none;">
																		<button class="btn btn-default pull-left" onclick="confirm_creditlakuemas(); return false;">Cancel</button>
																		<asp:Button runat="server" ID="save_payment_creditlakuemas" Text="Add Credit Laku Emas" OnClick="save_payment_creditlakuemas_Click" OnClientClick="showOverlay();" CssClass="btn btn-success pull-right" />
																	</div>
																</div>


																<script type="text/javascript">
																	function confirm_creditlakuemas() {
																		var x = document.getElementById("div_confirm_creditlakuemas");
																		var appcode = document.getElementById("<%=txt_creditlakuemas_approvalcode.ClientID%>").value;
																		if (appcode != "") {
																			document.getElementById("lbl_validator_appcode_creditlakuemas").style.display = "none";
																			if (x.style.display === "none") {
																				x.style.display = "block";
																			} else {
																				x.style.display = "none";
																			}
																			var y = document.getElementById("div_submit_creditlakuemas");
																			if (y.style.display === "none") {
																				y.style.display = "block";
																			} else {
																				y.style.display = "none";
																			}
																		}
																		else {
																			document.getElementById("lbl_validator_appcode_creditlakuemas").style.display = "";
																		}
																	}
																</script>
															</div>
															<!--QRIS LAKU EMAS-->
															<div id="divqrislakuemas" style="display:none">
																<div class="row">
																	<div class="col-xs-12">
																		<div class="row">
																			<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
																				<label>Customer Name</label>
																				<div class="input-group" style="width: 100%;">
																					<asp:TextBox CssClass="form-control" runat="server" ID="txt_qrislakuemas_custname" Style="text-transform: uppercase;" />
																				</div>
																			</div>
																			<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
																				<label>Customer Phone Number</label>
																				<div class="input-group" style="width: 100%;">
																					<asp:TextBox CssClass="form-control" runat="server" ID="txt_qrislakuemas_custphonenumber" Style="text-transform: uppercase;" />
																				</div>
																			</div>
																		</div>
																	</div>

																	<div class="col-xs-12">
																		<div class="row">
																			<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
																				<label>Approval Code</label>
																				<div class="input-group" style="width: 100%;">
																					<asp:TextBox CssClass="form-control" runat="server" ID="txt_qrislakuemas_transactionnumber" />
																					<small style="color:red;display:none" id="lbl_validator_trans_qrislakuemas">QRIS Code Must be Filled.</small>
																				</div>
																			</div>
																			<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
																				<label>EDC</label>
																				<div class="input-group">
																					<asp:DropDownList runat="server" ID="ddl_qrislakuemas_cardtype" />
																				</div>
																			</div>
																		</div>
																	</div>
																	<div class="col-xs-12">
																		<div class="row">
																			<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
																				<label>QRIS Issuer</label>
																				<div class="input-group">
																					<asp:DropDownList runat="server" ID="ddl_qrislakuemas_bankissuer" />
																				</div>
																			</div>
																			<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
																				<label style="color: blue;">Amount</label>
																				<div class="input-group">
																					<asp:TextBox class="form-control" runat="server" ID="txt_qrislakuemas_amount" Style="text-align: center;" />
																					<div class="input-group-addon">IDR</div>
																				</div>
																			</div>
																		</div>
																	</div>
																</div>


																<div class="row">
																	<asp:HiddenField runat="server" ID="hf_ecr_qrislakuemas_flag" Value="0" />
																	<asp:HiddenField runat="server" ID="hf_ecr_qrislakuemas_value" Value="" />
																	<div class="col-xs-12" id="div_confirm_qrislakuemas">
																		<button class="btn btn-warning pull-left" onclick="confirm_qrislakuemas(); return false;">Confirm</button>
																	</div>
																	<div class="col-xs-12" id="div_submit_qrislakuemas" style="display: none;">
																		<button class="btn btn-default pull-left" onclick="confirm_qrislakuemas(); return false;">Cancel</button>
																		<asp:Button runat="server" ID="save_payment_qrislakuemas" OnClick="save_payment_qrislakuemas_Click" OnClientClick="showOverlay();" Text="Add QRIS" CssClass="btn btn-success pull-right" />
																	</div>

																</div>


																<script type="text/javascript">
																	function confirm_qrislakuemas() {
																		var x = document.getElementById("div_confirm_qrislakuemas");
																		var appcode = document.getElementById("<%=txt_qrislakuemas_transactionnumber.ClientID%>").value;
																		
																		if (appcode != "") {
																			document.getElementById("lbl_validator_trans_qrislakuemas").style.display = "none";
																			
																			if (x.style.display === "none") {
																				x.style.display = "block";
																			} else {
																				x.style.display = "none";
																			}
																			var y = document.getElementById("div_submit_qrislakuemas");
																			if (y.style.display === "none") {
																				y.style.display = "block";
																			} else {
																				y.style.display = "none";
																			}
																		}
																		else {
																			if(appcode == ""){
																				document.getElementById("lbl_validator_trans_qrislakuemas").style.display = "";
																			}
																			else{
																				document.getElementById("lbl_validator_trans_qrislakuemas").style.display = "none";
																			}
																		}
																	}
																</script>
															</div>
															<script>
																function onselectedleipayment(e)
																{
																	if(e.value == "0")
																	{
																		document.getElementById("divlakutukar").style.display = "none";
																		document.getElementById("divdebitlakuemas").style.display = "none";
																		document.getElementById("divcreditlakuemas").style.display = "none";
																		document.getElementById("divqrislakuemas").style.display = "none";
																	}
																	else if(e.value == "1")
																	{
																		document.getElementById("divlakutukar").style.display = "block";
																		document.getElementById("divdebitlakuemas").style.display = "none";
																		document.getElementById("divcreditlakuemas").style.display = "none";
																		document.getElementById("divqrislakuemas").style.display = "none";
																	}
																	else if(e.value == "2")
																	{
																		document.getElementById("divlakutukar").style.display = "none";
																		document.getElementById("divdebitlakuemas").style.display = "block";
																		document.getElementById("divcreditlakuemas").style.display = "none";
																		document.getElementById("divqrislakuemas").style.display = "none";
																	}
																	else if(e.value == "3")
																	{
																		document.getElementById("divlakutukar").style.display = "none";
																		document.getElementById("divdebitlakuemas").style.display = "none";
																		document.getElementById("divcreditlakuemas").style.display = "block";
																		document.getElementById("divqrislakuemas").style.display = "none";
																	}
																	else if(e.value == "4")
																	{
																		document.getElementById("divlakutukar").style.display = "none";
																		document.getElementById("divdebitlakuemas").style.display = "none";
																		document.getElementById("divcreditlakuemas").style.display = "none";
																		document.getElementById("divqrislakuemas").style.display = "block";
																	}
																}
															</script>
                                                        </div>
														<div role="tabpanel" class="tab-pane <%= Active_CashLEI %>" id="cashlei">
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="text-center">
                                                                        <h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;">
                                                                            <span class="icon-money icon-large" aria-hidden="true"></span>&nbsp;&nbsp;Cash LEI
                                                                        </h3>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="alert alert-info" role="alert">
                                                                        <b><i>Notes :</i></b>
                                                                        <ul>
                                                                            <li><i><b>Cash out</b> hanya diberlakukan untuk pembayaran lebih khusus Down Payment.</i></li>
                                                                            <li><i>Pembayaran lebih bayar untuk Voucher Payment (Gift Certificate) <b>tidak dapat melanjutkan transaksi Cash out</b>.</i></li>
                                                                        </ul>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-xs-12 col-sm-6">
                                                                    <div style="text-align: center; padding-top: 30px;">
                                                                        <label class="radio-inline">
                                                                            <asp:RadioButton runat="server" GroupName="rdo_cashlei" ID="rdo_cashlei_out" />
                                                                            Cash Out
                                                                        </label>
                                                                        <label class="radio-inline">
                                                                            <asp:RadioButton runat="server" GroupName="rdo_cashlei" ID="rdo_cashlei_in" />
                                                                            Cash In
                                                                        </label>
                                                                    </div>
                                                                </div>
                                                                <div class="col-xs-12 col-sm-6">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                            <label style="color: blue;">Amount</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox class="form-control" runat="server" ID="txt_cashlei_amount" Style="text-align: center;" />
                                                                                <div class="input-group-addon">IDR</div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-xs-12" id="div_confirm_cashlei">
                                                                    <button class="btn btn-warning pull-left" onclick="confirm_cashlei(); return false;">Confirm</button>
                                                                </div>
                                                                <div class="col-xs-12" id="div_submit_cashlei" style="display: none;">
                                                                    <button class="btn btn-default pull-left" onclick="confirm_cashlei(); return false;">Cancel</button>
                                                                    <asp:Button runat="server" ID="save_payment_cashlei" OnClick="save_payment_cashlei_Click" OnClientClick="showOverlay();" Text="Add Cash" CssClass="btn btn-success pull-right" />
                                                                </div>
                                                            </div>
                                                            <script type="text/javascript">
                                                                function confirm_cashlei() {
                                                                    var x = document.getElementById("div_confirm_cashlei");
                                                                    if (x.style.display === "none") {
                                                                        x.style.display = "block";
                                                                    } else {
                                                                        x.style.display = "none";
                                                                    }
                                                                    var y = document.getElementById("div_submit_cashlei");
                                                                    if (y.style.display === "none") {
                                                                        y.style.display = "block";
                                                                    } else {
                                                                        y.style.display = "none";
                                                                    }
                                                                }

                                                            </script>
                                                        </div>
														<div role="tabpanel" class="tab-pane <%= Active_QRIS %>" id="qris">
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="text-center">
                                                                        <h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;">
                                                                            <span class="glyphicon glyphicon-credit-card" aria-hidden="true"></span>&nbsp;&nbsp;QRIS
                                                                        </h3>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label>Customer Name</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_qris_custname" Style="text-transform: uppercase;" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label>Customer Phone Number</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_qris_custphonenumber" Style="text-transform: uppercase;" />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-xs-12">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label>Reference Number</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_qris_transactionnumber" />
																				<small style="color:red;display:none" id="lbl_validator_trans_qris">QRIS Code Must be Filled.</small>
                                                                            </div>
                                                                        </div>
																		<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
																			<label>EDC</label>
																			<div class="input-group">
																				<asp:DropDownList runat="server" ID="ddl_qris_cardtype" />
																			</div>
																		</div>
                                                                    </div>
                                                                </div>
                                                                <div class="col-xs-12">
                                                                    <div class="row">
																		<div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
																			<label>QRIS Issuer</label>
																			<div class="input-group">
																				<asp:DropDownList runat="server" ID="ddl_qris_bankissuer" />
																			</div>
																		</div>
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label style="color: blue;">Amount</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox class="form-control" runat="server" ID="txt_qris_amount" Style="text-align: center;" />
                                                                                <div class="input-group-addon">IDR</div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>


                                                            <div class="row">
                                                                <asp:HiddenField runat="server" ID="hf_ecr_qris_flag" Value="0" />
                                                                <asp:HiddenField runat="server" ID="hf_ecr_qris_value" Value="" />
                                                                <div class="col-xs-12" id="div_confirm_qris">
                                                                    <button class="btn btn-warning pull-left" onclick="confirm_qris(); return false;">Confirm</button>
                                                                </div>
                                                                <div class="col-xs-12" id="div_submit_qris" style="display: none;">
                                                                    <button class="btn btn-default pull-left" onclick="confirm_qris(); return false;">Cancel</button>
                                                                    <asp:Button runat="server" ID="save_payment_qris" OnClick="save_payment_qris_Click" OnClientClick="showOverlay();" Text="Add QRIS" CssClass="btn btn-success pull-right" />
                                                                </div>

                                                            </div>


                                                            <script type="text/javascript">
                                                                function confirm_qris() {
                                                                    var x = document.getElementById("div_confirm_qris");
																	var appcode = document.getElementById("<%=txt_qris_transactionnumber.ClientID%>").value;
																	
                                                                    if (appcode != "") {
																		document.getElementById("lbl_validator_trans_qris").style.display = "none";
																		
																		if (x.style.display === "none") {
																			x.style.display = "block";
																		} else {
																			x.style.display = "none";
																		}
																		var y = document.getElementById("div_submit_qris");
																		if (y.style.display === "none") {
																			y.style.display = "block";
																		} else {
																			y.style.display = "none";
																		}
																	}
                                                                    else {
																		if(appcode == ""){
																			document.getElementById("lbl_validator_trans_qris").style.display = "";
																		}
																		else{
																			document.getElementById("lbl_validator_trans_qris").style.display = "none";
																		}
                                                                    }
                                                                }
                                                            </script>

                                                        </div>
														<div role="tabpanel" class="tab-pane <%= Active_Xendit %>" id="xendit">
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="text-center">
                                                                        <h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;">
                                                                            <span class="glyphicon glyphicon-phone" aria-hidden="true"></span>&nbsp;&nbsp;Xendit
                                                                        </h3>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label>Payment Via : </label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:DropDownList runat="server" ID="ddl_payment_xendit_methode" onchange="change_payment_xendit(this);">
																					<asp:ListItem Value="1">Virtual Account</asp:ListItem>
																					<asp:ListItem Value="2">Credit Card / Debit</asp:ListItem>
																				</asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label>Xendit Transaction ID</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_xendit_idtransaction" Style="text-transform: uppercase;" />
																				<small style="color:red;display:none" id="lbl_validator_trans_xendit">Xendit Transaction ID Must be Filled.</small>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-xs-12">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;display:none" id="div_bank_xendit">
                                                                            <label>Bank Issuer</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:DropDownList runat="server" ID="ddl_xendit_bankissuer">
																				</asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12 col-sm-6 pull-right" style="padding-bottom: 10px;">
                                                                            <label style="color: blue;">Amount</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox class="form-control" runat="server" ID="txt_xendit_amount" Style="text-align: center;" />
                                                                                <div class="input-group-addon">IDR</div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <asp:HiddenField runat="server" ID="hf_ecr_xendit_flag" Value="0" />
                                                                <asp:HiddenField runat="server" ID="hf_ecr_xendit_value" Value="" />
                                                                <div class="col-xs-12" id="div_confirm_xendit">
                                                                    <button class="btn btn-warning pull-left" onclick="confirm_xendit(); return false;">Confirm</button>
                                                                </div>
                                                                <div class="col-xs-12" id="div_submit_xendit" style="display: none;">
                                                                    <button class="btn btn-default pull-left" onclick="confirm_xendit(); return false;">Cancel</button>
                                                                    <asp:Button runat="server" ID="save_payment_xendit" OnClick="save_payment_xendit_Click" OnClientClick="showOverlay();" Text="Add Xendit" CssClass="btn btn-success pull-right" />
                                                                </div>
                                                            </div>
															<script type="text/javascript">
																function change_payment_xendit(e){
																	var ddl = document.getElementById("div_bank_xendit");
																	if(e.value == "1"){
																		ddl.style.display = "none";
																	}
																	else{
																		ddl.style.display = "block";
																	}
																}
                                                                function confirm_xendit() {
                                                                    var x = document.getElementById("div_confirm_xendit");
																	var appcode = document.getElementById("<%=txt_xendit_idtransaction.ClientID%>").value;
																	
                                                                    if (appcode != "") {
																		document.getElementById("lbl_validator_trans_xendit").style.display = "none";
																		
																		if (x.style.display === "none") {
																			x.style.display = "block";
																		} else {
																			x.style.display = "none";
																		}
																		var y = document.getElementById("div_submit_xendit");
																		if (y.style.display === "none") {
																			y.style.display = "block";
																		} else {
																			y.style.display = "none";
																		}
																	}
                                                                    else {
																		if(appcode == ""){
																			document.getElementById("lbl_validator_trans_xendit").style.display = "";
																		}
																		else{
																			document.getElementById("lbl_validator_trans_xendit").style.display = "none";
																		}
                                                                    }
                                                                }
                                                            </script>
                                                        </div>
														<div role="tabpanel" class="tab-pane <%= Active_Ecommerce %>" id="ecommerce">
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="text-center">
                                                                        <h3 style="border-bottom: solid 1px silver; padding: 0px 0px 15px 0px; margin: 0px 0px 15px 0px;">
                                                                            <span class="glyphicon glyphicon-phone" aria-hidden="true"></span>&nbsp;&nbsp;E-commerce
                                                                        </h3>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-xs-12">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label>Platform : </label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:DropDownList runat="server" ID="ddl_payment_ecommerce_platform">
																					
																				</asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="form-group col-xs-12 col-sm-6" style="padding-bottom: 10px;">
                                                                            <label>Transaction ID</label>
                                                                            <div class="input-group" style="width: 100%;">
                                                                                <asp:TextBox CssClass="form-control" runat="server" ID="txt_ecommerce_idtransaction" Style="text-transform: uppercase;" />
																				<small style="color:red;display:none" id="lbl_validator_trans_ecommerce">Transaction ID Must be Filled.</small>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="col-xs-12">
                                                                    <div class="row">
                                                                        <div class="form-group col-xs-12 col-sm-6 pull-right" style="padding-bottom: 10px;">
                                                                            <label style="color: blue;">Amount</label>
                                                                            <div class="input-group">
                                                                                <asp:TextBox class="form-control" runat="server" ID="txt_ecommerce_amount" Style="text-align: center;" />
                                                                                <div class="input-group-addon">IDR</div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <asp:HiddenField runat="server" ID="hf_ecr_ecommerce_flag" Value="0" />
                                                                <asp:HiddenField runat="server" ID="hf_ecr_ecommerce_value" Value="" />
                                                                <div class="col-xs-12" id="div_confirm_ecommerce">
                                                                    <button class="btn btn-warning pull-left" onclick="confirm_ecommerce(); return false;">Confirm</button>
                                                                </div>
                                                                <div class="col-xs-12" id="div_submit_ecommerce" style="display: none;">
                                                                    <button class="btn btn-default pull-left" onclick="confirm_ecommerce(); return false;">Cancel</button>
                                                                    <asp:Button runat="server" ID="save_payment_ecommerce" OnClick="save_payment_ecommerce_Click" OnClientClick="showOverlay();" Text="Add E-commerce" CssClass="btn btn-success pull-right" />
                                                                </div>
                                                            </div>
															<script type="text/javascript">
                                                                function confirm_ecommerce() {
                                                                    var x = document.getElementById("div_confirm_ecommerce");
																	var appcode = document.getElementById("<%=txt_ecommerce_idtransaction.ClientID%>").value;
																	
                                                                    if (appcode != "") {
																		document.getElementById("lbl_validator_trans_ecommerce").style.display = "none";
																		
																		if (x.style.display === "none") {
																			x.style.display = "block";
																		} else {
																			x.style.display = "none";
																		}
																		var y = document.getElementById("div_submit_ecommerce");
																		if (y.style.display === "none") {
																			y.style.display = "block";
																		} else {
																			y.style.display = "none";
																		}
																	}
                                                                    else {
																		if(appcode == ""){
																			document.getElementById("lbl_validator_trans_ecommerce").style.display = "";
																		}
																		else{
																			document.getElementById("lbl_validator_trans_ecommerce").style.display = "none";
																		}
                                                                    }
                                                                }
                                                            </script>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-xs-12">
                                                    <asp:Literal runat="server" ID="lit_message_general" />
                                                    <asp:Literal runat="server" ID="lit_message_test" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-6">
                                    <div class="row">
                                        <div class="col-xs-12">
                                            <div class="panel panel-default">
                                                <div class="panel-body">
                                                    <div class="panel panel-default">
                                                        <div class="panel-body">
                                                            <div class="col-xs-12 col-sm-4">
                                                                <span class="text-muted">Total Amount Due</span><br />
                                                                <span class="text-md" style="color: black; font-weight: bold;" id="spn_payment_totalamountdue" runat="server" />
                                                            </div>
                                                            <div class="col-xs-12 col-sm-4">
                                                                <span class="text-muted">Amount Paid</span><br />
                                                                <span class="text-md" style="color: green; font-weight: bold;" id="spn_payment_amountpaid" runat="server" />
                                                            </div>
                                                            <div class="col-xs-12 col-sm-4">
                                                                <span class="text-muted">Balance Due</span><br />
                                                                <span class="text-md" style="color: red; font-weight: bold;" id="spn_payment_balancedue" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="table-responsive">
                                                        <asp:Table ID="tb3" runat="server" CssClass="table-list table-striped" Width="100%">
                                                            <asp:TableHeaderRow>
                                                                <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                                                                <asp:TableHeaderCell Width="250">Detail</asp:TableHeaderCell>
                                                                <asp:TableHeaderCell Width="150">Amount</asp:TableHeaderCell>
																<asp:TableHeaderCell Width="50">Void</asp:TableHeaderCell>
                                                            </asp:TableHeaderRow>
                                                        </asp:Table>
                                                    </div>

                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>


                    <div class="panel panel-default">
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="file-subheader hidden-sm hidden-xs">
                                        Summary
                                    </div>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-6">
                                    <uc1:paymentSummary ID="paymentSummary" runat="server" />
                                </div>
                                <div class="col-xs-12 col-sm-6" runat="server" id="div_alert_submit">
                                    <div class="alert alert-danger">
                                        <ul>
                                            <li>Klik <b>Submit</b> untuk konfirmasi bahwa Nama Customer sudah sesuai dan Barang yang diinput serta Cara Bayar yang digunakan sudah benar.</li>
                                            <li>Jika tidak melakukan <b>Submit</b>, maka Sales Order akan gantung sehingga Customer tidak memperoleh poin, serta Report Penjualan di Myapps & Pembayaran pada GL tidak masuk.</li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-xs-12">
                                    <asp:Literal runat="server" ID="lit_keypostsap" />
                                </div>
                                <div class="col-xs-12">
                                    <asp:Literal runat="server" ID="lit_warning_payment" />
                                    <asp:Literal runat="server" ID="lit_error_payment" />
                                    <asp:Literal runat="server" ID="lit_error_sap" />
                                    <asp:Literal runat="server" ID="lit_error_stamps" />
                                    <asp:Literal runat="server" ID="lit_error_giftcard" />
                                    <button id="verifygiftcard" runat="server" class="btn btn-danger" onserverclick="verifygiftcard_Click" visible="false">
                                        <span class="glyphicon glyphicon-transfer" aria-hidden="true"></span>&nbsp;
				                        Verify
                                    </button>

                                </div>
                                <div class="col-xs-12">
                                    <div style="padding-top: 15px;float: right;">
                                        <button id="print" runat="server" class="btn btn-success" onserverclick="print_Click">
                                            Print
                                        </button>
										<button id="submit" runat="server" class="btn btn-success"
                                            onserverclick="save_Click" onclick="if (!onconfirmsubmit()) return false;">
                                            Submit
                                        </button>
                                        <script>
                                            function onconfirmsubmit() {
                                                $('#modal-submit').modal('show');
                                            }
                                        </script>
                                    </div>
                                </div>
                            </div>
						</div>
					</div>
				</div>

			</div>
		</div>
    </form>

    <script type="text/javascript">

        $(document).ready(function () {
            $("select").select2({ width: '100%' });
        });

        $(function () {
            $('[data-toggle="popover"]').popover(html = true);
        });

        $('#myTabs a').click(function (e) {
            e.preventDefault()
            $(this).tab('show')
        })

        $('#myTabs a[href="#profile"]').tab('show') // Select tab by name
        $('#myTabs a:first').tab('show') // Select first tab
        $('#myTabs a:last').tab('show') // Select last tab
        $('#myTabs li:eq(2) a').tab('show') // Select third tab (0-indexed)

        function preventMultipleSubmissions() {
            $('#<%=save_payment_creditcard.ClientID %>').prop('disabled', true);
            $('#<%=save_payment_debitcard.ClientID %>').prop('disabled', true);
            $('#<%=save_payment_cash.ClientID %>').prop('disabled', true);
            $('#<%=save_payment_transfer.ClientID %>').prop('disabled', true);
            $('#<%=save_payment_giftcard.ClientID %>').prop('disabled', true);
        }

        function showOverlay() {
            document.getElementById("overlay").style.display = "flex";
        }

        window.onbeforeunload = preventMultipleSubmissions;
    </script>
</asp:Content>

