<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DataCustomerStamps.aspx.cs" Inherits="Sales_DataCustomerStamps" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Stamps Data Customer
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <style type="text/css">
        .form-group .select2-container {
            position: relative;
            z-index: 2;
            float: left;
            width: 100%;
            margin-bottom: 0;
            display: table;
            table-layout: fixed;
        }

        table.table-list td {
            vertical-align: middle;
        }
    </style>

    <form runat="server" class="form-inline">
        <div id="customerlist" runat="server">
            <asp:Panel runat="server" DefaultButton="go">
                <div class="row">
                    <div class="col-sm-5">
                        <div class="form-group">
                            <div class="text-muted">
                                Customer (Name / Email / Phone Number) :
                            </div>
                            <div class="input-group">
                                <div class="input-group-addon" style="width: 100px;">
                                    <img runat="server" id="img_1" src="/Images/web/logo-stamps.png" class="img-responsive" border="0" style="height: 20px; margin: 0 auto;" />
                                </div>
                                <asp:TextBox ID="keyword" runat="server" CssClass="form-control" />
                                <span class="input-group-btn">
                                    <asp:LinkButton Style="width: 100px;" ID="go" CssClass="btn btn-info" runat="server" OnClick="refreshPage">
                                        <span class="glyphicon glyphicon-search" aria-hidden="true"></span>
                                        &nbsp;Search
                                        
                                    </asp:LinkButton>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <div>
                            <hr />
                        </div>
                        <div class="table-responsive">
                            <asp:Table ID="tb" runat="server" CssClass="table-list" Style="width: 100%;">
                                <asp:TableHeaderRow>
                                    <asp:TableHeaderCell Width="50" />
                                    <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="100">SID</asp:TableHeaderCell>
                                    <asp:TableHeaderCell>Customer Name / Email</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="100">Birthday</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="150">Customer Code</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="150">Customer Phone</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="150">Membership Tiering</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="80">Stamps</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                            </asp:Table>
                        </div>
                        <div>
                            <asp:Literal runat="server" ID="lit_Stamps" />
                            <asp:Literal runat="server" ID="lit_errMsg" />
                        </div>
                    </div>
                    <div class="col-xs-12">

                        <hr />
                        <a id="cancel" runat="server" class="btn btn-default" onserverclick="backlinkURLwithBackButton">
                            <i class="icon-arrow-left"></i>
                            Back
                        </a>
                    </div>
                </div>
            </asp:Panel>


        </div>

    </form>


</asp:Content>

