<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="ProductInfo.aspx.cs" inherits="Page_Create_PackagingAddr" %> 

<%@ register src="MenuProduct.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server"> Packaging </asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
  Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
  <uc1:menu id="menu" runat="server" />
  <p>
    <span class="text-muted">Nama :</span><br />
    <span class="text-lg" id="header_nama" runat="server" />
  </p>
  <hr />
  <form runat="server">
    <table class="table-form">
      <tr>
        <td
          colspan="3"
          id="err_message"
          runat="server"
          visible="false"
          class="text-danger"
        ></td>
      </tr>
      <tr>
        <td>Kode</td>
        <td>:</td>
        <td>
          <asp:textbox
            id="kode"
            runat="server"
            cssclass="form-control"
            width="300"
          />
        </td>
      </tr>
      <tr>
        <td>Satuan</td>
        <td>:</td>
        <td>
          <asp:textbox
            id="satuan"
            runat="server"
            cssclass="form-control"
            width="300"
          />
        </td>
      </tr>
	  <tr>
        <td>Tipe</td>
        <td>:</td>
        <td>
          <asp:DropDownList ID="ddltipe" runat="server" CssClass="form-control" width="300"
          />
        </td>
      </tr>
      <tr>
        <td>Harga</td>
        <td>:</td>
        <td>
          <div class="input-group">
            <asp:TextBox
              ID="harga"
              Text="0"
              runat="server"
              CssClass="form-control text-lg text-right"
              Width="300"
            />
            <span class="input-group-addon">IDR</span>
          </div>
        </td>
      </tr>
    </table>
    <hr />
    <div class="button-inline">
      <button
        id="save"
        runat="server"
        class="btn btn-primary"
        onserverclick="save_Click"
        onclick="if (!onconfirmalert()) return false;"
      >
        <i class="icon-arrow-right"></i>
        Next Step
      </button>
      <script>
        function onconfirmalert() {
          let nominals = document.getElementById("content_harga");
          let kodes = document.getElementById("content_kode");
          let satuans = document.getElementById("content_satuan");
          if (kodes.value == "") {
            alert("Kode packaging tidak boleh kosong.");
            kodes.focus();
            event.preventDefault(); // Mencegah form dikirim
            return false;
          } else if (satuans.value == "") {
            alert("Satuan packaging tidak boleh kosong.");
            satuans.focus();
            event.preventDefault(); // Mencegah form dikirim
            return false;
          } else if (nominals.value == "0" || nominals.value == "") {
            alert("Harga tidak boleh kosong atau bernilai 0.");
            nominals.focus();
            event.preventDefault(); // Mencegah form dikirim
            return false;
          } else {
            return true;
          }
        }
      </script>
    </div>
  </form>
  <script type="text/javascript">
    $(document).ready(function () {
      $("#content_harga").on("keyup", function () {
        var x = $("#content_harga").val();
        var y = addCommas(x);
        $("#content_harga").val(addCommas(x));
      });
    });

    function addCommas(x) {
      x = x.split(".").join("");
      var parts = x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
      //parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
      return parts;
    }
  </script>
</asp:content>
