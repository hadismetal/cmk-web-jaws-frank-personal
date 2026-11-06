<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="LD.aspx.cs" inherits="Page_Approval_LD" %>

<asp:content contentplaceholderid="h1" runat="server">
	Loose Diamond Rekap
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Report
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div id="form_area" runat="server">
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
        <form id="Form1" runat="server" class="form-inline">
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
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="Input PLU" />
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
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
						<asp:tableheadercell width="90" />
						<asp:tableheadercell width="40" />
						<asp:tableheadercell width="150">Nomor Dokumen</asp:tableheadercell>
                        <asp:tableheadercell width="150">Nomor Supplier</asp:tableheadercell>
                        <asp:tableheadercell width="150">Nama Supplier</asp:tableheadercell>
                        <asp:tableheadercell width="150">Total PLU</asp:tableheadercell>
                        <asp:tableheadercell width="120">Tanggal</asp:tableheadercell>
						<asp:tableheadercell width="175">Operator</asp:tableheadercell>
                        <asp:tableheadercell width="175">Pengirim</asp:tableheadercell>
					</asp:tableheaderrow>
				</asp:table>
			</div>
		</form>
	</div>
</asp:content>

