<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PaymentDP_ItemList.aspx.cs" Inherits="Page_Register_PaymentDP_ItemList" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Down Payment
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
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
    </style>
    <form id="Form1" runat="server" class="form-inline">

    <div runat="server" id="div_finished" visible="false">
            <div class="row">
                <div class="col-xs-12 col-sm-3 col-md-3">
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="panel panel-default">

                        <div class="panel-body">
                            <asp:Literal runat="server" ID="lit_info_invalid" />
                            <button id="redirect_summary" runat="server" style="width: 100%;" class="btn btn-default" onserverclick="redirect_report_Click" visible="false">
                                <span class="glyphicon glyphicon-arrow-left" aria-hidden="true"></span>
                                &nbsp;&nbsp;Back to Report DP
                            </button>
                        
                        </div>

                    </div>
                </div>
                <div class="col-xs-12 col-sm-3 col-md-3">
                </div>
            </div>

        </div>


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
                                    <td class="text-label">Tgl Transaksi</td>
                                    <td>:</td>
                                    <td id="tgl" runat="server" />
                                </tr>
                                <tr>
                                    <td class="text-label">Type</td>
                                    <td>:</td>
                                    <td id="tipe_dp" runat="server" />
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
                                    <td class="text-label">Keterangan</td>
                                    <td>:</td>
                                    <td id="keterangan" runat="server" />
                                </tr>
								<tr id="rowgwp" runat="server">
                                    <td class="text-label text-danger">Free GWP</td>
                                    <td>:</td>
                                    <td id="freegwp" runat="server" />
                                </tr>
                                   <tr>
                                    <td class="text-label">Estimasi Harga</td>
                                    <td>:</td>
                                    <td id="estimate" runat="server" />
                                </tr>
                                 
                            </table>
                        </div>
                    </div>

                </div>
            </div>
        </div>
   <div>
            <asp:Literal ID="lit_error" runat="server"></asp:Literal>
        </div>
        <hr />


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
                                                    <strong>Perhatian !</strong>&nbsp;Segala kesalahan input pembayaran (baik secara nominal ataupun voucher) <strong>TIDAK</strong> dapat dikembalikan. Silahkan gunakan fitur <strong>Void Down Payment</strong> untuk melakukan penginputan kembali dari awal.
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
                                                    }

                                                    h3 span {
                                                        font-size: 20px;
                                                    }
                                                </style>
                                                <ul class="nav nav-pills" role="tablist">
                                                    <li role="presentation" class="<%= Active_CreditCard %>"><a class="tabcustom" <%= Href_CreditCard %> aria-controls="creditcard" role="tab" data-toggle="<%= Tab_CreditCard %>"><span class="glyphicon glyphicon-credit-card aria-hidden="true"></span>
                                                        <br />
                                                        Credit Card</a></li>
                                                    <li role="presentation" class="<%= Active_DebitCard %>"><a class="tabcustom" <%= Href_DebitCard %> aria-controls="debitcard" role="tab" data-toggle="<%= Tab_DebitCard %>"><span class="glyphicon glyphicon-credit-card aria-hidden="true"></span>
                                                        <br />
                                                        Debit Card</a></li>
                                                    <li role="presentation" class="<%= Active_Cash %>"><a class="tabcustom padtop13" <%= Href_Cash %> aria-controls="cash" role="tab" data-toggle="<%= Tab_Cash %>"><span class="icon-money" style="font-size: 14px; padding-left: 6px; aria-hidden="true"></span>
                                                        <br />
                                                        Cash</a></li>
                                                    <li role="presentation" class="<%= Active_DownPayment %>"><a class="tabcustom" <%= Href_DownPayment %> aria-controls="creditcard" role="tab" data-toggle="<%= Tab_DownPayment %>"><span class="glyphicon glyphicon-download-alt aria-hidden="true"></span>
                                                        <br />
                                                        Down Payment</a></li>
                                                    <li role="presentation" class="<%= Active_Transfer %>"><a class="tabcustom" <%= Href_Transfer %> aria-controls="transfer" role="tab" data-toggle="<%= Tab_Transfer %>"><span class="glyphicon glyphicon-transfer aria-hidden="true"></span>
                                                        <br />
                                                        Transfer</a></li>
                                                    <li role="presentation" class="<%= Active_Voucher %>"><a class="tabcustom padtop13" <%= Href_Voucher %> aria-controls="voucher" role="tab" data-toggle="<%= Tab_Voucher %>"><span class="icon-ticket" style="font-size: 14px; padding-left: 6px; aria-hidden="true"></span>
                                                        <br />
                                                        Voucher</a></li>
                                                    <li role="presentation" class="<%= Active_GiftCard %>"><a class="tabcustom" <%= Href_GiftCard %> aria-controls="giftcard" role="tab" data-toggle="<%= Tab_GiftCard %>"><span class="glyphicon glyphicon-credit-card aria-hidden="true"></span>
                                                        <br />
                                                        Gift Card</a></li>
                                                    <li role="presentation" class="<%= Active_BrideStoryPay %>"><a class="tabcustom" <%= Href_BrideStoryPay %> aria-controls="bridestorypay" role="tab" data-toggle="<%= Tab_BrideStoryPay %>"><span class="glyphicon glyphicon-phone aria-hidden="true"></span>
                                                        <br />
                                                        Bridestory Pay</a></li>
													<li role="presentation" class="<%= Active_QRIS %>"><a class="tabcustom" <%= Href_QRIS %> aria-controls="qris" role="tab" data-toggle="<%= Tab_QRIS %>"><span class="glyphicon glyphicon-qrcode" aria-hidden="true"></span>
														<br />
														QRIS</a></li>
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
                                                                        <label style="color:blue;">Amount</label>
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
                                                                <asp:Button runat="server" ID="save_payment_creditcard" OnClick="save_payment_creditcard_Click" Text="Add Credit Card" CssClass="btn btn-success pull-right" />
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
																
																if(appcode != "" && ddledcs != "0" && ddlbanks != "0"){
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
																else{
																	if(appcode == ""){
																		document.getElementById("lbl_validator_appcode_cc").style.display = "";
																	}
																	else{
																		document.getElementById("lbl_validator_appcode_cc").style.display = "none";
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
                                                                        <label style="color:blue;">Amount</label>
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
                                                                <asp:Button runat="server" ID="save_payment_debitcard" OnClick="save_payment_debitcard_Click" Text="Add Debit Card" CssClass="btn btn-success pull-right" />
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
																
																if (appcode != "" && ddledcs != "0"  && ddlbanks != "0") {
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
																else{
																	if(appcode == ""){
																		document.getElementById("lbl_validator_appcode_debit").style.display = "";
																	}
																	else{
																		document.getElementById("lbl_validator_appcode_debit").style.display = "none";
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
                                                            <div class="col-xs-12 col-sm-6">
                                                                <div style="text-align: center; padding-top: 30px;">
                                                                    
                                                                    <label class="radio-inline">
                                                                        <asp:RadioButton runat="server" GroupName="rdo_cash" ID="rdo_cash_in" />
                                                                        Cash In
                                                                    </label>
                                                                </div>
                                                            </div>
                                                            <div class="col-xs-12 col-sm-6">
                                                                <div class="row">
                                                                    <div class="form-group col-xs-12" style="padding-bottom: 10px;">
                                                                        <label style="color:blue;">Amount</label>
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
                                                                <asp:Button runat="server" ID="save_payment_cash" OnClick="save_payment_cash_Click" Text="Add Cash" CssClass="btn btn-success pull-right" />
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
                                                                <asp:Button runat="server" ID="save_payment_downpayment" OnClick="save_payment_downpayment_Click" Text="Add Down Payment" CssClass="btn btn-success pull-right" />
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
                                                                        <label style="color:blue;">Amount</label>
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
                                                                <asp:Button runat="server" ID="save_payment_transfer" OnClick="save_payment_transfer_Click" Text="Add Transfer" CssClass="btn btn-success pull-right" />
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
                                                                <asp:Button runat="server" ID="save_payment_voucher" OnClick="save_payment_voucher_Click" Text="Add Voucher" CssClass="btn btn-success pull-right" />
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
                                                                        <label style="color:blue;">Amount</label>
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
                                                                                            <span class="">Please tap the card now !!</span><br>
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
                                                                <asp:Button runat="server" ID="save_payment_giftcard" OnClick="save_payment_giftcard_Click" Text="Submit" CssClass="btn btn-success pull-right" />
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
                                                                        <label style="color:blue;">Amount</label>
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
                                                                <asp:Button runat="server" ID="save_payment_bridestorypay" OnClick="save_payment_bridestorypay_Click" Text="Add Bridestory Pay" CssClass="btn btn-success pull-right" />
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
																<asp:Button runat="server" ID="save_payment_qris" OnClick="save_payment_qris_Click" Text="Add QRIS" CssClass="btn btn-success pull-right" />
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
                                                            <span class="text-muted">Total Amount DP</span><br />
                                                            <span class="text-md" style="color: green; font-weight: bold;" id="spn_payment_amountpaid" runat="server" />
                                                        </div>
                                                     
                                                    </div>
                                                </div>

                                                <div class="table-responsive">
                                                    <asp:Table ID="tb3" runat="server" CssClass="table-list table-striped" Width="100%">
                                                        <asp:TableHeaderRow>
                                                            <asp:TableHeaderCell Width="30"></asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="300">Detail</asp:TableHeaderCell>
                                                            <asp:TableHeaderCell Width="150">Amount</asp:TableHeaderCell>
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
            </div>

        <div class="col-xs-12">
                <div class="button-inline" runat="server" id="div_review" visible="true">
                <button id="review" runat="server" class="btn btn-warning" onserverclick="review_Click">
                    <span class="glyphicon glyphicon-saved" aria-hidden="true"></span>
                    Review
                </button>

                <span id="next_err_1" runat="server" class="text-validator" visible="false">Terjadi Kesalahan Teknis #001</span>
                <span id="next_err_2" runat="server" class="text-validator" visible="false">Terjadi Kesalahan Teknis #002</span>
                <span id="next_err_3" runat="server" class="text-validator" visible="false">Terjadi Kesalahan Teknis #003</span>
                <asp:Literal runat="server" ID="lit_test_text" />
            </div>
             </div>

        <div class="col-xs-12">
                <div runat="server" id="div_confirm" visible="false">
                <div class="alert alert-danger" role="alert">
                    <div class="row">
                        <div class="col-xs-12 col-sm-6" style="padding-bottom: 5px;">
                            This Payment transaction is <b>permanent</b>. Please check again to make sure there is no mistake input.<br />
                            <b>Are you sure you want to Save?</b>

                        </div>
                        <div class="col-xs-12 col-sm-4">
                            <div class="col-xs-12" style="padding: 0 0 5px 0;">
                                <button id="cancel" runat="server" style="width: 100%;" class="btn btn-danger" onserverclick="cancel_Click">
                                    <span class="glyphicon glyphicon-remove" aria-hidden="true"></span>
                                    &nbsp;&nbsp;No, I'd like to revise my action.
                                </button>
                            </div>
                            <div class="col-xs-12" style="padding: 0 0 5px 0;">

                                <button id="Button1" runat="server" style="width: 100%;" class="btn btn-success" onserverclick="save_Click">
                                    <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                                    &nbsp;&nbsp;Yes, I'm sure.
                                </button>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
             </div>

       <div class="col-xs-12">
           <div runat="server" id="div_print" visible="false">
            <div style="padding-top: 15px;">
            <button id="save" runat="server" class="btn btn-success" onserverclick="print_Click" disabled="disabled">
                <span class="glyphicon glyphicon-print" aria-hidden="true"></span>
                &nbsp;<span runat="server" id="txt_btn_print">Print Invoice</span>
            </button>
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

        function UserConfirmation() {
            txtConfirm =
				"Lanjutkan Proses PEMBAYARAN ?\n\n" +
				"transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";
            if (confirm(txtConfirm))
                return true;
            else
                return false;
        }
    </script>
</asp:Content>

