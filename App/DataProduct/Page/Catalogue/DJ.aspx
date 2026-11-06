<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DJ.aspx.cs" inherits="Page_DJ" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewellery
</asp:content>
<asp:Content ID="Content1" ContentPlaceHolderID="h1_st" runat="server">
    Information
</asp:Content>

<asp:content contentplaceholderid="content" runat="server">
    <script type = "text/javascript">
        function functionx(evt) {
            if (evt.charCode > 31 && (evt.charCode < 48 || evt.charCode > 57)) {
                alert("Allow Only Numbers");
                return false;
            }
        }
     </script>
    
    <div id="zero" runat="server" visible="false">
        <i class="icon-ok icon-5x pull-left text-success"></i>
        <h4>Proses Selesai</h4>
        <p class="text-muted">
            Semua data / transaksi dalam kondisi sudah menjadi SPK.
        </p>
        <div class="clearfix"></div>
        <hr />
    </div>
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            
            <div class="form-group">
                <div class="text-muted">
                    Search (PLU) :                    
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="200" placeholder="Input Keyword..." />
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
            <div class="form-group">
                <asp:Literal ID="err" runat="server" />                
            </div>
        </form>
        <div>
            <br />
            <asp:Literal runat="server" ID="lit_datacount" />
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="50" />
                    <asp:TableHeaderCell Width="50" />
                    <asp:TableHeaderCell Width="150">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Level</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

