<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="AMUpdate.aspx.cs" Inherits="Page_Account_AMUpdate" %>

<%@ Register Src="MenuAM.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Area Manager
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    <asp:Literal ID="h1_st" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_id" runat="server">
    ID:<asp:Literal ID="h1_id" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <form runat="server">
        <div class="row">
            <div class="col-sm-4">
                <div class="form-group">
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
                <div class="form-group">
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
        <div class="row">
            <div class="col-xs-12">
                
                <hr />
                <div class="form-group cb-single">
                    <div id="div_draft" runat="server">
                        <label for="content_disable">
                            <strong class="text-draft">Status : Draft</strong>
                            <small>Data tidak dimunculkan di dalam sistem</small>
                        </label>
                    </div>
                    <div id="div_disable" runat="server">
                        <asp:CheckBox ID="disable" runat="server" />
                        <label for="content_disable">
                            <strong class="text-warning">Status : Disable</strong>
                            <small>Data tidak dimunculkan di dalam sistem</small>
                        </label>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-ok"></i>
                Save
            </button>
            <asp:Button ID="del" runat="server" CssClass="btn btn-danger" Text="Delete" Width="80" OnClick="del_Click" />
            <span id="nodel" runat="server" class="text-validator" visible="false">Disable Delete : File Sudah Digunakan</span>
        </div>
        <asp:Literal ID="scripting" runat="server" />
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
