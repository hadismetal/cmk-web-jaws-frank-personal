<%@ Page Language="C#" MasterPageFile="~/MasterPagePGOuts.master" AutoEventWireup="true" CodeFile="PG_Outs.aspx.cs" Inherits="Page_Register_PG" %>

<%@ Register Src="../MenuPG.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
        html {
            font-size: 16px;
            overflow-x: hidden;
        }

        body {
            font-size: 16px;
        }

        h1 {
            border: none;
        }
    </style>
    <link href="/UI/Tailwind/tailwind-output.css" rel="stylesheet" />
    <div class="flex w-full flex-col gap-5 p-5">
        <div class="w-full md:w-3/4 lg:w-1/2">
            <h1 class="m-0 p-0 text-lg font-bold uppercase">Product Detail</h1>
            <div class="mt-5 flex w-full flex-col gap-5">
                <div>
                    <label for="SelectWarehouse" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Warehouse</label>
                    <select id="SelectWarehouse" class="select2" data-form="IDWarehouse"></select>
                </div>
                <div class="grid grid-cols-2 gap-5">
                    <div>
                        <label for="InputTanggal" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Tanggal*</label>
                        <input type="date" id="InputTanggal" data-form="Tanggal" data-required="1" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" />
                        <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>
                    </div>
                    <div>
                        <label for="InputTanggalTJT" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Tanggal TJT*</label>
                        <input type="date" id="InputTanggalTJT" data-form="TanggalTJT" data-required="1" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" />
                        <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>
                    </div>
                </div>
                <div>
                    <label for="SelectSupplier" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Supplier</label>
                    <select id="SelectSupplier" class="select2 min-w-full" data-form="IDSupplier"></select>
                </div>
                <div>
                    <label for="InputSupplier" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Supplier*</label>
                    <input type="text" id="InputSupplier" data-required="1" data-form="NamaSupplier" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Input Nama Supplier..." required />
                    <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>
                </div>
                <div>
                    <label for="InputSJSupplier" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">SJ Supplier*</label>
                    <input type="text" id="InputSJSupplier" data-required="1" data-form="NamaSJSupplier" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Input SJ Supplier..." required />
                    <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>
                </div>
                <div>
                    <label for="InputGoldRate" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Harga Emas / Gram (24K)*</label>
                    <input type="number" id="InputGoldRate" data-required="1" data-form="GoldRate" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Input Harga Emas..." required />
                    <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>

                </div>
                <div class="w-1/2">
                    <label class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">TGP / Fixed Rate</label>
                    <ul class="grid w-full gap-6 md:grid-cols-2">
                        <li>
                            <input type="radio" id="InputTGP" data-form="IsFixedRate" name="GoldRateType" value="0" class="peer hidden" checked required />
                            <label for="InputTGP" class="inline-flex w-full cursor-pointer items-center justify-between rounded-lg border border-gray-200 bg-white p-5 text-gray-500 peer-checked:border-blue-600 peer-checked:text-blue-600 hover:text-gray-600 hover:bg-gray-100 dark:border-gray-700 dark:text-gray-400 dark:bg-gray-800 dark:hover:text-gray-300 dark:peer-checked:text-blue-500 dark:hover:bg-gray-700">
                                <div class="block">
                                    <div class="w-full text-lg font-semibold">TGP</div>
                                </div>
                            </label>
                        </li>
                        <li>
                            <input type="radio" id="InputFixedRate" data-form="IsFixedRate" name="GoldRateType" value="1" class="peer hidden">
                            <label for="InputFixedRate" class="inline-flex w-full cursor-pointer items-center justify-between rounded-lg border border-gray-200 bg-white p-5 text-gray-500 peer-checked:border-blue-600 peer-checked:text-blue-600 hover:text-gray-600 hover:bg-gray-100 dark:border-gray-700 dark:text-gray-400 dark:bg-gray-800 dark:hover:text-gray-300 dark:peer-checked:text-blue-500 dark:hover:bg-gray-700">
                                <div class="block">
                                    <div class="w-full text-lg font-semibold">Fixed Rate</div>
                                </div>
                            </label>
                        </li>
                    </ul>
                </div>
                <div>
                    <label for="InputStockKeeper" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Stock Keeper*</label>
                    <input type="text" id="InputStockKeeper" data-required="1" data-form="StockKeeper" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Input Stock Keeper..." required />
                    <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>

                </div>
                <div>
                    <label for="InputRingSize" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Ring Size / Panjang Chain*</label>
                    <input type="number" id="InputRingSize" data-required="1" data-form="RingSize" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Input Ring Size..." required />
                    <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>

                </div>
                <div>
                    <label for="InputKeterangan" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Keterangan*</label>
                    <textarea type="text" id="InputKeterangan" data-required="1" data-form="Keterangan" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Input Keterangan..." required rows="5"></textarea>
                    <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>

                </div>
            </div>
        </div>
        <hr class="border-0 h-px bg-gray-200 dark:bg-gray-700" />
        <div class="w-full md:w-3/4 lg:w-1/2">
            <h1 class="m-0 p-0 text-lg font-bold uppercase">Product Information</h1>
            <div class="mt-5 grid w-full grid-cols-2 gap-5">
                <div>
                    <label for="SelectGoldLevel" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Product Level</label>
                    <select class="select2" id="SelectGoldLevel" data-form="ProductLevel"></select>
                </div>
                <div>
                    <label for="SelectGoldModel" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Product Model</label>
                    <select class="select2 min-w-full" id="SelectGoldModel" data-form="ProductModel"></select>
                </div>
                <div>
                    <label for="SelectProductItem" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Product Item</label>
                    <select class="select2 min-w-full" id="SelectProductItem" data-form="ProductItem"></select>
                </div>
                <div>
                    <label for="SelectFrameColor" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Warna Logam</label>
                    <select class="select2 min-w-full" id="SelectFrameColor" data-form="FrameColor"></select>
                </div>
                <div>
                    <label for="SelectTargetAge" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Kelompok Usia</label>
                    <select class="select2 min-w-full" id="SelectTargetAge" data-form="TargetAge"></select>
                </div>
                <div>
                    <label for="InputKadarLogam" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Kadar Logam*</label>
                    <input type="number" id="InputKadarLogam" data-required="1" data-form="KadarLogam" value="75" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Input Kadar Logam..." required />
                    <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>

                </div>
                <div>
                    <label for="InputKadarTukaranJual" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Kadar Tukaran Jual*</label>
                    <input type="number" id="InputKadarTukaranJual" data-required="1" data-form="KadarTukaranJual" value="82" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Input Kadar Tukaran Jual..." required />
                    <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>

                </div>
                <div>
                    <label for="InputKadarTukaranBeli" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Kadar Tukaran Beli*</label>
                    <input type="number" id="InputKadarTukaranBeli" data-required="1" data-form="KadarTukaranBeli" value="82" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Input Kadar Tukaran Beli..." required />
                    <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>

                </div>
                <div>
                    <label for="InputKodeBarang" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Kode Barang*</label>
                    <input type="text" id="InputKodeBarang" data-required="1" data-form="KodeBarang" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Input Kode Barang..." required />
                    <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>

                </div>
                <div>
                    <label for="InputNamaBarang" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Nama Barang*</label>
                    <input type="text" id="InputNamaBarang" data-required="1" data-form="NamaBarang" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Input Nama Barang..." required />
                    <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>

                </div>
                <div>
                    <label for="InputKodeMaterial" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Kode Material*</label>
                    <input type="text" id="InputKodeMaterial" data-required="1" data-form="KodeMaterial" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Input Kode Material..." required />
                    <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>

                </div>
                <div>
                    <label for="InputNomorValidasi" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Nomor Validasi*</label>
                    <input type="text" id="InputNomorValidasi" data-required="1" data-form="NomorValidasi" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Input Nomor Validasi..." required />
                    <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>
                </div>
            </div>
        </div>
        <hr class="border-0 h-px bg-gray-200 dark:bg-gray-700" />
        <div class="w-full md:w-3/4 lg:w-1/2">
            <div class="w-1/2">
                <label class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Printer Settings</label>
                <ul class="grid w-full gap-6 md:grid-cols-2">
                    <li>
                        <input type="radio" id="InputLogo" data-form="IsWithLogo" name="IsWithLogo" value="1" class="peer hidden" checked required />
                        <label for="InputLogo" class="inline-flex w-full cursor-pointer items-center justify-between rounded-lg border border-gray-200 bg-white p-5 text-gray-500 peer-checked:border-blue-600 peer-checked:text-blue-600 hover:text-gray-600 hover:bg-gray-100 dark:border-gray-700 dark:text-gray-400 dark:bg-gray-800 dark:hover:text-gray-300 dark:peer-checked:text-blue-500 dark:hover:bg-gray-700">
                            <div class="block">
                                <div class="w-full text-lg font-semibold">With Logo</div>
                            </div>
                        </label>
                    </li>
                    <li>
                        <input type="radio" id="InputWithoutLogo" data-form="IsWithLogo" name="IsWithLogo" value="0" class="peer hidden">
                        <label for="InputWithoutLogo" class="inline-flex w-full cursor-pointer items-center justify-between rounded-lg border border-gray-200 bg-white p-5 text-gray-500 peer-checked:border-blue-600 peer-checked:text-blue-600 hover:text-gray-600 hover:bg-gray-100 dark:border-gray-700 dark:text-gray-400 dark:bg-gray-800 dark:hover:text-gray-300 dark:peer-checked:text-blue-500 dark:hover:bg-gray-700">
                            <div class="block">
                                <div class="w-full text-lg font-semibold">No Logo</div>
                            </div>
                        </label>
                    </li>
                </ul>
            </div>
            <form id="FormGeneratePLU" class="mt-5">
                <h4 class="mb-5 text-lg font-bold uppercase">Generate Item Product</h4>
                <div class="grid grid-cols-4 gap-3">
                    <div>
                        <label for="InputCertificateNumber" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Nomor Sertifikat</label>
                        <input type="text" id="InputCertificateNumber" value="" data-required="0" data-form="NoCertificate" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Input Nomor Sertifikat..." />
                    </div>
                    <div>
                        <label for="InputWeight" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Berat Product*</label>
                        <input type="text" id="InputWeight" data-required="1" data-form="Weight" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" value="1" placeholder="Input Berat Product..." />
                        <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>

                    </div>
                    <div>
                        <label for="InputQuantity" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Quantity*</label>
                        <input type="number" min="1" id="InputQuantity" data-required="1" data-form="Qty" class="block w-full rounded-lg border border-gray-300 p-2.5 text-sm text-gray-900 focus:bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" value="1" placeholder="Input Quantity..." />
                        <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>
                    </div>
                    <div class="mt-7">
                        <button type="submit" id="ButtonGenerate" class="me-2 w-max rounded-lg bg-blue-700 px-5 py-2.5 text-sm font-medium text-white hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">Generate</button>
                    </div>
                </div>
            </form>
        </div>
        <div class="flex w-full flex-col gap-5" id="ContentGeneratedPLU">
        </div>
        <div id="LoadingIndicator" class="hidden w-full animate-pulse rounded-md bg-gray-200/50 px-5 py-5 text-center text-lg leading-none font-medium text-blue-500 md:w-3/4 lg:w-1/2 dark:bg-blue-900 dark:text-blue-200">Generating new data, please wait....</div>
        <button id="SubmitFormProduct" data-modal-target="popup-modal" data-modal-toggle="popup-modal" class="me-2 hidden w-max rounded-lg bg-blue-700 px-5 py-2.5 text-sm font-medium text-white hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">Submit Form</button>
    </div>

    <button class="hidden" id="hide-popup-modal" data-modal-hide="popup-modal"></button>

    <div id="popup-modal" data-modal-backdrop="static" tabindex="-1" class="h-100% fixed top-0 right-0 left-0 hidden max-h-full w-full items-center justify-center overflow-x-hidden overflow-y-auto bg-gray-800/80 md:inset-0">
        <div class="relative max-h-full w-full max-w-md p-4">
            <div class="relative rounded-lg bg-white shadow dark:bg-gray-700">
                <button type="button" class="bg-transparent absolute end-2.5 top-3 ms-auto inline-flex h-8 w-8 items-center justify-center rounded-lg text-sm text-gray-400 hover:bg-gray-200 hover:text-gray-900 dark:hover:bg-gray-600 dark:hover:text-white" data-modal-hide="popup-modal">
                    <svg class="h-3 w-3" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 14">
                        <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6" />
                    </svg>
                    <span class="sr-only">Close modal</span>
                </button>
                <div class="p-4 text-center md:p-5">
                    <svg class="mx-auto mb-4 h-12 w-12 text-gray-400 dark:text-gray-200" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 20">
                        <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 11V6m0 8h.01M19 10a9 9 0 1 1-18 0 9 9 0 0 1 18 0Z" />
                    </svg>
                    <h3 class="mb-5 text-lg font-normal text-gray-500 dark:text-gray-400">Are you sure you want to submit this product?</h3>
                    <button id="ModalSubmitFormProduct" type="button" class="me-2 w-max rounded-lg bg-blue-700 px-5 py-2.5 text-sm font-medium text-white hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
                        Yes, I'm sure
                    </button>
                    <button id="ModalCancelFormProduct" data-modal-hide="popup-modal" type="button" class="ms-3 rounded-lg border border-gray-200 bg-white px-5 py-2.5 text-sm font-medium text-gray-900 hover:bg-gray-100 hover:text-blue-700 focus:outline-none focus:z-10 focus:ring-4 focus:ring-gray-100 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:focus:ring-gray-700 dark:hover:text-white dark:hover:bg-gray-700">No, cancel</button>
                </div>
            </div>
        </div>
    </div>

    <script type="module" src="event-handler.js"></script>
    <script>
        $(document).ready(() => {
            $(".select2").select2();

            const getTodaysDate = () => {
                // Get today's date
                let today = new Date();

                // Format the date to YYYY-MM-DD
                let dd = String(today.getDate()).padStart(2, '0');
                let mm = String(today.getMonth() + 1).padStart(2, '0'); // Months are zero-based
                let yyyy = today.getFullYear();

                let formattedDate = yyyy + '-' + mm + '-' + dd;

                return formattedDate;
            };


            $("#InputTanggal").val(getTodaysDate());
            $("#InputTanggalTJT").val(getTodaysDate());


            const getDefaultData = () => {
                // Product Info
                $.ajax({
                    type: "GET",
                    url: "PG_Outs.aspx/GetProductInfo",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        const response = data['d'];


                        if (response['MESSAGE_TYPE'] === 'ERROR') {
                            toastr.error(response['MESSAGE']);
                            return;
                        }

                        Object.keys(response["DATA"]).forEach((key) => {
                            const productInfo = response["DATA"][key];
                            const element = $(`#${key}`);
                            // element.append(`<option value="0" data-id="0" data-nama-kode="" data-nama="-- Pilih --" selected>-- Pilih --</option>`);

                            productInfo.forEach(({
                                ID,
                                Nama,
                                NamaKode
                            }) => {
                                element.append(`<option value="${ID}" data-id="${ID}" data-nama-kode="${NamaKode}" data-nama="${Nama}">${Nama} (${NamaKode})</option>`);
                            });
                        });
                    },
                    error: function (error) {
                        console.error(error);
                        toastr.error("Terjadi kesalahan silahkan hubungi team IT!");
                    }
                });


                $.ajax({
                    type: "GET",
                    url: "PG_Outs.aspx/GetWarehouse",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        const response = data['d'];

                        if (response['MESSAGE_TYPE'] === 'ERROR') {
                            toastr.error(response['MESSAGE']);
                            return;
                        }

                        const element = $(`#SelectWarehouse`);
                        // element.append(`<option value="0" data-id="0" data-id-group="" data-nama-kode="" data-nama="-- Pilih --" selected>-- Pilih --</option>`);

                        const warehouse = response["DATA"];
                        warehouse.forEach(({
                            ID,
                            Nama,
                            NamaKode,
                            IDGroup
                        }) => {
                            element.append(`<option value="${ID}" data-id="${ID}" data-id-group="${IDGroup}" data-nama-group-supplier="${NamaKode}" data-nama="${Nama}">${Nama} (${NamaKode})</option>`);
                        });
                    },
                    error: function (error) {
                        console.error(error);
                        toastr.error("Terjadi kesalahan silahkan hubungi team IT!");
                    }
                });

                $.ajax({
                    type: "GET",
                    url: "PG_Outs.aspx/GetSupplier",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        const response = data['d'];

                        if (response['MESSAGE_TYPE'] === 'ERROR') {
                            toastr.error(response['MESSAGE']);
                            return;
                        }

                        const element = $(`#SelectSupplier`);
                        // element.append(`<option value="0" data-id="0" data-id-group="0" data-nama-kode="" data-nama="-- Pilih --" selected>-- Pilih --</option>`);


                        const supplier = response["DATA"];
                        supplier.forEach(({
                            ID,
                            Nama,
                            NamaSupplierGroup,
                            IDGroup
                        }) => {
                            element.append(`<option value="${ID}" data-id="${ID}" data-id-group="${IDGroup}" data-nama-supplier-group="${NamaSupplierGroup}" data-nama="${Nama}">${Nama} (${NamaSupplierGroup})</option>`);
                        });
                    },
                    error: function (error) {
                        console.error(error);
                        toastr.error("Terjadi kesalahan silahkan hubungi team IT!");
                    }
                });

                $.ajax({
                    type: "GET",
                    url: "PG_Outs.aspx/GetGoldRate",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        const response = data['d'];

                        if (response['MESSAGE_TYPE'] === 'ERROR') {
                            toastr.error(response['MESSAGE']);
                            return;
                        }

                        $("#InputGoldRate").val(response['DATA']);
                    },
                    error: function (error) {
                        console.error(error);
                        toastr.error("Terjadi kesalahan silahkan hubungi team IT!");
                    }
                });
            };


            getDefaultData();
        });
    </script>
</asp:Content>
