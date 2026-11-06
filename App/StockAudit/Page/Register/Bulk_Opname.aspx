<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Bulk_Opname.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Stock Audit
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="row">
                <div class="col-xs-12 col-sm-3 col-md-2">
                    <%--<div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-12">
                            <div class="col-xs-12">
                                <span class="text-muted">Audit ID</span><br />
                                <span class="text-lg" id="audit_id" runat="server" />
                            </div>
                            <div class="col-xs-12">
                                <span class="text-muted">Location</span><br />
                                <span class="text-lg" id="audit_location" runat="server" />
                            </div>
                            <div class="col-xs-12">
                                <span class="text-muted">Total Item</span><br />
                                <span class="text-lg" id="audit_total_item" runat="server" />
                            </div>
                        </div>
                    </div>--%>
                    <div class="form-group">
                        <div class="text-muted">
                            Add PLU (DJ / PG / GJ / LD) :
                        </div>
                        <asp:TextBox ID="kw" CssClass="form-control text-center" runat="server" placeholder="..." />
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Nomor Meja :
                        </div>
                        <asp:TextBox ID="nomeja" CssClass="form-control" runat="server" placeholder="..." />
                    </div>
                    <div>
                        <br />
                        <button id="save" runat="server" class="btn btn-primary" onserverclick="Add">
                            <i class="icon-plus"></i>
                            Add
                        </button>
                    </div>
                    
                    <div>
                        <br />
                        <span id="err" class="text-danger" runat="server" />
                    </div>

                    <div class="form-group">
                        <br />
                        <div class="text-muted">
                            Picture :
                        </div>
                        <asp:Image ID="imgpicture" runat="server" CssClass="img-thumbnail img-responsive" />
                    </div>
                    
                    <hr />
                    <asp:Table ID="tb2" runat="server" CssClass="table-list text-sm" style="width:100%; table-layout:auto;">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell>Item</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="60">Qty</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>

                    <asp:Table ID="tb3" runat="server" CssClass="table-list text-sm" style="width:100%; table-layout:auto;">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell>Meja</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="60">Qty</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                    
                    <div style="clear: both; font-size: 1px;">&nbsp;</div>
                    <div class="form-group">
                        <div>
                            <button id="Button2" runat="server" class="btn btn-default" onclick="if ( ! UserConfirmationClose()) return false;">
                                <i class="icon-remove"></i>
                                Close
                            </button>
                        </div>
                    </div>
                    <div class="form-group">
                        <div>
                            <button id="Button1" runat="server" class="btn btn-danger" onclick="if ( ! UserConfirmation()) return false;" onserverclick="Finish">
                                <span class="glyphicon glyphicon-saved" aria-hidden="true"></span>
                                Finish
                            </button>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-9 col-md-10">
                    <div>
                        <asp:Literal runat="server" ID="lit_countdata" />
                    </div>
                    <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="140">No. PLU<br />Item</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100">Harga</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="50">Gross</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="50">Netto</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="50">Butir</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="50">Carat</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="60">Gambar Tidak Ada</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="60">Gambar Tidak Sesuai</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="60">Bandrol Tidak Sesuai</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100">No. Meja</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100">Keterangan</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="200">Operator</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </div>
            </div>

            
            
            
            
            
        </form>
    </div>
    <script type="text/javascript">
        function UserConfirmationClose() {
            txtConfirm =
				"Tutup Layar Opname ?\n\n" +
				"transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";
            if (confirm(txtConfirm))
                window.close();
            else
                return false;
        }
    </script>
    <script type="text/javascript">
        function UserConfirmation() {
            txtConfirm =
				"Selesaikan Proses Opname ?\n\n" +
				"transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";
            if (confirm(txtConfirm))
                return true;
            else
                return false;
        }
    </script>
</asp:Content>

