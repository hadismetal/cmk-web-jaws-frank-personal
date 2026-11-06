<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="PLU_Delete.aspx.cs" inherits="Page_Approval_DJ" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewelry
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Delete PLU Non Jaws
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
		<form runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="Input PLU" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="tipe" CssClass="form-control" runat="server">
                    <asp:ListItem Value="1">Diamond Jewellery</asp:ListItem>
                    <asp:ListItem Value="2">Plain Gold</asp:ListItem>
                    <asp:ListItem Value="3">Loose Diamond</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="deletePLU">
                        <i class="icon-search"></i>
                        Delete
                    </button>
                </div>
            </div>
		</form>
	</div>
</asp:content>

