<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DPPO_Report_FormVoid.aspx.cs" inherits="Page_ReportSales_DPPO_Report_FormVoid" %>

<asp:content contentplaceholderid="h1" runat="server">
	Down Payment
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Void
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	
	<form runat="server">
        <div class="row">
            <div class="col-xs-12">
                <p>
		            <span class="text-muted">Down Payment :</span><br />
		            <span class="text-bold" id="so" runat="server" />
	            </p>
	            <hr />
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6" runat="server" id="div_keterangan_void">
                <div class="form-group width-500">
			        <label>
				        Keterangan Void :
			        </label>
			        <asp:textbox id="keterangan" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
			        <asp:Literal id="lit_keteranganvoid" runat="server" />
		        </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="alert alert-danger" runat="server" id="div_alert" visible="false">
                    
                    <asp:Literal runat="server" ID="lit_warning_payment_gc_exist" />
                    <asp:Literal runat="server" ID="lit_warning_payment_vc_exist" />


                    <asp:Literal runat="server" ID="lit_warning" />
                  
                    <asp:Literal runat="server" ID="lit_warning_payment_gc_response" />
                </div>
            </div>

        </div>
        <div class="row">
            <div class="col-xs-12">
                <div class="button-inline">
                    <button id="back" runat="server" class="btn btn-info" onserverclick="back_Click">
				        <i class="icon-arrow-left"></i>
				        Back
			        </button>
			        <button id="save" runat="server" class="btn btn-danger" onserverclick="save_Click">
				        <i class="icon-arrow-right"></i>
				        Void
			        </button>
		        </div>
            </div>
        </div>
		
	</form>
</asp:content>
