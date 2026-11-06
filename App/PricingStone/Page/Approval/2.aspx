<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="2.aspx.cs" Inherits="Page_Approval_2" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Colour Diamond
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
            <div>
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

                    <asp:TextBox CssClass="form-control text-center" Width="110" type="text" placeholder="Dari" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" />

                </div>
                <div class="form-group">
                    <div class="text-muted">
                        &nbsp;
                    </div>
                    <asp:TextBox CssClass="form-control text-center" Width="110" type="text" placeholder="Sampai" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" />
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
                        <asp:ListItem>Colour :</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:DropDownList ID="parcel03" runat="server" CssClass="form-control" Width="175">
                        <asp:ListItem>Size :</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:DropDownList ID="parcel04" runat="server" CssClass="form-control" Width="175">
                        <asp:ListItem>Colour Grading :</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="clearfix"></div>
                <div class="form-separator hidden-xs"></div>
                <div class="form-group">
                    <asp:DropDownList ID="parcel05" runat="server" CssClass="form-control" Width="175">
                        <asp:ListItem>Clarity :</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:DropDownList ID="parcel06" runat="server" CssClass="form-control" Width="175">
                        <asp:ListItem>Material :</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:DropDownList ID="parcel07" runat="server" CssClass="form-control" Width="175">
                        <asp:ListItem>Category :</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="clearfix">
                    <br class="hidden-xs" />
                </div>
                <div class="form-group">
                    <div class="text-muted">
                        Status :
                    </div>
                    <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                        <asp:ListItem>Semua</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <div class="text-muted">
                        Diurutkan :
                    </div>
                    <asp:DropDownList ID="order" runat="server" CssClass="form-control" Width="150">
                        <asp:ListItem Value="Nama">Nama</asp:ListItem>
                        <asp:ListItem Value="KodeParcel">Kode Parcel</asp:ListItem>
                        <asp:ListItem Value="ID">ID</asp:ListItem>
                    </asp:DropDownList>
                    <div class="text-00 visible-xs">&nbsp;</div>
                    <asp:DropDownList ID="orderby" runat="server" CssClass="form-control" Width="100">
                        <asp:ListItem>ASC</asp:ListItem>
                        <asp:ListItem>DESC</asp:ListItem>
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
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="100" />
                        <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="250">Nama</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="175">Operator</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130" CssClass="text-right"><small>USD</small> Cost</asp:TableHeaderCell>
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
                &nbsp;
                <button id="cancel" runat="server" class="btn btn-success" onserverclick="cancel_Click">
                    <i class="icon-fixed-width icon-check"></i>
                    Cancel All
                </button>
            </div>
        </form>
    </div>
</asp:Content>

