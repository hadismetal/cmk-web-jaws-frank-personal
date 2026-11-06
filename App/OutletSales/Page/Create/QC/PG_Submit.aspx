<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PG_Submit.aspx.cs" Inherits="Page_Create_PG_PricingSubmit" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Plain Gold
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    QC
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
	        .overlay {
			    position: fixed;
			    top: 0;
			    left: 0;
			    width: 100%;
			    height: 100%;
			    background: rgba(0,0,0,0.5);
			    z-index: 9999;
			    display: none;
			    align-items: center;
			    justify-content: center;
			    color: white;
			    font-size: 24px;
		    }

		    .spinner {
			    border: 6px solid #f3f3f3;
			    border-top: 6px solid #3498db;
			    border-radius: 50%;
			    width: 50px;
			    height: 50px;
			    animation: spin 1s linear infinite;
		    }

		    @keyframes spin {
			    0% { transform: rotate(0deg); }
			    100% { transform: rotate(360deg); }
		    }
    </style>
    <div id="overlay" class="overlay">
	    <div class="spinner"></div>
	    <div style="margin-top: 10px;">Loading...</div>
    </div>
    <form id="Form1" runat="server">
        <div class="panel panel-default width-500">
            <div class="panel-heading">
                Information PLU
            </div>
            <div class="panel-body">
                <table class="table-file">
                    <tr>
                        <td class="text-label">PLU</td>
                        <td>:</td>
                        <td class="text-bold text-right fixed-150" id="header_nomor" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Netto Weight</td>
                        <td>:</td>
                        <td class="text-bold text-right fixed-150" id="netto" runat="server" />
                        <td><small class="text-silver">Gram</small></td>
                    </tr>
                    <tr>
                        <td class="text-label">Kadar Logam</td>
                        <td>:</td>
                        <td class="text-bold text-right fixed-150" id="kadar" runat="server" />
                        <td><small class="text-silver">%</small></td>
                    </tr>
                    <tr>
                        <td class="text-label">Frame Color</td>
                        <td>:</td>
                        <td class="text-bold text-right fixed-150" id="frame" runat="server" />
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="panel panel-default width-500">
            <div class="panel-heading">
                QC
            </div>
            <div class="panel-body">
                <table class="table-file">
                    <!--<tr>
						<td class="text-muted">Petugas QC</td>
						<td>
							<div class="input-group">
								<asp:TextBox ID="petugasqcsearch" runat="server" CssClass="form-control" placeholder="Isi Dengan NIK" />
								<div class="input-group-btn">
									<asp:Button ID="btnsearchnik" runat="server" CssClass="btn btn-info" Text="Search" OnClick="btnSearchNIK_Click" />
								</div>
							</div>
						</td>
					</tr>-->
                    <tr>
                        <td class="text-muted">Petugas QC</td>
                        <td>
                            <asp:TextBox ID="petugasqc" runat="server" CssClass="form-control" Enabled="false" />
                            <small runat="server" id="lblerrorpetugasqc" class="text-danger" visible="false"></small>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Tipe Lokasi QC</td>
                        <td>
                            <asp:DropDownList runat="server" ID="tipe_lokasi" CssClass="form-control" OnSelectedIndexChanged="Location_Changed" AutoPostBack="true">
                            </asp:DropDownList>
                            <small runat="server" id="lblerrortipelokasiqc" class="text-danger" visible="false"></small>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Lokasi QC</td>
                        <td>
                            <asp:DropDownList runat="server" ID="lokasi" CssClass="form-control" >
                            </asp:DropDownList>
                            <small runat="server" id="lblerroridlokasi" class="text-danger" visible="false"></small>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Grafir Logo</td>
                        <td>
                            <asp:DropDownList ID="logo" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                            <%--<asp:TextBox ID="logo" runat="server" CssClass="form-control" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Grafir Kadar Emas</td>
                        <td>
                            <asp:DropDownList ID="kadaremas" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                            <%--<asp:TextBox ID="kadaremas" runat="server" CssClass="form-control" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Fisik Barang Keseluruhan</td>
                        <td>
                            <asp:DropDownList ID="fisik" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Net Weight Actual</td>
                        <td>
                            <div class="input-group">
                                <asp:TextBox ID="netweight" runat="server" CssClass="form-control" ClientIDMode="Static" onblur="onValidateNetWeight(this);" />
                                <span class="input-group-addon">Gram</span>
                                <asp:HiddenField ID="netweighthidden" runat="server" />
                            </div>
                            <small class="text-info">*penulisan menggunakan koma tanpa titik (e.g. 1,15 atau 1002,25) </small>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Kadar by Karatimeter</td>
                        <td>
                            <asp:RadioButtonList runat="server" ID="kadarkaratimeter" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Sesuai</asp:ListItem>
                                <asp:ListItem Value="2">Tidak Sesuai</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Frame Color</td>
                        <td>
                            <asp:DropDownList ID="framecolor" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                            <%--<asp:TextBox ID="framecolor" runat="server" CssClass="form-control" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Keterangan</td>
                        <td>
                            <asp:TextBox ID="keterangan" runat="server" TextMode="MultiLine" CssClass="form-control" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="panel-footer">
                <div class="button-left">
                    <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click" onclick="showOverlay();">
                        <i class="icon-check"></i>
                        Submit
                    </button>
                    <small runat="server" id="lblwarning" class="text-danger" visible="false"></small>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        document.addEventListener("DOMContentLoaded", function () {
            let valori = document.getElementById('content_netweighthidden').value;
            document.getElementById('netweight').value = valori;
        });

        function onValidateNetWeight(e) {
            const val = e.value;
            const valori = document.getElementById('content_netweighthidden').value;
            
            if (val.includes('.')) {
                alert("Net Weight Actual Tidak Valid");
                e.value = "";
                e.value = valori;
            }
            else if (val === "") {
                alert("Net Weight Actual Tidak Valid");
                e.value = valori;
            }
            else if (val.toLowerCase().includes('e')) {
                alert("Net Weight Actual Tidak Valid");
                e.value = valori;
            }
            else if (val.includes('-')) {
                alert("Net Weight Actual Tidak Valid");
                e.value = valori;
            }
            else {
                const valorant = val.replace(',','.')
                if (isNaN(valorant)) {
                    alert("Net Weight Actual Tidak Valid");
                    e.value = valori;
                }
                else {
                    let num = parseFloat(valorant);

					if (num <= 0) {
						alert("Net Weight Actual Harus Lebih dari 0 ");
						e.value = valori;
					} else {
						e.value = num.toString().replace('.', ',');
						document.getElementById('content_save').removeAttribute('disabled');
					}
                }
            }
        }

        function showOverlay() {
            document.getElementById("overlay").style.display = "flex";
        }
    </script>
</asp:Content>

