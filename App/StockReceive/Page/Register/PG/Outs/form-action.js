import { DEFAULT_INPUT, DEFAULT_LABEL, INVALID_INPUT, INVALID_LABEL, DEFAULT_BUTTON, DISABLED_BUTTON, SVG_LOADER, PRINT_ICON, DISABLED_BUTTON_OUTLINE, DEFAULT_BUTTON_OUTLINE, TRASH_ICON } from "./flowbite-css-class.js";

const CONTENT_GENERATED_PLU = $("#ContentGeneratedPLU");

const onLoadingNewData = ({ isLoading }) => {
    if (isLoading) {
        $("#LoadingIndicator").removeClass("hidden");

        $('#ContentGeneratedPLU').children().each(function () {
            $(this).addClass('hidden');
        });

        return;
    }

    $("#LoadingIndicator").addClass("hidden");

    $('#ContentGeneratedPLU').children().each(function () {
        $(this).removeClass('hidden');
    });
}

const formProductToElement = {
    SelectWarehouse: $("#SelectWarehouse"),
    InputTanggal: $("#InputTanggal"),
    InputTanggalTJT: $("#InputTanggalTJT"),
    SelectSupplier: $("#SelectSupplier"),
    InputSupplier: $("#InputSupplier"),
    InputSJSupplier: $("#InputSJSupplier"),
    InputGoldRate: $("#InputGoldRate"),
    InputRadioGoldRateType: $("input[name='GoldRateType']"),
    InputStockKeeper: $("#InputStockKeeper"),
    InputRingSize: $("#InputRingSize"),
    InputKeterangan: $("#InputKeterangan"),
    SelectGoldLevel: $("#SelectGoldLevel"),
    SelectGoldModel: $("#SelectGoldModel"),
    SelectProductItem: $("#SelectProductItem"),
    SelectFrameColor: $("#SelectFrameColor"),
    SelectTargetAge: $("#SelectTargetAge"),
    InputKadarLogam: $("#InputKadarLogam"),
    InputKadarTukaranJual: $("#InputKadarTukaranJual"),
    InputKadarTukaranBeli: $("#InputKadarTukaranBeli"),
    InputKodeBarang: $("#InputKodeBarang"),
    InputNamaBarang: $("#InputNamaBarang"),
    InputKodeMaterial: $("#InputKodeMaterial"),
    InputNomorValidasi: $("#InputNomorValidasi")
};

const formatNumberToIDR = (number) => {
    const formatter = new Intl.NumberFormat('id-ID', {
        style: 'currency',
        currency: 'IDR',
        maximumFractionDigits: 0
    });

    return formatter.format(number);
}

const validateFormSubmit = () => {
    let isInvalid = false;

    const objectForm = Object.create({});

    Object.keys(formProductToElement).forEach((key) => {
        const element = formProductToElement[key];
        const keyCodeBehind = element.data('form');
        let value = element.val();

        if (element.attr("type") == "radio") {
            value = element.filter(':checked').val();
        }

        if (!key.includes("Select") && !key.includes("Radio")) {


            //if (value == '') {
            //    if (element.attr("type") == 'text') {
            //        value = "TESTING";
            //    } else if (element.attr("type") == 'number') {
            //        if (element.attr("id") == "InputGoldRate") {
            //            value = "85.20";
            //        } else {
            //            value = "1";
            //        }
            //    } else {
            //        value = "2025-01-01";
            //    }
            //}

            const isInvalidInput = value.trim() == '' ? INVALID_INPUT : DEFAULT_INPUT;
            const isInvalidLabel = value.trim() == '' ? INVALID_LABEL : DEFAULT_LABEL;

            element.removeClass().addClass(isInvalidInput);
            element.siblings('label').removeClass().addClass(isInvalidLabel);

            if (value.trim() == '') {
                element.siblings('p').removeClass('hidden');
                isInvalid = true;
            } else {
                element.siblings('p').removeClass('hidden').addClass('hidden');
            }

        }

        Object.assign(objectForm, {
            [keyCodeBehind]: value
        });
    });


    if (isInvalid) toastr.error("Lengkapi form terlebih dahulu.");

    return { isInvalid, objectForm };
}

