<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Promo.aspx.cs" Inherits="Page_Register_LainLain_Promo" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Promo Lain Lain (Promo Bank, Event)
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
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
	<!-- Modal -->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Message</h4>
                </div>
                <div class="modal-body">
                    <h2 class="text-center">
                        <asp:Label runat="server" ID="lblMessage">Proses Berhasil</asp:Label>
                    </h2>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <form runat="server">
            <div class="panel panel-default width-800">
                <div class="panel-heading">
                    New Promo
                </div>
                <div class="panel-body">
					<div class="col-md-6">
						<div class="form-group">
							<label>Nama</label>
							<asp:RequiredFieldValidator ID="nama_RF" runat="server" />
							<asp:TextBox ID="nama" runat="server" CssClass="form-control" MaxLength="100" placeholder="Input Nama Promo" />								
						</div>
						<div class="form-group">
							<label>Tanggal Mulai</label>
							<asp:RequiredFieldValidator ID="datepicker_RF" runat="server" />
							<asp:TextBox CssClass="form-control" type="text" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" placeholder="Input Tanggal Mulai" />
						</div>
						<div class="form-group">
							<label>Tanggal Expire</label>
							<asp:RequiredFieldValidator ID="datepicker2_RF" runat="server" />
							<asp:TextBox CssClass="form-control" type="text" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" placeholder="Input Tanggal Expire" />
						</div>
					</div>
					<div class="col-md-6">
						<div class="form-group">
							<label>Keterangan</label>
							<asp:TextBox ID="keterangan" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" placeholder="Input Keterangan" />
						</div>
						<div class="form-group">
							<label>Tipe Promo</label>
							<asp:DropDownList ID="tipe" runat="server" CssClass="form-control" ClientIDMode="Static" onchange="onchangepromo(this);">
								<asp:ListItem Value="2">Event</asp:ListItem>
							</asp:DropDownList>
						</div>
					</div>
                </div>
                <div class="panel-footer">
                    <div class="button">
                        <asp:Button id="save" runat="server" CssClass="btn btn-primary" OnClientClick="return onsave();" OnClick="save_Click" Text="Save">
                        </asp:Button>
                        <script type="text/javascript">
                            function onsave() {
                                var nama = document.getElementById('content_nama').value;
                                var keterangan = document.getElementById('content_keterangan').value;
                                var startdate = document.getElementById('content_datepicker').value;
                                var expiredate = document.getElementById('content_datepicker2').value;
								var tipe = document.getElementById('tipe').value;
                                if (nama == "" || startdate == "" || expiredate == "") {
                                    if (nama == "") {
                                        alert("Nama Tidak Boleh Kosong.");
                                        $('#content_nama').focus();
                                    }
                                    else if (startdate == "") {
                                        alert("Tanggal Mulai Tidak Boleh Kosong.");
                                        $('#content_datepicker').focus();
                                    }
                                    else if (expiredate == "") {
                                        alert("Tanggal Expire Tidak Boleh Kosong.");
                                        $('#content_datepicker2').focus();
                                    }
                                    return false;
                                }
                            }
                        </script>
                    </div>
                </div>
            </div>
        </form>
        <hr />
    </div>
</asp:Content>
