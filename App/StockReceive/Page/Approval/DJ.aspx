<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ.aspx.cs" Inherits="Page_Approval_DJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewelry
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Approval
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <script>
        $(document).ready(function () {
            $('#<%=datepicker.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
            $('#<%=datepicker2.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
        });
    </script>
    <div id="zero" runat="server" visible="false">
        <i class="icon-ok icon-5x pull-left text-success"></i>
        <h4>Proses Selesai</h4>
        <p class="text-muted">
            Semua data / transaksi dalam kondisi sudah approval.
        </p>
        <div class="clearfix"></div>
        <hr />
    </div>
    <div id="form_area" runat="server">
        <form runat="server" class="form-inline">
            <div class="row">
                <div class="col-xs-12">

                    <div class="form-group">
                        <div class="text-muted">
                            Search (Kode Karet) :
                        </div>
                        <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="..." />
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Dari :
                        </div>

                        <asp:TextBox CssClass="form-control text-center" Width="110" type="text" placeholder="Dari" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" />

                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Sampai :
                        </div>
                        <asp:TextBox CssClass="form-control text-center" Width="110" type="text" placeholder="Dari" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" />
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Supplier :
                        </div>
                        <asp:DropDownList ID="supplier" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">Supplier</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <%--<div class="form-group">
                        <div class="text-muted">
                            Status :
                        </div>
                        <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="1">Approved</asp:ListItem>
                            <asp:ListItem Value="2">Pending Approval</asp:ListItem>
                        </asp:DropDownList>
                    </div>--%>
                    <div class="form-group">
                        <div class="hidden-xs">&nbsp;</div>
                        <div>
                            <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                                <i class="icon-search"></i>
                                Display
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <asp:Literal runat="server" ID="lit_msg" />
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">

                    <div class="table-responsive">
                        <asp:Table ID="tb" runat="server" CssClass="table-list">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell Width="80" />
                                <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                                <%--<asp:TableHeaderCell Width="120">Image</asp:TableHeaderCell>--%>
                                <asp:TableHeaderCell Width="150">No. Referensi</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="175">
                                    <span>Operator</span>
                                    <div style="clear:both; border-bottom:solid 1px silver;"></div>
                                    <span>Approval</span>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="100">Form</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="120">PLU</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="150">Supplier</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="150">
                                    <span>Item</span>
                                    <div style="clear:both; border-bottom:solid 1px silver;"></div>
                                    <span>Category</span>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="150">
                                    <span>Level</span>
                                    <div style="clear:both; border-bottom:solid 1px silver;"></div>
                                    <span>Stone Distribution</span>
                                </asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </div>
                </div>
            </div>

        </form>
    </div>
</asp:Content>

