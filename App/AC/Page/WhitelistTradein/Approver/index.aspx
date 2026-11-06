<%@ Page Language="C#" MasterPageFile="../MasterPage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Page_Register_Approver" %>


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
            padding: 0;
            margin: 0;
        }

        span {
            border: none;
            padding: 0;
            margin: 0;
        }
    </style>


    <div>
        <form id="element_FormSearchKaryawan" method="post" runat="server" defaultbutton="SubmitKaryawan">
            <div class="w-full p-10 rounded-lg shadow-lg">
                <h1 class="font-bold text-2xl">Pendaftaran Master Whitelist Trade-In</h1>
                <div class="flex gap-5 w-1/4">
                    <div class="w-full">
                        <label for="InputNIK" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Nomor Induk Karyawan</label>
                        <input runat="server" type="text" id="InputNIK" name="InputNIK" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="..." />
                    </div>
                    <div class="self-end justify-self-end">
                        <button type="submit" class="text-white w-max bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm w-full sm:w-auto px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">Search</button>
                    </div>
                </div>
                <h1 id="element_errorMessage" visible="true" runat="server" class="font-semibold text-red-500 text-sm mt-3"></h1>

                <div runat="server" id="element_contentKaryawan" visible="false">
                    <hr class="h-px my-8 bg-gray-200 border-0 dark:bg-gray-700" />
                    <div class="flex gap-5">
                        <div>
                            <img class="w-20 h-20 rounded-full" runat="server" id="element_image" alt="Rounded avatar" />
                        </div>
                        <div class="flex flex-col gap-3 text-gray-500">
                            <h1 class="font-bold text-black" id="element_name" runat="server"></h1>
                            <div class="flex gap-2">
                                <h1 id="element_divisi" runat="server"></h1>
                                <h1>/ </h1>
                                <h1 id="element_subdivisi" runat="server"></h1>
                            </div>
                            <h1 id="element_jabatan" runat="server"></h1>
                            <h1 id="element_email" runat="server"></h1>
                            <div class="flex items-center">
                                <input checked="checked" id="element_checkboxApprover" runat="server" type="checkbox" value="1" class="p-0 m-0 w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 rounded-sm focus:ring-blue-500 dark:focus:ring-blue-600 dark:ring-offset-gray-800 focus:ring-2 dark:bg-gray-700 dark:border-gray-600" />
                                <label for="element_checkboxApprover" class="p-0 m-0 ms-2 text-sm font-medium text-gray-900 dark:text-gray-300">Daftarkan sebagai approver.</label>
                            </div>
                            <asp:Button OnClick="OnSubmit_RegisterKaryawan" ID="SubmitKaryawan" runat="server" CssClass="self-start justify-self-start text-white w-max bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm w-full sm:w-auto px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800" Text="Register" />

                            <span id="element_errorMessageRegister" visible="false" runat="server" class="w-max font-bold bg-red-100 text-red-800 text-sm me-2 px-2.5 py-0.5 rounded-sm dark:bg-red-900 dark:text-red-300"></span>
                        </div>
                    </div>
                    <hr class="h-px my-8 bg-gray-200 border-0 dark:bg-gray-700" />
                </div>
            </div>

            <div class="w-full p-10 rounded-lg shadow-lg mt-10">
                <h1 class="text-2xl font-bold">Daftar User Whitelist Trade-In</h1>


                <div class="relative overflow-x-auto mt-5">
                    <asp:Repeater ID="RepeaterKaryawan" runat="server" OnItemDataBound="RepeaterKaryawan_ItemDataBound">
                        <HeaderTemplate>
                            <table class="w-full text-sm table-fixed text-left rtl:text-right text-gray-500 dark:text-gray-400">
                                <thead class="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                                    <tr>
                                        <th scope="col" class="px-6 py-3 w-[100px]">Image
                                        </th>
                                        <th scope="col" class="px-6 py-3 w-[300px]">Nama
                                        </th>
                                        <th scope="col" class="px-6 py-3 w-[300px]">Jabatan
                                        </th>
                                        <th scope="col" class="px-6 py-3 w-[300px]">Divisi
                                        </th>
                                        <th scope="col" class="px-6 py-3 w-[300px]">Email
                                        </th>
                                        <th scope="col" class="px-6 py-3 w-[150px]">Sebagai</th>
                                        <th scope="col" class="px-6 py-3 w-[150px]">Status</th>
                                        <th scope="col" class="px-6 py-3 w-[300px]">Action</th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 border-gray-200 text-xs">
                                <td class="px-6 py-4">
                                    <img class="w-12 h-12 rounded-full" src="<%# Eval("Image") %>" alt="Rounded avatar" />
                                </td>
                                <td class="px-6 py-4">
                                    <p class="font-bold text-black">
                                        <%# Eval("Nama") %>
                                    </p>
                                    <p class="font-semibold text-gray-500">
                                        <%# Eval("NIK") %>
                                    </p>
                                </td>
                                <td class="px-6 py-4 font-bold">
                                    <%# Eval("Jabatan") %>
                                </td>
                                <td class="px-6 py-4">
                                    <p class="font-bold text-black">
                                        <%# Eval("Divisi") %>
                                    </p>
                                    <p class="font-semibold text-gray-500">
                                        <%# Eval("Subdivisi") %>
                                    </p>
                                </td>
                                <td class="px-6 py-4 font-bold">
                                    <%# Eval("Email") %>
                                </td>
                                <td class="px-6 py-4 font-bold">
                                    <%# (int)Eval("IsApprover") == 1 ? "Approver" : "Mengetahui" %>
                                </td>
                                <td class="px-6 py-4 font-bold">
                                    <%# (int)Eval("IsDisabled") == 1 ? "Disabled" : "Enabled" %>
                                </td>
                                <td class="px-6 py-4 font-bold">
                                    <asp:Button OnCommand="ApproverMengetahuiMaster" ID="ButtonDataBoundApprover" OnClientClick="if (!confirm('Apakah anda yakin ingin mengubah ini?')) return false;" runat="server" />
                                    <asp:Button OnCommand="DisableEnableMaster" ID="ButtonDataBoundStatus" OnClientClick="if (!confirm('Apakah anda yakin ingin mengubah status ini?')) return false;" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>

            </div>

        </form>
    </div>
</asp:Content>
