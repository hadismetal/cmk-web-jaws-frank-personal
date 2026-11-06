<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="MasterKaret" %>



<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
        html {
            font-size: 16px;
        }

        body {
            font-size: 16px;
        }
    </style>
    <link href="/UI/Tailwind/tailwind-output.css" rel="stylesheet" />

    <div class="w-full rounded-md p-10 shadow-md">
        <div>
            <h1 class="border-0 border-b-0 m-0 p-0 font-bold text-[36px]">Register - Master Material PG SKK</h1>
        </div>

        <div class="mb-10 mt-3 w-full border border-gray-200"></div>
        <div>
            <form id="form-search">
                <div class="grid-cols-7 grid items-center gap-5">
                    <div class="flex flex-col gap-2">
                        <label for="Keywords" class="font-bold">Kode Karet Jaws / Kode SAP:</label>
                        <input placeholder="Input keywords..." name="Keywords" id="Keywords" class="block w-full rounded-lg border border-gray-300 bg-white p-2.5 text-sm text-gray-900 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" />
                    </div>
                    <div class="flex flex-col gap-2">
                        <label for="Item" class="font-bold">Item:</label>
                        <select name="Item" id="Item" class="select2">
                            <option value="ALL">-- ALL ITEM--</option>
                        </select>
                    </div>
                    <div class="flex flex-col gap-2">
                        <label for="Satuan" class="font-bold">Satuan:</label>
                        <select name="Satuan" id="Satuan" class="select2">
                            <option value="ALL">-- ALL SATUAN --</option>
                        </select>
                    </div>
                    <div class="flex flex-col gap-2">
                        <label for="Dist" class="font-bold">Dist:</label>
                        <select name="Dist" id="Dist" class="select2">
                            <option value="ALL">-- ALL DIST --</option>
                        </select>
                    </div>
                    <div class="flex flex-col gap-2">
                        <label for="Color" class="font-bold">Color:</label>
                        <select name="Color" id="Color" class="select2">
                            <option value="ALL">-- ALL COLOR --</option>
                        </select>
                    </div>
                    <div class="flex flex-col gap-2">
                        <label for="Status" class="font-bold">Status:</label>
                        <select name="Status" id="Status" class="select2">
                            <option value="-1">-- ALL STATUS --</option>
                            <option value="1">Already Posted</option>
                            <option value="0">Not Posted Yet</option>
                        </select>
                    </div>
                    <div class="flex flex-col gap-2">
                        <label class="invisible">H</label>
                        <button class="me-2 w-max rounded-lg bg-blue-700 px-5 py-2.5 text-sm font-medium text-white hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800" id="btn-submit" type="submit">Search</button>
                    </div>
                </div>
            </form>
            <div class="my-5 border-t pt-5">
                <button type="button" data-modal-target="add-data-modal" data-modal-toggle="add-data-modal" class="mb-2 me-2 rounded-lg bg-gray-800 px-5 py-2.5 text-sm font-medium text-white hover:bg-gray-900 focus:outline-none focus:ring-4 focus:ring-gray-300 dark:bg-gray-800 dark:border-gray-700 dark:hover:bg-gray-700 dark:focus:ring-gray-700">Add Data</button>
                <button type="button" data-modal-target="add-data-excel" data-modal-toggle="add-data-excel" class="mb-2 me-2 rounded-lg border border-gray-300 bg-white px-5 py-2.5 text-sm font-medium text-gray-900 hover:bg-gray-100 focus:outline-none focus:ring-4 focus:ring-gray-100 dark:bg-gray-800 dark:text-white dark:border-gray-600 dark:hover:bg-gray-700 dark:hover:border-gray-600 dark:focus:ring-gray-700">Add By Excel</button>
            </div>
            <div class="w-full overflow-x-auto">
                <p class="mb-5 w-max rounded-md bg-yellow-300 px-5 py-2 font-bold text-yellow-800">Note: To modify the data, Please double click the desired row and column. The data that already posted cannot be edited!</p>
                <table class="w-full table-auto border-collapse overflow-x-auto" id="table-data">
                    <thead>
                        <tr>
                            <th class="border-x-0 border-y-2 whitespace-nowrap border-gray-400 p-2">No. </th>
                            <th class="border-x-0 border-y-2 w-[10%] whitespace-nowrap border-gray-400 p-2">*Kode Karet</th>
                            <th class="border-x-0 border-y-2 whitespace-nowrap border-gray-400 p-2">*Kode Karet JAWS</th>
                            <th class="border-x-0 border-y-2 whitespace-nowrap border-gray-400 p-2">*Final Material SAP</th>
                            <th class="border-x-0 border-y-2 whitespace-nowrap border-gray-400 p-2">*Item</th>
                            <th class="border-x-0 border-y-2 whitespace-nowrap border-gray-400 p-2">*Satuan</th>
                            <th class="border-x-0 border-y-2 whitespace-nowrap border-gray-400 p-2">*Dist</th>
                            <th class="border-x-0 border-y-2 whitespace-nowrap border-gray-400 p-2">*Color</th>
                            <th class="border-x-0 border-y-2 w-[20%] whitespace-nowrap border-gray-400 p-2">*Notes</th>
                            <th class="border-x-0 border-y-2 whitespace-nowrap border-gray-400 p-2">Is Already Posted</th>
                        </tr>
                    </thead>
                    <tbody id="tbody-layout"></tbody>
                </table>
            </div>
        </div>
    </div>

    <!-- Main modal -->
    <div id="add-data-modal" tabindex="-1" aria-hidden="true" class="bg-gray-800/60 fixed left-0 right-0 top-0 z-50 hidden h-full max-h-full w-full items-center justify-center overflow-y-auto overflow-x-hidden md:inset-0">
        <div class="relative max-h-full w-full max-w-2xl p-4">
            <!-- Modal content -->
            <div class="relative rounded-lg bg-white shadow dark:bg-gray-700">
                <!-- Modal header -->
                <div class="flex items-center justify-between rounded-t border-b p-4 dark:border-gray-600 md:p-5">
                    <h3 class="text-lg font-semibold text-gray-900 dark:text-white">Create Master Material
                    </h3>
                    <button type="button" class="ms-auto inline-flex h-8 w-8 items-center justify-center rounded-lg bg-transparent text-sm text-gray-400 hover:bg-gray-200 hover:text-gray-900 dark:hover:bg-gray-600 dark:hover:text-white" data-modal-toggle="add-data-modal">
                        <svg class="h-3 w-3" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 14">
                            <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6" />
                        </svg>
                        <span class="sr-only">Close modal</span>
                    </button>
                </div>
                <!-- Modal body -->
                <form class="p-4 md:p-5" id="form-insert-material">
                    <div class="grid-cols-2 mb-4 grid gap-4">
                        <div>
                            <label for="InputKodeKaret" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">*Kode Karet</label>
                            <input type="text" name="InputKodeKaret" id="InputKodeKaret" class="block w-full rounded-lg border border-gray-300 bg-gray-50 p-2.5 text-sm text-gray-900 focus:ring-primary-600 focus:border-primary-600 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500" placeholder="Input kode karet..." />
                            <small class="mt-0.5 hidden text-sm text-red-600 dark:text-red-500">Input wajib di isi!</small>
                        </div>
                        <div>
                            <label for="InputKodeKaretJaws" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">*Kode Karet JAWS</label>
                            <input type="text" name="InputKodeKaretJaws" id="InputKodeKaretJaws" class="block w-full rounded-lg border border-gray-300 bg-gray-50 p-2.5 text-sm text-gray-900 focus:ring-primary-600 focus:border-primary-600 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500" placeholder="Input kode karet JAWS..." />
                            <small class="mt-0.5 hidden text-sm text-red-600 dark:text-red-500">Input wajib di isi!</small>
                        </div>
                        <div class="col-span-2">
                            <label for="InputFinalSAP" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">*Kode SAP</label>
                            <input type="text" name="InputKodeSAP" id="InputKodeSAP" class="block w-full rounded-lg border border-gray-300 bg-gray-50 p-2.5 text-sm text-gray-900 focus:ring-primary-600 focus:border-primary-600 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500" placeholder="Input kode SAP..." />
                            <small class="mt-0.5 hidden text-sm text-red-600 dark:text-red-500">Input wajib di isi!</small>
                        </div>
                        <div>
                            <label for="InputItem" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">*Item</label>
                            <input type="text" name="InputItem" id="InputItem" class="block w-full rounded-lg border border-gray-300 bg-gray-50 p-2.5 text-sm text-gray-900 focus:ring-primary-600 focus:border-primary-600 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500" placeholder="Input item..." />
                            <small class="mt-0.5 hidden text-sm text-red-600 dark:text-red-500">Input wajib di isi!</small>
                        </div>
                        <div>
                            <label for="InputSatuan" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">*Satuan</label>
                            <input type="text" name="InputSatuan" id="InputSatuan" class="block w-full rounded-lg border border-gray-300 bg-gray-50 p-2.5 text-sm text-gray-900 focus:ring-primary-600 focus:border-primary-600 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500" placeholder="Input satuan..." />
                            <small class="mt-0.5 hidden text-sm text-red-600 dark:text-red-500">Input wajib di isi!</small>
                        </div>
                        <div>
                            <label for="InputDist" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">*Dist</label>
                            <input type="text" name="InputDist" id="InputDist" class="block w-full rounded-lg border border-gray-300 bg-gray-50 p-2.5 text-sm text-gray-900 focus:ring-primary-600 focus:border-primary-600 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500" placeholder="Input color..." />
                            <small class="mt-0.5 hidden text-sm text-red-600 dark:text-red-500">Input wajib di isi!</small>
                        </div>
                        <div>
                            <label for="InputColor" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">*Color</label>
                            <input type="text" name="InputColor" id="InputColor" class="block w-full rounded-lg border border-gray-300 bg-gray-50 p-2.5 text-sm text-gray-900 focus:ring-primary-600 focus:border-primary-600 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500" placeholder="Input color..." />
                            <small class="mt-0.5 hidden text-sm text-red-600 dark:text-red-500">Input wajib di isi!</small>
                        </div>
                        <div class="col-span-2">
                            <label for="InputNotes" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">*Notes</label>
                            <textarea id="InputNotes" name="InputNotes" rows="4" class="block w-full rounded-lg border border-gray-300 bg-gray-50 p-2.5 text-sm text-gray-900 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="Input notes..."></textarea>
                            <small class="mt-0.5 hidden text-sm text-red-600 dark:text-red-500">Input wajib di isi!</small>
                        </div>
                        <label class="col-span-2 inline-flex w-max cursor-pointer items-center">
                            <input type="checkbox" id="InputStatus" name="InputStatus" value="1" class="peer sr-only" checked />
                            <div class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none peer-focus:ring-4 peer-focus:ring-blue-300 dark:peer-focus:ring-blue-800 rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600"></div>
                            <span class="ms-3 text-sm font-medium text-gray-900 dark:text-gray-300">Already Posted</span>
                        </label>
                    </div>

                    <div class="flex justify-between border-t pt-5">
                        <button type="button" class="invisible" id="btn-submit-insert-hidden" data-modal-toggle="add-data-modal">Hidden</button>
                        <button type="submit" id="btn-submit-insert" class="inline-flex items-center rounded-lg bg-blue-700 px-5 py-2.5 text-center text-sm font-medium text-white hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
                            Add Master Material
                        </button>
                    </div>
                </form>
            </div>
        </div>
    </div>


    <div id="add-data-excel" data-modal-placement="top-center" tabindex="-1" aria-hidden="true" class="bg-gray-800/60 z-[1050] fixed left-0 right-0 top-0 hidden h-full max-h-full w-full overflow-y-auto overflow-x-hidden md:inset-0">
        <div class="relative max-h-full w-full max-w-7xl p-4">
            <!-- Modal content -->
            <div class="relative rounded-lg bg-white shadow dark:bg-gray-700">
                <!-- Modal header -->
                <div class="flex items-center justify-between rounded-t border-b p-4 dark:border-gray-600 md:p-5">
                    <h3 class="text-lg font-semibold text-gray-900 dark:text-white">Create Master Material (Excel)
                    </h3>
                    <button type="button" class="ms-auto inline-flex h-8 w-8 items-center justify-center rounded-lg bg-transparent text-sm text-gray-400 hover:bg-gray-200 hover:text-gray-900 dark:hover:bg-gray-600 dark:hover:text-white" data-modal-toggle="add-data-excel">
                        <svg class="h-3 w-3" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 14">
                            <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6" />
                        </svg>
                        <span class="sr-only">Close modal</span>
                    </button>
                </div>
                <!-- Modal body -->
                <div class="p-4 md:p-5">
                    <h2 class="mb-2 text-lg font-semibold text-gray-900 dark:text-white">Cara Kerja:</h2>
                    <ol>
                        <li class="list-inside list-decimal space-y-1 text-gray-500 dark:text-gray-400">Silahkan download template file terlebih dahulu. <a class="text-blue-500" href="template-master-karet.xlsx">Download Template</a></li>
                        <li class="list-inside list-decimal space-y-1 text-gray-500 dark:text-gray-400">Upload file pada kolom upload yang disediakan dibawah.</a></li>
                        <li class="list-inside list-decimal space-y-1 text-gray-500 dark:text-gray-400">Sistem akan membaca file yang di upload agar tidak mengandung data duplikat antara Kode Karet Jaws dan Kode SAP.</li>
                        <li class="list-inside list-decimal space-y-1 text-gray-500 dark:text-gray-400">Sistem akan membaca database apakah Kode Karet Jaws yang akan di upload sudah terdaftar atau belum.</li>
                        <li class="list-inside list-decimal space-y-1 text-gray-500 dark:text-gray-400">Sistem akan membaca database apakah Kode SAP yang akan di upload sudah terdaftar atau belum.</li>
                        <li class="list-inside list-decimal space-y-1 text-gray-500 dark:text-gray-400">Pastikan file yang di upload sesuai dengan template Excel yang disediakan.</li>
                        <li class="list-inside list-decimal space-y-1 text-gray-500 dark:text-gray-400">Jika semua validasi terlewati sistem secara otomatis akan upload data ke database.</li>
                    </ol>
                    <div class="mt-5">
                        <input class="mb-5 block w-full cursor-pointer rounded-lg border border-gray-300 bg-gray-50 text-sm text-gray-900 focus:outline-none dark:text-gray-400 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400" type="file" id="input-file" accept=".xls,.xlsx" />
                    </div>
                    <div class="mt-5">
                        <div class="relative overflow-x-auto">
                            <table class="w-full text-left text-sm text-gray-500 rtl:text-right dark:text-gray-400">
                                <thead class="bg-gray-50 text-xs uppercase text-gray-700 dark:bg-gray-700 dark:text-gray-400">
                                    <tr>
                                        <th scope="col" class="px-6 py-3">No. </th>
                                        <th scope="col" class="px-6 py-3">Kode Karet</th>
                                        <th scope="col" class="px-6 py-3">Kode Karet Jaws</th>
                                        <th scope="col" class="px-6 py-3">Final SAP</th>
                                        <th scope="col" class="px-6 py-3">Item</th>
                                        <th scope="col" class="px-6 py-3">Satuan</th>
                                        <th scope="col" class="px-6 py-3">Dist</th>
                                        <th scope="col" class="px-6 py-3">Color</th>
                                        <th scope="col" class="px-6 py-3">Notes</th>
                                        <th scope="col" class="px-6 py-3">Status Posting</th>
                                    </tr>
                                </thead>
                                <tbody id="tbody-uploaded-excel"></tbody>
                            </table>
                        </div>
                    </div>
                    <div class="flex justify-between border-t pt-5">
                        <button type="button" class="invisible" id="btn-submit-insert-hidden-excel" data-modal-toggle="add-data-excel">Hidden</button>
                    </div>
                </div>
            </div>
        </div>
    </div>



    <script>
        $(document).ready(function () {
            const disabledClass = "text-white bg-blue-400 dark:bg-blue-500 cursor-not-allowed font-medium rounded-lg text-sm px-5 py-2.5 text-center w-max";
            const defaultClass = "text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800 w-max";
            const inputInvalidClass = "bg-red-50 border border-red-500 text-red-900 placeholder-red-700 text-sm rounded-lg focus:ring-red-500 dark:bg-gray-700 focus:border-red-500 block w-full p-2.5 dark:text-red-500 dark:placeholder-red-500 dark:border-red-500";
            const inputDefaultClass = "block w-full rounded-lg border border-gray-300 bg-gray-50 p-2.5 text-sm text-gray-900 focus:ring-primary-600 focus:border-primary-600 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white dark:focus:ring-primary-500 dark:focus:border-primary-500";
            const labelInvalidClass = "block mb-2 text-sm font-bold text-red-700 dark:text-red-500";
            const labelDefaultClass = "mb-2 block text-sm font-bold text-gray-900 dark:text-white";


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

            $(".select2").select2();

            $.ajax({
                type: "GET",
                url: "index.aspx/GetSelectOption",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    const response = data['d'];

                    const objectKeys = ['Color', 'Dist', 'Item', 'Satuan'];

                    objectKeys.forEach((key) => {
                        response[key].map((value) => {
                            $(`#${key}`).append(`<option value="${value['Value']}">${value['Value']}</option>`);
                        });
                    });
                },
                error: function (error) {
                    console.error(error);
                }
            });

            const arraysEqual = (a, b) => {
                if (a.length !== b.length) {
                    return false; // Arrays have different lengths, can't be equal
                }

                // Compare corresponding elements
                return a.every((value, index) => value === b[index]);
            }

            const checkDuplicateKaretAndKodeSap = (array) => {
                const seen = new Set();

                for (const object of array) {
                    const key = `${object.KodeKaretJaws}-${object.FinalSAP}`;

                    if (seen.has(key)) {
                        return true;  // Duplicate combination found
                    }

                    seen.add(key);
                }

                return false;  // No duplicates found                    
            }

            const onChangeValidation = () => {
                const data = $("#form-insert-material").serializeArray();

                const mapObjectInput = {
                    'valid': inputDefaultClass,
                    'invalid': inputInvalidClass,
                    'validLabel': labelDefaultClass,
                    'invalidLabel': labelInvalidClass
                };

                data.forEach((formData) => {
                    const element = $(`#${formData['name']}`);

                    if (!element.is(':checkbox')) {
                        element.on('input', function () {
                            const inputClass = element.val() == '' ? mapObjectInput['invalid'] : mapObjectInput['valid'];
                            const labelClass = element.val() == '' ? mapObjectInput['invalidLabel'] : mapObjectInput['validLabel'];
                            element.removeClass().addClass(inputClass);
                            element.siblings('label').removeClass().addClass(labelClass);

                            if (element.val() == '') {
                                element.siblings('small').removeClass('hidden');
                                return;
                            }

                            element.siblings('small').addClass('hidden');
                        });
                    }
                })
            }

            const validateFormSubmit = (args) => {
                let isValid = true;
                args.forEach((formData) => {
                    if (formData['value'] == '') {
                        const formInput = $(`#${formData['name']}`);

                        if (!formInput.is(':checkbox')) {
                            formInput.removeClass().addClass(inputInvalidClass);
                            formInput.siblings('small').removeClass("hidden").text("Input wajib di isi!");;
                            formInput.siblings('label').removeClass().addClass(labelInvalidClass)
                            isValid = false;
                        }
                    }
                });

                return isValid;
            }

            const validateFormErrors = (properties) => {
                let isValid = true;

                properties.forEach((property) => {
                    const formInput = $(`#${property['InputColumn']}`);

                    formInput.removeClass().addClass(inputInvalidClass);
                    formInput.siblings('small').removeClass("hidden").text(property.Message);
                    formInput.siblings('label').removeClass().addClass(labelInvalidClass);
                    isValid = false;
                })

                return isValid;
            }

            const onInsertData = (args) => {
                try {
                    const isFormValid = validateFormSubmit(args);

                    if (!isFormValid) return;

                    const objectArgs = Object.create({});

                    args.forEach((formData) => {
                        Object.assign(objectArgs, {
                            [formData['name']]: formData['value']
                        });
                    });

                    if (!("InputStatus" in objectArgs)) objectArgs['InputStatus'] = '0';

                    $("#add-data-modal").find("button:not(#btn-submit-insert-hidden)").prop("disabled", true);
                    $("#add-data-modal").find('#btn-submit-insert').removeClass().addClass(disabledClass).text("Loading...");


                    $.ajax({
                        type: "post",
                        url: "index.aspx/InsertMasterMaterial",
                        data: JSON.stringify({ input: objectArgs }),
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            const response = data['d'];

                            // set the value back to empty string
                            if (response['Status'] === 'success') {
                                $("#btn-submit-insert-hidden").click();

                                args.forEach((formData) => {
                                    const element = $(`#${formData['name']}`);

                                    if (!element.is(':checkbox')) {
                                        element.val('');
                                    }
                                });

                                if (objectArgs['InputStatus'] == '0') {
                                    $("#InputStatus").click();
                                }
                                toastr.success(response['Message']);

                                return;
                            }

                            toastr.error(response['Message']);
                            if (!("MessageProperties" in response)) return;


                            validateFormErrors(response['MessageProperties']);

                        },
                        error: function (error) {
                            console.error(error);
                            $("#add-data-modal").find("button").prop("disabled", false);
                            $("#add-data-modal").find('#btn-submit-insert').removeClass().addClass(defaultClass).text("Add master material");
                        },
                        complete: function (data) {
                            $("#add-data-modal").find("button").prop("disabled", false);
                            $("#add-data-modal").find('#btn-submit-insert').removeClass().addClass(defaultClass).text("Add master material");
                        }
                    });
                } catch (error) {
                    console.error(error);
                    $("#add-data-modal").find("button").prop("disabled", false);
                    $("#add-data-modal").find('#btn-submit-insert').removeClass().addClass(defaultClass).text("Add Master Material");
                }
            }


            const onSearchData = (args) => {
                try {
                    const objectArgs = Object.create({});

                    args.forEach((formData) => {
                        Object.assign(objectArgs, {
                            [formData['name']]: formData['value']
                        });
                    });


                    $("#btn-submit").removeClass().addClass(disabledClass).prop("disabled", true).text("Loading...");

                    $.ajax({
                        type: "POST",
                        url: "index.aspx/GetMasterMaterial",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify({ filter: objectArgs }),
                        success: function (data) {
                            const response = data['d'];

                            // Reinitialize the DataTable after new rows are added
                            $('#table-data').DataTable().destroy();
                            $("#tbody-layout").empty();


                            if (response["MessageType"] == "ERROR") {
                                toastr.error("Failed to fetch the data");
                                console.error(response);
                                return;
                            }


                            response["Data"].forEach(({ ID, KodeKaret, KodeKaretJaws, FinalSAP, Item, Satuan, Dist, Color, KetUploadSAP, IsUploadedToSAP }, key) => {
                                const isChecked = IsUploadedToSAP == 1 ? "checked disabled" : "";
                                $("#tbody-layout").append(`
                            <tr>
                                <td class="border-x-0 border-y-2 border-gray-200 p-2">${key + 1}.</td>
                                <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="KodeKaret">
                                    <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="KodeKaret">${KodeKaret}</textarea>
                                    <span data-note-id="${ID}" data-column-name="KodeKaret" data-popover-target="popover-default">${KodeKaret}</span>
                                </td>
                                <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="KodeKaretJaws">
                                    <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="KodeKaretJaws">${KodeKaretJaws}</textarea>
                                    <span data-note-id="${ID}" data-column-name="KodeKaretJaws" data-popover-target="popover-default">${KodeKaretJaws}</span>
                                </td>
                                <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="FinalSAP">
                                    <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="FinalSAP">${FinalSAP}</textarea>
                                    <span data-note-id="${ID}" data-column-name="FinalSAP" data-popover-target="popover-default">${FinalSAP}</span>
                                </td>
                                <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="Item">
                                    <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="Item">${Item}</textarea>
                                    <span data-note-id="${ID}" data-column-name="Item" data-popover-target="popover-default">${Item}</span>
                                </td>
                                <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="Satuan">
                                    <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="Satuan">${Satuan}</textarea>
                                    <span data-note-id="${ID}" data-column-name="Satuan" data-popover-target="popover-default">${Satuan}</span>
                                </td>
                                <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="Dist">
                                    <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="Dist">${Dist}</textarea>
                                    <span data-note-id="${ID}" data-column-name="Dist" data-popover-target="popover-default">${Dist}</span>
                                </td>
                                <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="Color">
                                    <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="Color">${Color}</textarea>
                                    <span data-note-id="${ID}" data-column-name="Color" data-popover-target="popover-default">${Color}</span>
                                </td>
                                <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="KetUploadSAP">
                                    <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="KetUploadSAP">${KetUploadSAP}</textarea>
                                    <span data-note-id="${ID}" data-column-name="KetUploadSAP" data-popover-target="popover-default">${KetUploadSAP}</span>
                                </td>
                                <td class="border-x-0 border-y-2 border-gray-200 p-2">
                                   <input type="checkbox" value="${IsUploadedToSAP}" data-note-id="${ID}" ${isChecked} />
                                </td>
                            </tr>
                        `);
                            });

                            $('#table-data').DataTable().draw();

                            toastr.success(response["Message"]);
                        },
                        error: function (error) {
                            console.error(error);
                            $("#btn-submit").removeClass().addClass(defaultClass).prop("disabled", false).text("Search");
                        },
                        complete: function (data) {
                            $("#btn-submit").removeClass().addClass(defaultClass).prop("disabled", false).text("Search");
                        }
                    });

                } catch (error) {
                    $("#btn-submit").removeClass().addClass(defaultClass).prop("disabled", false).text("Search");
                    console.error(error);
                }
            }

            $("#form-search").on('submit', function (e) {
                e.preventDefault();
                onSearchData($(this).serializeArray());
            });

            $("#form-insert-material").on('submit', function (e) {
                e.preventDefault();
                onInsertData($(this).serializeArray());
            });


            $(document).on('dblclick', 'td[data-note-id]', function (e) {

                var checkbox = $(this).closest('tr').find('td').has('input[type="checkbox"]').find('input[type="checkbox"]');

                if (checkbox.is(':checked')) {
                    toastr.error("This data is already sent to SAP, and cannot be updated");
                    return;
                }

                const id = $(this).data("note-id");
                const column = $(this).data("column-name");

                const span = $(`span[data-note-id="${id}"][data-column-name="${column}"]`);
                span.addClass("hidden");


                const textarea = $(`textarea[data-note-id="${id}"][data-column-name="${column}"]`);
                const textLength = textarea.val().length;
                textarea.removeClass("hidden").focus();
                textarea[0].setSelectionRange(textLength, textLength);
            });

            onChangeValidation();

            $(document).on('change', 'input[type="checkbox"][data-note-id]', function () {
                try {
                    const id = $(this).data("note-id");
                    const isChecked = $(this).is(':checked') ? "1" : "0";
                    $.ajax({
                        type: "post",
                        url: "index.aspx/UpdateStatusMasterMaterial",
                        data: JSON.stringify({ InputStatus: isChecked, ID: id }),
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            const response = data['d'];

                            // set the value back to empty string
                            if (response === 'success') {
                                toastr.success(`Success updating data.`);
                                return;
                            }

                            toastr.error(response);
                        },
                        error: function (error) {
                            toastr.error("ERROR: Something went wrong with the system.");
                        }
                    });
                } catch (error) {
                    console.error(error);
                    toastr.error("ERROR: Something went wrong with the system.");
                }
            });

            $(document).on({
                blur: function () {
                    try {
                        const id = $(this).data("note-id");
                        const column = $(this).data("column-name");
                        const span = $(`span[data-note-id="${id}"][data-column-name="${column}"]`);
                        const textarea = $(`textarea[data-note-id="${id}"][data-column-name="${column}"]`);


                        $.ajax({
                            type: "post",
                            url: "index.aspx/UpdateNotesMasterMaterial",
                            data: JSON.stringify({ InputNotes: this.value, ID: id, ColumnName: column }),
                            contentType: "application/json; charset=utf-8",
                            success: function (data) {
                                const response = data['d'];
                                const spanOldValue = span.text();
                                const textValue = textarea.val();

                                textarea.addClass("hidden");

                                console.log(textValue);

                                // set the value back to empty string
                                if (response === 'success') {
                                    toastr.success(`Success updating data.`);
                                    span.text(textValue).removeClass("hidden");
                                    return;
                                }

                                toastr.error(response);
                                span.text(spanOldValue).removeClass("hidden");
                                textarea.val(spanOldValue);
                            },
                            error: function (error) {
                                toastr.error("ERROR: Something went wrong with the system.");
                            }
                        });
                    } catch (error) {
                        console.error(error);
                        toastr.error("ERROR: Something went wrong with the system.");
                    }

                },
                input: function () {
                    // Reset the height to auto to shrink or expand based on content
                    $(this).height('auto');

                    // Set the height to the scroll height (which adjusts based on content)
                    $(this).height(this.scrollHeight);
                }
            }, 'textarea[data-column-name]');


            $("#input-file").on('change', function (e) {
                let file = e.target.files[0];
                // input canceled, return
                if (!file) return;

                let FR = new FileReader();
                FR.onload = function (e) {
                    let data = new Uint8Array(e.target.result);
                    let workbook = XLSX.read(data, { type: 'array' });
                    let firstSheet = workbook.Sheets[workbook.SheetNames[0]];

                    // header: 1 instructs xlsx to create an 'array of arrays'
                    let result = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });

                    // duplicate result and get the first array (headers)
                    const headers = [...result][0];
                    result.shift();
                    const arrayOfColumns = ["KodeKaret", "KodeKaretJaws", "FinalSAP", "Item", "Satuan", "Dist", "Color", "KetUploadSAP", "IsUploadedToSAP"];
                    const isValidFormat = arraysEqual(arrayOfColumns, headers);
                    $("#tbody-uploaded-excel").empty();

                    if (!isValidFormat) {
                        toastr.error("Invalid format detected. Please use the template correctly.");
                        return;
                    }

                    if (result.length === 0) {
                        toastr.error("There is no data to upload.");
                        return;
                    }

                    const resultToArrayOfObjects = result.map((item) => {
                        let object = Object.create({});
                        item = Array.from(item, value => value === undefined ? "" : value);  // Replace empty slots with null
                        item.forEach((value, index) => Object.assign(object, { [arrayOfColumns[index]]: value }));
                        return object;
                    });


                    const resultWithInvalidRows = resultToArrayOfObjects.map((item) => {
                        let isValid = true;

                        Object.keys(item).forEach((value) => {
                            if (item[value] === "") isValid = false;
                        });

                        if (!isValid) return item;
                    }).filter((item) => typeof item !== "undefined");


                    resultToArrayOfObjects.forEach(({
                        KodeKaret, KodeKaretJaws, FinalSAP, Item, Satuan, Dist, Color, KetUploadSAP, IsUploadedToSAP
                    }, key) => {
                        $("#tbody-uploaded-excel").append(
                            `
                            <tr class="border-b bg-white dark:bg-gray-800 dark:border-gray-700">
                                <td class="px-6 py-4">${key + 1}.</td>
                                <td class="px-6 py-4">${KodeKaret}</td>
                                <td class="px-6 py-4" data-column="KodeKaretJaws">${KodeKaretJaws}</td>
                                <td class="px-6 py-4" data-column="FinalSAP">${FinalSAP}</td>
                                <td class="px-6 py-4">${Item}</td>
                                <td class="px-6 py-4">${Satuan}</td>
                                <td class="px-6 py-4">${Dist}</td>
                                <td class="px-6 py-4">${Color}</td>
                                <td class="px-6 py-4">${KetUploadSAP}</td>
                                <td class="px-6 py-4">${IsUploadedToSAP == 1 ? "Yes" : IsUploadedToSAP == 0 ? "No" : "Unknown"}</td>
                            </tr>    
                        `);
                    });

                    if (resultWithInvalidRows.length > 0) {
                        toastr.error("There is an empty value in your uploaded file! Please fill the empty value to continue.");
                        return;
                    }

                    const isInvalidFormatStatusSAP = resultToArrayOfObjects.map((item) => item['IsUploadedToSAP']).filter((item) => item != 0 && item != 1).length;


                    if (isInvalidFormatStatusSAP > 0) {
                        toastr.error("Unexpected status posting SAP, The value can only be 0 or 1.");
                        return;
                    }


                    const isDuplicate = checkDuplicateKaretAndKodeSap(resultToArrayOfObjects);
                    if (isDuplicate) {
                        toastr.error("There is duplicate values between Kode Karet Jaws and Final SAP.");
                        return;
                    }


                    $.ajax({
                        type: "POST",
                        url: "index.aspx/ValidateMasterMaterial",
                        data: JSON.stringify({ input: resultToArrayOfObjects }),
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            const response = data['d'];

                            if (response['TypeMessage'] == 'ERROR') {
                                toastr.error(response['Message']);

                                const listOfDuplicateKodeKaret = response['DuplicateValues'].map((item) => item['KodeKaretJaws']);
                                const listOfDuplicateFinalSAP = response['DuplicateValues'].map((item) => item['FinalSAP']);

                                $('#tbody-uploaded-excel tr').each(function () {
                                    var cellsKodeKaret = $(this).find('td[data-column="KodeKaretJaws"]');
                                    var cellsFinalSAP = $(this).find('td[data-column="FinalSAP"]');


                                    cellsKodeKaret.each(function () {
                                        if (listOfDuplicateKodeKaret.includes($(this).text())) {
                                            $(this).closest('tr').addClass("bg-red-100").removeClass("bg-white");
                                        }
                                    });

                                    cellsFinalSAP.each(function () {
                                        if (listOfDuplicateFinalSAP.includes($(this).text())) {
                                            $(this).closest('tr').addClass("bg-red-100").removeClass("bg-white");
                                        }
                                    });
                                });
                                return;
                            }

                            toastr.success("All the validation passed, files is uploaded... Please wait...");

                            $.ajax({
                                type: "POST",
                                url: "index.aspx/InsertMasterMaterialExcel",
                                data: JSON.stringify({ input: resultToArrayOfObjects }),
                                contentType: "application/json; charset=utf-8",
                                success: function (data) {
                                    const response = data['d'];
                                    toastr.success(response["Message"]);

                                    // Reinitialize the DataTable after new rows are added
                                    $('#table-data').DataTable().destroy();
                                    $("#tbody-layout").empty();
                                    $("#tbody-uploaded-excel").empty();
                                    $("#input-file").val(null);

                                    response['DataAdded'].forEach(({ ID, KodeKaret, KodeKaretJaws, FinalSAP, Item, Satuan, Dist, Color, KetUploadSAP, IsUploadedToSAP }, key) => {
                                        const isChecked = IsUploadedToSAP == 1 ? "checked" : "";
                                        $("#tbody-layout").append(`
                                                <tr>
                                                    <td class="border-x-0 border-y-2 border-gray-200 p-2">${key + 1}.</td>
                                                    <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="KodeKaret">
                                                        <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="KodeKaret">${KodeKaret}</textarea>
                                                        <span data-note-id="${ID}" data-column-name="KodeKaret" data-popover-target="popover-default">${KodeKaret}</span>
                                                    </td>
                                                    <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="KodeKaretJaws">
                                                        <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="KodeKaretJaws">${KodeKaretJaws}</textarea>
                                                        <span data-note-id="${ID}" data-column-name="KodeKaretJaws" data-popover-target="popover-default">${KodeKaretJaws}</span>
                                                    </td>
                                                    <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="FinalSAP">
                                                        <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="FinalSAP">${FinalSAP}</textarea>
                                                        <span data-note-id="${ID}" data-column-name="FinalSAP" data-popover-target="popover-default">${FinalSAP}</span>
                                                    </td>
                                                    <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="Item">
                                                        <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="Item">${Item}</textarea>
                                                        <span data-note-id="${ID}" data-column-name="Item" data-popover-target="popover-default">${Item}</span>
                                                    </td>
                                                    <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="Satuan">
                                                        <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="Satuan">${Satuan}</textarea>
                                                        <span data-note-id="${ID}" data-column-name="Satuan" data-popover-target="popover-default">${Satuan}</span>
                                                    </td>
                                                    <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="Dist">
                                                        <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="Dist">${Dist}</textarea>
                                                        <span data-note-id="${ID}" data-column-name="Dist" data-popover-target="popover-default">${Dist}</span>
                                                    </td>
                                                    <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="Color">
                                                        <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="Color">${Color}</textarea>
                                                        <span data-note-id="${ID}" data-column-name="Color" data-popover-target="popover-default">${Color}</span>
                                                    </td>
                                                    <td class="border-x-0 border-y-2 border-gray-200 p-2" data-note-id="${ID}" data-column-name="KetUploadSAP">
                                                        <textarea class="hidden w-full" data-note-id="${ID}" data-column-name="KetUploadSAP">${KetUploadSAP}</textarea>
                                                        <span data-note-id="${ID}" data-column-name="KetUploadSAP" data-popover-target="popover-default">${KetUploadSAP}</span>
                                                    </td>
                                                    <td class="border-x-0 border-y-2 border-gray-200 p-2">
                                                       <input type="checkbox" value="${IsUploadedToSAP}" data-note-id="${ID}" ${isChecked} />
                                                    </td>
                                                </tr>
                                            `);
                                    });

                                    $('#table-data').DataTable().draw();

                                    $("#btn-submit-insert-hidden-excel").click();
                                },
                                error: function (error) {
                                    console.error(error);
                                    toastr.error("ERROR: Something went wrong when uploading files, please contact Team IT.");
                                }
                            });
                        },
                        error: function (error) {
                            console.error(error);
                            toastr.error("ERROR: Something went wrong when detecting files, please contact Team IT.");
                        }
                    });

                };
                FR.readAsArrayBuffer(file);
            });
        });
    </script>

</asp:Content>


