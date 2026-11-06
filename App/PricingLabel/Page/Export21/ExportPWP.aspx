<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExportPWP.aspx.cs" Inherits="Page_ExportPWP" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Export Label
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="h1_st" runat="server">
    PWP
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style type="text/css">
        .location-summary {
            max-height: 150px;   /* adjust height as needed */
            overflow-y: auto;    /* vertical scroll */
            overflow-x: hidden;  /* optional: hide horizontal scroll */
        }
        .form-group .select2-container {
            position: relative;
            z-index: 2;
            float: left;
            width: 100%;
            margin-bottom: 0;
            display: table;
            table-layout: fixed;
        }

        .form-group .select2-container {
            border: none;
            padding: 0;
        }

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
    <form id="Form1" runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-3 col-md-3">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        PWP
                   
                    </div>
                    <div class="panel-body">
                        <div class="form-group">
                            <label class="text-muted">Tipe</label>
                            <asp:DropDownList runat="server" ID="product_type" ClientIDMode="Static" CssClass="form-control">
                                <%--<asp:ListItem Value="" Text="Pilih Product"></asp:ListItem>--%>
                                <asp:ListItem Value="1" Text="Diamond Jewellery"></asp:ListItem>
                                <%--<asp:ListItem Value="2" Text="Plain Gold"></asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <label class="text-muted">PWP Name</label>
                            <asp:DropDownList ID="pwp_name" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <br />
                        <br />
                        <button type="button" class="btn btn-primary btn-block" onclick="getData();">
                            Display
                        </button>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-9 col-md-9">
                <div class="panel panel-info" id="div_detail">
                    <div class="panel-heading">
                        PWP
                    </div>
                    <div class="panel-body">
                        <div class="col-xs-12 col sm-4 col-md-4">
                            <div class="form-group">
                                <label class="text-muted">Item</label>
                                <asp:DropDownList runat="server" ID="item" ClientIDMode="Static" CssClass="form-control">
                                    <asp:ListItem Value="0" Text="All"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label class="text-muted">Location</label>
                                <asp:DropDownList runat="server" ID="lokasi" ClientIDMode="Static" CssClass="form-control">
                                    <asp:ListItem Value="0" Text="All" data-id="-1" data-tipe="-1"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <button type="button" id="btn-export" class="btn btn-primary btn-block" onclick="exportData();" style="display: none">
                                Export
                            </button>
                        </div>
                        <div class="col-xs-12 col sm-4 col-md-4">
                            <div class="form-group">
                                <label class="text-muted">No PLU</label>
                                <div class="row">
                                    <div class="col-xs-6 col sm-6 col-md-6">
                                        <asp:TextBox runat="server" ID="plu_start" ClientIDMode="Static" CssClass="form-control" placeholder="Start PLU" MaxLength="100"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col sm-6 col-md-6">
                                        <asp:TextBox runat="server" ID="plu_end" ClientIDMode="Static" CssClass="form-control" placeholder="End PLU" MaxLength="100"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="text-muted">Clarity</label>
                                <asp:DropDownList runat="server" ID="clarity" ClientIDMode="Static" CssClass="form-control">
                                    <asp:ListItem Value="0" Text="All"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <button type="button" id="btn-summary" class="btn btn-primary btn-block" onclick="getDataPreview();" style="display: none">
                                Preview Summary
                            </button>
                        </div>
                        <div class="col-xs-12 col sm-4 col-md-4">
                            <div class="form-group">
                                <label class="text-muted">Clarity Summary :</label>
                                <div id="summary-clarity"></div>
                            </div>
                            <div class="form-group">
                                <label class="text-muted">Location Summary :</label>
                                <div id="summary-location" class="location-summary"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="table-responseive">
                    <table id="tb" class="table table-bordered">
                        <thead>
                            <tr>
                                <th>No.</th>
                                <th>PLU</th>
                                <th>Karet</th>
                                <th>Item</th>
                                <th>Lokasi</th>
                                <th>Clarity</th>
                                <th>Kadar Logam</th>
                                <th>Harga</th>
                                <th>PWP</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </form>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            $('#tgl_start').datepicker({ format: 'dd-mm-yyyy' });
            $('#pwp_name').select2();
            //$('#kw').autocomplete

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
        });

        class RequestPWP {
            constructor({
                id = 0,
                item = 0,
                tipe_location = 0,
                id_location = 0,
                no_plu_start = "",
                no_plu_end = "",
                clarity = 0,
            } = {}) {
                this.id = id;
                this.item = item;
                this.id_location = id_location;
                this.tipe_location = tipe_location;
                this.no_plu_start = no_plu_start;
                this.no_plu_end = no_plu_end;
                this.clarity = clarity;
            }
        }

        function fillDropdownItem(listitem, value) {
            let select = document.getElementById("item");

            select.replaceChildren(); // hapus semua child

            let defaultopt = new Option();
            defaultopt.text = "All";
            defaultopt.value = "0";
            select.appendChild(defaultopt);

            listitem.forEach(item => {
                let opt = new Option();
                opt.text = item.nama;
                opt.value = item.id;
                select.appendChild(opt);
            });

            select.value = value;
        }

        function fillLocation(listlocation, value) {
            let select = document.getElementById("lokasi");
            const container = document.getElementById("summary-location");

            container.replaceChildren(); // refresh UL

            const ul = document.createElement("ul");
            ul.id = "ul-location";

            select.replaceChildren(); // hapus semua child

            let defaultopt = new Option();
            defaultopt.text = "All";
            defaultopt.value = "-1--1";
            defaultopt.setAttribute("data-id", "-1");
            defaultopt.setAttribute("data-tipe", "-1");
            select.appendChild(defaultopt);

            listlocation.forEach(item => {
                let opt = new Option();
                opt.text = item.nama + ' (' + item.kode + ')';
                opt.value = item.id + '-' + item.tipe;
                opt.setAttribute("data-id", item.id);
                opt.setAttribute("data-tipe", item.tipe);
                select.appendChild(opt);

                const li = document.createElement("li");
                li.innerHTML = item.nama + ' (<label style=\'color:blue\'>' + item.total + '</label>)';
                ul.appendChild(li);
            });
            container.appendChild(ul);
            select.value = value;
        }

        function fillClarity(listclarity, value) {
            let select = document.getElementById("clarity");
            const container = document.getElementById("summary-clarity");

            container.replaceChildren(); // refresh UL

            const ul = document.createElement("ul");
            ul.id = "ul-clarity";

            select.replaceChildren(); // hapus semua child

            let defaultopt = new Option();
            defaultopt.text = "All";
            defaultopt.value = "0";
            select.appendChild(defaultopt);

            listclarity.forEach(item => {
                let opt = new Option();
                opt.text = item.nama;
                opt.value = item.id;
                select.appendChild(opt);

                const li = document.createElement("li");
                li.innerHTML = item.nama + ' (<label style=\'color:blue\'>' + item.total + '</label>)';
                ul.appendChild(li);
            });
            container.appendChild(ul);
            select.value = value;
        }

        function getDataPreview() {
            document.getElementById("overlay").style.display = "flex";
            const url = "ExportPWP.aspx/Display";
            const id = document.getElementById('pwp_name').value == "" ? "0" : document.getElementById('pwp_name').value;
            const item = document.getElementById('item').value;
            const lokasi = document.getElementById('lokasi');
            const selectedOption = lokasi.options[lokasi.selectedIndex];
            const id_lokasi = selectedOption.getAttribute('data-id');
            const tipe_lokasi = selectedOption.getAttribute('data-tipe');
            const plu_start = document.getElementById('plu_start').value;
            const plu_end = document.getElementById('plu_end').value;
            const clarity = document.getElementById('clarity').value;

            const btn_export = document.getElementById('btn-export');
            
            const request = new RequestPWP({
                id: id,
                item: item,
                tipe_location: tipe_lokasi,
                id_location: id_lokasi,
                location: lokasi,
                no_plu_start: plu_start,
                no_plu_end: plu_end,
                clarity: clarity
            });

            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify({ request: request })
            }).then(response => response.json()).then(result => {
                const obj = result.d;
                if (obj.success) {
                    const tbody = document.querySelector("#tb tbody");
                    tbody.innerHTML = '';

                    let datas = obj.data_item;
                    let item_ddl = obj.data_product_item;
                    let location_ddl = obj.data_location;
                    let clarity_ddl = obj.data_clarity;

                    fillClarity(clarity_ddl, clarity);
                    fillLocation(location_ddl, id_lokasi + '-' + tipe_lokasi);
                    fillDropdownItem(item_ddl, item);

                    if (datas.length > 0) {
                        datas.forEach((item, index) => {
                            const tr = document.createElement('tr');

                            const tdNo = document.createElement('td');
                            tdNo.textContent = index + 1;
                            tdNo.setAttribute("data-id", item.id_product);
                            tdNo.setAttribute("data-tipe", item.tipe_product);
                            tr.appendChild(tdNo);

                            const tdPLU = document.createElement('td');
                            tdPLU.textContent = item.nomor;
                            tr.appendChild(tdPLU);

                            const tdKaret = document.createElement('td');
                            tdKaret.textContent = item.kode_karet;
                            tr.appendChild(tdKaret);

                            const tdItem = document.createElement('td');
                            tdItem.textContent = item.item;
                            tr.appendChild(tdItem);

                            const tdLokasi = document.createElement('td');
                            tdLokasi.textContent = item.location;
                            tr.appendChild(tdLokasi);

                            const tdClarity = document.createElement('td');
                            tdClarity.textContent = item.clarity;
                            tr.appendChild(tdClarity);

                            const tdKadarLogam = document.createElement('td');
                            tdKadarLogam.textContent = item.kadar_logam;
                            tr.appendChild(tdKadarLogam);

                            const tdHarga = document.createElement('td');
                            tdHarga.style.textAlign = 'right';
                            tdHarga.textContent = item.harga_barcode.toLocaleString("id-ID", {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2
                            });;
                            tr.appendChild(tdHarga);

                            const tdPWP = document.createElement('td');
                            tdPWP.style.textAlign = 'right';
                            tdPWP.textContent = item.harga_pwp.toLocaleString("id-ID", {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2
                            });;
                            tr.appendChild(tdPWP);

                            tbody.appendChild(tr);
                        });
                        btn_export.style.display = 'block';
                        toastr.success('Data Load Successfully');
                        document.getElementById("overlay").style.display = "none";
                    }
                    else {
                        btn_export.style.display = 'none';
                        toastr.info('Data Kosong');
                        document.getElementById("overlay").style.display = "none";
                    }
                }
                else {
                    btn_export.style.display = 'none';
                    toastr.error(obj.error);
                    document.getElementById("overlay").style.display = "none";
                }
            }).catch(error => toastr.error(error));
        }

        function getData() {
            document.getElementById("overlay").style.display = "flex";
            const url = "ExportPWP.aspx/Display";
            const id = document.getElementById('pwp_name').value == "" ? "0" : document.getElementById('pwp_name').value;
            const item = document.getElementById('item').value;
            const lokasi = document.getElementById('lokasi');
            const selectedOption = lokasi.options[lokasi.selectedIndex];
            const id_lokasi = selectedOption.getAttribute('data-id');
            const tipe_lokasi = selectedOption.getAttribute('data-tipe');
            const plu_start = document.getElementById('plu_start').value;
            const plu_end = document.getElementById('plu_end').value;
            const clarity = document.getElementById('clarity').value;

            const btn_export = document.getElementById('btn-export');
            const btn_summary = document.getElementById('btn-summary');

            const request = new RequestPWP({
                id: id,
                item: item,
                tipe_location: tipe_lokasi,
                id_location: id_lokasi,
                location: lokasi,
                no_plu_start: plu_start,
                no_plu_end: plu_end,
                clarity: clarity
            });

            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify({ request: request })
            }).then(response => response.json()).then(result => {
                const obj = result.d;
                if (obj.success) {
                    const tbody = document.querySelector("#tb tbody");
                    tbody.innerHTML = '';

                    let datas = obj.data_item;
                    let item_ddl = obj.data_product_item;
                    let location_ddl = obj.data_location;
                    let clarity_ddl = obj.data_clarity;

                    fillClarity(clarity_ddl, clarity);
                    fillLocation(location_ddl, id_lokasi + '-' + tipe_lokasi);
                    fillDropdownItem(item_ddl, item);

                    if (datas.length > 0) {
                        datas.forEach((item, index) => {
                            const tr = document.createElement('tr');

                            const tdNo = document.createElement('td');
                            tdNo.textContent = index + 1;
                            tdNo.setAttribute("data-id", item.id_product);
                            tdNo.setAttribute("data-tipe", item.tipe_product);
                            tr.appendChild(tdNo);

                            const tdPLU = document.createElement('td');
                            tdPLU.textContent = item.nomor;
                            tr.appendChild(tdPLU);

                            const tdKaret = document.createElement('td');
                            tdKaret.textContent = item.kode_karet;
                            tr.appendChild(tdKaret);

                            const tdItem = document.createElement('td');
                            tdItem.textContent = item.item;
                            tr.appendChild(tdItem);

                            const tdLokasi = document.createElement('td');
                            tdLokasi.textContent = item.location;
                            tr.appendChild(tdLokasi);

                            const tdClarity = document.createElement('td');
                            tdClarity.textContent = item.clarity;
                            tr.appendChild(tdClarity);

                            const tdKadarLogam = document.createElement('td');
                            tdKadarLogam.textContent = item.kadar_logam;
                            tr.appendChild(tdKadarLogam);

                            const tdHarga = document.createElement('td');
                            tdHarga.style.textAlign = 'right';
                            tdHarga.textContent = item.harga_barcode.toLocaleString("id-ID", {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2
                            });;
                            tr.appendChild(tdHarga);

                            const tdPWP = document.createElement('td');
                            tdPWP.style.textAlign = 'right';
                            tdPWP.textContent = item.harga_pwp.toLocaleString("id-ID", {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2
                            });;
                            tr.appendChild(tdPWP);

                            tbody.appendChild(tr);
                        });
                        btn_export.style.display = 'block';
                        btn_summary.style.display = 'block';
                        toastr.success('Data Load Successfully');
                        document.getElementById("overlay").style.display = "none";
                    }
                    else {
                        btn_export.style.display = 'none';
                        btn_summary.style.display = 'none';
                        toastr.info('Data Kosong');
                        document.getElementById("overlay").style.display = "none";
                    }
                }
                else {
                    btn_export.style.display = 'none';
                    btn_summary.style.display = 'none';
                    toastr.error(obj.error);
                    document.getElementById("overlay").style.display = "none";
                }
            }).catch(error => toastr.error(error));
        }

        function exportData() {
            document.getElementById("overlay").style.display = "flex";
            const selectedIds = [];

            const rows = document.querySelectorAll("#tb tbody tr");

            rows.forEach(row => {
                const firstTd = row.querySelector("td:first-child");

                if (firstTd) {
                    const id = firstTd.getAttribute("data-id");
                    selectedIds.push(id);
                }
            });


            fetch("ExportPWP.aspx/Export", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify({ idproducts: selectedIds })
            }).then(response => response.json()).then(res => {
                const result = res.d; // ASMX return selalu di dalam .d

                if (!result.success) {
                    alert(result.error);
                    return;
                }

                // Convert base64 ke Blob
                const byteCharacters = atob(result.filedata);
                const byteNumbers = new Array(byteCharacters.length);

                for (let i = 0; i < byteCharacters.length; i++) {
                    byteNumbers[i] = byteCharacters.charCodeAt(i);
                }

                const byteArray = new Uint8Array(byteNumbers);
                let mimeType = "text/csv";

                if (result.filename.toLowerCase().endsWith(".zip")) {
                    mimeType = "application/zip";
                }

                const blob = new Blob([byteArray], { type: mimeType });

                const link = document.createElement("a");
                link.href = URL.createObjectURL(blob);
                link.download = result.filename;
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);

                document.getElementById("overlay").style.display = "none";
            });
        }
    </script>
</asp:Content>
