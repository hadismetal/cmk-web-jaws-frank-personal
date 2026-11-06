<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="CreateSubmit.aspx.cs" inherits="Page_CreateSubmit" %>

<%@ register src="MenuCreate.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Create New User
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-lg" id="header_nama" runat="server" />
	</p>
	<hr />
    <div class="row">
        <div class="col-xs-12 col-sm-6">
            <div class="file-subheader hidden-sm hidden-xs">
		        account
	        </div>
	        <table class="table-file">
                <tr>
			        <td class="text-label">NIK</td>
			        <td>:</td>
			        <td id="nik" runat="server" />
		        </tr>
                <tr>
			        <td class="text-label">Nama</td>
			        <td>:</td>
			        <td id="nama" runat="server" />
		        </tr>
		        <tr>
			        <td class="text-label">Keterangan</td>
			        <td>:</td>
			        <td id="keterangan" runat="server" />
		        </tr>
		        <tr>
			        <td class="text-label">Alamat Email</td>
			        <td>:</td>
			        <td id="email" runat="server" />
		        </tr>
		        <tr>
			        <td class="text-label">SMS</td>
			        <td>:</td>
			        <td id="sms" runat="server" />
		        </tr>
	        </table>
	        <hr />
	        <div class="file-subheader hidden-sm hidden-xs">
		        password
	        </div>
	        <p>
		        <span class="text-muted">Generated Password :</span><br />
		        <span class="text-xl" id="pass" runat="server" />
	        </p>
            <hr />
	        <form id="Form1" runat="server">
		        <div class="button-left">
			        <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
				        <i class="icon-check"></i>
				        Finish
			        </button>
		        </div>
	        </form>
        </div>
        <div class="col-xs-12 col-sm-6">
	        <div class="file-subheader hidden-sm hidden-xs">
		        role settings
	        </div>
	        <div class="table-responsive">
		        <asp:table id="tb" runat="server" cssclass="table-list">
			        <asp:tableheaderrow>
				        <asp:tableheadercell width="230">Module</asp:tableheadercell>
				        <asp:tableheadercell width="90" cssclass="text-center">Manager</asp:tableheadercell>
				        <asp:tableheadercell width="90" cssclass="text-center">Operator</asp:tableheadercell>
				        <asp:tableheadercell width="90" cssclass="text-center">Download<br />File</asp:tableheadercell>
				        <asp:tableheadercell width="90" cssclass="text-center">Download<br />Excel</asp:tableheadercell>
			        </asp:tableheaderrow>
		        </asp:table>
	        </div>
        </div>
    </div>

	
	
	
</asp:content>