const generateTable = (data, weight, certificate, qty, IDPrint, BrandKode) => {
    const RADIO_LOGO_SETTINGS = $('input[type="radio"][name="IsWithLogo"]').filter(':checked').val();
    
    const result = [...data].slice(0, -1);

    let tableRows = '';

    const formatWeightAsGenerated = weight.split(",").join("_").split(".").join("_");

    result.forEach(({
        ID,
        PLU,
        HPPM,
        Harga24,
        Ongkos,
        TotalBerat24,
        TotalHarga24,
        TotalHargaJual,
        Weight
    }) => {
        tableRows += `
             <tr class="border-b bg-white dark:bg-gray-800 dark:border-gray-700">
                 <td class="px-6 py-4 hidden">${ID}</th>
                 <td class="px-6 py-4">${PLU}</th>
                 <td class="px-6 py-4">${Weight} gr</td>
                 <td class="px-6 py-4">${formatNumberToIDR(Harga24)}</td>
                 <td class="px-6 py-4">${formatNumberToIDR(Ongkos)}</td>
                 <td class="px-6 py-4">${formatNumberToIDR(TotalHarga24)}</td>
                 <td class="px-6 py-4">${formatNumberToIDR(TotalHargaJual)}</td>
                 <td class="px-6 py-4">${TotalBerat24}</td>
                 <td class="px-6 py-4">${formatNumberToIDR(HPPM)}</td>
                 <td class="px-6 py-4">
                    <div class="flex items-center">
                        <a href="zpl://?type=SINGLE&PLU=${PLU}&BRAND=${BrandKode}&LOGO=${RADIO_LOGO_SETTINGS}" class="flex-1 text-white bg-gray-700 hover:bg-gray-800 focus:ring-4 focus:outline-none focus:ring-blue-300 rounded-lg text-sm px-5 py-2.5 text-center inline-flex items-center me-2 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 hover:text-white hover:no-underline">
                            <span class="mr-3">${PRINT_ICON}</span> Print
                        </a>
                        <button id="DeletePlu_${PLU}" data-plu="${PLU}" data-formatted-weight="${formatWeightAsGenerated}" class="flex items-center me-2 rounded-lg bg-red-700 px-5 py-2.5 text-sm font-medium text-white hover:bg-red-800 focus:ring-4 focus:ring-red-300 focus:outline-none dark:bg-blue-600 dark:hover:bg-red-500 dark:focus:ring-red-800">
                             <span class="mr-3">${TRASH_ICON}</span> Delete
                        </button>
                    </div>
                </td>
              </tr>
        `;
    });


    const { HPPM, Harga24, Ongkos, TotalBerat24, TotalHarga24, TotalHargaJual, Weight } = data[data.length - 1];


    return `
        <div data-group-weight="${weight}" data-group-formatted-weight="${formatWeightAsGenerated}" class="rounded-xl border p-5 hidden">
                <p class="font-bold text-gray-700">Group Product</p>
                <p class="text-3xl font-medium text-gray-500" id="TextGroupWeight${formatWeightAsGenerated}">${parseFloat(weight).toFixed(2)} gr</p>
                <div>
                    <div class="mt-5 w-full md:w-3/4 lg:w-1/2">
                        <div class="grid-cols-4 grid gap-3">
                            <div>
                                <label for="UpdateCertificateNumber${formatWeightAsGenerated}" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Nomor Sertifikat</label>
                                <input type="text" id="UpdateCertificateNumber${formatWeightAsGenerated}" data-required="0" data-form="NoCertificate" disabled class="bg-gray-100 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 cursor-not-allowed dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-gray-400 dark:focus:ring-blue-500 dark:focus:border-blue-500" value="${certificate}" placeholder="Input Nomor Sertifikat..." />
                            </div>
                            <div>
                                <label for="UpdateWeight${formatWeightAsGenerated}" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Berat Product*</label>
                                <input type="number" min="0.1" step="0.1" id="UpdateWeight${formatWeightAsGenerated}" data-required="1" data-form="Weight" disabled class="bg-gray-100 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 cursor-not-allowed dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-gray-400 dark:focus:ring-blue-500 dark:focus:border-blue-500" value="${weight}" placeholder="Input Berat Product..." />
                                <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>
                            </div>
                            <div>
                                <label for="UpdateQuantity${formatWeightAsGenerated}" class="mb-2 block text-sm font-bold text-gray-900 dark:text-white">Quantity*</label>
                                <input type="number" min="1" id="UpdateQuantity${formatWeightAsGenerated}" data-required="1" data-form="Qty" disabled class="bg-gray-100 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 cursor-not-allowed dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-gray-400 dark:focus:ring-blue-500 dark:focus:border-blue-500" value="${qty}" placeholder="Input Quantity..." />
                                <p class="mt-2 hidden text-sm font-bold text-red-600 dark:text-red-500">Input wajib di isi.</p>
                            </div>
                            <div class="mt-7">
                                ${1 == 1 ? `` : `<button type="button" id="FormGenerateUpdate${formatWeightAsGenerated}"" class="me-2 w-max rounded-lg bg-blue-700 px-5 py-2.5 text-sm font-medium text-white hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 focus:outline-none dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">Update</button>`}
                                <button type="button" id="FormDeleteUpdate${formatWeightAsGenerated}" class="me-2 w-max rounded-lg bg-red-700 px-5 py-2.5 text-sm font-medium text-white hover:bg-red-800 focus:ring-4 focus:ring-red-300 focus:outline-none dark:bg-blue-600 dark:hover:bg-red-500 dark:focus:ring-red-800">Delete</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="mt-5">
                    <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
                        <table class="w-full text-left text-sm text-gray-500 rtl:text-right dark:text-gray-400">
                            <thead class="bg-gray-50 uppercase text-gray-700 dark:bg-gray-700 dark:text-gray-400">
                                <tr>
                                    <th scope="col" class="px-6 py-3 hidden">ID</th>
                                    <th scope="col" class="px-6 py-3">PLU</th>
                                    <th scope="col" class="px-6 py-3">Berat</th>
                                    <th scope="col" class="px-6 py-3">Harga (24)</th>
                                    <th scope="col" class="px-6 py-3">Ongkos</th>
                                    <th scope="col" class="px-6 py-3">Total Harga (24)</th>
                                    <th scope="col" class="px-6 py-3">Total Harga Jual</th>
                                    <th scope="col" class="px-6 py-3">Total Berat (24K)</th>
                                    <th scope="col" class="px-6 py-3">HPPM</th>
                                    <th scope="col" class="px-6 py-3">
                                        <a href="zpl://?type=BULK&IDPrint=${IDPrint}&BRAND=${BrandKode}&LOGO=${RADIO_LOGO_SETTINGS}" class="text-white bg-gray-800 no-underline hover:bg-gray-900 focus:outline-none focus:ring-4 focus:ring-gray-300 font-medium inline-flex justify-content-center align-items-center rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-gray-800 dark:hover:bg-gray-700 dark:focus:ring-gray-700 dark:border-gray-700 hover:text-white hover:no-underline"><span class="mr-3">${PRINT_ICON}</span> Print All</button>
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="Tbody${formatWeightAsGenerated}">
                               ${tableRows}
                            </tbody>
                            <tfoot id="Tfoot${formatWeightAsGenerated}" class="font-bold">
                                <tr class="border-b bg-gray-50 dark:bg-gray-800 dark:border-gray-700">
                                    <th scope="row" class="whitespace-nowrap px-6 py-4 font-medium text-gray-900 dark:text-white"></th>
                                    <td class="px-6 py-4">${Weight} gr</td>
                                    <td class="px-6 py-4">${formatNumberToIDR(Harga24)}</td>
                                    <td class="px-6 py-4">${formatNumberToIDR(Ongkos)}</td>
                                    <td class="px-6 py-4">${formatNumberToIDR(TotalHarga24)}</td>
                                    <td class="px-6 py-4">${formatNumberToIDR(TotalHargaJual)}</td>
                                    <td class="px-6 py-4">${TotalBerat24}</td>
                                    <td class="px-6 py-4">${formatNumberToIDR(HPPM)}</td>
                                    <td></td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </div>
            </div>
    `;
}

const updateTable = (data, BrandKode) => {
    const result = [...data].slice(0, -1);
    const RADIO_LOGO_SETTINGS = $('input[type="radio"][name="IsWithLogo"]').filter(':checked').val();
    let tableRows = '';

    result.forEach(({
        ID,
        PLU,
        HPPM,
        Harga24,
        Ongkos,
        TotalBerat24,
        TotalHarga24,
        TotalHargaJual,
        Weight
    }) => {

        tableRows += `
             <tr class="border-b bg-white dark:bg-gray-800 dark:border-gray-700">
                 <td class="px-6 py-4 hidden">${ID}</th>
                 <td class="px-6 py-4">${PLU}</th>
                 <td class="px-6 py-4">${Weight} gr</td>
                 <td class="px-6 py-4">${formatNumberToIDR(Harga24)}</td>
                 <td class="px-6 py-4">${formatNumberToIDR(Ongkos)}</td>
                 <td class="px-6 py-4">${formatNumberToIDR(TotalHarga24)}</td>
                 <td class="px-6 py-4">${formatNumberToIDR(TotalHargaJual)}</td>
                 <td class="px-6 py-4">${TotalBerat24}</td>
                 <td class="px-6 py-4">${formatNumberToIDR(HPPM)}</td>
                 <td class="px-6 py-4">
                    <a href="zpl://?type=SINGLE&PLU=${PLU}&BRAND=${BrandKode}&LOGO=${RADIO_LOGO_SETTINGS}" class="text-white hover:text-white bg-gray-800 hover:bg-gray-900 focus:outline-none focus:ring-4 focus:ring-gray-300 inline-flex items-center font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-gray-800 dark:hover:bg-gray-700 dark:focus:ring-gray-700 dark:border-gray-700 hover:text-white hover:no-underline">
                        <span class="mr-3">${PRINT_ICON}</span> Print
                    </a>
                    <button id="DeletePlu_${PLU}" data-plu="${PLU}" data-formatted-weight="${formatWeightAsGenerated}" class="me-2 w-max rounded-lg bg-red-700 px-5 py-2.5 text-sm font-medium text-white hover:bg-red-800 focus:ring-4 focus:ring-red-300 focus:outline-none dark:bg-blue-600 dark:hover:bg-red-500 dark:focus:ring-red-800">Delete</button>
                 </td>
              </tr>
        `;
    });

    const { HPPM, Harga24, Ongkos, TotalBerat24, TotalHarga24, TotalHargaJual, Weight } = data[data.length - 1];

    let tableFoots = `<tr class="border-b bg-gray-50 dark:bg-gray-800 dark:border-gray-700">
        <th scope="row" class="whitespace-nowrap px-6 py-4 font-medium text-gray-900 dark:text-white"></th>
        <td class="px-6 py-4 font-bold">${Weight} gr</td>
        <td class="px-6 py-4 font-bold">${formatNumberToIDR(Harga24)}</td>
        <td class="px-6 py-4 font-bold">${formatNumberToIDR(Ongkos)}</td>
        <td class="px-6 py-4 font-bold">${formatNumberToIDR(TotalHarga24)}</td>
        <td class="px-6 py-4 font-bold">${formatNumberToIDR(TotalHargaJual)}</td>
        <td class="px-6 py-4 font-bold">${TotalBerat24}</td>
        <td class="px-6 py-4 font-bold">${formatNumberToIDR(HPPM)}</td>
        <td class="px-6 py-4"></td>
    </tr>`;

    return { tableRows, tableFoots };
}

const getAllGroupWeight = (isUpdate = true) => {
    let isGroupExists = true;
    const generatedPluInTable = [];

    if (isUpdate) {
        if ($('div[data-group-formatted-weight]').length === 0) {
            isGroupExists = false;
            return { isGroupExists, generatedPluInTable };
        }
    }

    const mapObject = {
        0: 'ID',
        1: 'PLU',
        2: 'Weight'
    }

    $('div[data-group-formatted-weight]').each((index, element) => {
        const formattedWeight = $(element).attr('data-group-formatted-weight');
        const tbody = $(`#Tbody${formattedWeight}`);

        tbody.each((_, tableBody) => {
            $(tableBody).find('tr').each((index, row) => {
                const rowData = {};

                $(row).find('td').each((cellIndex, cell) => {
                    let cellText = "";
                    if (cellIndex == 2) {
                        cellText = $(cell).text().split(" gr")[0]
                    } else {
                        cellText = $(cell).text().trim();
                    }

                    if (mapObject[cellIndex] !== undefined) {
                        rowData[mapObject[cellIndex]] = cellText;
                    }
                });

                generatedPluInTable.push(rowData);
            });
        });
    });


    return { isGroupExists, generatedPluInTable }

}

