<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Repair.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Repair Invoice
</asp:Content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Report
</asp:content>

<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
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
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    No PLU / No Invoice Repair:
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input PLU..." />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Dari :
                </div>

                <asp:TextBox CssClass="form-control" type="text" placeholder="Dari" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" Width="100" />

            </div>
            <div class="form-group">
                <div class="text-muted">
                    Sampai :
                </div>
                <asp:TextBox CssClass="form-control" type="text" placeholder="Sampai" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" Width="100" />
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
                <asp:DropDownList ID="productlevel" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Level</asp:ListItem>
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
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <span id="err" runat="server" class="text-warning" />
                </div>
            </div>
            <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell RowSpan="2" Width="50">No.</asp:TableHeaderCell>
					<asp:TableHeaderCell RowSpan="2" Width="100">Print Invoice</asp:TableHeaderCell>
					<asp:TableHeaderCell RowSpan="2" Width="120">No. Document Repair</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="140">No. Invoice Repair</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="80">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="120">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="70">Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="70">Level</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="80">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="100">Operator</asp:TableHeaderCell>
                    <asp:TableHeaderCell ColumnSpan="2" Width="140">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell ColumnSpan="2" Width="140">Netto Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell ColumnSpan="2" Width="140">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell ColumnSpan="2" Width="140">Total Carat</asp:TableHeaderCell>
					<asp:TableHeaderCell RowSpan="2" Width="120">Status Repair</asp:TableHeaderCell>
					<asp:TableHeaderCell RowSpan="2" Width="120">Action</asp:TableHeaderCell>
				</asp:TableHeaderRow>
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Sebelum</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Sesudah</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Sebelum</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Sesudah</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Sebelum</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Sesudah</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Sebelum</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Sesudah</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        </form>
    </div>
</asp:Content>

