<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProductStoneRequest.aspx.cs" Inherits="Page_Register_DJ_Form" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Product Stone Request
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <script>
        $(document).ready(function () {
            $('#<%=datepicker.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
        });
    </script>
    <form runat="server">
        <div class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="dari" runat="server" CssClass="form-control text-center" Width="110" placeholder="Dari" />
                <script>
                    $(document).ready(function () { $('#<%=dari.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>

            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="sampai" runat="server" CssClass="form-control text-center" Width="110" placeholder="Sampai" />
                <script>
                    $(document).ready(function () { $('#<%=sampai.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>

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
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
        </div>
        <div class="form-group width-500">
            <label>
                Brand :
            </label>
            <asp:DropDownList CssClass="form-control" runat="server" ID="ddl_CompanyBrand" AutoPostBack="true" OnSelectedIndexChanged="Brand_Click" />
        </div>
        <div class="form-group width-500">
            <label>
                Tanggal Jatuh Tempo :
            </label>
            <asp:TextBox CssClass="form-control" placeholder="Tanggal" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" />

            <asp:RequiredFieldValidator ID="datepicker_RF" runat="server" />
            <span id="datepicker_ERR" runat="server" class="text-validator" visible="false">Format</span>
        </div>
        <div class="form-group width-500">
            <label>
                Keterangan :
            </label>
            <asp:TextBox TextMode="MultiLine" runat="server" ID="txt_keterangan" CssClass="form-control" />
        </div>
        <div class="control-group">
            <label class="control-label">&nbsp;</label>
            <asp:GridView ID="SPKGridView" runat="server" class="table table-striped" AutoGenerateColumns="false" OnRowDeleting="SPKGridView_onRowDeleting">
                <Columns>
                    <asp:TemplateField HeaderText="No." Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txbNumberList" Text="<%#Bind('number') %>"></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Kode Parcel" Visible="True">
                        <ItemTemplate>
                            <asp:Literal runat="server" ID="txtNoSPk"
                                Text="<%# Bind('nospk') %>"></asp:Literal>
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

        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Save
            </button>
            <a href="DJ.aspx" class="btn btn-default">
                <i class="icon-remove"></i>
                Cancel
            </a>
        </div>
    </form>
</asp:Content>
