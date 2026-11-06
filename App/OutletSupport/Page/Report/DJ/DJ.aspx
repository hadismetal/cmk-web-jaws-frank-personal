<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DJ.aspx.cs" inherits="Page_Approval_Product" %>

<asp:content contentplaceholderid="h1" runat="server">
	Promo Diamond Jewellery
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Report
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div id="form_area" runat="server">
		<form runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    PLU
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="175" placeholder="Input PLU" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                   Location Type
                </div>
                <asp:DropDownList ID="tipelokasi" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasi" AutoPostBack="true">
                
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Location
                </div>
                <asp:DropDownList ID="lokasi" runat="server" CssClass="form-control" Width="175">
                
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Dari
                </div>
                <asp:TextBox ID="dari" runat="server" CssClass="form-control text-center" Width="110" placeholder="Dari" />
                <script>
                    $(document).ready(function () { $('#<%=dari.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>

            </div>
            <div class="form-group">
                <div class="text-muted">
                    Sampai
                </div>
                <asp:TextBox ID="sampai" runat="server" CssClass="form-control text-center" Width="110" placeholder="Sampai" />
                <script>
                    $(document).ready(function () { $('#<%=sampai.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>

            </div>
            <div class="form-group">
                <div class="text-muted">
                    Status
                </div>
                <asp:DropDownList ID="status" runat="server" CssClass="form-control">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Active / Expired</asp:ListItem>
                    <asp:ListItem Value="2">Upcoming</asp:ListItem>
                    <asp:ListItem Value="3">Cancel</asp:ListItem>
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
        </form>
    </div>
    <div id="zero" runat="server" visible="false">
        <br />
		<i class="icon-ok icon-5x pull-left text-success"></i>
		<h4>Proses Selesai</h4>
		<p class="text-muted">
			Tidak ada promo.
		</p>
		<div class="clearfix"></div>
		<hr />
	</div>
    <div id="tbl_result" runat="server" style="padding-top:15px;">

		<asp:table id="tb" runat="server" cssclass="table-list text-sm" style="width:100%;">
			<asp:tableheaderrow>

				<asp:tableheadercell width="50">Cancel</asp:tableheadercell>
                <asp:tableheadercell width="50">Detail</asp:tableheadercell>
				<asp:tableheadercell width="50">No.</asp:tableheadercell>
                <asp:tableheadercell><div style="border-bottom:solid 1px gray;">Nama Promo</div><div>Operator</div></asp:tableheadercell>
                <asp:tableheadercell width="50">Qty</asp:tableheadercell>
                <asp:tableheadercell width="90">Status</asp:tableheadercell>
				<asp:tableheadercell width="160"><div style="border-bottom:solid 1px gray;">Start</div><div>Expired</div></asp:tableheadercell>
                <asp:tableheadercell width="100">Tipe</asp:tableheadercell>
                <asp:tableheadercell width="130" cssclass="text-right">Promo 1 Harga <small>IDR</small></asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right">Discount Nominal <small>IDR</small></asp:tableheadercell>
				<asp:tableheadercell width="100" cssclass="text-right">Discount Persen <small>%</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Point Reward</asp:tableheadercell>
						
			</asp:tableheaderrow>
		</asp:table>

	</div>
</asp:content>

