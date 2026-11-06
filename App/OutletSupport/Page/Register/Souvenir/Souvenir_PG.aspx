<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Souvenir_PG.aspx.cs" inherits="Page_Create_ProductAddr" %>

<%@ register src="MenuPromo.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Souvenir
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />	
	<hr />
	<form id="Form1" runat="server">
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="upl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
        <div class="form-inline">
            <asp:Panel DefaultButton="btn_inputproduct">
                <div class="form-group width-300">
                    <div class="text-muted">
                        Product Code :
                    </div>
                    <asp:TextBox ID="inputproduct" runat="server" CssClass="form-control text-bold" MaxLength="20" />
                </div>
                <div class="form-group">
                    <div class="hidden-xs">&nbsp;</div>
                    <div>
                        <asp:Button ID="btn_inputproduct" OnClick="btn_inputproduct_Click" Text="Enter" runat="server" class="btn btn-primary">
                            <%--<i class="icon-arrow-right"></i>--%>
                        </asp:Button>
                        &nbsp;
                        <asp:Literal runat="server" ID="lit_Message" />
                    </div>
                </div>
            </asp:Panel>
        </div>
        <br />
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30" />
                    <asp:TableHeaderCell Width="110">Image</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="185">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Nett Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="300">Nama</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>

            <div class="form-inline">
                <asp:CheckBoxList runat="server" ID="cbl_Product" />
            </div>
        </div>
		
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next
			</button>
            <button id="skip" runat="server" class="btn btn-success" onserverclick="skip_Click">
				<i class="icon-remove"></i>
				Skip
			</button>
		</div>
	</form>
</asp:content>