export const onDeleteGeneratedPLU = (e) => {
    const element = $(e.target);
    const id = element.attr("id");
    const formattedWeight = id.replace(/^FormDeleteUpdate/, '');

    const tbody = $(`#Tbody${formattedWeight}`);

    const generatedPluInTable = [];

    const mapObject = {
        0: 'ID',
        1: 'PLU',
        2: 'Weight'
    }

    tbody.each((_, tableBody) => {
        $(tableBody).find('tr').each((index, row) => {
            const rowData = {};

            $(row).find('td').each((cellIndex, cell) => {
                let cellText = "";
                if (cellIndex == 2) {
                    cellText = $(cell).text().split(" gr")[0]
                } else {
                    cellText = $(cell).text().trim();
                }

                if (mapObject[cellIndex] !== undefined) {
                    rowData[mapObject[cellIndex]] = cellText;
                }
            });

            generatedPluInTable.push(rowData);
        });
    });

    const { generatedPluInTable: generatedPluInTableAll } = getAllGroupWeight();

    onLoadingNewData({isLoading: true});

    $.ajax({
        url: "PG_Outs.aspx/DeleteGeneratedPLU",
        data: JSON.stringify({ UDT_PGOuts_PLU_Group: generatedPluInTable, UDT_PGOuts_PLU_All: generatedPluInTableAll, IsSingle: 0, PLU: "" }),
        contentType: "application/json; charset=utf-8",
        type: "POST",
        success: function (data) {
            const response = data['d'];
            if (typeof response === 'object' && 'MESSAGE' in response) {
                const message = response['MESSAGE'];
                if (message['MESSAGE_TYPE'] == 'ERROR') {
                    toastr.error(message['ERROR_MESSAGE']);
                    return;
                }

                const generatedData = response['DATA'];
                const additionalData = response['ADDITIONAL'];

                toastr.success("Successfully delete PLU");

                $('div[data-group-formatted-weight]').each(function (_, element) {
                    $(element).remove();
                });

                const uniqueWeightGroup = generatedData.reduce((accumulator, calculator) => {
                    if (!accumulator.includes(calculator['WeightGroup'])) {
                        accumulator = [...accumulator, calculator['WeightGroup']];
                    }
                    return accumulator;
                }, []);

                uniqueWeightGroup.forEach((value) => {
                    const filterWeightGroup = generatedData.filter((item) => item['WeightGroup'] == value);
                    const filterPrint = additionalData.find((item) => item['WeightGroup'] == value);
                    CONTENT_GENERATED_PLU.append(generateTable(filterWeightGroup, value.toFixed(2), "", filterWeightGroup.length - 1, filterPrint['IDPRINT'], filterPrint['KODEBRAND']));
                })

                return;
            }

            toastr.error(response);
        },
        error: function (error) {
            console.error(error);
        },
        complete: function (data) {
            onLoadingNewData({isLoading: false});

            if ($('div[data-group-formatted-weight]').length > 0) {
                $("#SubmitFormProduct").removeClass("hidden");
                return;
            }

            $("#SubmitFormProduct").addClass("hidden");
        }
    });

}

