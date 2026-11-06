<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Summary.aspx.cs" inherits="Page_Summary" %>

<asp:content contentplaceholderid="h1" runat="server">
	Summary
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div class="row">
		<div class="col-lg-6">
			<div class="table-responsive">
				<asp:table id="tb1" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
						<asp:tableheadercell width="200">Diamond</asp:tableheadercell>
						<asp:tableheadercell width="250" />
						<asp:tableheadercell width="50" />
					</asp:tableheaderrow>
				</asp:table>
			</div>
		</div>
		<div class="col-lg-6">
			<div class="table-responsive">
				<asp:table id="tb2" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
						<asp:tableheadercell width="200">Colour Diamond</asp:tableheadercell>
						<asp:tableheadercell width="250" />
						<asp:tableheadercell width="50" />
					</asp:tableheaderrow>
				</asp:table>
			</div>
		</div>
	</div>
	<div class="row">
		<div class="col-lg-6">
			<div class="table-responsive">
				<asp:table id="tb3" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
						<asp:tableheadercell width="200">Precious Stone</asp:tableheadercell>
						<asp:tableheadercell width="250" />
						<asp:tableheadercell width="50" />
					</asp:tableheaderrow>
				</asp:table>
			</div>
		</div>
		<div class="col-lg-6">
			<div class="table-responsive">
				<asp:table id="tb4" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
						<asp:tableheadercell width="200">Semi Precious</asp:tableheadercell>
						<asp:tableheadercell width="250" />
						<asp:tableheadercell width="50" />
					</asp:tableheaderrow>
				</asp:table>
			</div>
		</div>
	</div>
	<div class="row">
		<div class="col-lg-6">
			<div class="table-responsive">
				<asp:table id="tb5" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
						<asp:tableheadercell width="200">Pearl</asp:tableheadercell>
						<asp:tableheadercell width="250" />
						<asp:tableheadercell width="50" />
					</asp:tableheaderrow>
				</asp:table>
			</div>
		</div>
		<div class="col-lg-6">
			<div class="table-responsive">
				<asp:table id="ts1" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
						<asp:tableheadercell width="450">Settings (Size Control)</asp:tableheadercell>
						<asp:tableheadercell width="50" />
					</asp:tableheaderrow>
				</asp:table>
			</div>
			<div class="table-responsive">
				<asp:table id="ts2" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
						<asp:tableheadercell width="450">Settings (Shape Control)</asp:tableheadercell>
						<asp:tableheadercell width="50" />
					</asp:tableheaderrow>
				</asp:table>
			</div>
		</div>
	</div>
</asp:content>

