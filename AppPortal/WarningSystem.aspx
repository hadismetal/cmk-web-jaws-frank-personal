<%@ Page Title="" Language="C#" MasterPageFile="~/MasterIT.master" AutoEventWireup="true" CodeFile="WarningSystem.aspx.cs" Inherits="WarningSystem" %>

<asp:content contentplaceholderid="h1" runat="server">
	JAWS Warning System
</asp:content>

<asp:content ID="Content1" contentplaceholderid="h1_st" runat="server">
	<asp:Literal runat="server" ID="lit_subtitle" />
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <form id="form1" runat="server">
        <div runat="server" id="div_1" visible="false">
            <div class="table-responsive">
		        <asp:table id="tb1" runat="server" cssclass="table-list">
                    <asp:tableheaderrow>
                        <asp:tableheadercell width="50">No</asp:tableheadercell>
				        <asp:tableheadercell width="220">SP1</asp:tableheadercell>
				        <asp:tableheadercell width="100">SP2</asp:tableheadercell>
				        <asp:tableheadercell width="220">SA</asp:tableheadercell>
                        <asp:tableheadercell width="220">SRC1</asp:tableheadercell>
                        <%--<asp:tableheadercell width="220">SRC2</asp:tableheadercell>--%>
                        <asp:tableheadercell width="220">SRT</asp:tableheadercell>

			        </asp:tableheaderrow>
		        </asp:table>
	        </div>
	    </div>
	    <div runat="server" id="div_2" visible="false">
            <div class="row">
                <div class="col-xs-12">
                    <asp:Button runat="server" ID="btn_2_deletecustomer_multiple_nontransaction" CssClass="btn btn-danger" OnClick="btn_2_deletecustomer_multiple_nontransaction_Click" Text="Delete Customer Multiple Nontransaction" />

                </div>
                <div class="col-xs-12">
                    <asp:Literal runat="server" ID="lit_2_message" />
                </div>
                <div class="col-xs-12">
                    <div class="table-responsive">
		                <asp:table id="tb2" runat="server" cssclass="table-list">
                            <asp:tableheaderrow>
                                <asp:tableheadercell width="50">No</asp:tableheadercell>
                                <asp:tableheadercell width="80">ID JAWS</asp:tableheadercell>
                                <asp:tableheadercell width="50">RPR</asp:tableheadercell>
                                <asp:tableheadercell width="50">RPRR</asp:tableheadercell>
                                <asp:tableheadercell width="50">TTP</asp:tableheadercell>
                                <asp:tableheadercell width="50">RS</asp:tableheadercell>
                                <asp:tableheadercell width="50">SO</asp:tableheadercell>
                                <asp:tableheadercell width="50">SORF</asp:tableheadercell>
                                <asp:tableheadercell width="50">SORP</asp:tableheadercell>
                                <asp:tableheadercell width="50">DPPO</asp:tableheadercell>
                                <asp:tableheadercell width="50">TI</asp:tableheadercell>
				                <asp:tableheadercell width="130">No Cust. JAWS</asp:tableheadercell>
                                <asp:tableheadercell width="180">Nama Cust. MyApps</asp:tableheadercell>
				                <asp:tableheadercell width="180">Nama Cust. JAWS</asp:tableheadercell>
                                <asp:tableheadercell width="150">Tanggal</asp:tableheadercell>
			                </asp:tableheaderrow>
		                </asp:table>
	                </div>
                </div>
            </div>
            
            
	    </div>
        <div runat="server" id="div_3" visible="false">
            <div class="table-responsive">
		        <asp:table id="tb3" runat="server" cssclass="table-list">
                    <asp:tableheaderrow>
                        <asp:tableheadercell width="50">No</asp:tableheadercell>
                        <asp:tableheadercell width="160">Stone Type</asp:tableheadercell>
				        <asp:tableheadercell width="140">Stone Name</asp:tableheadercell> 
                        <asp:tableheadercell width="70">ID</asp:tableheadercell>
				        <asp:tableheadercell width="100">PLU</asp:tableheadercell> 
				        <asp:tableheadercell width="170">Location</asp:tableheadercell> 
				        <asp:tableheadercell width="70">Carat Size</asp:tableheadercell>
                        <asp:tableheadercell width="50">+/-</asp:tableheadercell>
                        <asp:tableheadercell width="70">Carat Min</asp:tableheadercell>
                        <asp:tableheadercell width="70">Carat Max</asp:tableheadercell>
                        <asp:tableheadercell width="100">Operator</asp:tableheadercell>
                        <asp:tableheadercell width="150">Tanggal</asp:tableheadercell>
                        <asp:tableheadercell width="100">Keterangan</asp:tableheadercell>
			        </asp:tableheaderrow>
		        </asp:table>
	        </div>
	    </div>
	    <div id="footer">
		
	    </div>
    </form>
</asp:content>

