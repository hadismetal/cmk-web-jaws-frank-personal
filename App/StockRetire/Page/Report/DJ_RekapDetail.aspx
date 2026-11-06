<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_RekapDetail.aspx.cs" Inherits="Page_Report_DJ_RekapDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" Runat="Server">
    Diamond Jewelry
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" Runat="Server">
    Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" Runat="Server">
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
    <div id="form_area" runat="server">
		<form runat="server" class="form-inline">
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
                    Status :
                </div>
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Asal</asp:ListItem>
                    <asp:ListItem Value="1" Selected="True">Terima</asp:ListItem>
                    <asp:ListItem Value="2">Rusak</asp:ListItem>
                    <asp:ListItem Value="3">Selisih</asp:ListItem>                   
                </asp:DropDownList>
            </div>           
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                        <i class="icon-search"></i>
                        Display</button>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Export">
                        <i class="icon-search"></i>
                        Export
                    </button>
                </div>
            </div>
			<div class="table-responsive">
				<asp:table id="tb" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
                        <asp:tableheadercell width="50">No</asp:tableheadercell>
						<asp:tableheadercell width="200">Parcel</asp:tableheadercell>
                        <asp:tableheadercell width="200">Total Butir</asp:tableheadercell>
						<asp:tableheadercell width="150">Carat</asp:tableheadercell>
						<asp:tableheadercell width="150">M</asp:tableheadercell>
                        <asp:tableheadercell width="150">$</asp:tableheadercell>
					</asp:tableheaderrow>
				</asp:table>
			</div>
		</form>
	</div>
</asp:Content>

