<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DJ.aspx.cs" inherits="Page_Approval_DJ" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewellery
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
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
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="Input PLU" />                
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Mulai:
                </div>
                <asp:TextBox CssClass="form-control" type="text" placeholder="Month" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" />

            </div>
            <div class="form-group" id="samp" runat="server">
                <div class="text-muted">
                    Sampai :
                </div>
                <asp:TextBox CssClass="form-control" type="text" placeholder="Sampai" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" />
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                        <i class="icon-search"></i>
                        Display
                    </button>
                    <asp:Label ID="lb_valid" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>

			<div class="table-responsive">
				<asp:table id="tb" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
						<asp:tableheadercell width="100">Action</asp:tableheadercell>
						<asp:tableheadercell width="40">No</asp:tableheadercell>
                        <asp:tableheadercell width="120">Image</asp:tableheadercell>
						<asp:tableheadercell width="120">PLU</asp:tableheadercell>
						<asp:tableheadercell width="150">Form</asp:tableheadercell>
						<asp:tableheadercell width="120">Product Item</asp:tableheadercell>
						<asp:tableheadercell width="75">Category</asp:tableheadercell>
						<asp:tableheadercell width="75">Level</asp:tableheadercell>
                        <asp:tableheadercell width="200">From</asp:tableheadercell>
					</asp:tableheaderrow>
				</asp:table>
			</div>
		</form>
	</div>
</asp:content>

