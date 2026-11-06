<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QCDJ_Approve.aspx.cs" Inherits="Page_Report_QCDJ_Approve" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Point of Sales - Frank & co.</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <!-- CSS -->
    <link rel="stylesheet" href="/UI/CSS/bootstrap.css" />
    <link rel="stylesheet" href="/UI/CSS/bootstrap.datepicker.css" />
    <link rel="stylesheet" href="/UI/CSS/bootstrap.font.css" />
    <link rel="stylesheet" href="/UI/CSS/custom.css" />

    <!-- JavaScript -->
    <script type="text/javascript" src="/UI/JS/jquery.js"></script>
    <script type="text/javascript" src="/UI/JS/bootstrap.js"></script>
    <script type="text/javascript" src="/UI/JS/datepicker.js"></script>
    <script type="text/javascript" src="/UI/JS/custom.js"></script>
</head>
<body>
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
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }
    </style>
    <div id="overlay" class="overlay">
        <div class="spinner"></div>
        <div style="margin-top: 10px;">Loading...</div>
    </div>
    <!-- Content -->
    <div class="row">
        <div class="col-xs-12">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 20%; text-align: left;">
                        <img src="/Images/logo_web.jpg" border="0" runat="server" id="img_logo_web" style="height: 50px;" />
                    </td>
                    <td style="width: 60%; text-align: center;">
                        <span style="font-size: 30px;">&nbsp;QC DJ&nbsp;</span><br />
                        <span style="font-size: 20px;">
                            <asp:Literal runat="server" ID="lit_nomor_repair" /></span>
                    </td>
                    <td style="width: 20%; text-align: right;">
                        <asp:Literal ID="cabang" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
            <hr />
        </div>
    </div>
    <form id="Form1" runat="server">
        <div class="modal fade" role="dialog" id="modal-approve" data-backdrop="static">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Warning</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true"></span>
                        </button>
                    </div>
                    <div class="modal-body text-center">
                        <strong>Klik / pilih Approve untuk melanjutkan transaksi.</strong>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnApprove"
                            runat="server"
                            CssClass="btn btn-success"
                            Text="Approve"
                            OnClick="Approve_Click" 
                            OnClientClick="isapprove();" />
                        <button type="button" class="btn" data-dismiss="modal" onclick="iscancelapprove();">Close</button>
                        <script type="text/javascript">
                            function isapprove() {
                                document.getElementById("overlay").style.display = "flex";
                                $('#modal-approve').modal('hide');
                            }

                            function iscancelapprove() {
                                $('#modal-approve').modal('hide');
                            }
                        </script>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" role="dialog" id="modal-reject" data-backdrop="static">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Warning</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true"></span>
                        </button>
                    </div>
                    <div class="modal-body text-center">
                        <strong>Klik / pilih Reject untuk melanjutkan transaksi. Wajib input keterangan jika ingin melakukan Reject.</strong><br />
                        <textarea runat="server" id="keterangan_reject" style="width: 50%"></textarea>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnReject"
                            runat="server"
                            CssClass="btn btn-danger"
                            Text="Reject"
                            OnClick="Rejected_Click"
                            OnClientClick="return isreject();" />
                        <button type="button" class="btn" data-dismiss="modal" onclick="iscancelreject();">Close</button>
                        <script type="text/javascript">
                            function isreject() {
                                let keterangan = document.getElementById('keterangan_reject');

                                if (keterangan && keterangan.value.trim() !== "") {
                                    document.getElementById("overlay").style.display = "flex";
                                } else {
                                    alert('Keterangan Reject wajib diisi.')
                                    return false;
                                }
                            }

                            function iscancelreject() {
                                $('#modal-reject').modal('hide');
                            }
                        </script>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-8">
            <div class="form-group">
                <table>
                    <tr>
                        <td>
                            <p class="text-bold" style="font-size: large">
                                Nomor<p>
                        </td>
                        <td style="padding-left: 10px; padding-right: 10px">
                            <p class="text-bold" style="font-size: large">:</p>
                        </td>
                        <td>
                            <p class="text-bold" style="font-size: large" runat="server" id="nodocqc"></p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p class="text-bold" style="font-size: large">
                                Tanggal QC<p>
                        </td>
                        <td style="padding-left: 10px; padding-right: 10px">
                            <p class="text-bold" style="font-size: large">:</p>
                        </td>
                        <td>
                            <p class="text-bold" style="font-size: large" runat="server" id="tgldocqc"></p>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="col-md-8">
            <div class="panel panel-default">
                <div class="panel-heading">
                    QC Product
                </div>
                <div class="panel-body" style="margin-top: 10px">
                    <table class="table table-bordered table-responsive">
                        <tr>
                            <td class="text-muted" style="width: 30%">Image
                            </td>
                            <td style="width: 40%">
                                <asp:Image ID="imgpicture" runat="server" CssClass="img-thumbnail" Height="100" />
                            </td>
                        </tr>
                        <tr>
                            <td class="text-muted">PLU
                            </td>
                            <td>
                                <label runat="server" id="plu"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="text-muted">Petugas QC</td>
                            <td>
                                <label runat="server" id="petugasqc"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="text-muted">Gross Weight Original</td>
                            <td>
                                <label runat="server" id="grossweightoriginal"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="text-muted">Gross Weight Actual</td>
                            <td>
                                <label runat="server" id="grossweight"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="text-muted">Grafir Logo</td>
                            <td>
                                <label runat="server" id="logo"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="text-muted">Grafir Kadar Emas</td>
                            <td>
                                <label runat="server" id="kadaremas"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="text-muted">Frame Color</td>
                            <td>
                                <label runat="server" id="framecolor"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="text-muted">Fisik Barang Keseluruhan</td>
                            <td>
                                <label runat="server" id="fisik"></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="text-muted">Keterangan</td>
                            <td>
                                <label runat="server" id="keterangan"></label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <br />
        <br />
        <br />
        <div class="col-md-8">
            <div class="panel panel-default">
                <div class="panel-heading">
                    QC Product
                </div>
                <div class="panel-body">
                    <asp:Table runat="server" ID="tb" CssClass="table table-responsive" Style="font-size: small">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell>No.</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="250">Stone</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="90">Tgl.</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="130" CssClass="text-center">Total Butir</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="130" CssClass="text-center">Total Carat</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="130" CssClass="text-center">Total Butir Actual</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="130" CssClass="text-center">Total Carat Actual</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100" CssClass="text-center">Sesuai Sertifikat</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="200" CssClass="text-center">Keterangan</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </div>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-8">
                <div class="col-md-12">
                    <button runat="server" id="approve" class="btn btn-success" onclick="return onclickApprove();">
                        <i class="icon-check"></i>
                        Approve
                    </button>
                    <button runat="server" class="btn btn-danger" id="reject" onclick="return onclickReject();">
                        <i class="icon-remove"></i>
                        Reject
                    </button>
                    <label runat="server" id="lbl_warning" class="text-danger">Only SM / AM Who Can Approve / Reject.</label>
                </div>
            </div>
        </div>
    </form>
    <script>
        function showOverlay() {
            document.getElementById("overlay").style.display = "flex";
        }

        function onclickApprove() {
            //e.preventDefault(); // stop submit
            $('#modal-approve').modal('show');
            return false;
        }

        function onclickReject() {
            //e.preventDefault(); // stop submit
            $('#modal-reject').modal('show');
            return false;
        }
    </script>
</body>
</html>
