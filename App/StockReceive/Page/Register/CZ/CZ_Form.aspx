<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CZ_Form.aspx.cs" Inherits="Page_Register_CZ_Form" %>

<%@ Register Src="MenuCZ.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Cubic Zirconia
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
   
     <uc1:Menu ID="menu" runat="server" />
    <p>
        <span class="text-muted">Warehouse :</span><br />
        <span class="text-bold" id="warehouse" runat="server" />
    </p>
    <hr />
    <form runat="server">

        <table class="table-form">
            <tr>
                <td>Tanggal</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tgl" runat="server" CssClass="form-control text-center" Width="110" />
                    <script>
                        $(document).ready(function () { $('#content_tgl').datepicker({ format: 'dd-mm-yyyy' }); });
                    </script>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="tgl_RF" runat="server" />
                    <span id="tgl_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
            </tr>
            <tr>
                <td>Tanggal TJT</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tgltjt" runat="server" CssClass="form-control text-center" Width="110" />
                    <script>
                        $(document).ready(function () { $('#<%=tgltjt.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                    </script>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="tgltjt_RF" runat="server" />
                    <span id="tgltjt_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
            </tr>
            <tr>
                <td>Supplier</td>
                <td>:</td>
                <td>
                    <asp:DropDownList CssClass="form-control" runat="server" ID="ddlsupplier" AutoPostBack="true" OnSelectedIndexChanged="calculatePriceModel" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>Nama Supplier</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="supplier" runat="server" CssClass="form-control text-center" Width="200" />

                </td>
                <td></td>
            </tr>
            <tr>
                <td>SJ Supplier</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="sjsupplier" runat="server" CssClass="form-control text-center" Width="200" />

                </td>
                <td></td>
            </tr>
            <tr>
                <td>Harga Emas/Gram (24K)</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="goldrate" runat="server" CssClass="form-control text-center" Width="110" />

                </td>
                <td>
                    <asp:RadioButtonList ID="ratetype" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="LoadModel">
                        <asp:ListItem Value="0" Selected>TGP</asp:ListItem>
                        
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>Stock Keeper</td>
                <td>:</td>
                <td>
                    <div class="input-group">
                    <div class="input-group-btn">
                        <asp:TextBox ID="idsk" runat="server" Enabled="false" CssClass="form-control text-center" Width="50" />&nbsp;
                        <asp:HiddenField ID="idskhidden" runat="server" />
                    </div>

                    <asp:TextBox ID="stockkeeper" runat="server" CssClass="form-control text-center" Width="200" />
                        </div>
                </td>
                <td></td>
            </tr>
            <%--<tr>
                <td>Ring Size / Panjang Chain</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="dimensir" runat="server" CssClass="form-control text-center" Width="200" />

                </td>
                <td></td>
            </tr>--%>
        </table>
        <div class="form-group width-500">
            <label>
                Keterangan :
            </label>
            <asp:TextBox ID="keterangan" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
        </div>
        <%-- <div class="form-group width-500">
            <label>
                Kadar Tukaran Jual :
            </label>
            <asp:TextBox OnTextChanged="calculatePrice" AutoPostBack="true" CssClass="form-control" Text="82" placeholder="Kadar Jual" ID="kadar_jual" runat="server" />
         </div>--%>
        
       
        <p>
            <br />
            
            <br />
            <span class="text-bold">Gold Information</span>

        </p>
        <hr />


        <div class="control-group">
            <label class="control-label">&nbsp;</label>

                      <asp:GridView ID="EmasGridView" runat="server" ShowFooter="true" class="table table-striped" AutoGenerateColumns="false" >
                <Columns>

                   <asp:TemplateField HeaderText="No" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtnumber"  Text="<%# Bind('numb') %>"></asp:Literal>                           
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="aa" Text="Total"></asp:Literal>
                        </FooterTemplate>
                      </asp:TemplateField>
                    

                      <asp:TemplateField HeaderText="No PLU" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtPLU"  Text="<%# Bind('plu') %>"></asp:Literal>                           
                        </ItemTemplate>
                      </asp:TemplateField>

                     <asp:TemplateField HeaderText="Kode Barang" Visible="True">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtKodeBarang"  Text="<%# Bind('kodebarang') %>" Width="130"></asp:TextBox>                           
                        </ItemTemplate>
                      </asp:TemplateField>

                     <asp:TemplateField HeaderText="Nama Barang" Visible="True">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtNamaBarang" width="130" Text="<%# Bind('namabarang') %>"></asp:TextBox>                           
                        </ItemTemplate>
                      </asp:TemplateField>

                              <asp:TemplateField HeaderText="Ring Size (Panjang Chain)" Visible="True">
                        <ItemTemplate>
                            <asp:Textbox runat="server" ID="txtDimensir"  Text="<%# Bind('dimensir') %>" Width="70"></asp:Textbox>                           
                        </ItemTemplate>
                        </asp:TemplateField>

                     

                    <asp:TemplateField HeaderText="Kadar Tukaran Jual" Visible="True">
                        <ItemTemplate>
                            <asp:Textbox runat="server" ID="txtKjual"  Text="<%# Bind('kjual') %>" Width="70"></asp:Textbox>                          
                        </ItemTemplate>
                        </asp:TemplateField>

                    <asp:TemplateField HeaderText="Berat" Visible="True">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtBerat" Text="<%# Bind('berat') %>" Width="50" OnTextChanged="calculatePrice" AutoPostBack="true" ></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="SumBerat"></asp:Literal>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Berat" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtTotalBerat"
                                Text="<%# Bind('total_berat') %>"></asp:Literal>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="SumTotalBerat"></asp:Literal>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Stone Qty" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtStoneqty"  
                                Text="<%# Bind('stoneqty') %>"> </asp:Literal>                           
                        </ItemTemplate>
                      </asp:TemplateField>

                    <asp:TemplateField HeaderText="Stone Weight" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtStoneweight"  
                                Text="<%# Bind('stoneweight') %>"> </asp:Literal>                           
                        </ItemTemplate>
                      </asp:TemplateField>

                      <asp:TemplateField HeaderText="Qty" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtQty"  Text="1"></asp:Literal>                           
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="SumQty"></asp:Literal>
                        </FooterTemplate>
                    </asp:TemplateField>

             

                     <asp:TemplateField HeaderText="Kadar Tukaran Beli" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtKbeli"  Text="<%# Bind('kbeli') %>"></asp:Literal>                           
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Product Level" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtPlevel"  Text="<%# Bind('plevel') %>"></asp:Literal>                           
                        </ItemTemplate>
                      </asp:TemplateField>

                     <asp:TemplateField HeaderText="Model" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtModel"  Text="<%# Bind('model') %>"></asp:Literal>                           
                        </ItemTemplate>
                      </asp:TemplateField>

                      <asp:TemplateField HeaderText="Product Item" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtPitem"  Text="<%# Bind('pitem') %>"></asp:Literal>                           
                        </ItemTemplate>
                      </asp:TemplateField>

                      <asp:TemplateField HeaderText="Frame Color" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtFrame"  Text="<%# Bind('fcolor') %>"></asp:Literal>                           
                        </ItemTemplate>
                      </asp:TemplateField>

                      <asp:TemplateField HeaderText="Target Age" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtTarget"  Text="<%# Bind('target') %>"></asp:Literal>                           
                        </ItemTemplate>
                      </asp:TemplateField>

                      <asp:TemplateField HeaderText="Kadar Logam" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtKlogam"  Text="<%# Bind('klogam') %>"></asp:Literal>                           
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Harga (24)" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtHarga"
                                Text="<%# Bind('harga') %>"></asp:Literal>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="SumHarga"></asp:Literal>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ongkos" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtOngkos"
                                Text="<%# Bind('ongkos') %>"></asp:Literal>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="SumOngkos"></asp:Literal>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Harga (24)" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtTotalHarga"
                                Text="<%# Bind('total_harga') %>"></asp:Literal>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="SumTotalHarga"></asp:Literal>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Harga Jual" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtTotalHargaJual"
                                Text="<%# Bind('total_hargajual') %>"></asp:Literal>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="SumTotalHargaJual"></asp:Literal>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Berat (24K)" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtTotalBerat24"
                                Text="<%# Bind('total_berat24') %>"></asp:Literal>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="SumTotalBerat24"></asp:Literal>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="HPPM" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtHarga24"
                                Text="<%# Bind('harga24') %>"></asp:Literal>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="SumTotalHarga24"></asp:Literal>
                        </FooterTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="No kirim" Visible="false">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtNokirim"  Text="<%# Bind('nokirim') %>"></asp:Literal>                           
                        </ItemTemplate>
                    </asp:TemplateField>
                   
                     <asp:TemplateField HeaderText="Picture" Visible="false">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtPicture"  Text="<%# Bind('picture') %>"></asp:Literal>                           
                        </ItemTemplate>
                    </asp:TemplateField>

                   <%-- <asp:TemplateField Visible="True">
                        <ItemTemplate>
                            <asp:Button ID="Button1" CommandName="Delete" runat="server" Text="Hapus" CssClass="btnRed" />
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                </Columns>

      

            </asp:GridView>
        </div>
        <%--<div class="control-group spacing-top">

            <div class="controls">
                <i class="icon-plus">
                    <asp:Button runat="server" ID="btn_add_emas" Text="Add" OnClick="btn_add_emas_click" OnClientClick="setValue()" /></i><br />

            </div>
        </div>--%>
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Save
            </button>
            <a href="CZ.aspx" class="btn btn-default">
                <i class="icon-remove"></i>
                Cancel
            </a>
            <span id="dir_ERR" runat="server" class="text-validator" visible="false">Folder Error : </span>
        </div>

        <script>
            function setValue() {
                $('#<%=idskhidden.ClientID%>').val($("#<%= idsk.ClientID %>").val());
            }
            $("#<%=stockkeeper.ClientID%>").autocomplete({
                source: "Search.aspx",
                minLength: 2,
                select: function (event, ui) {
                    $("#<%=idsk.ClientID%>").val(ui.item.id);
                    
                }
            });
        </script>
    </form>
     
</asp:Content>