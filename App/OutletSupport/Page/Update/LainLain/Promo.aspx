<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Promo.aspx.cs" Inherits="Page_Update_LainLain_Promo" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Promo Lain Lain (Promo Bank, Event)
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Update
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div class="container-fluid">
        <form runat="server">
            <div class="col-md-12">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        Search Form
                    </div>
                    <div class="panel-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-xs-4">
									<label>Search</label>
                                    <input runat="server" id="search" placeholder="Search Promo" class="form-control" />
                                </div>
								<div class="col-xs-2">
									<label>Tipe</label>
                                    <asp:DropDownList runat="server" id="tipe" placeholder="Search Promo" class="form-control">
										<asp:ListItem Value="0">All</asp:ListItem>
										<asp:ListItem Value="1">Promo Bank</asp:ListItem>
										<asp:ListItem Value="2">Event</asp:ListItem>
									</asp:DropDownList>
                                </div>
								<div class="col-xs-2">
									<label>Start Date</label>
                                    <input runat="server" id="datepicker" placeholder="Start Date" class="form-control" />
                                </div>
								<div class="col-xs-2">
									<label>End Date</label>
                                    <input runat="server" id="datepicker2" placeholder="End Date" class="form-control" />
                                </div>
								<div class="col-xs-2">
									<label>&nbsp;</label>
									<div>
										<asp:Button runat="server" ID="searchbutton" OnClick="searchPromo_Click" CssClass="btn btn-primary" Text="Search"></asp:Button>
									</div>
								</div>
                            </div>
                        </div>
                        <asp:Table class="table table-hover table-bordered table-responsive" runat="server" ID="tb">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell CssClass="text-center">No.</asp:TableHeaderCell>
                                <asp:TableHeaderCell CssClass="text-center">Nama</asp:TableHeaderCell>
								<asp:TableHeaderCell CssClass="text-center">Tipe Promo</asp:TableHeaderCell>
                                <asp:TableHeaderCell CssClass="text-center">Tanggal Mulai</asp:TableHeaderCell>
                                <asp:TableHeaderCell CssClass="text-center">Tanggal Expire</asp:TableHeaderCell>
								<asp:TableHeaderCell CssClass="text-center">Status</asp:TableHeaderCell>
                                <asp:TableHeaderCell CssClass="text-center">Keterangan</asp:TableHeaderCell>
                                <asp:TableHeaderCell CssClass="text-center">Created By</asp:TableHeaderCell>
                                <asp:TableHeaderCell CssClass="text-center"></asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </div>
                </div>
            </div>
        </form>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#content_datepicker').datepicker({
                format: 'dd-mm-yyyy'
            });
            $('#content_datepicker2').datepicker({
                format: 'dd-mm-yyyy'
            });
        });

        function onclickdetail(e) {
            var nama = $(e).closest('tr').find('td').eq(0).text(); // nama
            var tglmulai = $(e).closest('tr').find('td').eq(1).text(); // tglmulai
            var tglend = $(e).closest('tr').find('td').eq(2).text(); // tglend
            var ket = $(e).closest('tr').find('td').eq(3).text(); // keterangan
            var creat = $(e).closest('tr').find('td').eq(4).text(); // createby
            var id = $(e).attr('data-id'); //id

            window.location.href = "PromoDetail.aspx?id=" + id;
            return false;
        }
    </script>
</asp:Content>
