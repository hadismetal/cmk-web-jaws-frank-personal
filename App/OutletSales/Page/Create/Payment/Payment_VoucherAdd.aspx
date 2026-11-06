<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Payment_VoucherAdd.aspx.cs" inherits="Page_Register_Sales_ItemAdd" %>

<asp:content contentplaceholderid="h1" runat="server">
	Sales Order
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Voucher
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p>
		<span class="text-muted">Nomor :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<hr />
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Search Voucher (Payment) :
			</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="250" placeholder="Input Keyword..." />
		</div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
				<a id="cancel" runat="server" class="btn btn-default" href="Payment_ItemList.aspx">
					<i class="icon-remove"></i>
					Cancel
				</a>
			</div>
		</div>
        <div>
            <br />
            <asp:Literal runat="server" ID="lit_msg" />
            <br />
        </div>
        <div>
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
	</form>
</asp:content>

