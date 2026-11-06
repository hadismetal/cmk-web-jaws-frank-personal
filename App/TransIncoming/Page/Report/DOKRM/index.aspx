<%@ Page EnableEventValidation="false" Language="C#"
    MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="index.aspx.cs" Inherits="DOKRM_index" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">DO KRM </asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Report
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    </form>
    <div
        style="border: 1px solid rgb(216, 216, 216); border-radius: 10px; margin: 3px; padding: 20px;">
        <h2 style="border-bottom: 1px solid gray; width: max-content">Delivery Order - Report
        </h2>

        <div style="display: flex; flex-direction: row; gap: 10px">
            <div class="col-lg-2" style="padding: 10px 0 0 0">
                <label for="ddl_brand_location_asal">No DO KRM</label>
                <input id="kw" class="form-control" style="width: 200px" placeholder="Input Keyword..." />
            </div>
            <div class="col-lg-2" style="padding: 10px 0 0 5px">
                <label for="ddl_location_tujuan">Start Date</label>
                <input id="dari" class="form-control text-center" style="width: 110" placeholder="Dari" />
                <script>
                    $(document).ready(function () {
                        $('#dari').datepicker({ format: 'dd-mm-yyyy' });
                    });
                </script>
            </div>
            <div class="col-lg-2" style="padding: 10px 0 0 5px">
                <label for="ddl_location_tujuan">End Date</label>
                <input id="sampai" class="form-control text-center" style="width: 110" placeholder="Sampai" />
                <script>
                    $(document).ready(function () {
                        $('#sampai').datepicker({ format: 'dd-mm-yyyy' });
                    });
                </script>
            </div>
            <div class="col-lg-2" style="padding: 10px 0 0 5px">
                <label for="ddl_status">Status</label>
                <select class="form-control"
                    id="ddl_status">
                    <option value="2">Outgoing - Approved / On The Way</option>
                    <option value="4" selected>Delivered</option>
                </select>
            </div>
            <div style="display: flex; flex-direction: row; gap: 10px">
                <div class="col-lg-2" style="padding: 10px 0 0 5px">
                    <label for="btn_search_krm">&nbsp;</label>
                    <button class="btn btn-primary btn-md" id="btn_search_krm" type="button" onclick="appendTableBody();">
                        Display
                    </button>
                </div>
            </div>
			<div>
				<div class="col-lg-2" style="padding: 10px 0 0 0px">
                    <label for="btn_search_krm">&nbsp;</label>
                    <button class="btn btn-primary btn-md" id="btn_export" type="button" onclick="appendTableBodyExcel();">
                        Excel
                    </button>
                </div>
			</div>
        </div>
        <div style="margin-top: 20px">
            <div class="row" style="padding: 0; margin: 0">
                <div class="table-responsive">
                    <table id="tb_result" class="table table-bordered" style="max-width:100%">
                        <thead>
                            <tr>
                                <th>No.</th>
                                <th>NO DO KRM</th>
								<th>NO KRM</th>
								<th>Tujuan</th>
								<th>Qty</th>
								<th>Estimasi Harga</th>
                                <th>Kurir</th>
                                <th>Status DO</th>
                                <th>Approval Outgoing</th>
                                <th>Approval Outgoing Tgl</th>
                                <th>Approval Incoming</th>
                                <th>Approval Incoming Tgl</th>
                                <th>ETD</th>
                                <th>Keterangan</th>
                            </tr>
                        </thead>
                        <tbody id="tbody-data"></tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function appendTableBody() {
            const btnSearch = $("#btn_search_krm");
            const inputValue = $("#kw");
            const inputDari = $("#dari");
            const inputSampai = $("#sampai");
            const statusDO = $("#ddl_status");
            btnSearch.prop('disabled', true);

            $.ajax({
                type: "GET",
                url: "./index.aspx/ReportKRM",
                data: {
                    keyword: `'${inputValue.val()}'`,
                    start: `'${inputDari.val()}'`,
                    end: `'${inputSampai.val()}'`,
                    loccode: ``,
                    statusdo: `'${statusDO.val()}'`
                },
                contentType: "application/json; charset=utf-8",
                beforeSend: function (request) {
                    request.withCredentials = false;
                },
                success: function (data) {
                    let response = data.d;
                    $('#tbody-data > tr').remove();
                    if (response.length > 0) {
						let totalbarang = 0;
						let totalhargabarang = 0;
                        $(response).each(function (index) {
                            $("#tbody-data").append(`
						<tr>
							<td>${index + 1}</td>
							<td style="display:none">${response[index].id}</td>
                            <td>${response[index].nodokrm}</td>
                            <td>${response[index].nokrm}</td>
							<td>${response[index].tujuan}</td>
							<td>${response[index].qty}</td>
							<td>${response[index].estimasi_harga_int}</td>
							<td>${response[index].kurir}</td>
							<td>${response[index].statusdo}</td>    
							<td>${response[index].approvaloutgoingnama}</td>
							<td>${response[index].approvaloutgoingtgl}</td>
                            <td>${response[index].approvalincomingnama}</td>
                            <td>${response[index].approvalincomingtgl}</td>
							<td>${response[index].etd}</td>
							<td>${response[index].keterangan}</td>
						</tr>
						`);
						totalbarang += parseInt(response[index].qty);
						totalhargabarang += parseInt(response[index].estimasi_harga_int);
                        });
						$("#tbody-data").append(`
						<tr>
							<td colspan="4"><b>Total</b></td>
							<td><b>${totalbarang}</b></td>
							<td><b>${totalhargabarang}</b></td>
							<td colspan="8"></td>
						</tr>
						`)
                        toastr.success("Nomor KRM berhasil didaftarkan.");
                    }
					else{
						toastr.success("Data kosong.");
					}
                    btnSearch.prop("disabled", false);
                    inputValue.val("");
                },
                error: function (error) {
                    console.error(error);
                    btnSearch.prop("disabled", false);
                    toastr.error("Terjadi kesalahan!");
                },
            });
            btnSearch.prop('disabled', false);
        }
		
		function numberWithCommas(x) {
			return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
		}
		
		function appendTableBodyExcel() {
            const btnSearch = $("#btn_export");
            const inputValue = $("#kw");
            const inputDari = $("#dari");
            const inputSampai = $("#sampai");
            const statusDO = $("#ddl_status");
            btnSearch.prop('disabled', true);

            $.ajax({
                type: "GET",
                url: "./index.aspx/ReportKRM",
                data: {
                    keyword: `'${inputValue.val()}'`,
                    start: `'${inputDari.val()}'`,
                    end: `'${inputSampai.val()}'`,
                    loccode: ``,
                    statusdo: `'${statusDO.val()}'`
                },
                contentType: "application/json; charset=utf-8",
                beforeSend: function (request) {
                    request.withCredentials = false;
                },
                success: function (data) {
                    let response = data.d;
                    $('#tbody-data > tr').remove();
                    if (response.length > 0) {
						let totalbarang = 0;
						let totalhargabarang = 0;
                        $(response).each(function (index) {
                            $("#tbody-data").append(`
						<tr>
							<td>${index + 1}</td>
                            <td>${response[index].nodokrm}</td>
                            <td>${response[index].nokrm}</td>
							<td>${response[index].tujuan}</td>
							<td>${response[index].qty}</td>
							<td>${response[index].estimasi_harga_int}</td>
							<td>${response[index].kurir}</td>
							<td>${response[index].statusdo}</td>    
							<td>${response[index].approvaloutgoingnama}</td>
							<td>${response[index].approvaloutgoingtgl}</td>
                            <td>${response[index].approvalincomingnama}</td>
                            <td>${response[index].approvalincomingtgl}</td>
							<td>${response[index].etd}</td>
							<td>${response[index].keterangan}</td>
						</tr>
						`);
						totalbarang += parseInt(response[index].qty);
						totalhargabarang += parseInt(response[index].estimasi_harga_int);
                        });
						$("#tbody-data").append(`
						<tr>
							<td colspan="4"><b>Total</b></td>
							<td><b>${totalbarang}</b></td>
							<td><b>${totalhargabarang}</b></td>
							<td colspan="8"></td>
						</tr>
						`)
                        toastr.success("Nomor KRM berhasil didaftarkan.");
					
						var location = 'data:application/vnd.ms-excel;base64,';
						var excelTemplate = '<html> ' +
							'<head> ' +
							'<meta http-equiv="content-type" content="text/plain; charset=UTF-8"/> ' +
							'</head> ' +
							'<body> ' +
							document.getElementById("tb_result").outerHTML +
							'</body> ' +
							'</html>'
						window.location.href = location + window.btoa(excelTemplate);
					}
					else{
						toastr.success("Data kosong.");
					}
					
					
                    btnSearch.prop("disabled", false);
                    inputValue.val("");
                },
                error: function (error) {
                    console.error(error);
                    btnSearch.prop("disabled", false);
                    toastr.error("Terjadi kesalahan!");
                },
            });
            btnSearch.prop('disabled', false);
        }

        $(document).ready(function () {
            var tglsekarang = new Date();
            const tgl = String("0" + tglsekarang.getDate()).slice(-2);
            const bulan = String("0" + (tglsekarang.getMonth() + 1)).slice(-2);
            const tahun = String(tglsekarang.getFullYear());
            $('#dari').val(tgl + '-' + bulan + '-' + tahun);
            $('#sampai').val(tgl + '-' + bulan + '-' + tahun);
			
			toastr.options = {
                closeButton: true,
                progressBar: true,
                positionClass: "toast-bottom-center",
                showDuration: "100",
                hideDuration: "100",
                timeOut: "1000",
				"showMethod": "slideDown",
				"hideMethod": "slideUp"
            };
        });
    </script>
</asp:Content>