export const onSingleDeleteGeneratedPLU = (e) => {
    const element = $(e.target);
    const plu = element.attr("data-plu");
    const formattedWeight = element.attr("data-formatted-weight");

    const tbody = $(`#Tbody${formattedWeight}`);

    const generatedPluInTable = [];

    const mapObject = {
        0: 'ID',
        1: 'PLU',
        2: 'Weight'
    }

    tbody.each((_, tableBody) => {
        $(tableBody).find('tr').each((index, row) => {
            const rowData = {};

            $(row).find('td').each((cellIndex, cell) => {
                let cellText = "";
                if (cellIndex == 2) {
                    cellText = $(cell).text().split(" gr")[0]
                } else {
                    cellText = $(cell).text().trim();
                }

                if (mapObject[cellIndex] !== undefined) {
                    rowData[mapObject[cellIndex]] = cellText;
                }
            });

            generatedPluInTable.push(rowData);
        });
    });

    const { generatedPluInTable: generatedPluInTableAll } = getAllGroupWeight();

    onLoadingNewData({isLoading: true});

    $.ajax({
        url: "PG_Outs.aspx/DeleteGeneratedPLU",
        data: JSON.stringify({ UDT_PGOuts_PLU_Group: generatedPluInTable, UDT_PGOuts_PLU_All: generatedPluInTableAll, IsSingle: 1, PLU: plu }),
        contentType: "application/json; charset=utf-8",
        type: "POST",
        success: function (data) {
            const response = data['d'];
            if (typeof response === 'object' && 'MESSAGE' in response) {
                const message = response['MESSAGE'];
                if (message['MESSAGE_TYPE'] == 'ERROR') {
                    toastr.error(message['ERROR_MESSAGE']);
                    return;
                }

                const generatedData = response['DATA'];
                const additionalData = response['ADDITIONAL'];

                toastr.success("Successfully delete PLU");

                $('div[data-group-formatted-weight]').each(function (_, element) {
                    $(element).remove();
                });

                const uniqueWeightGroup = generatedData.reduce((accumulator, calculator) => {
                    if (!accumulator.includes(calculator['WeightGroup'])) {
                        accumulator = [...accumulator, calculator['WeightGroup']];
                    }
                    return accumulator;
                }, []);

                uniqueWeightGroup.forEach((value) => {
                    const filterWeightGroup = generatedData.filter((item) => item['WeightGroup'] == value);
                    const filterPrint = additionalData.find((item) => item['WeightGroup'] == value);
                    CONTENT_GENERATED_PLU.append(generateTable(filterWeightGroup, value.toFixed(2), "", filterWeightGroup.length - 1, filterPrint['IDPRINT'], filterPrint['KODEBRAND']));
                })

                return;
            }

            toastr.error(response);
        },
        error: function (error) {
            console.error(error);
        },
        complete: function (data) {
            onLoadingNewData({isLoading: false});

            if ($('div[data-group-formatted-weight]').length > 0) {
                $("#SubmitFormProduct").removeClass("hidden");
                return;
            }

            $("#SubmitFormProduct").addClass("hidden");
        }
    });
}

