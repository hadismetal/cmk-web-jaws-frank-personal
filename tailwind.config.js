/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
        "./App/StockReceive/Page/Register/MasterKaret/*.{html,js,aspx,cs, aspx.cs}",
        "./App/AC/Page/WhitelistTradein/Approver/*.{html,js,aspx,cs,aspx.cs}",
        "./App/StockReceive/Page/Register/PG/Outs/*.{html,js,aspx,cs,aspx.cs}"
  ],
  theme: {
    extend: {},
  },
  plugins: [require("flowbite/plugin")],
};
