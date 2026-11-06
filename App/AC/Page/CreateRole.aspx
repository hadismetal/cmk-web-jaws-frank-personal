<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateRole.aspx.cs" Inherits="Page_CreateRole" %>

<%@ Register Src="MenuCreate.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Create New User
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <p>
        <span class="text-muted">Nama :</span><br />
        <span class="text-lg" id="header_nama" runat="server" />
    </p>
    <hr />
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
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="250">
                        <asp:DropDownList ID="select_all" runat="server"
                            CssClass="form-control" Width="230"
                            AutoPostBack="true" OnSelectedIndexChanged="select_All" />
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Module</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
        </div>
    </form>
</asp:Content>

