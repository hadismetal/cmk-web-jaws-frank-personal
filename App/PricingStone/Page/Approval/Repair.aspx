<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Repair.aspx.cs" Inherits="Page_Approval_Repair" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Repair
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
            <div class="form-group">
                <div class="text-muted">
                    Search :
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Periode :
                </div>

                <asp:TextBox CssClass="form-control" type="text" placeholder="Dari" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" />

            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox CssClass="form-control" type="text" placeholder="Sampai" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" />
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="text-muted">
                Parcel :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel01" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Shape :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel02" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Size :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Status :
                </div>
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="150">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Approved</asp:ListItem>
                    <asp:ListItem Value="2">Pending Approval</asp:ListItem>
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
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="100" />
                        <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="250">Nama</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="175">Operator</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130" CssClass="text-right"><small>USD</small> Harga</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130" CssClass="text-right"><small>USD</small> Pending</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150" CssClass="text-right">Mutasi</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
            <div class="button-left">
                <asp:DropDownList ID="process_id" runat="server" Visible="false" />
                <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                    <i class="icon-fixed-width icon-check"></i>
                    Approved All
                </button>
                &nbsp;<button id="cancel" runat="server" class="btn btn-success" onserverclick="cancel_Click">
                    <i class="icon-fixed-width icon-check"></i>
                    Cancel All
                </button>
            </div>
        </form>
    </div>
</asp:Content>

