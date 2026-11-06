<%@ page EnableEventValidation="false" language="C#"
masterpagefile="~/MasterPage.master" autoeventwireup="true"
codefile="Product_Outgoing.aspx.cs" inherits="DJ" %>

<asp:content contentplaceholderid="h1" runat="server"> Packaging </asp:content>
<asp:content contentplaceholderid="h1_st" runat="server"> List </asp:content>

<asp:content contentplaceholderid="content" runat="server">
  <script>
    $(document).ready(function () {
      $("#content_lokasifrom").select2();
      $("#content_lokasito").select2();
    });
  </script>
  <form runat="server" class="form-inline">
    <table class="table-form">
      <tr>
        <td>Periode</td>
        <td>:</td>
        <td>
          <asp:TextBox
            ID="tgl"
            runat="server"
            CssClass="form-control text-center"
            Width="110"
          />
          <script>
            $(document).ready(function () {
              $("#content_tgl").datepicker({ format: "dd-mm-yyyy" });
            });
          </script>
        </td>
        <td>
          <asp:RequiredFieldValidator ID="tgl_RF" runat="server" />
          <span
            id="tgl_ERR"
            runat="server"
            class="text-validator"
            visible="false"
            >Format</span
          >
        </td>
        <td>s/d&nbsp;&nbsp;</td>
        <td>
          <asp:TextBox
            ID="tgl2"
            runat="server"
            CssClass="form-control text-center"
            Width="110"
          />
          <script>
            $(document).ready(function () {
              $("#content_tgl2").datepicker({ format: "dd-mm-yyyy" });
            });
          </script>
        </td>
        <td>
          <asp:RequiredFieldValidator ID="tgl2_RF" runat="server" />
          <span
            id="tgl2_ERR"
            runat="server"
            class="text-validator"
            visible="false"
            >Format</span
          >
        </td>
        <td>
          <asp:TextBox
            ID="kw"
            Visible="false"
            runat="server"
            CssClass="form-control text-center"
            Width="300"
            placeholder="No. Dokumen"
          />
        </td>
      </tr>
    </table>
    <div class="form-group">
      <div class="text-muted">Company Brand :</div>
      <asp:DropDownList
        ID="brand"
        runat="server"
        CssClass="form-control"
        Width="200"
      >
        <asp:ListItem Value="-1">All</asp:ListItem>
      </asp:DropDownList>
    </div>
    <div class="form-group">
      <div class="text-muted">Location Type (From)</div>
      <asp:DropDownList
        ID="tipelokasifrom"
        runat="server"
        CssClass="form-control"
        Width="175"
        OnSelectedIndexChanged="FillLokasi"
        AutoPostBack="true"
      >
      </asp:DropDownList>
    </div>
    <div class="form-group">
      <div class="text-muted">Location (From)</div>
      <asp:DropDownList ID="lokasifrom" runat="server" Width="175">
      </asp:DropDownList>
    </div>
    <div class="form-group">
      <div class="text-muted">Location Type (To)</div>
      <asp:DropDownList
        ID="tipelokasito"
        runat="server"
        CssClass="form-control"
        Width="175"
        OnSelectedIndexChanged="FillLokasiTo"
        AutoPostBack="true"
      >
      </asp:DropDownList>
    </div>
    <div class="form-group">
      <div class="text-muted">Location (To)</div>
      <asp:DropDownList ID="lokasito" runat="server" Width="175">
      </asp:DropDownList>
    </div>
    <div class="form-group">
      <div class="text-muted">Packaging</div>
      <asp:DropDownList
        ID="packaging"
        runat="server"
        CssClass="form-control"
        Width="175"
      >
      </asp:DropDownList>
    </div>
    <div class="form-group">
      <div class="text-muted">Status Outgoing :</div>
      <asp:DropDownList
        ID="status"
        runat="server"
        CssClass="form-control"
        Width="200"
      >
        <asp:ListItem Value="-1">All</asp:ListItem>
      </asp:DropDownList>
    </div>
    <div class="form-group">
      <div class="hidden-xs">&nbsp;</div>
      <div>
        <button
          id="go"
          runat="server"
          class="btn btn-primary"
          onserverclick="refreshPage"
        >
          <i class="icon-search"></i>
          Display
        </button>
        <button
          id="export"
          runat="server"
          class="btn btn-primary"
          onserverclick="Export"
        >
          <i class="icon-search"></i>
          Export
        </button>
      </div>
    </div>
    <div class="table-responsive">
      <asp:table id="tb" runat="server" cssclass="table-list">
        <asp:tableheaderrow>
          <asp:tableheadercell width="40">No.</asp:tableheadercell>
          <asp:tableheadercell width="200">No Dokumen</asp:tableheadercell>
          <asp:tableheadercell width="200">Tanggal</asp:tableheadercell>
          <asp:tableheadercell width="150">From</asp:tableheadercell>
          <asp:tableheadercell width="150">To</asp:tableheadercell>
          <asp:tableheadercell width="250">Packaging</asp:tableheadercell>
          <asp:tableheadercell width="100">Qty Kirim</asp:tableheadercell>
          <asp:tableheadercell width="100">Qty Terima</asp:tableheadercell>
          <asp:tableheadercell width="100">Selisih Terima</asp:tableheadercell>
          <asp:tableheadercell width="100">Status</asp:tableheadercell>
        </asp:tableheaderrow>
      </asp:table>
    </div>
  </form>
</asp:content>
