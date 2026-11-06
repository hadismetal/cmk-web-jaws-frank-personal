<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="1B.aspx.cs" inherits="Page_Approval_1B" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Rapaport
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Approval
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
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
                    <asp:ListItem>Size :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel03" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Colour :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel04" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Clarity :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="parcel05" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Cutting :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="parcel06" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Brand :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="parcel07" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Category :</asp:ListItem>
                </asp:DropDownList>
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
				<div class="text-muted">
					Display :
				</div>
				<asp:dropdownlist id="display" runat="server" cssclass="form-control" width="175">
					<asp:listitem>Harga Final</asp:listitem>
					<asp:listitem>Komponen Rumus</asp:listitem>
				</asp:dropdownlist>
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
				<asp:table id="tb" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
						<asp:tableheadercell width="100" />
						<asp:tableheadercell width="40">No.</asp:tableheadercell>
						<asp:tableheadercell width="250">Nama</asp:tableheadercell>
						<asp:tableheadercell width="175">Operator</asp:tableheadercell>
						<asp:tableheadercell width="130" cssclass="text-right"><small>USD</small> Cost</asp:tableheadercell>
						<asp:tableheadercell width="130" cssclass="text-right"><small>USD</small> Harga</asp:tableheadercell>
						<asp:tableheadercell width="130" cssclass="text-right"><small>USD</small> Pending</asp:tableheadercell>
						<asp:tableheadercell width="150" cssclass="text-right">Mutasi</asp:tableheadercell>
                        <asp:tableheadercell width="150" cssclass="text-right">Mark Up</asp:tableheadercell>
                        <asp:tableheadercell width="150" cssclass="text-right">Mark Up Pending</asp:tableheadercell>
					</asp:tableheaderrow>
				</asp:table>
			</div>
			<div class="button-left">
				<asp:dropdownlist id="process_id" runat="server" visible="false" />
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
</asp:content>

