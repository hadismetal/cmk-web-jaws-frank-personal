<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PaymentRepair_ItemList.aspx.cs" Inherits="Page_Register_Payment_ItemList" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Payment Repair
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Detail
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <p>
        <span class="text-muted">Nomor :</span><br />
        <span class="text-lg" id="header_nomor" runat="server" />
    </p>
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
        transaction form
    </div>
    <div class="row">
        <div class="col-md-5">
            <table class="table-file">
                <tr>
                    <td class="text-label">Market Brand</td>
                    <td>:</td>
                    <td id="brand" runat="server" />
                </tr>
            </table>
        </div>
        <br class="visible-sm visible-xs" />
        <div class="col-md-5">
            <table class="table-file">
                <tr>
                    <td class="text-label">Customer</td>
                    <td>:</td>
                    <td id="customer" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Keterangan</td>
                    <td>:</td>
                    <td id="keterangan" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Tanggal</td>
                    <td>:</td>
                    <td id="tgl" runat="server" />
                </tr>
            </table>
        </div>
    </div>
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
        transaction data
    </div>

    <div class="table-responsive">
        <asp:Table ID="tb" runat="server" CssClass="table-list">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="110">No. Invoice</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="110">PLU</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="110">Product Item</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Total Bayar</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>

    <div class="file-subheader hidden-sm hidden-xs">
        Total
    </div>

    <div class="table-responsive">
        <asp:Table ID="Table1" runat="server" CssClass="table-list">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="160">Nilai Sales Order</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Discount</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Total Bayar</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Terbayar</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Kekurangan</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Pembayaran</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                
                <asp:TableCell ID="nilaiso" runat="server"></asp:TableCell>
                <asp:TableCell ID="discount" runat="server"></asp:TableCell>
                <asp:TableCell ID="totalbayar" runat="server"></asp:TableCell>
                <asp:TableCell ID="terbayar" runat="server"></asp:TableCell>
                <asp:TableCell ID="kekurangan" runat="server"></asp:TableCell>
                <asp:TableCell ID="pembayaran" runat="server"></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    <form id="Form1" runat="server" class="form-inline">
        <div class="control-group spacing-top">

            <div class="controls">
                <i class="icon-plus">
                    <asp:Button runat="server" ID="btn_add_payment" Text="Add Payment" OnClick="btn_add_payment_click" /></i><br />

            </div>
        </div>
        <div class="control-group">
            <label class="control-label">&nbsp;</label>

            <asp:GridView ID="GridView" runat="server" OnDataBinding="Bind" ShowFooter="true" class="table table-striped" AutoGenerateColumns="false" OnRowDeleting="GridView_onRowDeleting">
                <Columns>
                    <asp:TemplateField HeaderText="" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txbNumberList" Text="<%#Bind('number') %>"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Payment Type" Visible="True">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlType" runat="server"
                                Width="120px" >
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Card Type" Visible="True">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlCardType" runat="server"
                                Width="120px" >
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="EDC" Visible="True">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlEDC" runat="server"
                                Width="120px" >
                                
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Bank Issuer" Visible="True">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlBank" runat="server"
                                Width="120px" >
                                
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="No. Kartu" Visible="True">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtNoKartu"
                                Text="<%# Bind('nokartu') %>"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Program" Visible="True">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlProgram" runat="server"
                                Width="120px" >
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nominal" Visible="True">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtNominal"
                                Text="<%# Bind('nominal') %>"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MDR" Visible="True">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtMDR"
                                Text="<%# Bind('mdr') %>"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField Visible="True">
                        <ItemTemplate>
                            <asp:Button ID="Button1" CommandName="Delete" runat="server" Text="Hapus" CssClass="btnRed" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

        </div>

        <div class="button-left">
			<button id="save" runat="server" class="btn btn-success" OnClick="if ( ! UserConfirmation()) return false;" onserverclick="save_Click">
				<i class="icon-check"></i>
				Finish
			</button>
		</div>
    </form>
    <script type="text/javascript">
        function UserConfirmation() {
            txtConfirm =
				"Lanjutkan Proses PEMBAYARAN ?\n\n" +
				"transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";
            if (confirm(txtConfirm))
                return true;
            else
                return false;
        }
    </script>
</asp:Content>