export const onGeneratePLU = () => {

    const weight = $("#InputWeight").val();
    const qty = $("#InputQuantity").val();
    const certificate = $("#InputCertificateNumber").val();


    if (weight == "" || weight == 0) {
        toastr.error("Pastikan berat item terisi.");
        return;
    }

    if (qty == "" || qty == 0) {
        toastr.error("Pastikan quantity item terisi.");
        return;
    }


    //let isGroupExists = false;

    //const groupWeight = $("div[data-group-weight]");

    //groupWeight.each((_, element) => {
    //    const currentWeightGroup = $(element).attr("data-group-weight");
    //    const changeFormatWeightGroup = currentWeightGroup.split(",").join(".");
    //    const changeFormatWeight = weight.split(",").join(".");



    //    if (parseFloat(changeFormatWeightGroup).toFixed(2) == parseFloat(changeFormatWeight).toFixed(2)) {
    //        toastr.error("Berat sudah terdaftar, silahkan gunakan update pada group table yang tersedia.");
    //        isGroupExists = true;
    //    };
    //});

    //if (isGroupExists) {
    //    return;
    //}

    const { generatedPluInTable, isGroupExists } = getAllGroupWeight(false);

    if (!isGroupExists) return;

    const { isInvalid, objectForm } = validateFormSubmit();

    if (isInvalid) return;

    Object.assign(objectForm, {
        NoCertificate: certificate,
        Qty: qty,
        Weight: weight,
    });

    $("#ButtonGenerate").html(SVG_LOADER).removeClass().addClass(DISABLED_BUTTON);

    const defaultTable = [
        {
            ID: 0,
            PLU: "",
            GrossWeight: 0
        }
    ];

    onLoadingNewData({ isLoading: true });


    $.ajax({
        url: "PG_Outs.aspx/GeneratePLU",
        data: JSON.stringify({ stock: objectForm, UDT_PGOuts_PLU: generatedPluInTable.length === 0 ? defaultTable : generatedPluInTable }),
        contentType: "application/json; charset=utf-8",
        type: "POST",
        success: function (data) {
            const response = data['d'];
            if (typeof response === 'object' && 'MESSAGE' in response) {
                const message = response['MESSAGE'];
                if (message['MESSAGE_TYPE'] == 'ERROR') {
                    toastr.error(message['ERROR_MESSAGE']);
                    return;
                }

                const generatedData = response['DATA'];
                const additionalData = response["ADDITIONAL"];
                toastr.success("Successfully generated PLU");



                $('div[data-group-formatted-weight]').each(function (_, element) {
                    $(element).remove();
                });

                const uniqueWeightGroup = generatedData.reduce((accumulator, calculator) => {
                    if (!accumulator.includes(calculator['WeightGroup'])) {
                        accumulator = [...accumulator, calculator['WeightGroup']];
                    }

                    return accumulator;
                }, []);

                uniqueWeightGroup.forEach((value) => {
                    const filterWeightGroup = generatedData.filter((item) => item['WeightGroup'] == value);
                    const filterPrint = additionalData.find((item) => item['WeightGroup'] == value);
                    CONTENT_GENERATED_PLU.append(generateTable(filterWeightGroup, value.toFixed(2), certificate, filterWeightGroup.length - 1, filterPrint['IDPRINT'], filterPrint['KODEBRAND']));
                })


            } else {
                toastr.error(response);
            }
        },
        error: function (error) {
            console.error(error);
        },
        complete: function (data) {
            onLoadingNewData({isLoading: false});

            $("#ButtonGenerate").text("Generate").removeClass().addClass(DEFAULT_BUTTON);
            $("#SubmitFormProduct").removeClass("hidden");
        }
    });
};

