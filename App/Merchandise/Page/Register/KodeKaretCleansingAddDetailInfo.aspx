<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="KodeKaretCleansingAddDetailInfo.aspx.cs" Inherits="KodeKaretCleansingAddDetailInfo" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Kode Karet Cleansing
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <form id="Form1" runat="server" class="form-inline">
        <div class="row">
            <div class="col-xs-12 col-sm-3">
                <div class="form-group">

                    <label>
                        Kode Karet JAWS :
                    </label>
                    <div class="input-group input-group-md">
                        <asp:TextBox ID="txt_kodekaretjaws" runat="server" CssClass="form-control" Style="text-transform: uppercase;" disabled="disabled" />
                        <span class="input-group-addon" runat="server" id="span_jaws"></span>
                    </div>
                </div>

                <div class="form-group">
                    <label>
                        Kode Karet (Recommendation) :
                    </label>

                    <div class="input-group input-group-md">
                        <asp:TextBox ID="txt_kodekaretrecommendation" runat="server" CssClass="form-control" disabled="disabled" />
                        <span class="input-group-addon" runat="server" id="span_recommendation"></span>
                        <span class="input-group-btn">
                            <button id="btnCopy" runat="server" class="btn btn-primary" onserverclick="btnCopy_Click">
                                <i class="icon-copy"></i>
                            </button>
                        </span>
                    </div>
                </div>

                <div class="form-group">
                    <label>
                        Kode Karet (Revision) :
                    </label>
                    <div class="input-group input-group-md">
                        <asp:TextBox ID="txt_kodekaretrevisi" runat="server" CssClass="form-control text-bold text-blue" style="text-transform:uppercase;" MaxLength="7" />
                        <span class="input-group-addon" runat="server" id="span_revision"></span>
                    </div>

                </div>

                <div runat="server" id="err" visible="false">
                    <div class="clearfix">
                        <br class="hidden-xs" />
                    </div>
                    <div class="alert alert-xs alert-warning text-center" role="alert">
                        <asp:Literal runat="server" ID="lit_err" /></div>
                </div>

            </div>
            <div class="col-xs-12 col-sm-3">

                <div class="form-group">
                    <label>
                        Picture (Latest BRJ) :
                    </label>
                    <br />
                    <asp:Literal runat="server" ID="lit_img_brj" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-3">

                <div class="form-group">
                    <label>
                        Picture (Latest SPK) :
                    </label>
                    <br />
                    <asp:Literal runat="server" ID="lit_img_spk" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-3">
                <div class="alert alert-info">
                    Notes :
                    <ul>
                        <asp:Literal runat="server" ID="lit_rule_1" />
                        <li><i>untuk membatalkan revisi kode karet silahkan kosongkan kolom isian revisi kode karet</i></li>
                    </ul>

                </div>
            </div>
        </div>

        <div class="button-inline">
            <button id="cancel" runat="server" class="btn btn-default" onserverclick="cancel_Click">
                <i class="icon-arrow-left"></i>
                Cancel
            </button>
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-ok"></i>
                Save
            </button>
        </div>
    </form>
    <script type="text/javascript">
        var x = document.getElementById('content_txt_kodekaretrevisi');
        var y = document.getElementById('content_span_revision');

        x.oninput = function () {
            this.onkeydown = null;
            // Your code goes here
            y.textContent = x.value.length;
        };
        x.onkeydown = function () {
            // Your code goes here
            y.textContent = x.value.length;
        };
    </script>
</asp:Content>

