<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PreLEI.aspx.cs" Inherits="PreLEI" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    PreLEI Reborn
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Approval
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form runat="server" class="form-inline">
		<div class="form-group">
            <div class="text-muted">
                NO. RTS :
            </div>
            <asp:TextBox ID="txt_rts" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="refreshPage" Width="200" />
        </div>
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <asp:Button ID="btn_inputproduct" OnClick="refreshPage" Text="Enter" runat="server" class="btn btn-primary">
				</asp:Button>
            </div>
        </div>
        <div class="form-group">
            <asp:Literal runat="server" ID="lit_valid" />
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="100"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. Return </asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Nama Asal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Nama Tujuan</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="150">Operator</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="150">Operator Tgl</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </form>
</asp:Content>