export const updateOrGeneratePLU = (e) => {
    const element = $(e.target);
    const id = element.attr("id");

    const formattedWeight = id.replace(/^FormGenerateUpdate/, '');


    const qty = $(`#UpdateQuantity${formattedWeight}`);
    const weight = $(`#UpdateWeight${formattedWeight}`);
    const certificate = $(`#UpdateCertificateNumber${formattedWeight}`);
    const tbody = $(`#Tbody${formattedWeight}`);
    const tfoot = $(`#Tfoot${formattedWeight}`);
    const TextWeight = $(`#TextGroupWeight${formattedWeight}`);
    const parentGroupWeightFormatted = $(`div[data-group-formatted-weight='${formattedWeight}']`);
    const deleteButton = $(`#FormDeleteUpdate${formattedWeight}`);
    const parentGroupWeight = $(`div[data-group-weight='${formattedWeight.split("_").join(".")}']`);
    const buttonPrintAll = $(`#Print${formattedWeight}`)
    const newFormattedWeight = weight.val().toString().split(".").join("_").split(",").join("_");
    let isGroupExists = false;


    if (weight.val() == "" || weight.val() == 0) {
        toastr.error("Pastikan berat item terisi.");
        return;
    }

    if (qty.val() == "" || qty.val() == 0) {
        toastr.error("Pastikan quantity item terisi.");
        return;
    }


    const groupWeight = $("div[data-group-weight]");
    groupWeight.each((_, element) => {
        const currentWeightGroup = $(element).attr("data-group-weight");
        const changeFormatWeightGroup = currentWeightGroup.split(",").join(".");
        const changeFormatWeight = weight.val().split(",").join(".");
        const oldWeight = TextWeight.text().split(" gr")[0]

        if (oldWeight == parseFloat(changeFormatWeightGroup).toFixed(2)) return;
        if (changeFormatWeightGroup == changeFormatWeight) {
            toastr.error("Berat sudah terdaftar, silahkan gunakan update pada group table yang tersedia.");
            isGroupExists = true;
        };
    });

    if (isGroupExists) {
        return;
    }

    const generatedPluInTable = [];

    const mapObject = {
        0: 'ID',
        1: 'PLU',
        2: 'Weight'
    }

    tbody.each((_, tableBody) => {
        $(tableBody).find('tr').each((index, row) => {
            const rowData = {};

            $(row).find('td').each((cellIndex, cell) => {
                let cellText = "";
                if (cellIndex == 2) {
                    cellText = $(cell).text().split(" gr")[0]
                } else {
                    cellText = $(cell).text().trim();
                }

                if (mapObject[cellIndex] !== undefined) {
                    rowData[mapObject[cellIndex]] = cellText;
                }
            });

            generatedPluInTable.push(rowData);
        });
    });

    const objectForm = {
        NoCertificate: certificate.val(),
        Qty: qty.val(),
        Weight: weight.val(),
    };

    TextWeight.empty().text(`${parseFloat(weight.val()).toFixed(2)} gr`);


    onLoadingNewData({ isLoading: true });
    $.ajax({
        url: "PG_Outs.aspx/GenerateOrEditPLU",
        data: JSON.stringify({ stock: objectForm, UDT_PGOuts_PLU: generatedPluInTable }),
        contentType: "application/json; charset=utf-8",
        type: "POST",
        success: function (data) {
            const response = data['d'];
            if (typeof response === 'object' && 'MESSAGE' in response) {
                const message = response['MESSAGE'];
                if (message['MESSAGE_TYPE'] == 'ERROR') {
                    toastr.error(message['ERROR_MESSAGE']);
                    return;
                }

                const generatedData = response['DATA'];
                const KODEBRAND = response['ADDITIONAL']["KODEBRAND"];
                toastr.success("Successfully generated PLU");

                const { tableRows, tableFoots } = updateTable(generatedData, KODEBRAND);

                tbody.empty().append(tableRows);
                tfoot.empty().append(tableFoots);


                qty.attr("id", `UpdateQuantity${newFormattedWeight}`);
                certificate.attr("id", `UpdateCertificateNumber${newFormattedWeight}`);
                weight.attr("id", `UpdateWeight${newFormattedWeight}`);
                tbody.attr("id", `Tbody${newFormattedWeight}`);
                tfoot.attr("id", `Tfoot${newFormattedWeight}`);
                TextWeight.attr("id", `TextGroupWeight${newFormattedWeight}`);
                element.attr("id", `FormGenerateUpdate${newFormattedWeight}`);
                parentGroupWeightFormatted.attr("data-group-formatted-weight", newFormattedWeight);
                parentGroupWeight.attr("data-group-weight", weight.val());
                deleteButton.attr("id", `FormDeleteUpdate${newFormattedWeight}`);
                buttonPrintAll.attr("id", `Print${newFormattedWeight}`);
            } else {
                toastr.error(response);
            }

        },
        error: function (error) {
            console.error(data);
        }, complete: function (data) {
            onLoadingNewData({ isLoading: false });

        }
    });
}

