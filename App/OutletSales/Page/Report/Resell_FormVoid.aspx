<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Resell_FormVoid.aspx.cs" Inherits="Page_Report_Resell_FormVoid" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Resell
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Void
</asp:Content>

<asp:Content contentplaceholderid="content" runat="server">
    <form runat="server">
        <div class="row">
            <div class="col-xs-12">
                <p>
		            <span class="text-muted">Resell :</span><br />
		            <span class="text-bold" id="rs" runat="server" />
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
			        <asp:Textbox id="keterangan" runat="server" cssclass="form-control" MaxLength="200" textmode="MultiLine" rows="4" />
		        </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="alert alert-danger" runat="server" id="div_alert" visible="false">
                    <asp:Literal runat="server" ID="lit_error" />
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
</asp:Content>
