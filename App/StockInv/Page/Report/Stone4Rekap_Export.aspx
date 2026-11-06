<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="Stone4Rekap_Export.aspx.cs" Inherits="Page_Account_Stone" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
       <form runat="server" class="form-inline">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:tableheaderrow>
                    <asp:tableheadercell width="40">No.</asp:tableheadercell>				
                    <asp:tableheadercell width="200">Nama Lokasi</asp:tableheadercell>
                    <asp:tableheadercell width="200">Nama</asp:tableheadercell>
                    <asp:tableheadercell width="120">Colour</asp:tableheadercell>
                    <asp:tableheadercell width="120">Shape</asp:tableheadercell>                
                    <asp:tableheadercell width="120">Size</asp:tableheadercell>
                    <asp:tableheadercell width="120">Total Butir</asp:tableheadercell>
                    <asp:tableheadercell width="120">Total Carat</asp:tableheadercell>
                    
                </asp:tableheaderrow>
            </asp:Table>
        </div>
    </form>
</asp:Content>

