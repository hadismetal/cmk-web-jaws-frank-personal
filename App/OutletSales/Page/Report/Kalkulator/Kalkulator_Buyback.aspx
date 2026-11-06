<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Kalkulator_Buyback.aspx.cs" Inherits="Page_Report_Kalkulator_Kalkulator_Buyback" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Kalkulator Buyback
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Report
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server" class="form-inline">
        <div class="form-group">
            <div class="text-muted">
                Search
            </div>
            <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" ClientIDMode="Static" />
        </div>
        <div class="form-group">
            <div class="text-muted">
                Search By
            </div>
            <asp:DropDownList ID="searchby" runat="server" CssClass="form-control" Width="100" ClientIDMode="Static">
                <asp:ListItem Value="QC">QC</asp:ListItem>
                <asp:ListItem Value="PLU">PLU</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                &nbsp;
            </div>
            <asp:DropDownList ID="tipe" runat="server" CssClass="form-control" Width="200" ClientIDMode="Static">
                <asp:ListItem Value="DJ">Diamond Jewellery</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                &nbsp;
            </div>
            <button id="go" type="button" class="btn btn-primary">Display</button>
            <small runat="server" class="text-danger" id="lblwarning" visible="false"></small>
        </div>
        <div class="table-responsive" style="max-width: 50%">
            <label id="lbl_top_transaction" class="text-danger">Display Only Top 20 Data...</label>
            <table id="tb" class="table table-bordered" style="white-space: nowrap;">
                <thead>
                    <tr>
                        <th>&nbsp;</th>
                        <th>No.</th>
                        <th>Image</th>
                        <th>PLU</th>
                        <th>No Doc QC</th>
                        <th>No SO</th>
                        <th>Tgl. SO</th>
                        <th>Tipe</th>
                        <th>Item</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
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
        });

        class RequestQC {
            constructor({
                kw = "",
                searchby = "",
                tipe_product = ""
            } = {}) {
                this.kw = kw;
                this.searchby = searchby;
                this.tipe_product = tipe_product;
            }
        }

        document.getElementById('go').addEventListener('click', function () {
            const url = `Kalkulator_Buyback.aspx/Display`
            const kawe = document.getElementById('kw').value;
            const search_by = document.getElementById('searchby').value;
            const tipe_product = document.getElementById('tipe').value;

            this.setAttribute('disabled', true);

            if (kawe !== '') {
                const request = new RequestQC({
                    kw: kawe,
                    searchby: search_by,
                    tipe_product: tipe_product
                })

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
                        const datas = obj.data;
                        const tbody = document.querySelector("#tb tbody");

                        // Clear existing rows
                        tbody.innerHTML = '';

                        if (datas.length > 0) {
                            toastr.success('Data Load Successfully');
                            datas.forEach((item, index) => {
                                const tr = document.createElement('tr');

                                const tdHref = document.createElement('td');
                                tdHref.innerHTML = `<a class='btn btn-primary' target='_blank' href='${item.href}'><i class='icon-search'></button>`;
                                tr.appendChild(tdHref);

                                const tdNo = document.createElement('td');
                                tdNo.textContent = index + 1;
                                tr.appendChild(tdNo);

                                const tdImage = document.createElement('td');
                                tdImage.innerHTML = `<img class='img-thumbnail' src='${item.image}' />`;
                                tr.appendChild(tdImage);

                                const tdPLU = document.createElement('td');
                                tdPLU.textContent = item.plu;
                                tr.appendChild(tdPLU);

                                const tdNoQC = document.createElement('td');
                                tdNoQC.textContent = item.no_qc;
                                tr.appendChild(tdNoQC);

                                const tdNoSO = document.createElement('td');
                                tdNoSO.textContent = item.no_so;
                                tr.appendChild(tdNoSO);

                                const tdTglSO = document.createElement('td');
                                tdTglSO.textContent = item.tgl_so;
                                tr.appendChild(tdTglSO);

                                const tdTipe = document.createElement('td');
                                tdTipe.textContent = 'DJ';
                                tr.appendChild(tdTipe);

                                const tdItem = document.createElement('td');
                                tdItem.textContent = item.item;
                                tr.appendChild(tdItem);

                                tbody.appendChild(tr);
                            });
                        } else {
                            toastr.info('Data Kosong.');
                        }
                    } else {
                        toastr.error(obj.error);
                    }
                }).catch(error => toastr.error(error))
            } else {
                toastr.info('PLU / QC belum diinput');
                document.getElementById('kw').focus();
            }

            this.removeAttribute('disabled');
        });

    </script>
</asp:Content>
