<%@ page language="C#" masterpagefile="~/MasterFile.master"
autoeventwireup="true" codefile="ProductUpdate.aspx.cs"
inherits="Page_Account_PackagingUpdate" %> <%@ register src="MenuProduct.ascx"
tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server"> Packaging </asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
  <asp:literal id="h1_st" runat="server" />
</asp:content>
<asp:content contentplaceholderid="h1_id" runat="server">
  ID:<asp:literal id="h1_id" runat="server" />
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
  <uc1:menu id="menu" runat="server" />
  <form runat="server">
    <div class="form-group width-500">
      <div class="row">
        <div class="col-xs-7">
          <label> Brand : </label>
          <asp:DropDownList cssclass="form-control" id="brand" runat="server" />
        </div>
      </div>
    </div>
    <div class="form-group width-500">
      <div class="row">
        <div class="col-xs-7">
          <label> Nama : </label>
          <asp:requiredfieldvalidator id="nama_RF" runat="server" />
          <asp:textbox
            id="nama"
            runat="server"
            cssclass="form-control text-bold"
            maxlength="50"
          />
        </div>
      </div>
    </div>
    <div class="form-group width-500">
      <div class="row">
        <div class="col-xs-7">
          <label> Harga : </label>
          <asp:textbox
            id="harga"
            runat="server"
            cssclass="form-control text-bold"
            maxlength="50"
          />
        </div>
      </div>
    </div>
    <div class="form-group width-500">
      <label> Keterangan : </label>
      <asp:textbox
        id="keterangan"
        runat="server"
        cssclass="form-control"
        textmode="MultiLine"
        rows="4"
      />
    </div>

    <div class="form-group cb-single">
      <div id="div_draft" runat="server">
        <label for="content_disable">
          <strong class="text-draft">Status : Draft</strong>
          <small>Data tidak dimunculkan di dalam sistem</small>
        </label>
      </div>
      <div id="div_disable" runat="server">
        <asp:checkbox id="disable" runat="server" />
        <label for="content_disable">
          <strong class="text-warning">Status : Disable</strong>
          <small>Data tidak dimunculkan di dalam sistem</small>
        </label>
      </div>
    </div>
    <hr />
    <div class="button-inline">
      <button
        id="save"
        runat="server"
        class="btn btn-primary"
        onserverclick="save_Click"
        onclick="if (!onconfirmalert()) return false;"
      >
        <i class="icon-ok"></i>
        Save
      </button>

      <script>
        function onconfirmalert() {
          let nominals = document.getElementById("content_harga");
          if (nominals.value == "0" || nominals.value == "") {
            alert("Harga tidak boleh kosong atau bernilai 0.");
            nominals.focus();
            event.preventDefault(); // Mencegah form dikirim
            return false;
          } else {
            return true;
          }
        }
      </script>

      <asp:button
        id="del"
        runat="server"
        cssclass="btn btn-danger"
        text="Delete"
        width="80"
        onclick="del_Click"
      />
      <span id="nodel" runat="server" class="text-validator" visible="false"
        >Disable Delete : File Sudah Digunakan</span
      >
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
