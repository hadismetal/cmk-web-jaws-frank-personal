<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJLD.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery & Loose Diamond Combination List
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="Input PLU" />
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Characteristic Product :
                </div>
                <asp:DropDownList ID="productlevel" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Level</asp:ListItem>
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
                <asp:DropDownList ID="productitem" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Item</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
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
        </form>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="120"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">No. PLU DJ</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">No. PLU LD</asp:TableHeaderCell>
                    
                    <asp:TableHeaderCell Width="150">Lokasi</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>

            
    <div style="display: none;" id="startROW" ClientIDMode="Static" runat="server">1</div> 
    
    <div class="btn-group" role="group" id="paginationGRP" runat="server" ClientIDMode="Static">
                
    </div>
        </div>
        <script type="text/javascript">
            function goDelete(id) {
                txtConfirm =
                    "Lanjutkan Proses DELETE ?\n\n" +
                    "Penghapusan data / transaksi sifatnya adalah permanen.\n" +
                    "Dianjurkan untuk melakukan backup terlebih dahulu.";

                if (confirm(txtConfirm))
                    location.href = 'DJLD_Delete.aspx?id=' + id;
            }
	</script>
    </div>
</asp:Content>

