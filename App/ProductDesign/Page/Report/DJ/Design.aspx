<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Design.aspx.cs" Inherits="Page_Design" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Design
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server" class="form-inline">
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
            <div class="row">
                <div class="col-xs-12">
                    <div class="form-group">
                        <div class="text-muted">
                            Search :
                        </div>
                        <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            &nbsp;
                        </div>
                        <asp:TextBox ID="dari" runat="server" CssClass="form-control text-center" Width="110" placeholder="Dari" />
                        <script>
                            $(document).ready(function () { $('#<%=dari.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                        </script>

                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            &nbsp;
                        </div>
                        <asp:TextBox ID="sampai" runat="server" CssClass="form-control text-center" Width="110" placeholder="Sampai" />
                        <script>
                            $(document).ready(function () { $('#<%=sampai.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                        </script>

                    </div>
                    <div class="clearfix">
                        <br class="hidden-xs" />
                    </div>
                    <div class="text-muted">
                        Characteristic Design :
                    </div>
                    <div class="form-group">
                        <asp:DropDownList ID="designcategory" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">Design Category :</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:DropDownList ID="designconcept" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">Design Concept :</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:DropDownList ID="targetage" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">Target Age :</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:DropDownList ID="targetgender" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">Target Gender :</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="clearfix">
                        <br class="hidden-xs" />
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Characteristic Product :
                        </div>
                        <asp:DropDownList ID="productitem" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">Product Item</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            &nbsp;
                        </div>
                        <asp:DropDownList ID="productlevel" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">Product Level</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            &nbsp;
                        </div>
                        <asp:DropDownList ID="productcategory" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">Product Category</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            &nbsp;
                        </div>
                        <asp:DropDownList ID="stonedistribution" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">Stone Distribution</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            &nbsp;
                        </div>
                        <asp:DropDownList ID="segmen" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">Segmen</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="clearfix">
                        <br class="hidden-xs" />
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            &nbsp;
                        </div>
                        <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="-1">Status</asp:ListItem>
                            <asp:ListItem Value="0">All</asp:ListItem>
                            <asp:ListItem Value="1">Approved</asp:ListItem>
                            <asp:ListItem Value="2">Draft</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            &nbsp;
                        </div>
                        <asp:DropDownList ID="designer" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">Designer</asp:ListItem>
                        </asp:DropDownList>
                    </div>
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
                <div class="col-xs-12">
                    <asp:Literal runat="server" ID="lit_msg" />
                </div>
                <div class="col-xs-12">
                    <div class="table-responsive">
                        <asp:Table ID="tb" runat="server" CssClass="table-list">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell Width="100" />
                                <asp:TableHeaderCell Width="40">
                                    <span>No.</span>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="120">
                                    <span>Image</span>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="140">
                                    <span>Document No.</span>
                                    <div style="clear:both; border-bottom:solid 1px silver;"></div>
                                    <span>Tgl</span>
                                    <div style="clear:both; border-bottom:solid 1px silver;"></div>
                                    <span>Designer</span>
                                </asp:TableHeaderCell>
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
                                <asp:TableHeaderCell Width="150">
                                    <span>Design Category</span>
                                    <div style="clear:both; border-bottom:solid 1px silver;"></div>
                                    <span>Design Concept</span>
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="120">Segmen</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="120">Naming</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="200">SAP Material Code</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </div>
                </div>
            </div>





        </div>
    </form>
</asp:Content>

