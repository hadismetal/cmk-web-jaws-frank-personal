<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Design.aspx.cs" inherits="Page_Approval_Design" %>

<asp:content contentplaceholderid="h1" runat="server">
	Gold Jewelry
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Approval
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
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
		<form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Search :
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
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
            <div class="text-muted">
                Characteristic Design :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="designcategory" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Design Category :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="designconcept" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Design Concept :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="targetage" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Target Age :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="targetgender" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Target Gender :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Characteristic Product :
                </div>
                <asp:DropDownList ID="productitem" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Item</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="productcategory" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Category</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="-1">Status</asp:ListItem>
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Approved</asp:ListItem>
                    <asp:ListItem Value="2">Draft</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="designer" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Designer</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <div class="text-muted">
                    Brand :
                </div>
                <asp:DropDownList ID="brand" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Brand :</asp:ListItem>
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
				<asp:table id="tb" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
						<asp:tableheadercell width="90" />
                        <asp:tableheadercell width="40" />
						<asp:tableheadercell width="120" />
						<asp:tableheadercell width="120">Nomor</asp:tableheadercell>
						<asp:tableheadercell width="175">Operator</asp:tableheadercell>
						<asp:tableheadercell width="150">No. Referensi</asp:tableheadercell>
						<asp:tableheadercell width="150">Designer</asp:tableheadercell>
						<asp:tableheadercell width="120">Product</asp:tableheadercell>
						<asp:tableheadercell width="75" />
						<asp:tableheadercell width="75" />
					</asp:tableheaderrow>
				</asp:table>
			</div>
		</form>
	</div>
</asp:content>

