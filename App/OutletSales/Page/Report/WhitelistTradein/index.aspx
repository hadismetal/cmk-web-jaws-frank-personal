<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Page_Report_WhitelistTradeIn" %>


<asp:Content ContentPlaceHolderID="content" runat="server">

    <style>
        :root, :host {
            --font-sans: ui-sans-serif, system-ui, sans-serif, 'Apple Color Emoji', 'Segoe UI Emoji', 'Segoe UI Symbol', 'Noto Color Emoji';
            --font-mono: ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, 'Liberation Mono', 'Courier New', monospace;
            --color-yellow-50: oklch(98.7% 0.026 102.212);
            --color-yellow-600: oklch(68.1% 0.162 75.834);
            --color-blue-100: oklch(93.2% 0.032 255.585);
            --color-blue-200: oklch(88.2% 0.059 254.128);
            --color-blue-600: oklch(54.6% 0.245 262.881);
            --color-gray-50: oklch(98.5% 0.002 247.839);
            --color-gray-100: oklch(96.7% 0.003 264.542);
            --color-gray-200: oklch(92.8% 0.006 264.531);
            --color-gray-500: oklch(55.1% 0.027 264.364);
            --color-red-50: oklch(97.1% 0.013 17.38);
            --color-red-600: oklch(57.7% 0.245 27.325);
            --color-green-50: oklch(98.2% 0.018 155.826);
            --color-green-600: oklch(62.7% 0.194 149.214);
            --spacing: 0.4rem;
            --text-xs: 1.25rem;
            --text-xs--line-height: calc(1.50 / 1);
            --text-sm: 1.375rem;
            --text-sm--line-height: calc(1.75 / 1.375);
            --text-lg: 1.625rem;
            --text-lg--line-height: calc(2.25 / 1.625);
            --font-weight-semibold: 600;
            --font-weight-bold: 700;
            --radius-md: 0.875rem;
            --radius-xl: 1.5rem;
            --default-font-family: var(--font-sans);
            --default-mono-font-family: var(--font-mono);
        }


        *, ::after, ::before, ::backdrop, ::file-selector-button {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
            border: 0 solid;
        }

        *, ::before, ::after, ::backdrop {
            --tw-border-style: solid;
            --tw-font-weight: initial;
        }

        html, :host {
            line-height: 1.5;
            -webkit-text-size-adjust: 100%;
            tab-size: 4;
            font-family: var(--default-font-family, ui-sans-serif, system-ui, sans-serif, 'Apple Color Emoji', 'Segoe UI Emoji', 'Segoe UI Symbol', 'Noto Color Emoji');
            font-feature-settings: var(--default-font-feature-settings, normal);
            font-variation-settings: var(--default-font-variation-settings, normal);
            -webkit-tap-highlight-color: transparent;
        }

        hr {
            height: 0;
            color: inherit;
            border-top-width: 1px;
        }

        abbr:where([title]) {
            -webkit-text-decoration: underline dotted;
            text-decoration: underline dotted;
        }

        h1, h2, h3, h4, h5, h6 {
            font-size: inherit;
            font-weight: inherit;
            padding: 0;
            margin: 0;
        }

        a {
            color: inherit;
            -webkit-text-decoration: inherit;
            text-decoration: inherit;
        }

        b, strong {
            font-weight: bolder;
        }

        code, kbd, samp, pre {
            font-family: var(--default-mono-font-family, ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, 'Liberation Mono', 'Courier New', monospace);
            font-feature-settings: var(--default-mono-font-feature-settings, normal);
            font-variation-settings: var(--default-mono-font-variation-settings, normal);
            font-size: 1em;
        }

        small {
            font-size: 80%;
        }

        sub, sup {
            font-size: 75%;
            line-height: 0;
            position: relative;
            vertical-align: baseline;
        }

        sub {
            bottom: -0.25em;
        }

        sup {
            top: -0.5em;
        }

        table {
            text-indent: 0;
            border-color: inherit;
            border-collapse: collapse;
        }

        :-moz-focusring {
            outline: auto;
        }

        progress {
            vertical-align: baseline;
        }

        summary {
            display: list-item;
        }

        ol, ul, menu {
            list-style: none;
        }

        img, svg, video, canvas, audio, iframe, embed, object {
            display: block;
            vertical-align: middle;
        }

        img, video {
            max-width: 100%;
            height: auto;
        }

        button, input, select, optgroup, textarea, ::file-selector-button {
            font: inherit;
            font-feature-settings: inherit;
            font-variation-settings: inherit;
            letter-spacing: inherit;
            color: inherit;
            border-radius: 0;
            background-color: transparent;
            opacity: 1;
        }

        :where(select:is([multiple], [size])) optgroup {
            font-weight: bolder;
        }

            :where(select:is([multiple], [size])) optgroup option {
                padding-inline-start: 20px;
            }

        ::file-selector-button {
            margin-inline-end: 4px;
        }

        ::placeholder {
            opacity: 1;
        }

        @supports (not (-webkit-appearance: -apple-pay-button)) or (contain-intrinsic-size: 1px) {
            ::placeholder {
                color: currentcolor;

                @supports (color: color-mix(in lab, red, red)) {
                    color: color-mix(in oklab, currentcolor 50%, transparent);
                }
            }
        }

        textarea {
            resize: vertical;
        }

        ::-webkit-search-decoration {
            -webkit-appearance: none;
        }

        ::-webkit-date-and-time-value {
            min-height: 1lh;
            text-align: inherit;
        }

        ::-webkit-datetime-edit {
            display: inline-flex;
        }

        ::-webkit-datetime-edit-fields-wrapper {
            padding: 0;
        }

        ::-webkit-datetime-edit, ::-webkit-datetime-edit-year-field, ::-webkit-datetime-edit-month-field, ::-webkit-datetime-edit-day-field, ::-webkit-datetime-edit-hour-field, ::-webkit-datetime-edit-minute-field, ::-webkit-datetime-edit-second-field, ::-webkit-datetime-edit-millisecond-field, ::-webkit-datetime-edit-meridiem-field {
            padding-block: 0;
        }

        :-moz-ui-invalid {
            box-shadow: none;
        }

        button, input:where([type='button'], [type='reset'], [type='submit']), ::file-selector-button {
            appearance: button;
        }

        ::-webkit-inner-spin-button, ::-webkit-outer-spin-button {
            height: auto;
        }

        [hidden]:where(:not([hidden='until-found'])) {
            display: none !important;
        }

        .m-5 {
            margin: calc(var(--spacing) * 5);
        }

        .my-5 {
            margin-top: calc(var(--spacing) * 5);
            margin-bottom: calc(var(--spacing) * 5);
        }

        .mt-5 {
            margin-top: calc(var(--spacing) * 5);
        }

        .mb-5 {
            margin-bottom: calc(var(--spacing) * 5);
        }

        .my-3 {
            margin-block: calc(var(--spacing) * 3);
        }

        .mt-1 {
            margin-top: calc(var(--spacing) * 1);
        }

        .flex {
            display: flex;
        }

        .grid {
            display: grid;
        }

        .w-6 {
            width: calc(var(--spacing) * 6);
        }

        .w-full {
            width: 100%;
        }

        .w-1\/2 {
            width: 50%;
        }

        .w-3\/4 {
            width: 75%;
        }

        .w-max {
            width: max-content;
        }

        .grid-cols-1 {
            grid-template-columns: repeat(1, minmax(0, 1fr));
        }

        .flex-col {
            flex-direction: column;
        }

        .justify-between {
            justify-content: space-between;
        }

        .gap-2 {
            gap: calc(var(--spacing) * 2);
        }

        .gap-3 {
            gap: calc(var(--spacing) * 3);
        }

        .gap-5 {
            gap: calc(var(--spacing) * 5);
        }

        .rounded {
            border-radius: 0.75rem;
        }

        .rounded-full {
            border-radius: calc(infinity * 1px);
        }

        .rounded-md {
            border-radius: var(--radius-md);
        }

        .rounded-xl {
            border-radius: var(--radius-xl);
        }

        .border {
            border-style: var(--tw-border-style);
            border-width: 1px;
        }

        .border-t {
            border-top-style: var(--tw-border-style);
            border-top-width: 1px;
        }

        .border-blue-600 {
            border-color: var(--color-blue-600);
        }

        .border-gray-100 {
            border-color: var(--color-gray-100);
        }

        .border-gray-200 {
            border-color: var(--color-gray-200);
        }

        .border-yellow-600 {
            border-color: var(--color-yellow-600);
        }

        .border-green-600 {
            border-color: var(--color-green-600);
        }

        .border-red-600 {
            border-color: var(--color-red-600);
        }

        .bg-blue-100 {
            background-color: var(--color-blue-100);
        }

        .bg-blue-200 {
            background-color: var(--color-blue-200);
        }

        .bg-gray-50 {
            background-color: var(--color-gray-50);
        }

        .bg-yellow-50 {
            background-color: var(--color-yellow-50);
        }

        .bg-green-50 {
            background-color: var(--color-green-50);
        }

        .bg-red-50 {
            background-color: var(--color-red-50);
        }

        .p-2 {
            padding: calc(var(--spacing) * 2);
        }

        .p-5 {
            padding: calc(var(--spacing) * 5);
        }

        .px-3 {
            padding-inline: calc(var(--spacing) * 3);
        }

        .px-5 {
            padding-inline: calc(var(--spacing) * 5);
        }

        .py-0\.5 {
            padding-block: calc(var(--spacing) * 0.5);
        }

        .py-2 {
            padding-block: calc(var(--spacing) * 2);
        }

        .pb-5 {
            padding-bottom: calc(var(--spacing) * 5);
        }

        .text-lg {
            font-size: var(--text-lg);
            line-height: var(--tw-leading, var(--text-lg--line-height));
        }

        .text-sm {
            font-size: var(--text-sm);
            line-height: var(--tw-leading, var(--text-sm--line-height));
        }

        .text-xs {
            font-size: var(--text-xs);
            line-height: var(--tw-leading, var(--text-xs--line-height));
        }

        .font-bold {
            --tw-font-weight: var(--font-weight-bold);
            font-weight: var(--font-weight-bold);
        }

        .font-semibold {
            --tw-font-weight: var(--font-weight-semibold);
            font-weight: var(--font-weight-semibold);
        }

        .text-blue-600 {
            color: var(--color-blue-600);
        }


        .text-green-600 {
            color: var(--color-green-600);
        }

        .text-red-600 {
            color: var(--color-red-600);
        }

        .text-gray-500 {
            color: var(--color-gray-500);
        }

        .text-yellow-600 {
            color: var(--color-yellow-600);
        }

        .uppercase {
            text-transform: uppercase;
        }

        .sm\:grid-cols-3 {
            @media (width >= 40rem) {
                grid-template-columns: repeat(3, minmax(0, 1fr));
            }
        }

        .sm\:flex-row {
            @media (width >= 40rem) {
                flex-direction: row;
            }
        }

        .sm\:gap-0 {
            @media (width >= 40rem) {
                gap: calc(var(--spacing) * 0);
            }
        }

        .sm\:w-3\/4 {
            @media (width >= 40rem) {
                width: 75%;
            }
        }
    </style>

    <h1 style="border-bottom: dotted 2px #CCC; padding-bottom: 25px; margin-bottom: 25px; font-size: 36px;">
        <small id="module">Point of Sales</small>
        <br />
        Whitelist Tradein

    </h1>

    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="row">
                <div class="col-lg-12">
                    <div class="row">
                        <div class="col-lg-2">
                            <div class="input-group" style="width: 100%;">
                                <label for="Dari">Dari</label>
                                <asp:TextBox TextMode="Date" ID="Dari" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:Label ID="error_message" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <div class="col-lg-2">
                            <div class="input-group" style="width: 100%;">
                                <label for="Dari">Hingga</label>
                                <asp:TextBox TextMode="Date" ID="Hingga" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-2">
                            <div class="input-group" style="width: 100%;">
                                <label for="Dari">Tipe Lokasi</label>
                                <asp:DropDownList ID="tipe_lokasi" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Tipe_Lokasi_Change">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-2">
                            <div class="input-group" style="width: 100%;">
                                <label for="Dari">Outlet</label>
                                <asp:DropDownList ID="outlet" CssClass="form-control" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-2">
                            <div class="input-group" style="width: 100%;">
                                <label for="Dari">Status</label>
                                <asp:DropDownList ID="status" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="0">All</asp:ListItem>
                                    <asp:ListItem Value="1">Waiting For Approval</asp:ListItem>
                                    <asp:ListItem Value="98">Full Approved</asp:ListItem>
                                    <asp:ListItem Value="99">Rejected</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div style="margin-top: 10px;">
                <asp:Button runat="server" Text="Search" ID="Button_Search" CssClass="btn btn-primary" OnClick="OnClickFormSearch" />
            </div>

            <asp:Repeater ID="rptTransactions" runat="server">
                <ItemTemplate>
                    <div class="my-5">
                        <div class="flex w-full sm:w-3/4 flex-col rounded-xl border border-gray-200">

                            <!-- HEADER -->
                            <div class="w-full rounded-xl bg-gray-50 p-5">
                                <div class="flex flex-col gap-3 sm:flex-row sm:gap-0 w-full justify-between">
                                    <div class="flex flex-col">
                                        <h6 class="text-xs font-semibold text-gray-500">PLU</h6>
                                        <h4 class="text-sm font-bold"><%# Eval("Header.PLU") %></h4>
                                    </div>
                                    <div class="flex flex-col">
                                        <h6 class="text-xs font-semibold text-gray-500">No. Sales Order</h6>
                                        <h4 class="text-sm font-bold"><%# Eval("Header.NomorSalesOrder") %></h4>
                                    </div>
                                    <div class="flex flex-col">
                                        <h6 class="text-xs font-semibold text-gray-500">No. Invoice</h6>
                                        <h4 class="text-sm font-bold"><%# Eval("Header.Invoice") %></h4>
                                    </div>
                                </div>
                            </div>

                            <!-- TRANSACTION DETAILS -->
                            <div class="p-5">
                                <h5 class="font-semibold uppercase">Detail Transaksi</h5>
                                <div class="my-3 grid grid-cols-1 sm:grid-cols-3 gap-2 mt-5">
                                    <div>
                                        <h6 class="text-xs font-semibold text-gray-500">Store</h6>
                                        <h4 class="text-sm font-bold"><%# Eval("Header.Store") %></h4>
                                    </div>
                                    <div>
                                        <h6 class="text-xs font-semibold text-gray-500">Customer</h6>
                                        <h4 class="text-sm font-bold"><%# Eval("Header.Customer") %></h4>
                                    </div>
                                    <div>
                                        <h6 class="text-xs font-semibold text-gray-500">Tanggal Transaksi</h6>
                                        <h4 class="text-sm font-bold"><%# Eval("Header.TanggalSalesOrder") %></h4>
                                    </div>
                                </div>
                                <div class="rounded border border-blue-600 bg-blue-100 px-5 py-2">
                                    <h6 class="text-xs font-semibold text-blue-600">Nominal Transaksi</h6>
                                    <h4 class="text-lg font-bold text-blue-600"><%# Eval("Header.Nominal") %></h4>
                                </div>
                            </div>

                            <!-- REQUEST DETAILS -->
                            <div class="px-5">
                                <h5 class="font-semibold uppercase">Detail PENGAJUAN</h5>
                                <div class="flex flex-col gap-2">
                                    <div class="grid grid-cols-1 sm:grid-cols-3 gap-2 mt-5">
                                        <div>
                                            <h6 class="text-xs font-semibold text-gray-500">Store Pengaju</h6>
                                            <h4 class="text-sm font-bold"><%# Eval("Header.StorePengaju") %></h4>
                                        </div>
                                        <div>
                                            <h6 class="text-xs font-semibold text-gray-500">Tanggal Pengajuan</h6>
                                            <h4 class="text-sm font-bold"><%# Eval("Header.TanggalPengajuan") %></h4>
                                        </div>
                                        <div>


                                            <h6 class="text-xs font-semibold text-gray-500">Status Pengajuan</h6>
                                            <div class="mt-1 w-max rounded-full border 
                                                <%# 
                                                    Eval("Header.Status").ToString() == "98" ? "border-green-600 bg-green-50 text-green-600" :
                                                    Eval("Header.Status").ToString() == "99" ? "border-red-600 bg-red-50 text-red-600" :
                                                    "border-yellow-600 bg-yellow-50 text-yellow-600" 
                                                %> px-3 py-0.5 text-xs">
                                                <%# Eval("Header.StatusText") %>
                                            </div>
                                        </div>
                                    </div>
                                    <div>
                                        <h6 class="text-xs font-semibold text-gray-500">Jenis Alasan</h6>
                                        <h4 class="text-sm font-bold"><%# Eval("Header.Reason") %></h4>
                                    </div>
                                    <div>
                                        <h6 class="text-xs font-semibold text-gray-500">Alasan Pengajuan Whitelist</h6>
                                        <h4 class="text-sm font-bold"><%# Eval("Header.Keterangan") %></h4>
                                    </div>
                                    <div>
                                        <h6 class="text-xs font-semibold text-gray-500">Attachment</h6>
                                        <div class="mt-1 flex gap-2">
                                            <asp:Repeater ID="rptAttachments" runat="server" DataSource='<%# Eval("Files") %>'>
                                                <ItemTemplate>
                                                    <a target="_blank"
                                                        href="<%# Eval("Extension").ToString() != ".pdf" ? "/Images.aspx?path=" + Eval("FileName") : "/Pdf.aspx?path=" + Eval("FileName") %>">
                                                        <div class="w-max rounded-md border border-blue-600 bg-blue-200 p-2">
                                                            <img src='<%# Eval("Extension").ToString() == ".pdf" ? "/Images/png/pdf.png" : "/Images/png/img.png"  %>' class="w-6" />
                                                        </div>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- APPROVER -->
                            <div class="m-5 border-t"></div>
                            <div class="px-5 pb-5">
                                <h5 class="font-semibold uppercase">Approver</h5>
                                <div class="flex flex-col gap-5 mt-5">
                                    <asp:Repeater ID="rptApprovers" runat="server" DataSource='<%# Eval("Approvers") %>'>
                                        <ItemTemplate>
                                            <div class="grid grid-cols-1 sm:grid-cols-3 gap-2">
                                                <div>
                                                    <h6 class="text-xs font-semibold text-gray-500">Nama</h6>
                                                    <h4 class="text-sm font-bold"><%# Eval("Approval") %></h4>
                                                </div>
                                                <div>
                                                    <h6 class="text-xs font-semibold text-gray-500">Tanggal</h6>
                                                    <h4 class="text-sm font-bold"><%# Eval("ApprovalTgl") %></h4>
                                                </div>
                                                <div>
                                                    <h6 class="text-xs font-semibold text-gray-500">Keterangan</h6>
                                                    <div class="text-sm font-bold"><%# Eval("Keterangan") %></div>
                                                </div>
                                            </div>
                                            <div class="border border-gray-100"></div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </form>
    </div>

</asp:Content>


