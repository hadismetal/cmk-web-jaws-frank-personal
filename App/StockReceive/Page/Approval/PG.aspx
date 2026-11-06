<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PG.aspx.cs" Inherits="Page_Approval_PG" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Plain Gold
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Approval
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
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
            <div class="form-inline">
                <div class="form-group">
                    <label class="control-label">Periode</label>
                    <br />
                    <asp:TextBox class="form-control" type="text" placeholder="Dari" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" Width="110" />
                    <script>
                        $(document).ready(function () { $('#<%=datepicker.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                    </script>
                    <asp:RequiredFieldValidator ID="datepicker_RF" runat="server" />
                    <span id="datepicker_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </div>
                <div class="form-group">
                    <label class="control-label">&nbsp;</label>
                    <br />
                    <asp:TextBox class="form-control" type="text" placeholder="Sampai" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" Width="110" />
                    <script>
                        $(document).ready(function () { $('#<%=datepicker2.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                    </script>
                    <asp:RequiredFieldValidator ID="datepicker2_RF" runat="server" />
                    <span id="datepicker2_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </div>
                <div class="form-group">
                    <label class="control-label">Product Level</label>
                    <br />
                    <asp:DropDownList CssClass="form-control" runat="server" ID="ddl_ProductLevel">
                        <asp:ListItem Value="0">Product Level</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label class="control-label">Item</label>
                    <br />
                    <asp:DropDownList CssClass="form-control" runat="server" ID="ddl_ProductItem">
                        <asp:ListItem Value="0">Product Item</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label class="control-label">Model</label>
                    <br />
                    <asp:DropDownList CssClass="form-control" runat="server" ID="ddl_Model">
                        <asp:ListItem Value="0">Model</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label class="control-label">Warna</label>
                    <br />
                    <asp:DropDownList CssClass="form-control" runat="server" ID="ddl_RangkaWarna">
                        <asp:ListItem Value="0">Warna</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label class="control-label">Supplier</label>
                    <br />
                    <asp:DropDownList CssClass="form-control" runat="server" ID="ddl_Supplier">
                        <asp:ListItem Value="0">Supplier</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="btn_display_Click">
                        <i class="icon-search"></i>
                        Display
                    </button>
                </div>
            </div>
            <div class="table-responsive">

                <div class="control-group">
                    <label class="control-label">&nbsp;</label>

                    <asp:GridView ID="EmasGridView" runat="server" ShowFooter="true" class="table table-striped" AutoGenerateColumns="false" OnRowDeleting="EmasGridView_onRowDeleting">
                        <Columns>
                            <asp:TemplateField HeaderText="" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txbNumberList" Text="<%#Bind('number') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" Visible="False">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtID"
                                        Text="<%# Bind('id') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rate Type">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="check"
                                        Text="<%# Bind('check') %>"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rate Type">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtRateType"
                                        Text="<%# Bind('ratetype') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Image" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="tbImg"
                                        Text="<%# Bind('img') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Kode Barang" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="kodebarang"
                                        Text="<%# Bind('kodebarang') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="No TTB" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtNoTTB"
                                        Text="<%# Bind('no_ttb') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SJ Supplier" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtSJSupplier"
                                        Text="<%# Bind('sj_supplier') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tanggal" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtTgl"
                                        Text="<%# Bind('tgl') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product Level" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtProductLevel"
                                        Text="<%# Bind('product_level') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Model" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtModel"
                                        Text="<%# Bind('model') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Product Item" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtProductItem"
                                        Text="<%# Bind('product_item') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
							<asp:TemplateField HeaderText="Kadar Tukaran Beli" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtKadarTukaranBeli" 
                                        Text="<%# Bind('kadar_tukaran_beli') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>					
                            <asp:TemplateField HeaderText="Qty" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtQty"
                                        Text="<%# Bind('qty') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Berat" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtTotalBerat"
                                        Text="<%# Bind('total_berat') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Harga M" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtHarga"
                                        Text="<%# Bind('harga') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ongkos" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtOngkos"
                                        Text="<%# Bind('ongkos') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Harga M" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtTotalHarga"
                                        Text="<%# Bind('total_harga') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Harga Jual" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtTotalHargaJual"
                                        Text="<%# Bind('total_hargajual') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Berat (24K)" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtTotalBerat24"
                                        Text="<%# Bind('total_berat24') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Harga (24K)" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtHarga24"
                                        Text="<%# Bind('harga24') %>"></asp:Literal>
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
                <div class="control-group">
                    <label class="control-label">&nbsp;</label>
                    <div class="controls">
                        <asp:Button runat="server" ID="btn_submit" CssClass="btn btn-primary button-loading btn-default" data-loading-text="Searching..." Text="Approve" OnClick="btn_submit_Click" />
                    </div>
                </div>
            </div>
        </form>
    </div>
</asp:Content>

