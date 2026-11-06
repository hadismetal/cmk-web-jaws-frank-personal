<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Resell_ItemAddPLUPG.aspx.cs" Inherits="Page_Register_DJ_CharProduct01" %>

<%@ Register Src="MenuDJ.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Plain Gold
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <form runat="server">
        <div class="row">
            <div class="col-md-6">
                <table class="table-form">
                    <tr>
                        <td>Tanggal</td>
                        <td>:</td>
                        <td colspan ="2">
                            <asp:TextBox ID="tgl" runat="server" CssClass="form-control text-center" Width="140" />
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
                        <td>Cabang Asal</td>
                        <td>:</td>
                        <td id="Td2" runat="server">
                            <asp:DropDownList ID="tipelokasifrom" runat="server" CssClass="form-control" Width="140" OnSelectedIndexChanged="FillLokasi" AutoPostBack="true">
                                <asp:ListItem Value="-1">Tipe Lokasi</asp:ListItem>
                                <asp:ListItem Value="1">Outlet</asp:ListItem>
                                <asp:ListItem Value="2">Exhibition</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="lokasifrom" runat="server" CssClass="form-control" Width="150">
                                <asp:ListItem Value="0">Lokasi</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>PLU</td>
                        <td>:</td>
                        <td colspan ="2">
                            <div class="input-group">
                                <asp:TextBox ID="plu" runat="server" CssClass="form-control text-lg text-center" Width="300" />
                            </div>
                        </td>
                        <td>
                            <span id="err_plu" runat="server" />
                            <asp:RequiredFieldValidator ID="plu_RF" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Harga</td>
                        <td>:</td>
                        <td colspan ="2">
                            <div class="input-group">
                                <asp:TextBox ID="harga" runat="server" CssClass="form-control text-lg text-center" Width="300" Text="0" />
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="form-group width-500">
                    <label>
                        Keterangan :
                    </label>
                    <asp:TextBox ID="keterangan" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                </div>

                <p>
                    <br />
                    <span class="text-bold">Product Information</span>
                    
                    <br />
                </p>
                <div class ="pull-left" style="color:red;font-style:italic;font-size:medium"> 
                    Perhatian! Tolong jangan lupa untuk menginput Product Item dengan benar

                </div>
                   
                <hr />
                <table class="table-form width-800">
                    <tr>
                        <td width="400">
                            <div class="block spacing-top">


                                <div class="control-group">
                                    <label class="control-label">Product Level</label>
                                    <div class="controls">
                                        <asp:DropDownList CssClass="form-control" runat="server" ID="ddl_ProductLevel" />
                                    </div>
                                </div>

                                <div class="control-group">
                                    <label class="control-label">Product Item</label>
                                    <div class="controls">
                                        <asp:DropDownList CssClass="form-control" runat="server" ID="ddl_ProductItem" />
                                    </div>
                                </div>


                                <div class="control-group">
                                    <label class="control-label">Kelompok Usia</label>
                                    <div class="controls">
                                        <asp:DropDownList CssClass="form-control" runat="server" ID="ddl_KelompokUsia" />
                                    </div>
                                </div>

                            </div>
                        </td>
                        <td width="400">
                            <div class="block spacing-top">
                                <div class="control-group">
                                    <label class="control-label">Model</label>
                                    <div class="controls">
                                        <asp:DropDownList CssClass="form-control" runat="server" ID="ddl_Model" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Warna Logam</label>
                                    <div class="controls">
                                        <asp:DropDownList CssClass="form-control" runat="server" ID="ddl_RangkaWarna" />
                                    </div>
                                </div>
                                <div class="control-group">
                                    <label class="control-label">Kadar logam</label>
                                    <div class="controls">
                                        <asp:TextBox AutoPostBack="true" CssClass="form-control" Text="75" placeholder="Kadar Logam" ID="kadar_logam" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <p>
                    <br />
                    <span class="text-bold">Gold Information</span>

                </p>
                <div class ="pull-left" style="color:red;font-style:italic;font-size:medium"> 
					*penulisan menggunakan koma tanpa titik (e.g. 1,15 atau 1002,25)
                </div>
                <hr />
                <div class="control-group">
                    <label class="control-label">&nbsp;</label>

                    <asp:GridView ID="EmasGridView" runat="server" ShowFooter="true" class="table table-striped" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="" Visible="True">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txbNumberList" Text="<%#Bind('number') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" Visible="False">
                                <ItemTemplate>
                                    <asp:Literal runat="server" ID="txtID" Text="<%#Bind('id') %>"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="No. Certificate" Visible="True">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtNoCert"
                                        Width="100" Text="<%# Bind('nocert') %>"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Berat" Visible="True">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtBerat"
                                        Width="100" Text="<%# Bind('berat') %>"></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Literal runat="server" ID="SumBerat"></asp:Literal>
                                </FooterTemplate>
                            </asp:TemplateField>

                        </Columns>

                    </asp:GridView>
                </div>

                <div class="button-inline">
                    <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                        <i class="icon-arrow-right"></i>
                        Save
                    </button>
                    <a href="PG.aspx" class="btn btn-danger">
                        <i class="icon-remove"></i>
                        Cancel
                    </a>
                    <span id="dir_ERR" runat="server" class="text-validator" visible="false">Folder Error : </span>
                </div>
            </div>
        </div>
    </form>
</asp:Content>

