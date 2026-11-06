<%@ page language="C#" masterpagefile="~/MasterFile.master"
autoeventwireup="true" codefile="ProductUpdateInfo.aspx.cs"
inherits="Page_Account_PackagingUpdateAddr" %> <%@ register
src="MenuProduct.ascx" tagname="Menu" tagprefix="uc1" %>

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
    <table class="table-form">
      <tr>
        <td>Kode:</td>
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
        Save
      </button>
      <script>
        function onconfirmalert() {
          let kodes = document.getElementById("content_kode");
          let satuan = document.getElementById("content_satuan");
          if (kodes.value == "") {
            alert("Kode packaging tidak boleh kosong.");
            kodes.focus();
            event.preventDefault(); // Mencegah form dikirim
            return false;
          } else if (satuan.value == "") {
            alert("Satuan packaging tidak boleh kosong.");
            satuan.focus();
            event.preventDefault(); // Mencegah form dikirim
            return false;
          } else {
            return true;
          }
        }
      </script>
    </div>
  </form>
</asp:content>
