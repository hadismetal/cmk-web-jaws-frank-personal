<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="DJCrossBrand_Print_Sertifikat.aspx.cs" Inherits="DJ_Print" %>

<div runat="server" id="div_Error"><asp:Literal runat="server" id="lit_ErrMsg" /></div>
<div runat="server" id="div_Print">
<div>
    <div>
        <h1>Bukti Kirim Sertifikat</h1>
        <h2>Lampiran Sertifikat</h2>
    </div>
    <div>
        <table class="table-file">
            <tr>
                <td class="text-label">No. Dokumen</td>
                <td>:</td>
                <td id="nodokumen" runat="server" class="text-bold" />
            </tr>
            <tr>
                <td class="text-label">Tanggal Kirim</td>
                <td>:</td>
                <td id="tanggalkirim" runat="server" class="text-bold" />
            </tr>
            <tr>
                <td class="text-label">Asal</td>
                <td>:</td>
                <td id="asal" runat="server" class="text-bold" />
            </tr>
            <tr>
                <td class="text-label">Tujuan</td>
                <td>:</td>
                <td id="tujuan" runat="server" class="text-bold" />
            </tr>
        </table>
    </div>
    <div class="table-responsive">
        <asp:table id="tb" runat="server" cssclass="table-report" style="border:solid 1px black; border-collapse:collapse;">
			<asp:tableheaderrow>
				<asp:tableheadercell style="text-align:left; vertical-align:top;" width="40">No.</asp:tableheadercell>
                <asp:tableheadercell style="text-align:left; vertical-align:top;" width="140">No. Sertifikat</asp:tableheadercell>
                <asp:tableheadercell style="text-align:left; vertical-align:top;" width="120">No. PLU</asp:tableheadercell>
                <asp:tableheadercell style="text-align:left; vertical-align:top;" width="120">Product Item</asp:tableheadercell>
            </asp:tableheaderrow>
		</asp:table>
    </div>
    <div>
        <div>
            
        </div>
        <div>
            <div style="float:right;">
                <br />
                <table style="border:solid 1px black; border-collapse:collapse;">
                    <tr>
                        <td style="width:150px;text-align:center;border:solid 1px black; ">Pembuat<br /><br /><br /><br /><br /></td>
                        <td style="width:150px;text-align:center;border:solid 1px black; ">Menyerahkan<br /><br /><br /><br /><br /></td>
                        <td style="width:150px;text-align:center;border:solid 1px black; ">Penerima<br /><br /><br /><br /><br /></td>
                    </tr>
                </table>
            </div>
            <div style="clear:both" />
        </div>
    </div>
</div>
<style type="text/css">

table { page-break-inside:avoid; page-break-after:auto }

@media print
{
table { page-break-inside:avoid; page-break-after:auto }
}
</style>
</div>

