<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_CrossBrand.aspx.cs" Inherits="DJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewelry
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Report
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <form runat="server" class="form-inline">

        <table class="table-form">
            <tr>
                <td>Periode</td>
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
                <td>s/d&nbsp;&nbsp;</td>
                <td>
                    <asp:TextBox ID="tgl2" runat="server" CssClass="form-control text-center" Width="110" />
                    <script>
                        $(document).ready(function () { $('#content_tgl2').datepicker({ format: 'dd-mm-yyyy' }); });
                    </script>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="tgl2_RF" runat="server" />
                    <span id="tgl2_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
                <td>
                    <asp:TextBox ID="kw" runat="server" CssClass="form-control text-center" Width="300" placeholder="No. Dokumen" />
                </td>
            </tr>
        </table>
        <div class="form-group">
            <div class="text-muted">
                Company Brand From:
            </div>
            <asp:DropDownList ID="brandFrom" runat="server" CssClass="form-control" Width="200">
                <asp:ListItem Value="-1">All</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
               Location Type (From)
            </div>
            <asp:DropDownList ID="tipelokasifrom" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasi" AutoPostBack="true">
                
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Location (From)
            </div>
            <asp:DropDownList ID="lokasifrom" runat="server" CssClass="form-control" Width="175">
                
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Company Brand From:
            </div>
            <asp:DropDownList ID="brandTo" runat="server" CssClass="form-control" Width="200" OnSelectedIndexChanged="ddl_to_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="-1">All</asp:ListItem>
                <asp:ListItem Value="7">FRANK & CO (F)</asp:ListItem>
                <asp:ListItem Value="3">MONDIAL (M)</asp:ListItem>
                <asp:ListItem Value="4">MISS MONDIAL (S)</asp:ListItem>
                <asp:ListItem Value="5">THE PALACE (P)</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
               Location Type (To)
            </div>
            <asp:DropDownList ID="tipelokasito" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="ddl_to_SelectedIndexChanged" AutoPostBack="true">
                
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Location (To)
            </div>
            <asp:DropDownList ID="lokasito" runat="server" CssClass="form-control" Width="175">
                
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Status Outgoing :
            </div>
            <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="200">
                <asp:ListItem Value="-1">All</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                    <i class="icon-search"></i>
                    Display
                </button>
                <button id="Button2" runat="server" class="btn btn-primary" onserverclick="Export">
                    <i class="icon-search"></i>
                    Export
                </button>
                <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Print">
                    <i class="icon-search"></i>
                    Print
                </button>
            </div>
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                
            </asp:Table>
        </div>
    </form>
</asp:Content>

