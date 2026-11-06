<%@ Page Title="" Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="09Create.aspx.cs" Inherits="Page_Parcel3_09Create" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" Runat="Server">
    Precious Stone / Color Comment
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" Runat="Server">
    Create New
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="h1_id" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <form runat="server">
		<div class="form-group width-500">
			<div class="row">
				<div class="col-xs-7">
					<label>
						Nama :
					</label>
					<asp:requiredfieldvalidator id="nama_RF" runat="server" />
					<asp:textbox id="nama" runat="server" cssclass="form-control text-bold" maxlength="50" />
				</div>
				<div class="col-xs-3">
					<label>
						Kode :
					</label>
					<asp:requiredfieldvalidator id="namakode_RF" runat="server" />
					<asp:textbox id="namakode" runat="server" cssclass="form-control text-bold text-center" maxlength="20" />
				</div>
			</div>
		</div>
		<div class="form-group width-500">
			<label>
				Keterangan :
			</label>
			<asp:textbox id="keterangan" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
		</div>
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary"  onserverclick="save_Click">
				<i class="icon-ok"></i>
				Save
			</button>
			<a href="javascript:closeWindow();" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:Content>