export const updateGeneratedPLU = () => {
    const { isGroupExists, generatedPluInTable } = getAllGroupWeight();
    if (!isGroupExists) return;

    const { isInvalid, objectForm } = validateFormSubmit();
    if (isInvalid) {
        toastr.error("Gagal update, terdapat form yang tidak valid.");
        return;
    };

    onLoadingNewData({isLoading: true});

    $.ajax({
        url: "PG_Outs.aspx/UpdateGeneratedPLU",
        data: JSON.stringify({ stock: objectForm, UDT_PGOuts_PLU: generatedPluInTable }),
        contentType: "application/json; charset=utf-8",
        type: "POST",
        success: function (data) {
            const response = data['d'];
            if (typeof response === 'object' && 'MESSAGE' in response) {
                const message = response['MESSAGE'];
                if (message['MESSAGE_TYPE'] == 'ERROR') {
                    toastr.error(message['ERROR_MESSAGE']);
                    return;
                }

                const generatedData = response['DATA'];
                const additionalData = response["ADDITIONAL"];
                toastr.success("Successfully generated PLU");



                $('div[data-group-formatted-weight]').each(function (_, element) {
                    $(element).remove();
                });

                const uniqueWeightGroup = generatedData.reduce((accumulator, calculator) => {
                    if (!accumulator.includes(calculator['WeightGroup'])) {
                        accumulator = [...accumulator, calculator['WeightGroup']];
                    }

                    return accumulator;
                }, []);

                uniqueWeightGroup.forEach((value) => {
                    const filterWeightGroup = generatedData.filter((item) => item['WeightGroup'] == value);
                    const filterPrint = additionalData.find((item) => item['WeightGroup'] == value);
                    CONTENT_GENERATED_PLU.append(generateTable(filterWeightGroup, value.toFixed(2), "", filterWeightGroup.length - 1, filterPrint['IDPRINT'], filterPrint['KODEBRAND']));
                })


            } else {
                toastr.error(response);
            }
        },
        error: function (error) {
            console.error(data);
        }, complete: function (data) {
            onLoadingNewData({ isLoading: false });
        }
    });
}

