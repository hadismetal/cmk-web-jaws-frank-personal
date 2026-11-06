<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="DJ_Submit.aspx.cs" Inherits="Page_Approval_DJ_PricingSubmit" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewelry
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    QC
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-2">
                <p>
                    <span class="text-muted">PLU :</span><br />
                    <span class="text-lg" id="header_plu" runat="server" />
                </p>
                <p>
                    <span class="text-muted">Item :</span><br />
                    <span class="text-lg" id="header_item" runat="server" />
                </p>
            </div>
            <div class="col-xs-12 col-sm-2">
                <span class="text-muted">Picture :</span><br />
                <asp:Image ID="imgpicture" runat="server" CssClass="img-thumbnail" Height="100" />
            </div>
        </div>
        <hr />
        <div class="row">
            <div class="col-xs-12 col-sm-5">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Petugas QC</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="petugasqc" runat="server" CssClass="form-control input-sm" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Keterangan</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="keterangan" runat="server" CssClass="form-control input-sm" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Blemish untuk > 0,3</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="blemish" runat="server" CssClass="form-control input-sm" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Kecocokan Jumlah Butir</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="jlhbutir" runat="server" CssClass="form-control input-sm" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Kecocokan Sertifikat</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="sertifikat" runat="server" CssClass="form-control input-sm" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Grafir Total Carat - Non CZ</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="noncz" runat="server" CssClass="form-control input-sm" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-5">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Grafir Total Carat - CZ</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="cz" runat="server" CssClass="form-control input-sm" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Grafir Logo</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="logo" runat="server" CssClass="form-control input-sm" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Grafir Kadar Emas</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="kadaremas" runat="server" CssClass="form-control input-sm" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Frame Color</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="framecolor" runat="server" CssClass="form-control input-sm" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Grafir Merk</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="merk" runat="server" CssClass="form-control input-sm" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-sm-5 control-label">Fisik Barang Keseluruhan</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="fisik" runat="server" CssClass="form-control input-sm" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            frame
        </div>
        <table class="table-form">
            <tr>
                <td>Gross Weight</td>
                <td>&nbsp;&nbsp;</td>
                <td>
                    <div class="input-group">
						<asp:textbox id="grossweightoriginal" runat="server" cssclass="form-control text-lg text-center" width="125" Enabled="false" />
						<span class="input-group-addon">Gram</span>
					</div>
                </td>
                <td><span class="glyphicon glyphicon-arrow-right" aria-hidden="true"></span></td>
                <td>
                    <div class="input-group">
						<asp:textbox id="grossweight" runat="server" cssclass="form-control text-lg text-center" width="125" />
						<span class="input-group-addon">Gram</span>
					</div>
                </td>
            </tr>
        </table>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            stone
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-report">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Stone</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tgl.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-center">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-center">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200" CssClass="text-center">Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

        <hr />

        <div class="button-left">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-check"></i>
                Submit
            </button>
        </div>
    </form>
</asp:Content>

