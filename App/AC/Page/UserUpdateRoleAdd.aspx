<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="UserUpdateRoleAdd.aspx.cs" inherits="Page_UserUpdateRoleAdd" %>

<%@ register src="MenuFile.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	User Account
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	<asp:literal id="h1_st" runat="server" />
</asp:content>
<asp:content contentplaceholderid="h1_id" runat="server">
	ID:<asp:literal id="h1_id" runat="server" />
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form runat="server">
        <div class="panel panel-default">
            <div class="panel-body">
                <div class="row">
                    <div class="col-xs-6 col-sm-4 col-md-3 text-center" style="padding-bottom:5px;">
                        <button id="btn_role_superadmin" runat="server" class="btn btn-danger btn-xs" onserverclick="select_Superadmin">
                            <i class="icon-ok"></i>
                            Superadmin
                        </button>
                    </div>
                    <div class="col-xs-6 col-sm-4 col-md-3 text-center" style="padding-bottom:5px;">
                        <button id="btn_role_store_sales" runat="server" class="btn btn-info btn-xs" onserverclick="select_JR">
                            <i class="icon-ok"></i>
                            Jewellery Representative
                        </button>
                    </div>
                    <div class="col-xs-6 col-sm-4 col-md-3 text-center" style="padding-bottom:5px;">
                        <button id="btn_role_store_admin" runat="server" class="btn btn-info btn-xs" onserverclick="select_AE_SM">
                            <i class="icon-ok"></i>
                            Admin Executive / Store Manager
                        </button>
                    </div>
                    <div class="col-xs-6 col-sm-4 col-md-3 text-center" style="padding-bottom:5px;">
                        <button id="btn_role_store_manager" runat="server" class="btn btn-info btn-xs" onserverclick="select_AM">
                            <i class="icon-ok"></i>
                            Area Manager
                        </button>
                    </div>
                    <div class="col-xs-6 col-sm-4 col-md-3 text-center" style="padding-bottom:5px;">
                        <button id="btn_role_corporate_sales" runat="server" class="btn btn-info btn-xs" onserverclick="select_CorpSales">
                            <i class="icon-ok"></i>
                            Corporate Sales
                        </button>
                    </div>
                    <div class="col-xs-6 col-sm-4 col-md-3 text-center" style="padding-bottom:5px;">
                        <button id="btn_role_accounting" runat="server" class="btn btn-success btn-xs" onserverclick="select_ACC">
                            <i class="icon-ok"></i>
                            Accounting
                        </button>
                    </div>
                    <div class="col-xs-6 col-sm-4 col-md-3 text-center" style="padding-bottom:5px;">
                        <%--<button id="btn_role_stock_keeper_pricing" runat="server" class="btn btn-info btn-xs" onserverclick="save_Click">
				            <i class="icon-ok"></i>
				            Stock Keeper (Pricing)
			            </button>--%>
                    </div>
                    <div class="col-xs-6 col-sm-4 col-md-3 text-center" style="padding-bottom:5px;">

                    </div>
                    <div class="col-xs-6 col-sm-4 col-md-3 text-center" style="padding-bottom:5px;">

                    </div>
                    <div class="col-xs-6 col-sm-4 col-md-3 text-center" style="padding-bottom:5px;">

                    </div>
                    <div class="col-xs-6 col-sm-4 col-md-3 text-center" style="padding-bottom:5px;">

                    </div>
                    <div class="col-xs-6 col-sm-4 col-md-3 text-center" style="padding-bottom:5px;">

                    </div>
                    <div class="col-xs-6 col-sm-4 col-md-3 text-center" style="padding-bottom:5px;">

                    </div>
                </div>





                
            </div>
        </div>
		<div class="table-responsive">
			<asp:table id="tb" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="250">
						<asp:dropdownlist id="select_all" runat="server"
							cssclass="form-control" width="230"
							autopostback="true" onselectedindexchanged="select_All" />
					</asp:tableheadercell>
					<asp:tableheadercell width="250">Module</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
		</div>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-ok"></i>
				Save
			</button>
			<a id="back" runat="server" href="UserUpdateRole.aspx" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:content>

