<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RepairDetail.aspx.cs" Inherits="Page_RepairDetail" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Report Detail Reparasi
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
        .table-responsive {
            /* max-height: 500px; */
            overflow-y: auto;
            /* overflow-x: auto; */
            border: 1px solid #ccc;
        }

        .report-table {
            border-collapse: collapse;
            border-spacing: 0;
            width: 100%;
            font-family: Arial;
            table-layout: fixed; 
        }
    
        .report-table th, .report-table td {
            border: 1px solid black;
            position: sticky;
            z-index: 2;
            padding: 10px;
        }
        
        .report-table th {
            text-align: center;
        }

        .report-table td {
            text-align: left;
        }
    
        .header {
            background-color: lightblue;
            font-weight: bold;
        }
    
        .total-row {
            font-weight: bold;
            background-color: #f2f2f2;
        }

        .text-right {
            text-align: right !important;
        }


        .total-row {
            font-weight: bold;
            background-color: #f2f2f2;
            text-align: right !important;
        }

        .total-label {
            text-align: center !important;
        }

        .jasa-column {
            background-color: #c8f2b8; /* biru muda */
            /* background-color: #90ee90;  */
        }

        .gold-column{
            background-color: #fffacd; /* kuning muda */
        }

        .stone-column{
            background-color: #e6e6fa; /* ungu muda */
        }


    </style>
    <div id="zero" runat="server" visible="false">
        <i class="icon-ok icon-5x pull-left text-success"></i>
        <h4>Proses Selesai</h4>
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
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="300" placeholder="Input Nomor Repair..." />
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
            <div class="form-group">
                <div class="text-muted">
                    Type / Jenis Repair :
                </div>
                <asp:DropDownList ID="filter" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="-1">All</asp:ListItem>
                    <asp:ListItem Value="1">Gratis</asp:ListItem>
                    <asp:ListItem Value="0">Berbayar</asp:ListItem>
                </asp:DropDownList>
            </div>
			<div class="form-group">
				<div class="text-muted">
					Tipe Lokasi
				</div>
				<asp:DropDownList ID="tipelokasi" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasi" AutoPostBack="true">
					<asp:ListItem Value="">All</asp:ListItem>
				</asp:DropDownList>
			</div>
			<div class="form-group">
				<div class="text-muted">
					Kode Lokasi
				</div>
				<asp:DropDownList ID="locationcode" runat="server" CssClass="form-control" Width="175" >
					<asp:ListItem Value="">All</asp:ListItem>
				</asp:DropDownList>
			</div>			
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                        <i class="icon-search"></i>
                        Display
                    </button>                   
                    <button id="export" runat="server" class="btn btn-primary" onserverclick="exportTableNew">
                        <i class="icon-print"></i>
                        Export
                    </button>                   
                </div>
            </div>
        </form>

        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="report-table"></asp:Table>
        </div>
        

    </div>
</asp:Content>

