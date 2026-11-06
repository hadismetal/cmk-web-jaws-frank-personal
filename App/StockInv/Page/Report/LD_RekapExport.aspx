<%@ Page Language="C#" masterpagefile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="LD_RekapExport.aspx.cs" Inherits="Page_Report_LD_RekapExport" %>

<asp:content ID="Content1" contentplaceholderid="content" runat="server">
    <div id="form_area" runat="server">
        
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:tableheadercell width="40">No.</asp:tableheadercell>
				    <asp:tableheadercell width="120">Nama</asp:tableheadercell>
                    <asp:tableheadercell width="120">Shape</asp:tableheadercell>
                    <asp:tableheadercell width="120">Colour</asp:tableheadercell>
                    <asp:tableheadercell width="120">Clarity</asp:tableheadercell>
                    <asp:tableheadercell width="120">Cutting</asp:tableheadercell>
                    <asp:tableheadercell width="120">Brand</asp:tableheadercell>
				    <asp:tableheadercell width="120">Total Butir</asp:tableheadercell>
				    <asp:tableheadercell width="120">Total Carat</asp:tableheadercell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>
