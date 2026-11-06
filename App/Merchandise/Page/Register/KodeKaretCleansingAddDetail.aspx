<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="KodeKaretCleansingAddDetail.aspx.cs" Inherits="KodeKaretCleansingAddDetail" %>

<%@ Register Src="MenuFile.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Kode Karet Cleansing
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

        .form-group .select2-container {
            border: none;
            padding: 0;
        }

        
    </style>
    <form id="Form1" runat="server" class="form-inline">
        <div class="row">
            <div class="col-xs-12 col-sm-3">

                <div class="panel panel-info">
                    <div class="panel-body">
                        <div class="form-inline">
                            <div class="form-group">
                                <div class="text-muted">
                                    Basic / Non-Basic
                                </div>
                                <asp:DropDownList ID="ddl_basic" runat="server" CssClass="form-control" Width="175">
                                    <asp:ListItem Value="0">...</asp:ListItem>
                                    <asp:ListItem Value="1">Basic</asp:ListItem>
                                    <asp:ListItem Value="2">Non-Basic</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div>
                                <div class="text-muted">
                                    Search
                                </div>
                                <asp:TextBox runat="server" ID="txt_search" CssClass="form-control" />
                            </div>
                            <div>
                                <div class="text-muted">
                                    &nbsp;
                                </div>
                                <button id="btnDisplay" runat="server" class="btn btn-info pull-left" style="text-align: left;" onserverclick="btnDisplay_Click">
                                    <i class="icon-search"></i>
                                    Display
                                </button>
                                <button id="btnSubmit" runat="server" class="btn btn-warning pull-right" style="text-align: left;" onserverclick="btnSubmit_Click" visible="false">
                                    <i class="icon-arrow-right"></i>
                                    Submit
                                </button>
                                <button id="btnCancel" runat="server" class="btn btn-warning pull-right" style="text-align: left;" onserverclick="btnCancel_Click" visible="false">
                                    <i class="icon-exclamation"></i>
                                    Revise
                                </button>
                            </div>
                        </div>

                        <div runat="server" id="err" visible="false">
                            <div class="clearfix">
                                <br class="hidden-xs" />
                            </div>
                            <div class="alert alert-xs alert-warning text-center" role="alert">
                                <asp:Literal runat="server" ID="lit_err" /></div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-info">
                    <div class="panel-body">
                        <table class="table-file" style="width:100%;">
                            <tr>
                                <td>Status</td>
                                <td>&nbsp;:&nbsp;</td>
                                <td>
                                    <asp:Literal runat="server" ID="lit_status" /></td>
                            </tr>
                            <tr>
                                <td>Total Revision</td>
                                <td>&nbsp;:&nbsp;</td>
                                <td>
                                    <asp:Literal runat="server" ID="lit_totalRevision" /></td>
                            </tr>
                            <tr>
                                <td>Sumber</td>
                                <td>&nbsp;:&nbsp;</td>
                                <td>
                                    <asp:Literal runat="server" ID="lit_sumber" /></td>
                            </tr>
                            <tr>
                                <td>Product Item</td>
                                <td>&nbsp;:&nbsp;</td>
                                <td>
                                    <asp:Literal runat="server" ID="lit_productitem" /></td>
                            </tr>
                            <tr>
                                <td colspan="3"><hr /></td>
                            </tr>
                            <tr>
                                <td>Basic / Non-Basic</td>
                                <td>&nbsp;:&nbsp;</td>
                                <td>
                                    <asp:Literal runat="server" ID="lit_basic" /></td>
                            </tr>
                            <tr>
                                <td>Search Keyword</td>
                                <td>&nbsp;:&nbsp;</td>
                                <td>
                                    <asp:Literal runat="server" ID="lit_search" /></td>
                            </tr>
                            <tr>
                                <td>Total Search</td>
                                <td>&nbsp;:&nbsp;</td>
                                <td>
                                    <asp:Literal runat="server" ID="lit_total" /></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div>
                    <a href="KodeKaretCleansing.aspx" class="btn btn-default" style="text-align: left;">
                        <i class="icon-arrow-left"></i>
                        Back
                    </a>

                </div>
            </div>
            <div class="col-xs-12 col-sm-9">
                <uc1:Menu ID="menu" runat="server" />
                <div runat="server" id="div_table" visible="false">
                    <div class="row" runat="server" id="div_paging" visible="false">
                        <div class="col-xs-12 col-sm-3">
                            <asp:Panel runat="server" DefaultButton="go">

                            <div class="input-group input-group-xs" runat="server" id="div_paging_gotopage" visible="false">
					            <span class="input-group-addon"><span class="glyphicon glyphicon-share" aria-hidden="true"></span></span>
                                <asp:TextBox runat="server" ID="txt_gopage" CssClass="form-control text-center" />
                                <span class="input-group-btn">
                                    <%--<button runat="server" id="go" class="btn btn-primary" onserverclick="btnGo_Click">
				                        Go!
			                        </button>--%>
                                    <asp:Button runat="server" id="go" class="btn btn-primary" OnClick="btnGo_Click" Text="Go!" />
                                </span>
				            </div>

                            <asp:TextBox runat="server" ID="txt_hidden_max" style="width:1px; height:1px; border:none;" />

                            </asp:Panel>
                        </div>
                        <div class="col-xs-12 col-sm-9">
                            <div>
                                <asp:Literal runat="server" ID="lit_paging" />
                            </div>

                        </div>

                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12">
                            <br />
                            <asp:Table ID="tbl" runat="server" CssClass="table-list" Style="width: 100%;">
                                <asp:TableHeaderRow>
                                    <asp:TableHeaderCell Width="50" />
                                    <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="120">Kode Karet JAWS</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="120">Kode Karet</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="50">Total</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="120">Last BRJ Image</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="120">Last SPK Image</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="60">Basic</asp:TableHeaderCell>
                                    <asp:TableHeaderCell Width="120">Kode Karet Rev</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                            </asp:Table>
                        </div>
                    </div>
                
                </div>
                </div>
                
        </div>
    </form>
    <script>
        $(document).ready(function () {
            $("select").select2({ width: '100%' });
        });

    </script>
</asp:Content>

