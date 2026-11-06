<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Sales_ItemAdd.aspx.cs" inherits="Page_Register_Sales_ItemAdd" %>

<%@ register src="MenuTradeIn.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	TradeIn
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form id="Form1" runat="server" class="form-inline">
        <div class="row">
            <div class="col-xs-12" style="padding-bottom:15px;">
		        <div class="form-group">
			        <div class="text-muted">
				        Search :
			        </div>
			        <asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
		        </div>
		        <div class="form-group">
			        <div class="hidden-xs">&nbsp;</div>
			        <div>
				        <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					        <i class="icon-search"></i>
					        Display
				        </button>
				        <a id="cancel" runat="server" class="btn btn-default" href="TradeIn_Data.aspx">
					        <i class="icon-remove"></i>
					        Cancel
				        </a>
			        </div>
		        </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div>
                    <h3>Diamond Jewellery</h3>
                </div>
                <asp:table id="tbdj" runat="server" cssclass="table-list" style="table-layout:auto; width:100%;">
                    <asp:tableheaderrow>
					    <asp:tableheadercell width="50" />
					    <asp:tableheadercell width="50">No.</asp:tableheadercell>
					    <asp:tableheadercell>PLU & Item</asp:tableheadercell>
                        <asp:tableheadercell width="120">Price</asp:tableheadercell>
				    </asp:tableheaderrow>
			    </asp:table>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div>
                    <h3>Plain Gold</h3>
                </div>
                <asp:table id="tbpg" runat="server" cssclass="table-list" style="table-layout:auto; width:100%;">
                    
				    <asp:tableheaderrow>
					    <asp:tableheadercell width="50" />
					    <asp:tableheadercell width="50">No.</asp:tableheadercell>
					    <asp:tableheadercell>PLU & Item</asp:tableheadercell>
                        <asp:tableheadercell width="120">Price</asp:tableheadercell>
				    </asp:tableheaderrow>
			    </asp:table>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div>
                    <h3>Gold Jewellery</h3>
                </div>
                <asp:table id="tbgj" runat="server" cssclass="table-list" style="table-layout:auto; width:100%;">
                    
				    <asp:tableheaderrow>
					    <asp:tableheadercell width="50" />
					    <asp:tableheadercell width="50">No.</asp:tableheadercell>
					    <asp:tableheadercell>PLU & Item</asp:tableheadercell>
                        <asp:tableheadercell width="120">Price</asp:tableheadercell>
				    </asp:tableheaderrow>
			    </asp:table>

                <br />
                <div>
                    <h3>Loose Diamond</h3>
                </div>
                <asp:table id="tbld" runat="server" cssclass="table-list" style="table-layout:auto; width:100%;">
                    <asp:tableheaderrow>
					    <asp:tableheadercell width="50" />
					    <asp:tableheadercell width="50">No.</asp:tableheadercell>
					    <asp:tableheadercell>PLU & Item</asp:tableheadercell>
                        <asp:tableheadercell width="120">Price</asp:tableheadercell>
				    </asp:tableheaderrow>
			    </asp:table>
            </div>
            
        </div>

	</form>
</asp:content>

