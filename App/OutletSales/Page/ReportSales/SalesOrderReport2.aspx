<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SalesOrderReport2.aspx.cs" Inherits="SalesOrderReport2" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Sales Order
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
        .status-label {
          display: inline-block;
          padding: 4px 10px;
          border-radius: 2px;
          color: white;
          font-size: 0.9rem;
          font-weight: 400;
        }

        .status-lunas {
          background-color: #28a745; /* green */
        }

        .status-belumlunas {
          background-color: #ffc107; /* yellow */
          color: black;
        }

        .status-waitforsubmit {
          background-color: orange; /* orange */
        }

        .status-void {
          background-color: #dc3545; /* red */
        }

        .status-draft {
          background-color: #6c757d; /* gray */
        }
    </style>
    <div id="zero" runat="server" visible="false">
        <i class="icon-ok icon-5x pull-left text-success"></i>
        <h4>Proses Selesai, harap hubungi Store Manager atau Area Manager untuk approve</h4>
        <p class="text-muted">
            Tidak ada transaksi penjualan
        </p>
        <div class="clearfix"></div>
        <hr />
    </div>
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Search :
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Start :
                </div>
                <asp:TextBox ID="dari" runat="server" CssClass="form-control text-center" Width="110" placeholder="Dari" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    End :
                </div>
                <asp:TextBox ID="sampai" runat="server" CssClass="form-control text-center" Width="110" placeholder="Sampai" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Status :
                </div>
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Draft</asp:ListItem>
					<asp:ListItem Value="2">Active</asp:ListItem>
                    <asp:ListItem Value="4">Waiting For Submit</asp:ListItem>
                    <asp:ListItem Value="5">Lunas</asp:ListItem>
                    <asp:ListItem Value="6">Belum Lunas</asp:ListItem>
                    <asp:ListItem Value="3">Void</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage" onclick="if(!ondisplayclick(this)) return false;">
                        <i class="icon-search"></i>
                        Display
                    </button>
					<button id="export" runat="server" class="btn btn-primary" onserverclick="exportTableNew">
                        <i class="icon-search"></i>
                        Export
                    </button>
                </div>
            </div>
        </form>
        <div class="table-responsive">
            <asp:Literal runat="server" ID="lit_error" Text=""></asp:Literal>
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm" Width="100%">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="100" />
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="120">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. Referensi</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="100">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Nama Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Terbayar</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Belum dibayar</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Void Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
	<script type="text/javascript">
		$(document).ready(function(){
			$('#<%=sampai.ClientID%>').datepicker({ format: 'dd-mm-yyyy' });
			$('#<%=dari.ClientID%>').datepicker({ format: 'dd-mm-yyyy' });
		});
		
		function ondisplayclick(e) {
			$(e).prop('disabled', true);
			let tgl_dari = $('#<%=dari.ClientID%>').val();
			let tgl_sampai = $('#<%=sampai.ClientID%>').val();

			let tgl_dari_parts = tgl_dari.split("-");
			let tgl_dari_beneran = new Date(tgl_dari_parts[2], tgl_dari_parts[1] - 1, tgl_dari_parts[0]);

			let tgl_sampai_parts = tgl_sampai.split("-");
			let tgl_sampai_beneran = new Date(tgl_sampai_parts[2], tgl_sampai_parts[1] - 1, tgl_sampai_parts[0]);
			let days_difference = getDaysBetweenDates(tgl_sampai_beneran, tgl_dari_beneran);
			if (days_difference > 31) {
				alert('Jika periode lebih dari 31 hari, klik tombol Export');
				$(e).removeAttr('disabled');
				return false;
			}
			else{
				return true;
			}
		}
		
		function getDaysBetweenDates(date1, date2) {
			// Convert both dates to timestamp (milliseconds)
			let timestamp1 = new Date(date1).getTime();
			let timestamp2 = new Date(date2).getTime();

			// Calculate the difference in milliseconds
			let differenceInMillis = Math.abs(timestamp2 - timestamp1);

			// Convert milliseconds to days
			let differenceInDays = differenceInMillis / (1000 * 3600 * 24);

			return differenceInDays;
		}
	</script>
</asp:Content>