export const postingToStockProduct = (e) => {

    const element = $("#popup-modal").find('button');
    element.each((_, button) => {
        $(button).attr('disabled', true);
    });


    $("#ModalSubmitFormProduct").removeClass().addClass(DISABLED_BUTTON).html(SVG_LOADER);
    $("#ModalCancelFormProduct").removeClass().addClass(DISABLED_BUTTON_OUTLINE).html(SVG_LOADER);

    const { isGroupExists, generatedPluInTable } = getAllGroupWeight();
    if (!isGroupExists) {
        toastr.error("Tidak ada data yang ingin diposting!");
        element.each((_, button) => {
            $(button).attr('disabled', false);
        });

        $("#ModalSubmitFormProduct").removeClass().addClass(DEFAULT_BUTTON).html("Yes, I'm Sure");
        $("#ModalCancelFormProduct").removeClass().addClass(DEFAULT_BUTTON_OUTLINE).html("No, cancel");
        $("#hide-popup-modal").click();
        return;
    }

    try {
        $.ajax({
            url: "PG_Outs.aspx/PostingToStockProduct",
            data: JSON.stringify({ UDT_PGOuts_PLU: generatedPluInTable }),
            contentType: "application/json; charset=utf-8",
            type: "POST",
            success: function (data) {
                const response = data['d'];
                if (typeof response === 'object' && 'MESSAGE' in response) {
                    const message = response['MESSAGE'];
                    if (message['MESSAGE_TYPE'] == 'ERROR') {
                        toastr.error(message['ERROR_MESSAGE']);
                        return;
                    }


                    toastr.success("Successfully posting product!");

                    let protocol = window.location.protocol;
                    let host = window.location.host;
                    let result = protocol + '//' + host;

                    //window.open(
                    //    "https://dev3.cmk.co.id/StockReceive/Page/Report/PG_Detail.aspx?refreshPage=1&kw=&cabang=-1&productitem=0&model=0&productlevel=0&tipe=0&dari=01-12-2024&sampai=17-01-2025&supplier=0&consignment=2", "_blank");

                    window.location.reload();
                    window.open(result + response['DATA']["URL"], "_blank");
                    return;
                }

                toastr.error(response);
            },
            error: function (error) {
                console.error(data);
            }, complete: function (data) {
                $("#ModalSubmitFormProduct").removeClass().addClass(DEFAULT_BUTTON).html("Yes, I'm Sure");
                $("#ModalCancelFormProduct").removeClass().addClass(DEFAULT_BUTTON_OUTLINE).html("No, cancel");
                $("#hide-popup-modal").click();
                element.each((_, button) => {
                    $(button).attr('disabled', false);
                });
            }
        });
    } catch (error) {
        console.error(error);
        $("#ModalSubmitFormProduct").removeClass().addClass(DEFAULT_BUTTON).html("Yes, I'm Sure");
        $("#ModalCancelFormProduct").removeClass().addClass(DEFAULT_BUTTON_OUTLINE).html("No, cancel");
        $("#hide-popup-modal").click();
        element.each((_, button) => {
            $(button).attr('disabled', false);
        });
    }
}