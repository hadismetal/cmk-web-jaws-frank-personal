<%@ Page EnableEventValidation="false" Language="C#"
    MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="PreLEI.aspx.cs" Inherits="PreLEI" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    PreLEI Reborn
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
        .custom-file-upload {
            display: inline-block;
            padding: 6px 12px;
            cursor: pointer;
            background-color: white;
            color: black;
            border-radius: 4px;
            font-size: 14px;
            font-weight: 500;
            border: 1px solid #2e6da4;
            margin: 0px;
            width: 200px
        }

        .custom-file-upload:hover {
            background-color: #286090;
            border-color: #204d74;
            color: white;
        }

        input[type="file"] {
            display: none;
        }
    </style>
    <div class="modal" role="dialog" id="modal-example" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Warning</h5>
                    <button
                        type="button"
                        class="close"
                        data-dismiss="modal"
                        aria-label="Close">
                        <span aria-hidden="true"></span>
                    </button>
                </div>
                <div class="modal-body text-justify">
                    <h3>
                        <b style="font-family: Consolas">Nomor Retur <span id="no_rtr"></span>akan dikirimkan ke lokasi
                          tujuan <span id="wrh_prelei"></span>brand PreLEI. Mohon dicek
                          kembali plu dan lokasi tujuan. Karena Nomor Retur yang sudah
                          dikirim tidak dapat dikirim kembali!!!
                        </b>
                    </h3>
                </div>
                <div class="modal-footer">
                    <button
                        class="btn btn-success"
                        runat="server"
                        onserverclick="save_Click"
                        onclick="issentreceipt();">
                        Lanjutkan
                    </button>
                    <button type="button" class="btn btn-muted" data-dismiss="modal">
                        Cancel
                    </button>
                    <script type="text/javascript">
                        function issentreceipt() {
                            $("#modal-example").modal("hide");
                        }
                    </script>
                </div>
            </div>
        </div>
    </div>
    <div class="modal" role="dialog" id="modal-tgp" data-backdrop="static">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Warning</h5>
                    <button
                        type="button"
                        class="close"
                        data-dismiss="modal"
                        aria-label="Close">
                        <span aria-hidden="true"></span>
                    </button>
                </div>
                <div class="modal-body text-justify">
                    <h3 style="font-family: Consolas">
                        <b>Input Gagal, Kadar Tukaran Yang Tersedia Adalah:</b>
                    </h3>
                    <ul style="font-family: Consolas; padding-left: 15px;">
                        <li style="margin-right: 20px;">Preloved Excellent 75% (18K)  ->  84</li>
                        <!--<li style="margin-right: 20px;">Preloved Good 75% (18K) 	  ->  80</li>-->
                        <li style="margin-right: 20px;">Preloved Excellent 37,5% (9K) ->  40</li>
                        <!--<li style="margin-right: 20px;">Preloved Good 37,5%	(9K) 	  ->  38</li>-->
                    </ul>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-muted" data-dismiss="modal">
                        Close
                    </button>
                </div>
            </div>
        </div>
    </div>
    <form runat="server" class="form-inline">
        <div class="form-group">
            <div class="text-muted">Tipe Product :</div>
            <asp:DropDownList
                runat="server"
                ID="ddl_tipe_plu"
                CssClass="form-control"
                Width="200" onchange="onChangeTipeProduct(this);">
                <asp:ListItem Selected="True" Value="PG"> Plain Gold </asp:ListItem>
                <asp:ListItem Value="DJ"> Diamond Jewellery </asp:ListItem>
                <%--<asp:ListItem Value="GJ"> Gold Jewellery </asp:ListItem>--%>
                <%--<asp:ListItem Value="LD"> Loose Diamond </asp:ListItem>--%>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">NO. RTS :</div>
            <asp:TextBox
                ID="txt_rts"
                runat="server"
                CssClass="form-control"
                AutoPostBack="true"
                OnTextChanged="refreshPage"
                Width="200" />
        </div>
        <div class="form-group">
            <div class="text-muted">Lokasi Tujuan :</div>
            <asp:DropDownList
                runat="server"
                ID="ddl_to"
                CssClass="form-control"
                Width="200">
                <asp:ListItem Value="0">All</asp:ListItem>
                <asp:ListItem Value="83">SK - PUSAT CMK LEI</asp:ListItem>
                <asp:ListItem Value="85">SK - PRELOVED LEI</asp:ListItem>
                <asp:ListItem Value="86">SK - REPAIR LEI</asp:ListItem>
                <asp:ListItem Value="87">SK - LEBUR LEI</asp:ListItem>
                <asp:ListItem Value="91">Preloved K2</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <asp:Button
                    ID="btn_inputproduct"
                    OnClick="refreshPage"
                    Text="Enter"
                    runat="server"
                    class="btn btn-primary">
                </asp:Button>
            </div>
        </div>
        <div class="form-group" id="div_download_template">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <asp:Button
                    ID="btn_download_template"
                    OnClick="DownloadTemplate"
                    Text="Download Template"
                    runat="server"
                    CssClass="btn btn-primary">
                </asp:Button>
            </div>
        </div>
        <div class="row col-md-12" id="div_upload_file">
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <label for="fileupload" class="custom-file-upload">
                        Choose File
                    </label>
                    <asp:FileUpload ID="fileupload"
                        runat="server" Style="display: none" />
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <asp:Button
                        ID="btn_upload"
                        OnClick="UploadRTS"
                        Text="Upload"
                        runat="server"
                        class="btn btn-primary">
                    </asp:Button>
                </div>
            </div>
        </div>
        <div class="row col-md-12">
            <div class="form-group">
                <asp:Label ID="lblFileName" runat="server" Text="" Style="margin-left: 10px;"></asp:Label>
            </div>
        </div>
        <div class="form-group">
            <div style="font-family:'Lucida Console';font-size:large;color:maroon">
                <asp:Literal runat="server" ID="lit_valid" />
            </div>
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list"></asp:Table>
        </div>
        <asp:HiddenField runat="server" ID="hf_id" />
        <asp:HiddenField runat="server" ID="hf_warehouse_id" />
        <asp:HiddenField runat="server" ID="hf_tipe_plu" />
        <asp:HiddenField runat="server" ID="hf_keterangan" />
        <div class="button-inline">
            <asp:Button
                ID="save"
                runat="server"
                CssClass="btn btn-primary"
                OnClick="save_Click"
                Visible="false"
                Text="Finish"
                OnClientClick="onconfirmexample(); return false;"></asp:Button>
        </div>
    </form>
    <script type="text/javascript">
        // Show selected file name in label (optional enhancement)
        document.addEventListener("DOMContentLoaded", function () {
            let fileInput = document.getElementById("content_fileupload");
            let label = document.getElementById("content_lblFileName");
            let triggerLabel = document.querySelector("label[for='fileupload']");
            let btnupload = document.getElementById("content_btn_upload");

            btnupload.setAttribute("disabled","disabled");

            triggerLabel.addEventListener("click", function () {
                fileInput.click();
            });

            fileInput.addEventListener("change", function () {
                let allowedExtensions = [".xlsx", ".xls"];
                if (fileInput.files.length > 0) {
                    let fileName = fileInput.files[0].name;
                    let ext = fileName.substring(fileName.lastIndexOf('.')).toLowerCase();

                    if (!allowedExtensions.includes(ext)) {
                        label.textContent = "❌ Invalid file type. Please upload an Excel file.";
                        label.style.color = "red";
                        fileInput.value = ""; // Reset file input
                        btnupload.setAttribute("disabled","disabled");
                    } else {
                        label.textContent = fileName;
                        label.style.color = "black";
                        btnupload.removeAttribute("disabled");
                    }
                }
            });
        });

        function onconfirmexample() {
            var toko = $("#content_ddl_to").find(":selected").text();
            if (toko != "All") {
                $("#wrh_prelei").text(toko);
                $("#no_rtr").text($("#content_txt_rts").val());
                $("#modal-example").modal("show");
            } else {
                $("#content_ddl_to").focus();
                alert("Harap Pilih Lokasi Tujuan.");
            }
        }

        function onbluray(e) {
            const getTableData = $("input[id*='actw']");
            let total = 0;
            $(getTableData).each((idx, elem) => {
                total += parseFloat($(elem).val());
            });
            $("#cuy").text(total.toFixed(2));
        }

        function onValidateWeight(e) {
            const weightact = e.closest('td').getElementsByTagName('input')[0].value;
            const weightori = e.closest('td').getElementsByTagName('input')[2].value;

            if (weightact === "") {
                alert("Berat Tidak Valid");
                e.closest('td').getElementsByTagName('input')[0].value = weightori;
            }
            else if (weightact.toLowerCase().includes('e')) {
                alert("Berat Tidak Valid");
                e.closest('td').getElementsByTagName('input')[0].value = weightori;
            }
            else if (weightact.includes('-')) {
                alert("Berat Tidak Valid");
                e.closest('td').getElementsByTagName('input')[0].value = weightori;
            }
            else if (isNaN(weightact)) {
                alert("Berat Tidak Valid");
                e.closest('td').getElementsByTagName('input')[0].value = weightori;
            }
            else {
                if (parseFloat(weightact) > 100000) {
                    alert("Berat Maksimal 100.000 gr");
                    e.closest('td').getElementsByTagName('input')[0].value = weightori;
                }
                else if (parseFloat(weightact) <= 0) {
                    alert("Berat Harus Lebih dari 0 ");
                    e.closest('td').getElementsByTagName('input')[0].value = weightori;
                }
                else {
                    e.closest('td').getElementsByTagName('input')[0].value = parseFloat(weightact);
                }
            }

            const getTableData = $("input[id*='actw']");
            let total = 0;
            $(getTableData).each((idx, elem) => {
                total += parseFloat($(elem).val());
            });
            $("#cuy").text(total.toFixed(2));
        }

        function onValidateKadarTukaranBeli(e) {
            const kadartukaranbeliact = e.closest('td').getElementsByTagName('input')[0].value;
            const kadartukaranbeliori = e.closest('td').getElementsByTagName('input')[1].value;

            if (kadartukaranbeliact === "") {
                alert("Kadar Tukaran Beli Tidak Valid");
                e.closest('td').getElementsByTagName('input')[0].value = kadartukaranbeliori;
            }
            else if (kadartukaranbeliact.toLowerCase().includes('e')) {
                alert("Kadar Tukaran Beli Tidak Valid");
                e.closest('td').getElementsByTagName('input')[0].value = kadartukaranbeliori;
            }
            else if (kadartukaranbeliact.includes('-')) {
                alert("Kadar Tukaran Beli Tidak Valid");
                e.closest('td').getElementsByTagName('input')[0].value = kadartukaranbeliori;
            }
            else if (isNaN(kadartukaranbeliact)) {
                alert("Kadar Tukaran Beli Tidak Valid");
                e.closest('td').getElementsByTagName('input')[0].value = kadartukaranbeliori;
            }
            else {
                if (parseFloat(kadartukaranbeliact) > 100) {
                    alert("Kadar Tukaran Beli Maksimal 100");
                    e.closest('td').getElementsByTagName('input')[0].value = kadartukaranbeliori;
                }
                else if (parseFloat(kadartukaranbeliact) <= 0) {
                    alert("Kadar Tukaran Beli Harus Lebih dari 0 ");
                    e.closest('td').getElementsByTagName('input')[0].value = kadartukaranbeliori;
                }
            }
        }

        function onValidateKadarTukaranJual(e) {
            const kadartukaranjualact = e.closest('td').getElementsByTagName('input')[0].value;
            const kadartukaranjualori = e.closest('td').getElementsByTagName('input')[1].value;

            if (kadartukaranjualact === "") {
                alert("Kadar Tukaran Jual Tidak Valid");
                e.closest('td').getElementsByTagName('input')[0].value = kadartukaranjualori;
            }
            else if (kadartukaranjualact.toLowerCase().includes('e')) {
                alert("Kadar Tukaran Jual Tidak Valid");
                e.closest('td').getElementsByTagName('input')[0].value = kadartukaranjualori;
            }
            else if (kadartukaranjualact.includes('-')) {
                alert("Kadar Tukaran Jual Tidak Valid");
                e.closest('td').getElementsByTagName('input')[0].value = kadartukaranjualori;
            }
            else if (isNaN(kadartukaranjualact)) {
                alert("Kadar Tukaran Jual Tidak Valid");
                e.closest('td').getElementsByTagName('input')[0].value = kadartukaranjualori;
            }
            else {
                if (parseFloat(kadartukaranjualact) > 100) {
                    alert("Kadar Tukaran Jual Maksimal 100");
                    e.closest('td').getElementsByTagName('input')[0].value = kadartukaranjualori;
                }
                else if (parseFloat(kadartukaranjualact) <= 0) {
                    alert("Kadar Tukaran Jual Harus Lebih dari 0 ");
                    e.closest('td').getElementsByTagName('input')[0].value = kadartukaranjualori;
                }
                else {
                    onFindMapping(e);
                }
            }
        }

        function onFindMapping(e) {
            const url = "PreLEI.aspx/GetMappingLevel";
            const row = e.closest('tr');
            const innerrow = row.cells[15].getElementsByTagName('span')[0].innerText;
            const kadartukaranori = row.cells[13].innerText;
            const levelori = row.cells[15].getElementsByTagName('input')[0].value;
            const tipe_pg = row.cells[8].getElementsByTagName('select')[0].value;
            
            const value = (e.value == '' || e.value == null) ? "0" : e.value;

            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    kadartukaranjual: value,
                    kadartukaranjualori: parseFloat(kadartukaranori),
                    producttype: tipe_pg
                })
            })
                .then(response => response.json('test')) // or .text(), .blob(), etc.
                .then(data => {
                    if (data.d.Name !== "")
                        row.cells[15].getElementsByTagName('span')[0].innerText = data.d.Name;
                    else {
                        $('#modal-tgp').modal('show');
                        row.cells[14].getElementsByTagName('input')[0].value = kadartukaranori;
                        row.cells[15].getElementsByTagName('span')[0].innerText = levelori;
                        row.cells[14].getElementsByTagName('input')[0].focus();
                    }
                })
                .catch(error => {
                    console.log(error);
                });
        }

        function onFindMappingDDL(e) {
            const url = "PreLEI.aspx/GetMappingLevel";
            const row = e.closest('tr');
            const innerrow = row.cells[15].getElementsByTagName('span')[0].innerText;
            const kadartukaranori = row.cells[13].innerText;
            const levelori = row.cells[15].getElementsByTagName('input')[0].value;
            const value = row.cells[14].getElementsByTagName('input')[0].value;
            
            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    kadartukaranjual: value,
                    kadartukaranjualori: parseFloat(kadartukaranori),
                    producttype: e.value
                })
            })
                .then(response => response.json()) // or .text(), .blob(), etc.
                .then(data => {
                    if (data.d.Name !== "")
                        row.cells[15].getElementsByTagName('span')[0].innerText = data.d.Name;
                    else {
                        $('#modal-tgp').modal('show');
                        row.cells[14].getElementsByTagName('input')[0].value = kadartukaranori;
                        row.cells[15].getElementsByTagName('span')[0].innerText = levelori;
                        row.cells[14].getElementsByTagName('input')[0].focus();
                    }
                })
                .catch(error => {
                    console.log(error);
                });
        }

        function onChangeTipeProduct(e) {
            const choose = e.value;
            
            if (choose == "DJ") {
                document.getElementById('div_download_template').style.display = "none";
                document.getElementById('div_upload_file').style.display = "none";
                document.getElementById('content_save').style.display = "none";
                document.getElementById('content_txt_rts').value = "";
            
                const tbody = document.getElementById("content_tb");
                tbody.innerHTML = ``;
                tbody.innerHTML = `
                  <tr class="no-result">
                    <td colspan="1" style="width:990px;">No Data</td>
                  </tr>
                `;
            }
            else if (choose == "PG") {
                document.getElementById('div_download_template').style.display = "";
                document.getElementById('div_upload_file').style.display = "";
                document.getElementById('content_save').style.display = "none";
                document.getElementById('content_txt_rts').value = "";

                
                const tbody = document.querySelector("content_tb");
                tbody.innerHTML = ``;
                tbody.innerHTML = `
                  <tr class="no-result">
                    <td colspan="1" style="width:990px;">No Data</td>
                  </tr>
                `;
            }
        }
    </script>
</asp:Content>
