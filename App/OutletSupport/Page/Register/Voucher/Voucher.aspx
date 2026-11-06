<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Voucher.aspx.cs" Inherits="Page_Register_PG_Form" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Voucher
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form runat="server">

        <table class="table-form">
            <tr>
                <td>Brand</td>
                <td>:</td>
                <td>
                    <asp:DropDownList CssClass="form-control" runat="server" ID="ddlbrand" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>Tanggal Input</td>
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
                <td>Masa Berlaku</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="masaberlaku" runat="server" CssClass="form-control" placeholder="Hari" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>Peruntukan</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="peruntukan" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>Customer</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="customer" runat="server" CssClass="form-control" />
                    <asp:HiddenField ID="idcustomerhidden" runat="server" />
                </td>
                <td></td>
            </tr>
            <tr>
                <td>Jenis Voucher</td>
                <td>:</td>
                <td>
                    <asp:DropDownList CssClass="form-control" runat="server" ID="ddljenis">
                        <asp:ListItem Value="1">Gift Certificate</asp:ListItem>
                        <asp:ListItem Value="2">Discount Card</asp:ListItem>
                        <asp:ListItem Value="3">Birthday Voucher</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>
        </table>

        <p>
            <br />
            <span class="text-bold">Voucher Information</span><br />

        </p>
        <span id="err" runat="server" class="text-danger" />
        <hr />


        <div class="control-group">
            <label class="control-label">&nbsp;</label>

            <asp:GridView ID="EmasGridView" runat="server" ShowFooter="true" class="table table-striped" AutoGenerateColumns="false" OnRowDeleting="EmasGridView_onRowDeleting">
                <Columns>
                    <asp:TemplateField HeaderText="" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txbNumberList" Text="<%#Bind('number') %>"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="No. Voucher" Visible="True">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtNoVoucher" OnTextChanged="checkVoucher" AutoPostBack="true"
                                Width="100" Text="<%# Bind('novoucher') %>"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nominal" Visible="True">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtNominal"
                                Width="100" Text="<%# Bind('nominal') %>"></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Literal runat="server" ID="SumNominal"></asp:Literal>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField Visible="True">
                        <ItemTemplate>
                            <asp:Button ID="Button1" CommandName="Delete" runat="server" Text="Hapus" CssClass="btnRed" />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>
        </div>
        <div class="control-group spacing-top">

            <div class="controls">
                <i class="icon-plus">
                    <asp:Button runat="server" ID="btn_add_emas" Text="Add" OnClick="btn_add_emas_click" /></i><br />

            </div>
        </div>
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Save
            </button>
            <a href="PG.aspx" class="btn btn-default">
                <i class="icon-remove"></i>
                Cancel
            </a>
            <span id="dir_ERR" runat="server" class="text-validator" visible="false">Folder Error : </span>
        </div>
    </form>
    <script>
        $("#<%=customer.ClientID%>").autocomplete({
            source: "Search.aspx",
            minLength: 3,
            select: function (event, ui) {
                $("#<%=idcustomerhidden.ClientID%>").val(ui.item.id);

            }
            });
    </script>
</asp:Content>
