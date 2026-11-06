<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AM.aspx.cs" Inherits="Page_Create_AdminSalesAccount" %>

<%@ Register Src="MenuAM.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Area Manager
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Create New
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <hr />
    <form runat="server">
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group width-300">
                    <label>
                        Username :
                    </label>
                    <asp:DropDownList ID="username" runat="server" CssClass="form-control text-bold">
                        <asp:ListItem Value="0">Username</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group width-300">
                    <label>
                        Nama (Area) :
                    </label>
                    <asp:RequiredFieldValidator ID="nama_RF" runat="server" />
                    <asp:TextBox ID="nama" runat="server" CssClass="form-control text-bold" />
                </div>
                <div class="form-group width-300">
                    <label>
                        Tanggal :
                    </label>
                    <asp:TextBox ID="tgl" runat="server" CssClass="form-control text-center" Width="110" />
                    <script>
                        $(document).ready(function () { $('#content_tgl').datepicker({ format: 'dd-mm-yyyy' }); });
                    </script>
                    <asp:RequiredFieldValidator ID="tgl_RF" runat="server" />
                    <span id="tgl_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </div>
                
                
            </div>
            <div class="col-sm-4">
                <div class="form-group">
                    <label>
                        Keterangan :
                    </label>
                    <asp:TextBox ID="keterangan" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col-xs-12">
                        <div class="form-group">
                            <label>
                                Location (Search) :
                            </label>
                            <asp:TextBox ID="outlet" runat="server" CssClass="form-control" />
                        </div>

                    </div>
                    <div class="col-xs-12">
                        <select name="outletlist" id="outletlist" class="form-control" size="10" multiple="multiple"></select>
                        <a id="remove" style="cursor: pointer;">Remove</a>
                    </div>
                </div>
            </div>
        </div>
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
            <a href="AdminSales_LocationType.aspx" class="btn btn-default">
                <i class="icon-remove"></i>
                Cancel
            </a>
        </div>
    </form>
    <script>
        $('#remove').click(function () {
            $("#outletlist option:selected").remove();
            $("#outletlist option").prop("selected", true);
        });
        $("#<%=outlet.ClientID%>").autocomplete({
            source: "Search.aspx",
            minLength: 2,
            select: function (event, ui) {
                var item = ui.item.id;
                var label = ui.item.label;
                $('#outletlist').append('<option value="' + item + '">' + label + '</option>');
                $("#outletlist option").prop("selected", true);
            }
        });
    </script>
</asp:Content>

