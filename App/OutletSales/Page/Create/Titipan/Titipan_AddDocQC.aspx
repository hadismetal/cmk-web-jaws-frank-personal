<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Titipan_AddDocQC.aspx.cs" inherits="Page_Register_Titipan" %>

<%@ register src="MenuTitipan.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content ID="Content1" contentplaceholderid="h1" runat="server">
	Titipan
</asp:content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content ID="Content3" contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form id="Form1" runat="server" class="form-inline">
		<div class="table-responsive">
            <h3>Document QC</h3>
			<asp:table id="tb" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
                    <asp:tableheadercell width="150">No. Dokumen</asp:tableheadercell>
                    <asp:tableheadercell width="150">Tanggal</asp:tableheadercell>
                    <asp:tableheadercell width="150">Petugas QC</asp:tableheadercell>
                    <asp:tableheadercell width="250">Keterangan</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
            
		</div>
	</form>
</asp:content>

