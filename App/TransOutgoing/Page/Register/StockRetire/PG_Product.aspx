<%@ page EnableEventValidation="false" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="PG_Product.aspx.cs" inherits="PG_Product" %>

<%@ register src="MenuPG.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Plain Gold
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">

	<uc1:menu id="menu" runat="server" />
    <table class="table-file">
		<tr>
			<td class="text-label">Market Brand</td>
			<td>:</td>
			<td id="brand" runat="server" class="text-bold" />
		</tr>
		<tr>
			<td class="text-label">From <asp:Literal runat="server" ID="lit_type_from" /></td>
			<td>:</td>
			<td id="from" runat="server" class="text-bold" />
		</tr>
        <tr>
			<td class="text-label">To <asp:Literal runat="server" ID="lit_type_to" /></td>
			<td>:</td>
			<td id="to" runat="server" class="text-bold" />
		</tr>
	</table>
    <hr />
	<form runat="server">
<%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="upl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
            <div class="form-inline">
                <asp:Panel DefaultButton="btn_inputproduct">
                <div class="form-group width-300">
                    <div class="text-muted">
				        Product Code :
			        </div>
			        <asp:textbox id="inputproduct" runat="server" cssclass="form-control text-bold" maxlength="20" />
		        </div>
                <div class="form-group">
			        <div class="hidden-xs">&nbsp;</div>
			        <div>
                        <asp:button id="btn_inputproduct" OnClick="btn_inputproduct_Click" Text="Enter" runat="server" class="btn btn-primary">
				            <%--<i class="icon-arrow-right"></i>--%>
			            </asp:button>
                        &nbsp;
                        <asp:Literal runat="server" ID="lit_Message" />
                    </div>
                </div>
                </asp:Panel>
            </div>
            <br />
		    <div class="form-inline">
                <asp:CheckBoxList runat="server" ID="cbl_Product" />
                <style type="text/css">
                    label {
                        display: block;
                        border:solid 1px black;
                        border-collapse:collapse;
                    }
                </style>
		    </div>
        <%--</ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_inputproduct" EventName="Click" />
        </Triggers>
        </asp:UpdatePanel>--%>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
		</div>
	</form>
</asp:content>

