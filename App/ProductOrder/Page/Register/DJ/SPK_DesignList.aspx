<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SPK_DesignList.aspx.cs" Inherits="Page_SPK_DesignList" %>

<%@ Register Src="MenuSPK.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <form runat="server">
		    
        <div>
            <button id="back" runat="server" class="btn btn-default" onserverclick="back_Click">
			    <i class="icon-arrow-left"></i>
			    Back
		    </button>
            <br /><br />Data ditampilkan 12 bulan terakhir.
        </div>
    </form>
    
    <div id="zero" runat="server" visible="false">
        <i class="icon-ok icon-5x pull-left text-success"></i>
        <h4>Proses Selesai</h4>
        <p class="text-muted">
            Semua data / transaksi dalam kondisi sudah menjadi SPK.
        </p>
        <div class="clearfix"></div>
        <hr />
    </div>
    <div id="form_area" runat="server">
        
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list">
                    <asp:TableHeaderRow>

                        <asp:TableHeaderCell Width="50" />
                        <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Image</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">
                            <span>No. Referensi</span>
                            <div style="clear:both; border-bottom:solid 1px silver;"></div>
                            <span>Designer</span>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">
                            <span>Operator</span>
                            <div style="clear:both; border-bottom:solid 1px silver;"></div>
                            <span>Tgl & Jam</span>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">
                            <span>Approval</span>
                            <div style="clear:both; border-bottom:solid 1px silver;"></div>
                            <span>Tgl & Jam</span>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">
                            <span>P. Item</span>
                            <div style="clear:both; border-bottom:solid 1px silver;"></div>
                            <span>P. Category</span>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">
                            <span>P. Level</span>
                            <div style="clear:both; border-bottom:solid 1px silver;"></div>
                            <span>P. Stone Distribution</span>
                        </asp:TableHeaderCell>

                    </asp:TableHeaderRow>
                </asp:Table>
            </div>

    </div>
</asp:Content>

