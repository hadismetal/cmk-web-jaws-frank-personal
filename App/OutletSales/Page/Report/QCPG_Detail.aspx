<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QCPG_Detail.aspx.cs" Inherits="Page_Report_QCPG_Detail" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>QC PG Report</title>

    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <link rel="stylesheet" href="/UI/CSS/bootstrap.css" />

    <script src="/UI/JS/jquery.js"></script>
    <script src="/UI/JS/bootstrap.js"></script>

    <style>
        body {
            font-family: Arial;
            font-size: 12px;
            background: #f5f5f5;
        }

        /* REPORT CONTAINER */

        .report-container {
            max-width: 900px;
            margin: auto;
            background: white;
            padding: 20px;
        }

        /* HEADER */

        .report-header {
            display: flex;
            align-items: center;
        }

        .report-logo {
            width: 33%;
        }

        .report-title {
            width: 34%;
            text-align: center;
        }

        .report-branch {
            width: 33%;
            text-align: right;
        }

        .report-title h2 {
            margin: 0;
            font-weight: bold;
        }

        .report-title h4 {
            margin: 0;
        }

        /* SECTION */

        .section-title {
            font-weight: bold;
            margin-top: 15px;
            margin-bottom: 5px;
        }

        /* PRODUCT */

        .product-section {
            display: flex;
            gap: 20px;
        }

        .product-image {
            width: 200px;
        }

            .product-image img {
                max-height: 160px;
            }

        /* TABLE */

        .table-compact td {
            padding: 4px !important;
        }

        /* SIGNATURE */

        .signature {
            margin-top: 30px;
            display: flex;
            justify-content: flex-end;
            gap: 80px;
        }

        .signature-box {
            text-align: center;
        }

        /* PRINT */

        @media print {

            body {
                background: white;
            }

            #btn-print {
                display: none;
            }

            .report-container {
                width: 100%;
                max-width: 100%;
                padding: 10px;
            }

            .signature {
                position: absolute;
                right: 40px;
                bottom: 40px;
            }

            table {
                page-break-inside: avoid;
            }

            .panel {
                page-break-inside: avoid;
            }
        }
    </style>

</head>

<body>

    <div class="report-container">

        <form id="Form1" runat="server">

            <!-- HEADER -->

            <div class="report-header">

                <div class="report-logo">
                    <img src="/Images/logo_web.jpg" runat="server" id="img_logo_web" style="height: 45px" />
                </div>

                <div class="report-title">
                    <h2>QC PG</h2>
                    <h4>
                        <asp:Literal runat="server" ID="lit_nomor_repair" /></h4>
                </div>

                <div class="report-branch">
                    <asp:Literal ID="cabang" runat="server"></asp:Literal>
                </div>

            </div>

            <hr />


            <!-- DOC INFO -->

            <table class="table table-bordered table-compact" style="width: 350px">

                <tr>
                    <td width="120"><b>Nomor</b></td>
                    <td width="10">:</td>
                    <td><span runat="server" id="nodocqc"></span></td>
                </tr>

                <tr>
                    <td><b>Tanggal QC</b></td>
                    <td>:</td>
                    <td><span runat="server" id="tgldocqc"></span></td>
                </tr>

            </table>



            <!-- QC PRODUCT -->

            <div class="section-title">
                QC PRODUCT
            </div>

            <div class="product-section">

                <div class="product-image">

                    <asp:Image
                        ID="imgpicture"
                        runat="server"
                        CssClass="img-thumbnail" />

                </div>

                <div style="flex: 1">

                    <table class="table table-bordered table-compact">

                        <tr>
                            <td width="220">PLU</td>
                            <td>
                                <label runat="server" id="plu"></label>
                            </td>
                        </tr>

                        <tr>
                            <td>Netto Weight</td>
                            <td>
                                <label runat="server" id="netto"></label>
                            </td>
                        </tr>

                        <tr>
                            <td>Kadar Logam</td>
                            <td>
                                <label runat="server" id="kadar"></label>
                            </td>
                        </tr>

                        <tr>
                            <td>Frame Color</td>
                            <td>
                                <label runat="server" id="frame"></label>
                            </td>
                        </tr>

                        <tr>
                            <td>Petugas QC</td>
                            <td>
                                <label runat="server" id="petugasqc"></label>
                            </td>
                        </tr>

                        <tr>
                            <td>Grafir Logo</td>
                            <td>
                                <label runat="server" id="logo"></label>
                            </td>
                        </tr>

                        <tr>
                            <td>Grafir Kadar Emas</td>
                            <td>
                                <label runat="server" id="kadaremas"></label>
                            </td>
                        </tr>

                        <tr>
                            <td>Fisik Barang Keseluruhan</td>
                            <td>
                                <label runat="server" id="fisik"></label>
                            </td>
                        </tr>

                        <tr>
                            <td>Net Weight Actual</td>
                            <td>
                                <label runat="server" id="netweight"></label>
                            </td>
                        </tr>

                        <tr>
                            <td>Kadar by Karatimeter</td>
                            <td>
                                <label runat="server" id="kadarkaratimeter"></label>
                            </td>
                        </tr>

                        <tr>
                            <td>Frame Color Actual</td>
                            <td>
                                <label runat="server" id="framecoloractual"></label>
                            </td>
                        </tr>

                        <tr>
                            <td>Keterangan</td>
                            <td>
                                <label runat="server" id="keterangan"></label>
                            </td>
                        </tr>

                    </table>

                </div>

            </div>



            <!-- PRINT BUTTON -->

            <br />

            <button
                type="button"
                id="btn-print"
                onclick="onPrint()"
                class="btn btn-success">
                Print

            </button>



            <!-- SIGNATURE -->

            <div class="signature" id="div_ttd" style="display: none">

                <div class="signature-box">

                    <b>Check By</b>

                    <br>
                    <br>
                    <br>

                    <b runat="server" id="checkedby"></b>

                </div>

                <div class="signature-box">

                    <b>Approved By</b>

                    <br>
                    <br>
                    <br>

                    <b runat="server" id="approvedby"></b>
                    <br>
                    <b runat="server" id="approvedtgl"></b>

                </div>

            </div>


        </form>

    </div>


    <script>

        function onPrint() {

            document.getElementById("div_ttd").style.display = "flex";
            document.getElementById("btn-print").style.display = "none";

            window.print();

        }

        window.addEventListener("afterprint", () => {

            document.getElementById("btn-print").style.display = "";
            document.getElementById("div_ttd").style.display = "none";

        });

    </script>

</body>

</html>
