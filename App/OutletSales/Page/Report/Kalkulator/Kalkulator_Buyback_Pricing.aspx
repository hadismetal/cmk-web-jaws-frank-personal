<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Kalkulator_Buyback_Pricing.aspx.cs" Inherits="Page_Report_Kalkulator_Kalkulator_Buyback_Pricing" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    <label style="font-family: consolas">Kalkulator Buyback Pricing</label>
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Report
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
        
    </style>
    <form id="Form1" runat="server" style="font-family: consolas">
        <h4 class="col-md-12">Data Product</h4>
        <div class="col-md-4">
            <table class="table table-bordered">
                <tbody>
                    <tr style="background-color: skyblue;">
                        <td colspan="2">
                            <h4>Product</h4>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 40%"><b>PLU</b></td>
                        <td>
                            <label id="plu"></label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Nomor Sales Order</b></td>
                        <td>
                            <label id="noso"></label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Lokasi Beli</b></td>
                        <td>
                            <label id="lokasi"></label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Item</b></td>
                        <td>
                            <label id="item"></label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Kategori</b></td>
                        <td>
                            <label id="kategori"></label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Tanggal Beli</b></td>
                        <td>
                            <label id="tglbeli"></label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Harga Beli</b></td>
                        <td>
                            <label id="hargabeli"></label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Tanggal Jual</b></td>
                        <td>
                            <label id="tgljual"></label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Harga Acuan Resell</b></td>
                        <td>
                            <label id="hargaacuanresell"></label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Gross Weight Actual</b></td>
                        <td style="background-color: yellow">
                            <label id="grossweight"></label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Nomor QC</b></td>
                        <td>
                            <label id="qc"></label>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Gross Weight QC</b></td>
                        <td style="background-color: limegreen">
                            <label id="grossweightqc"></label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="col-md-8">
            <table id="tbstone" class="table table-bordered">
                <thead>
                    <tr style="background-color: skyblue;">
                        <th colspan="8">
                            <h4>Stone</h4>
                        </th>
                    </tr>
                    <tr style="background-color: #f0ead5;">
                        <th style="width: 40px;">No.</th>
                        <th style="width: 250px;">Stone</th>
                        <th style="width: 250px;">Stone Type</th>
                        <th style="width: 130px;">Total Butir</th>
                        <th style="width: 130px;">Total Butir QC</th>
                        <th style="width: 130px;">Total Carat</th>
                        <th style="width: 130px;">Total Carat QC</th>
                        <th style="width: 130px;">GIA</th>
                    </tr>
                </thead>
                <tbody>
                    <!-- rows here -->
                </tbody>
            </table>
        </div>
        <div class="col-md-12">
            <div id="asp">
            </div>
        </div>
        <br />
        <div class="col-md-8">
            <br />
            <div class="form-horizontal" style="font-size: 16px">
                <label>Sesuai dengan tabel buyback di atas maka dapat dihitung harga buyback sebagai berikut</label>
                <br />
                <br />
                <div class="form-group">
                    <label class="col-md-3">Periode</label>
                    <label class="col-md-1">:</label>
                    <label class="col-md-8" style="color: green" id="lblperiode">Periode</label>
                </div>
                <div class="form-group" id="div_trade_in">
                    <label class="col-md-3">Harga TradeIn</label>
                    <label class="col-md-1">:</label>
                    <label class="col-md-8" style="color: green" id="lbltradein">Periode</label>
                </div>
                <div class="form-group" id="div_resell">
                    <label class="col-md-3">Harga Resell</label>
                    <label class="col-md-1">:</label>
                    <label class="col-md-8" style="color: green" id="lblresell">Periode</label>
                </div>
                <div class="form-group">
                    <label class="col-md-12" style="color: red" id="lblwarningbuyback"></label>
                </div>
                <div class="form-group">
                    <div class="col-md-12">
                        <button type="button" id="print">Print</button>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-bottom-center",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "3000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }

            GetData();
        });

        document.getElementById('print').addEventListener('click', () => {
            const printBtn = document.getElementById('print');
            printBtn.style.display = 'none';
            window.print();
        });

        window.addEventListener('afterprint', () => {
            const printBtn = document.getElementById('print');
            printBtn.style.display = 'block';
        });

        class RequestKalkulator {
            constructor({
                id_product = 0,
                id_qc = 0
            } = {}) {
                this.id_product = id_product;
                this.id_qc = id_qc;
            }
        }

        function GetData() {
            const params = new URLSearchParams(window.location.search);
            const id_product = params.get('idproduct');
            const tipe = params.get('tipe');
            const id_qc = params.get('idqc');

            const request = new RequestKalkulator({
                id_product: id_product,
                id_qc: id_qc
            })

            const url = `Kalkulator_Buyback_Pricing.aspx/Display`;
            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    request: request
                })
            }).then(response => response.json()).then(data => {
                const obj = data.d;
                console.log(obj);
                if (obj.success) {
                    const product = obj.data;
                    const stone = obj.data.stone;
                    const asp = obj.data.asp;
                    const summary = obj.data.summary;

                    const lbl_plu = document.getElementById('plu');
                    const lbl_noso = document.getElementById('noso');
                    const lbl_lokasi = document.getElementById('lokasi');
                    const lbl_item = document.getElementById('item');
                    const lbl_kategori = document.getElementById('kategori');
                    const lbl_tanggal_beli = document.getElementById('tglbeli');
                    const lbl_harga_beli = document.getElementById('hargabeli');
                    const lbl_tanggal_jual = document.getElementById('tgljual');
                    const lbl_harga_acuan_resell = document.getElementById('hargaacuanresell');
                    const lbl_nomor_qc = document.getElementById('qc');
                    const lbl_gross_weight = document.getElementById('grossweight');
                    const lbl_gross_weight_qc = document.getElementById('grossweightqc');

                    lbl_plu.innerText = product.plu;
                    lbl_noso.innerText = product.no_so;
                    lbl_lokasi.innerText = product.lokasi_so;
                    lbl_item.innerText = product.item;
                    lbl_kategori.innerText = product.kategori;
                    lbl_tanggal_beli.innerText = product.tanggal_so;
                    lbl_harga_beli.innerText = 'IDR ' + product.harga_so.toLocaleString();
                    lbl_tanggal_jual.innerText = product.tanggal_jual + ' (Tanggal Hari Ini)';
                    lbl_harga_acuan_resell.innerText = 'IDR ' + product.harga_acuan_buyback.toLocaleString();
                    lbl_nomor_qc.innerText = product.no_qc;
                    lbl_gross_weight.innerText = product.gross_weight.toLocaleString() + ' gr';
                    lbl_gross_weight_qc.innerText = product.gross_weight_qc.toLocaleString() + ' gr';

                    const tbody = document.querySelector('#tbstone tbody');
                    tbody.innerHTML = '';

                    let grand_total_butir = 0;
                    let grand_total_carat = 0;
                    let grand_total_butir_qc = 0;
                    let grand_total_carat_qc = 0;
                    stone.forEach((item, index) => {
                        const stone_name = item.nama;
                        const stone_type = item.stone_type;
                        const total_butir = item.total_butir;
                        const total_butir_qc = item.total_butir_qc;
                        const total_carat = item.total_carat;
                        const total_carat_qc = item.total_carat_qc;
                        const gia = item.gia;

                        grand_total_butir += total_butir;
                        grand_total_carat += total_carat;
                        grand_total_butir_qc += total_butir_qc;
                        grand_total_carat_qc += total_carat_qc;

                        const row = `
                            <tr>
                                <td>${index + 1}</td>
                                <td>${stone_name}</td>
                                <td>${stone_type}</td>
                                <td style='background-color:yellow'>${total_butir.toLocaleString()}</td>
                                <td style='background-color:limegreen'>${total_butir_qc.toLocaleString()}</td>
                                <td style='background-color:yellow'>${total_carat.toLocaleString()}</td>
                                <td style='background-color:limegreen'>${total_carat_qc.toLocaleString()}</td>
                                <td>${gia}</td>
                            </tr>
                        `;
                        tbody.insertAdjacentHTML('beforeend', row);
                    });

                    const sumrow = `
                    <tr style='background-color:skyblue'>
                        <td></td>
                        <td><h4 style='color:MediumPurple;'>Grand Total<h4></td>
                        <td></td>
                        <td>${grand_total_butir.toLocaleString()}</td>
                        <td>${grand_total_butir_qc.toLocaleString()}</td>
                        <td>${grand_total_carat.toLocaleString()}</td>
                        <td>${grand_total_carat_qc.toLocaleString()}</td>
                        <td></td>
                    </tr>`

                    tbody.insertAdjacentHTML('beforeend', sumrow);

                    const divasp = document.getElementById('asp');

                    divasp.innerHTML = '';

                    if (asp.nama === '' && !(product.item.toLowerCase().includes('wedding ring') || product.item.toLowerCase().includes('aretino')))
                        divasp.innerHTML = AspNoBuyback(product.brand);
                    else if (asp.nama === 'PG FIXED')
                        divasp.innerHTML = AspPG(product.brand);
                    else if (asp.nama == 'DOS BRANDED')
                        divasp.innerHTML = AspDossier(asp.me_periode_max, asp.ti01_periode_max, asp.ti0_periode_max, asp.ti1_periode_max, asp.ti2_periode_max, asp.ti3_periode_max, asp.r1_periode_max, asp.r2_periode_max, asp.me_rumus, asp.ti01_rumus, asp.ti0_rumus, asp.ti1_rumus, asp.ti2_rumus, asp.ti3_rumus, asp.r1_rumus, asp.r2_rumus, asp.me_periode_max_day, product.brand);
                    else if (asp.nama === '' && (product.item.toLowerCase().includes('wedding ring') || product.item.toLowerCase().includes('aretino')))
                        divasp.innerHTML = AspWeddingRing(product.frame_material);
                    else if (asp.nama !== '')
                        divasp.innerHTML = AspNormal(asp.me_periode_max, asp.ti1_periode_max, asp.ti2_periode_max, asp.ti3_periode_max, asp.r1_periode_max, asp.r2_periode_max, asp.me_rumus, asp.ti1_rumus, asp.ti2_rumus, asp.ti3_rumus, asp.r1_rumus, asp.r2_rumus, asp.me_periode_max_day, product.brand, asp.nama);


                    const lbl_periode = document.getElementById('lblperiode');
                    const lbl_trade_in = document.getElementById('lbltradein');
                    const lbl_resell = document.getElementById('lblresell');
                    const lbl_warning_buybcak = document.getElementById('lblwarningbuyback');

                    lbl_periode.innerText = summary.umur_tahun + ' Tahun ' + summary.umur_bulan + ' Bulan ' + summary.umur_hari + ' Hari';
                    lbl_trade_in.innerText = 'IDR ' + summary.harga_trade_in.toLocaleString() + ' ' + summary.status_trade_in;
                    lbl_resell.innerText = 'IDR ' + summary.harga_resell.toLocaleString() + ' ' + summary.status_resell;

                    let error_warning = Warning(grand_total_butir, grand_total_butir_qc, product.gross_weight, product.gross_weight_qc, summary.umur_tahun);
                    lbl_warning_buybcak.innerText = error_warning;

                    if (asp.nama == `SNIPS Lebih Dari Atau Sama Dengan 0.3 Carat Bersertifikat`) {
                        if (summary.status_trade_in == `(*Call Pusat.)`) {
                            lbl_trade_in.innerText = summary.status_trade_in;
                        }
                        lbl_resell.innerText = summary.status_resell;
                    }
                    else {
                        if (error_warning == '(*Call Pusat. Total Carat antara data product dan qc tidak sama.)' || error_warning == '(*Call Pusat. Total Berat Emas antara data product dan qc tidak sama.)' || error_warning == "") {
                            document.getElementById('div_trade_in').style.display = 'none';
                            document.getElementById('div_resell').style.display = 'none';
                        }
                        else if (summary.umur_tahun >= 10) {
                            document.getElementById('div_trade_in').style.display = 'none';
                            document.getElementById('div_resell').style.display = 'none';
                            lbl_warning_buybcak.innerText = "(*Call Pusat. Barang berusia di atas 10 tahun.)";
                        }
                    }

                    toastr.success('Data Load Successfully.');
                } else {
                    toastr.error(obj.error);
                }

            }).catch(error => toastr.error(error));
        }

        function AspNoBuyback(brand) {
            const text = `<div>
            <table class="table table-bordered" style="width:100%; text-align:center;">
                <thead>
                    <tr style="background-color: skyblue">
                        <td colspan="3">
                            <h4>After Sales Policy / Kebijakan Penjualan</h4>
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr align="center">
                        <td style="width: 300px;">Merchandise Exchange<br />
                            <i>(Penukaran Barang)</i></td>
                        <td style="width: 500px;">Trade In<br />
                            <i>(Tukar Tambah)</i></td>
                        <td style="width: 400px;">Customer's option to resell<br />
                            <i>(Opsi Penjualan Kembali)</i></td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: center; width: 100%; padding: 20px;"><i>"No Exchange - No Resell"</i>. <em>Tidak dapat ditukar dan Tidak dapat dijual kembali.</em> </td>
                    </tr>
                </tbody>
            </table>
            </div>`;

            return text;
        }

        function AspPG(brand) {
            const text = `<div>
            <table class="table table-bordered" style="width:100%; text-align:center;">
                <thead>
                    <tr style="background-color: skyblue">
                        <td colspan="1">
                            <h4>After Sales Policy / Kebijakan Penjualan</h4>
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td style="text-align: center; width: 1200px; padding: 50px 20px;"><i>Only gold value calculated based on <label>${brand}</label> today's buy back gold price</i><br />
                            Hanya nilai emas yang diperhitungkan berdasarkan harga beli <label>${brand}</label> pada hari pengembalian
                        </td>
                    </tr>
                </tbody>
            </table>
            </div>`;

            return text;
        }

        function AspWeddingRing(material) {
            let text = ``;
            if (material.toLowerCase() == `emas`) {
                text = `<div>
                <table class="table table-bordered" style="width:100%; text-align:center;">
                    <thead>
                        <tr style="background-color: skyblue">
                            <td colspan="3">
                                <h4>After Sales Policy / Kebijakan Penjualan</h4>
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr align="center">
                            <td style="width: 300px;">Merchandise Exchange<br />
                                <i>(Penukaran Barang)</i></td>
                            <td style="width: 500px;">Trade In<br />
                                <i>(Tukar Tambah)</i></td>
                            <td style="width: 400px;">Customer's option to resell<br />
                                <i>(Opsi Penjualan Kembali)</i></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center; width: 80%; padding: 20px;"><i>"No Exchange - No Tradein"</i>. <em>Tidak dapat ditukar dan tidak dapat ditukar tambah.</em> </td>
			                <td style="text-align: center; width: 20%; padding: 20px;">50%<br/><i>Original Sales Amount</i>. <br/>Nilai Penjualan Asal</td>
                        </tr>
                    </tbody>
                </table>
            </div>`;
            } else {
                text = `<div>
                <table class="table table-bordered" style="width:100%; text-align:center;">
                    <thead>
                        <tr style="background-color: skyblue">
                            <td colspan="3">
                                <h4>After Sales Policy / Kebijakan Penjualan</h4>
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr align="center">
                            <td style="width: 300px;">Merchandise Exchange<br />
                                <i>(Penukaran Barang)</i></td>
                            <td style="width: 500px;">Trade In<br />
                                <i>(Tukar Tambah)</i></td>
                            <td style="width: 400px;">Customer's option to resell<br />
                                <i>(Opsi Penjualan Kembali)</i></td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: center; width: 80%; padding: 20px;"><i>"No Exchange - No Tradein"</i>. <em>Tidak dapat ditukar dan tidak dapat ditukar tambah.</em> </td>
                        </tr>
                    </tbody>
                </table>
            </div>`;
            }
            return text;
        }

        function AspNormal(me, ti1, ti2, ti3, rs1, rs2, me_rumus1, ti_rumus1, ti_rumus2, ti_rumus3, rs_rumus1, rs_rumus2, me_max_periode_days, brand, nama) {
            let ti1_new = 0;
            let ti1_unit = `bulan`;
            if (ti1 == 12) {
                ti1_new = ti1 / 12;
                ti1_unit = `tahun`;
            } else {
                ti1_new = ti1;
            }

            ti2 = ti2 / 12;
            ti3 = ti2;

            rs1 = rs1 / 12;
            rs2 = rs1;

            let me_text = `< ${me} Month - ${me} Bulan`;
            if (me_max_periode_days > 0) {
                me_text = `< ${me_max_periode_days} Days - ${me_max_periode_days} Hari`;
            }

            let ti1_text = ``;
            if (ti1_unit == `tahun`) {
                ti1_text = `< ${ti1_new} Year - ${ti1_new} Tahun`;
            } else {
                ti1_text = `< ${ti1_new} Month - ${ti1_new} Bulan`;
            }
            let ti2_text = `< ${ti2} Year - ${ti2} Tahun`;
            let ti3_text = `> ${ti3} Year - ${ti3} Tahun`;
            let rs1_text = `< ${rs1} Year - ${rs1} Tahun`;
            let rs2_text = `> ${rs2} Year - ${rs2} Tahun`;

            let me_rumus1_text = `${me_rumus1} %<br/><label>Original sales amount</label><br /><label>Nilai Penjualan Asal</label>`;
            let ti_rumus1_text = `${ti_rumus1} %<br/><label>Original sales amount</label><br /><label>Nilai Penjualan Asal</label>`;
            let ti_rumus2_text = `${ti_rumus2} %<br/><label>Original sales amount</label><br /><label>Nilai Penjualan Asal</label>`;
            let ti_rumus3_text = `<label>${brand} buying price</label><br /><label>Harga beli ${brand}</label>`;
            let rs_rumus1_text = `${rs_rumus1} %<br/><label>Original sales amount</label><br /><label>Nilai Penjualan Asal</label>`;
            let rs_rumus2_text = `<label>${brand} buying price</label><br /><label>Harga beli ${brand}</label>`;

            if (nama == `SNIPS Lebih Dari Atau Sama Dengan 0.3 Carat Bersertifikat`) {
                ti_rumus1_text = `<label>${brand} buying price</label><br /><label>Harga beli ${brand}</label>`;
                ti_rumus2_text = `<label>${brand} buying price</label><br /><label>Harga beli ${brand}</label>`;
                rs_rumus1_text = `<label>${brand} buying price</label><br /><label>Harga beli ${brand}</label>`;
            }

            const text = `<div>
                <table class="table table-bordered" style="width:100%; text-align:center;">
                    <thead>
                        <tr style="background-color: skyblue">
                            <td colspan="6">
                                <h4>After Sales Policy / Kebijakan Penjualan</h4>
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
	                        <td>
		                        <em>Merchandise Exchange</em><br />
		                        (Penukaran Barang)
	                        </td>
                            <td colspan="3" style="width:45%;">
		                        <em>Trade In</em><br />
		                        (Tukar Tambah)
	                        </td>
                            <td colspan="2" style="width:30%;">
		                        <em>Customer's option to resell</em><br />
		                        (Opsi Penjualan Kembali)
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        <em>Period</em> - Dalam waktu
	                        </td>
                            <td colspan="3" style="width:45%;">
		                        <em>Period</em> - Dalam waktu
	                        </td>
                            <td colspan="2" style="width:30%;">
		                        <em>Period</em> - Dalam waktu
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        <label>${me_text}</label>
	                        </td>
                            <td style="width:15%;">
		                        <label>${ti1_text}</label>
	                        </td>
                            <td style="width:15%;">
		                        <label>${ti2_text}</label>
	                        </td>
                            <td style="width:15%;">
		                        <label>${ti3_text}</label>
	                        </td>
                            <td style="width:15%;">
		                        <label>${rs1_text}</label>
	                        </td>
                            <td style="width:15%;">
		                        <label>${rs2_text}</label>
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        <label>${me_rumus1_text}<label>
	                        </td>
                            <td style="width:15%;">
                                <label>${ti_rumus1_text}</label>
	                        </td>
                            <td style="width:15%;">
                                <label>${ti_rumus2_text}</label>
	                        </td>
                            <td style="width:15%;">
                                <label>${ti_rumus3_text}</label>
	                        </td>
                            <td style="width:15%;">
                                <label>${rs_rumus1_text}</label>
	                        </td>
                            <td style="width:15%;">
                                <label>${rs_rumus2_text}</label>
	                        </td>
                        </tr>
                    </tbody>
                </table>
            </div>`

            return text;
        }

        function AspDossier(medoss, tidoss01, tidoss0, tidoss1, tidoss2, tidoss3, rsdoss1, rsdoss2, medoss_rumus, tidoss_rumus01, tidoss_rumus0, tidoss_rumus1, tidoss_rumus2, tidoss_rumus3, rsdoss_rumus1, rsdoss_rumus2, me_max_periode_days, brand) {
            tidoss1 = tidoss1 / 12;
            tidoss2 = tidoss2 / 12;
            tidoss3 = tidoss2;

            rsdoss1 = rsdoss1 / 12;
            rsdoss2 = rsdoss1;

            let medoss_text = `< ${medoss} Month - ${medoss} Bulan`;

            if (me_max_periode_days > 0) {
                medoss_text = `< ${me_max_periode_days} Days - ${me_max_periode_days} Hari`;
            }

            let tidoss01_text = `< ${tidoss01} Month - ${tidoss01} Bulan`;
            let tidoss0_text = `< ${tidoss0} Month - ${tidoss0} Bulan`;
            let tidoss1_text = `< ${tidoss1} Year - ${tidoss1} Tahun`;
            let tidoss2_text = `< ${tidoss2} Year - ${tidoss2} Tahun`;
            let tidoss3_text = `> ${tidoss2} Year - ${tidoss2} Tahun`;
            let rsdoss1_text = `< ${rsdoss1} Year - ${rsdoss1} Tahun`;
            let rsdoss2_text = `> ${rsdoss2} Year - ${rsdoss2} Tahun`;

            let medoss_rumus_text = `${medoss_rumus} %<br/><label>Original sales amount</label><br /><label>Nilai Penjualan Asal</label>`;
            let tidoss_rumus01_text = `${tidoss_rumus01} %<br/><label>Original sales amount</label><br /><label>Nilai Penjualan Asal</label>`;
            let tidoss_rumus0_text = `${tidoss_rumus0} %<br/><label>Original sales amount</label><br /><label>Nilai Penjualan Asal</label>`;
            let tidoss_rumus1_text = `${tidoss_rumus1} %<br/><label>Original sales amount</label><br /><label>Nilai Penjualan Asal</label>`;
            let tidoss_rumus2_text = `${tidoss_rumus2} %<br/><label>Original sales amount</label><br /><label>Nilai Penjualan Asal</label>`;
            let tidoss_rumus3_text = `<label>${brand} buying price</label><br /><label>Harga beli ${brand}</label>`;
            let rsdoss_rumus1_text = `${rsdoss_rumus1} %<br/><label>Original sales amount</label><br /><label>Nilai Penjualan Asal</label>`;
            let rsdoss_rumus2_text = `<label>${brand} buying price</label><br /><label>Harga beli ${brand}</label>`;

            const text = `<div>
                <table class="table table-bordered" style="width:100%; text-align:center;">
                    <thead>
                        <tr style="background-color: skyblue">
                            <td colspan="8">
                                <h4>After Sales Policy / Kebijakan Penjualan</h4>
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
			                <td style="width:15%;">
		                        <em>Merchandise Exchange</em><br />
		                        (Penukaran Barang)
	                        </td>
                            <td colspan="5" style="width:60%;">
		                        <em>Trade In</em><br />
		                        (Tukar Tambah)
	                        </td>
                            <td colspan="2" style="width:25%;">
		                        <em>Customer's option to resell</em><br />
		                        (Opsi Penjualan Kembali)
	                        </td>
                        </tr>
                        <tr>
			                <td style="width:15%;">
		                        <em>Period</em> - Dalam waktu
	                        </td>
                            <td colspan="5" style="width:60%;">
		                        <em>Period</em> - Dalam waktu
	                        </td>
                            <td colspan="2" style="width:25%;">
		                        <em>Period</em> - Dalam waktu
	                        </td>
                        </tr>
                        <tr>
			                <td style="width:10%;">
		                        <label>${medoss_text}</label>
	                        </td>
			                <td style="width:10%;">
		                        <label>${tidoss01_text}</label>
	                        </td>
	                        <td style="width:10%;">
		                        <label ID="">${tidoss0_text}</label>
	                        </td>
                            <td style="width:10%;">
		                        <label ID="tidoss1">${tidoss1_text}</label>
	                        </td>
                            <td style="width:10%;">
		                        <label ID="tidoss2">${tidoss2_text}</label>
	                        </td>
                            <td style="width:10%;">
		                        <label ID="tidoss3">${tidoss3_text}</label>
	                        </td>
                            <td style="width:10%;">
		                        <label ID="rsdoss1">${rsdoss1_text}</label>
	                        </td>
                            <td style="width:10%;">
		                        <label ID="rsdoss2">${rsdoss2_text}</label>
	                        </td>
                        </tr>
                        <tr>
			                <td style="width:10%;">
		                        <label>${medoss_rumus_text}</label>
	                        </td>
			                <td style="width:10%;">
		                        <label>${tidoss_rumus01_text}</label>
	                        </td>
	                        <td style="width:10%;">
		                        <label>${tidoss_rumus0_text}</label>
	                        </td>
                            <td style="width:10%;">
                                <label>${tidoss_rumus1_text}</label>
	                        </td>
                            <td style="width:10%;">
                                <label>${tidoss_rumus2_text}</label>
	                        </td>
                            <td style="width:10%;">
                                <label>${tidoss_rumus3_text}</label>
	                        </td>
                            <td style="width:10%;">
                                <label>${rsdoss_rumus1_text}</label>
	                        </td>
                            <td style="width:10%;">
                                <label>${rsdoss_rumus2_text}</label>
	                        </td>
                        </tr>
                    </tbody>
                </table>
            </div>`
            return text;
        }

        function AspBts(brand, mebts, tibts1, tibts2, tibts3, mebts_rumus1, tibts_rumus1, tibts_rumus2, tibts_rumus3, brand) {
            const text = `<div>
                <table class="table table-bordered" style="width:100%; text-align:center;">
                    <thead>
                        <tr style="background-color: skyblue">
                            <td colspan="6">
                                <h4>After Sales Policy / Kebijakan Penjualan</h4>
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
	                        <td>
		                        <em>Merchandise Exchange</em><br />
		                        (Penukaran Barang)
	                        </td>
                            <td colspan="3" style="width:45%;">
		                        <em>Trade In</em><br />
		                        (Tukar Tambah)
	                        </td>
                            <td colspan="2" style="width:30%;">
		                        <em>Customer's option to resell</em><br />
		                        (Opsi Penjualan Kembali)
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        <em>Period</em> - Dalam waktu
	                        </td>
                            <td colspan="3" style="width:45%;">
		                        <em>Period</em> - Dalam waktu
	                        </td>
                            <td colspan="2" rowspan="3" style="width:30%;">
		                        <i>Only gold value calculated based on <label>${brand}</label> today's buy back gold price</i><br />
                                Hanya nilai emas yang diperhitungkan berdasarkan harga beli <label>${brand}</label> pada hari pengembalian
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        <label>${mebts}</label>
	                        </td>
                            <td style="width:15%;">
		                        <label>${tibts1}</label>
	                        </td>
                            <td style="width:15%;">
		                        <label>${tibts2}</label>
	                        </td>
                            <td style="width:15%;">
		                        <label>${tibts3}</label>
	                        </td>
                        </tr>
                        <tr>
	                        <td>
		                        <label>${mebts_rumus1}</label>
	                        </td>
                            <td style="width:15%;">
                                <label>${tibts_rumus1} </label>
	                        </td>
                            <td style="width:15%;">
                                <label>${tibts_rumus2}</label>
	                        </td>
                            <td style="width:15%;">
                                <label>${tibts_rumus3}</label>
	                        </td>
                        </tr>
                    </tbody>
                </table>
            </div>`;
            return text;
        }

        function Warning(totalcaratori, totalcaratreal, totalweightori, totalweightreal, tahun) {
            let text = "";
            if (totalcaratori != totalcaratreal) {
                text = "(*Call Pusat. Total Carat antara data product dan qc tidak sama.)";
            } else if (totalweightori != totalweightreal) {
                text = "(*Call Pusat. Total Berat Emas antara data product dan qc tidak sama.)";
            } else if (tahun >= 10) {
                text = "(*Call Pusat. Barang berusia di atas 10 tahun.)";
            } else {
                text = "*(Harga yang terlampir adalah untuk barang dengan kondisi QC baik.)";
            }
            return text;
        }

    </script>
</asp:Content>
