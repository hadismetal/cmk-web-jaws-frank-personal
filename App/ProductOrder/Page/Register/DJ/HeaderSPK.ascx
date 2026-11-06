<%@ control language="C#" autoeventwireup="true" codefile="HeaderSPK.ascx.cs" inherits="Page_Register_HeaderSPK" %>
<div class="row">
    <div class="col-xs-12 col-sm-3">
        <p>
            <span class="text-muted">Tipe SPK :</span><br />
            <span class="text-lg" id="tipespk" runat="server" />
        </p>
    </div>
    <div class="col-xs-12 col-sm-3">
        <p>
            <span class="text-muted">Designer :</span><br />
            <span class="text-lg" id="designer" runat="server" />
        </p>
    </div>
    <div class="col-xs-12 col-sm-3" runat="server" id="div_ro_1" visible="false">
        <p>
            <span class="text-muted">Kode Karet Lama :</span><br />
            <span class="text-lg" id="kodekaretlama" runat="server" />
        </p>
    </div>
    <div class="col-xs-12 col-sm-3" runat="server" id="div_ro_2" visible="false">
        <p>
            <span class="text-muted">Kode Karet JAWS :</span><br />
            <span class="text-lg" id="kodekaretjaws" runat="server" />
        </p>
    </div>
    <div class="col-xs-12 col-sm-3" runat="server" id="div_no_1" visible="false">
        <p>
            <span class="text-muted">Referensi :</span><br />
            <span class="text-lg" id="referensinomor" runat="server" />
        </p>
    </div>
    <div class="col-xs-12 col-sm-3" runat="server" id="div_no_2" visible="false">
        <p>
            <span class="text-muted">Tanggal :</span><br />
            <span class="text-lg" id="referensitanggal" runat="server" />
        </p>
    </div>
</div>
<div class="row">
    <div class="col-xs-12">
        <div class="clearfix"></div>
        <hr />
    </div>
</div>