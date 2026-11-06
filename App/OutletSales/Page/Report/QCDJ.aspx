<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="QCDJ.aspx.cs" Inherits="Page_Report_QCDJ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="server">
    QC
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server" class="form-inline">
        <div class="table-responsive">
            <h3>Document QC</h3>
            <div class="form-group">
                <div class="text-muted">
                    Search :
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
            </div>
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
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="150" />
                        <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">No. Dokumen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Tanggal</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Petugas QC</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="220">Status</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="250">Keterangan Reject</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </div>
    </form>
</asp:Content>

