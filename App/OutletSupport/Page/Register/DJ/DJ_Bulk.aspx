<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_Bulk.aspx.cs" Inherits="Page_DJ_Bulk" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Promo Diamond Jewellery Bulk
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
    </style>
    <form runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-4">
                <div class="form-group">
                    <label>
                        Nama Promo:
                    </label>
                    <%--<asp:RequiredFieldValidator ID="nama_RF" runat="server" />--%>
                    <%--<asp:TextBox ID="nama" runat="server" CssClass="form-control" MaxLength="50" onblur="OnKeyUpNamaPromo(this);" />--%>
                    <asp:TextBox ID="nama" runat="server" CssClass="form-control" MaxLength="50"/>
                </div>
            </div>
            <div class="col-xs-12 col-sm-4">
                <div class="row">
                    <div class="form-group col-xs-12 col-sm-6">
                        <label>
                            Tanggal Mulai:
                        </label>
                        <%--<asp:RequiredFieldValidator ID="datepicker_RF" runat="server" />--%>
                        <asp:TextBox CssClass="form-control text-center" type="text" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" Width="120" style="cursor:default;background-color:white;"/>
                    </div>
                    <div class="form-group col-xs-12 col-sm-6">
                        <label>
                            Tanggal Expire:
                        </label>
                        <%--<asp:RequiredFieldValidator ID="datepicker2_RF" runat="server" />--%>
                        <asp:TextBox CssClass="form-control text-center" type="text" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" Width="120" style="cursor:default;background-color:white;"/>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-4">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <label>
                            Modifikasi:
                        </label>
                        <div class="form-group">
                            <label>Tipe Buyback</label>
                            <asp:DropDownList runat="server" ID="ddltipebuyback" onchange="OnChangeTipeBuyback(this);" CssClass="form-control">
                                <asp:ListItem Text="Custom" Value="0"></asp:ListItem>
                                <asp:ListItem Text="DJ Emas" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Non Diamond Emas (GJ)" Value="2"></asp:ListItem>
                                <asp:ListItem Text="DJ Platinum" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Non Diamond Platinum (GJ)" Value="4"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox ID="statusasp" runat="server" Checked="true" />
                                    No Exchange / No Resell (ASP)
                                </label>
                            </div>
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox ID="statusmembership" runat="server" Checked="true" />
                                    No Discount Membership
                                </label>
                            </div>
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox ID="statusdiscnorm" runat="server" Checked="true" />
                                    No Discount Normal
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6">
                <div class="form-group">
                    <div class="row col-md-5" style="margin-right: 3px">
                        <asp:FileUpload runat="server" ID="fileupload" CssClass="form-control" onchange="ValidatorFile();" />
                    </div>
                    <button id="upload" runat="server" class="btn btn-primary" onserverclick="Upload">
                        <i class="icon-upload"></i>
                        Upload
                    </button>
                    <button id="template" runat="server" class="btn btn-primary" onserverclick="DownloadTemplate">
                        <i class="icon-download"></i>
                        Download Template
                    </button>
                </div>
            </div>
        </div>

        <div class="col-xs-12">
            <div class="text-danger">
                <asp:Literal runat="server" ID="lit_msg" />
            </div>
        </div>

        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table table-bordered">
                <asp:TableHeaderRow Style="background-color: lightblue">
                    <asp:TableHeaderCell Width="30">
                        <asp:CheckBox ID="chk_all" runat="server" onchange="OnCheckAll();"/>
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Image</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="155">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="155">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">T. Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">T. Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Harga Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">One Price</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <button runat="server" class="btn btn-success" type="button" onclick="OnClickModalSave();" id="btn_click_modal_save">Save</button>
        <!-- Modal -->
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title" id="myModalLabel">WARNING !!!
                        </h4>
                    </div>
                    <div class="modal-body">
                        Apakah anda yakin ingin approve Promo One Price Per Kode Karet berikut? Jika anda sudah yakin, klik tombol 'Yakin', jika tidak klik tombol 'Batal'.
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">
                            Batal
                        </button>
                        <button
                            type="button"
                            class="btn btn-primary"
                            id="btn_save"
                            data-dismiss="modal" onclick="OnSaveClick();">
                            Yakin
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script>
        $(function () {
            $('#content_datepicker').datepicker({
                format: 'dd-mm-yyyy'
            });
            $('#content_datepicker2').datepicker({
                format: 'dd-mm-yyyy'
            });

            $('#content_datepicker').attr("readonly", true);
            $('#content_datepicker2').attr("readonly", true);

            $('#content_statusmembership').attr("disabled", false);
            $('#content_statusasp').attr("disabled", false);
            $('#content_statusdiscnorm').attr("disabled", false);

            $('#content_promoharga').keyup(function () {
                this.value = this.value.replace(/[^0-9]/g, '').replace(/\B(?=(\d{3})+(?!\d))/g, '.');
            })

            $('#content_upload').attr("disabled", true);
        });

        function ValidatorFile() {
            var filename = document.getElementById('content_fileupload').value.split("\\").pop();
            var extension = filename.split(".").pop();
            if (extension == "xls" || extension == "xlsx") {
                document.getElementById('content_upload').removeAttribute("disabled");
            }
            else {
                alert("File extension must be '.xls' or '.xlsx'.");
                document.getElementById('content_fileupload').value = null;
                document.getElementById('content_upload').setAttribute("disabled", "disabled");
            }
        }

        function OnChangeTipeBuyback(e) {
            //custom
            if (e.value == "0") {
                $('#content_statusmembership').prop("checked", true);
                $('#content_statusasp').prop("checked", true);
                $('#content_statusdiscnorm').prop("checked", true);

                $('#content_statusmembership').prop("disabled", false);
                $('#content_statusasp').prop("disabled", false);
                $('#content_statusdiscnorm').prop("disabled", false);
            }
            //dj emas
            else if (e.value == "1") {
                $('#content_statusmembership').prop("checked", false);
                $('#content_statusasp').prop("checked", false);
                $('#content_statusdiscnorm').prop("checked", false);

                $('#content_statusmembership').prop("disabled", true);
                $('#content_statusasp').prop("disabled", true);
                $('#content_statusdiscnorm').prop("disabled", true);
            }
            // non diamond emas
            else if (e.value == "2") {
                $('#content_statusmembership').prop("checked", true);
                $('#content_statusasp').prop("checked", false);
                $('#content_statusdiscnorm').prop("checked", true);

                $('#content_statusmembership').prop("disabled", true);
                $('#content_statusasp').prop("disabled", true);
                $('#content_statusdiscnorm').prop("disabled", true);
            }
            // dj platinum
            else if (e.value == "3") {
                $('#content_statusmembership').prop("checked", false);
                $('#content_statusasp').prop("checked", true);
                $('#content_statusdiscnorm').prop("checked", false);

                $('#content_statusmembership').prop("disabled", true);
                $('#content_statusasp').prop("disabled", true);
                $('#content_statusdiscnorm').prop("disabled", true);
            }
            // non dj platinum
            else if (e.value == "4") {
                $('#content_statusmembership').prop("checked", true);
                $('#content_statusasp').prop("checked", true);
                $('#content_statusdiscnorm').prop("checked", true);

                $('#content_statusmembership').prop("disabled", true);
                $('#content_statusasp').prop("disabled", true);
                $('#content_statusdiscnorm').prop("disabled", true);
            }
        }

        function OnClickModalSave() {
            $('#myModal').modal('show');
        }

        function OnSaveClick() {
            let nama = document.getElementById('content_nama').value;
            let keterangan = "";
            let tglstart = document.getElementById('content_datepicker').value;
            let tglexpired = document.getElementById('content_datepicker2').value;
            let no_ex = document.getElementById('content_statusasp').checked;
            let no_dm = document.getElementById('content_statusmembership').checked;
            let no_dn = document.getElementById('content_statusdiscnorm').checked;

            let val_nama = "";
            let val_tanggal = "";

            let partsstart = tglstart.split("-");

            let daystart = partsstart[0];
            let monthstart = partsstart[1];
            let yearstart = partsstart[2];

            // Create a new date string in YYYY-MM-DD format
            let formattedDatestart = `${yearstart}-${monthstart}-${daystart}`;

            let partsexpired = tglexpired.split("-");

            let dayexpired = partsexpired[0];
            let monthexpired = partsexpired[1];
            let yearexpired = partsexpired[2];

            // Create a new date string in YYYY-MM-DD format
            let formattedDateexpired = `${yearexpired}-${monthexpired}-${dayexpired}`;

            let d_tglstart = new Date(formattedDatestart);
            let d_tglexpired = new Date(formattedDateexpired);
            let d_tglnow = new Date();

            let y = d_tglnow.getFullYear();
            let m = String(d_tglnow.getMonth() + 1).padStart(2, '0');
            let d = String(d_tglnow.getDate()).padStart(2, '0');

            let dateOnly = `${y}-${m}-${d}`;
            let d_tglnownew = new Date(dateOnly);

            if (!nama)
                val_nama = "Nama Promo Tidak Boleh Kosong."

            if (d_tglexpired < d_tglstart)
                val_tanggal = "Tanggal Expired Tidak Boleh Kurang Dari Tanggal Mulai.";

            if (d_tglexpired < d_tglnownew)
                val_tanggal = "Tanggal Expired Tidak Boleh Kurang Dari Tanggal Hari Ini.";

            if (d_tglstart < d_tglnownew)
                val_tanggal = "Tanggal Start Tidak Boleh Kurang Dari Tanggal Hari Ini.";

            if (val_nama == "" && val_tanggal == "") {
                let list = [];
                let table = document.getElementById('content_tb');
                let row = table.rows;
                // Iterate over all rows to count the cells in each row
                for (let i = 1; i < row.length; i++) {
                    let ischecked = row[i].cells[0].querySelector('input[type="checkbox"]').checked;
                    if (ischecked) {
                        let kodekaret = row[i].cells[3].innerText;
                        let plu = row[i].cells[4].innerText;
                        let oneprice = parseFloat(row[i].cells[12].innerText.replace(/\./g, '').replace(/,/g, '.'));
                        let pd = new Product(kodekaret, plu, oneprice);
                        list.push(pd);
                    }
                }

                let pds = new Products(nama, keterangan, tglstart, tglexpired, list, no_ex, no_dm, no_dn);
                
                $.ajax({
                    type: "POST",
                    url: "DJ_Bulk.aspx/Save",
                    data: JSON.stringify({ products: pds }),
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function (request) {
                        request.withCredentials = false;
                    },
                    success: function (result) {
                        let result_new = result.d;
                        if (result_new.success) {
                            alert("Promo '" + nama + "' berhasil Dibuat. Check di Menu Approval Untuk Approve.");

                            let newUrl = window.location.protocol + "//" + window.location.host + "/OutletSupport/Page/Approval/DJ/DJ.aspx";

                            setTimeout(function () {
                                window.location.href = newUrl;
                            }, 1000);
                        }
                        else {
                            alert(result_new.msg);
                        }
                    },
                    error: function (xhr, status, error) {
                        alert(error);
                    }
                });
            }
            else {
                if (val_nama != "")
                    alert(val_nama);
                else
                    alert(val_tanggal);
            }
        }

        function OnCheckAll() {
            let cek = document.getElementById('content_chk_all');
            let table = document.getElementById('content_tb');
            let row = table.rows;

            // Iterate over all rows to count the cells in each row
            for (let i = 1; i < row.length; i++) {
                row[i].cells[0].querySelector('input[type="checkbox"]').checked = cek.checked;
            }
        }

        function OnCheckOne(e) {
            let cek = document.getElementById('content_chk_all');
            let table = document.getElementById('content_tb');
            let row = table.rows;
            let checkedz = e.querySelector('input[type="checkbox"]').checked;

            let counttrue = 0;
            let countfalse = 0;
            // Iterate over all rows to count the cells in each row

            if (checkedz) {
                for (let i = 1; i < row.length; i++) {
                    let x = row[i].cells[0].querySelector('input[type="checkbox"]');
                    if (x.checked) {
                        counttrue++;
                    }
                }

                if (counttrue == row.length - 1) {
                    cek.checked = checkedz;
                }
            }
            else {
                for (let i = 1; i < row.length; i++) {
                    let x = row[i].cells[0].querySelector('input[type="checkbox"]');
                    if (!x.checked) {
                        countfalse++;
                    }
                }

                if (countfalse != row.length - 1) {
                    cek.checked = checkedz;
                }
            }
        }

        function OnKeyUpNamaPromo(e) {
            $.ajax({
                type: "POST",
                url: "DJ_Bulk.aspx/CheckAvailableNamaPromo",
                data: JSON.stringify({ "nama": e.value }),
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                beforeSend: function (request) {
                    request.withCredentials = false;
                },
                success: function (result) {
                    let result_new = result.d;
                    if (result_new.success) {

                    }
                    else {

                    }
                },
                error: function (xhr, status, error) {

                }
            });
        }

        function Products(Nama, Keterangan, TglStart, TglExpired, products, NoExchange, NoDiscountMember, NoDiscountNormal) {
            this.Nama = Nama;
            this.Keterangan = Keterangan;
            this.TglStart = TglStart;
            this.TglExpired = TglExpired;
            this.products = products;
            this.NoExchange = NoExchange;
            this.NoDiscountMember = NoDiscountMember;
            this.NoDiscountNormal = NoDiscountNormal;
        }

        function Product(KodeKaret, PLU, OnePrice) {
            this.KodeKaret = KodeKaret;
            this.PLU = PLU;
            this.OnePrice = OnePrice;
        }
    </script>
</asp:Content>
