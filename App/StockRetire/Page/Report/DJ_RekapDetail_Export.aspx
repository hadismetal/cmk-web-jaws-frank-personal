<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="DJ_RekapDetail_Export.aspx.cs" Inherits="Page_Report_DJ_RekapDetail_Export" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" Runat="Server">
       <div id="form_area" runat="server">
        <!-- -->
        <div class="table-responsive">
            <asp:table id="tb" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
                        <asp:tableheadercell width="50">No</asp:tableheadercell>
						<asp:tableheadercell width="200">Parcel</asp:tableheadercell>
                        <asp:tableheadercell width="200">Total Butir</asp:tableheadercell>
						<asp:tableheadercell width="150">Carat</asp:tableheadercell>
						<asp:tableheadercell width="150">M</asp:tableheadercell>
                        <asp:tableheadercell width="150">$</asp:tableheadercell>
					</asp:tableheaderrow>
				</asp:table>
        </div>

    </div>
</asp:Content>

