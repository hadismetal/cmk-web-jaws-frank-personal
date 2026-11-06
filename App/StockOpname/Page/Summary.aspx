<%@ page language="C#" masterpagefile="~/MasterPage.master"
autoeventwireup="true" codefile="Summary.aspx.cs" inherits="Page_Summary" %>

<asp:content contentplaceholderid="h1" runat="server"> Summary </asp:content>

<asp:content contentplaceholderid="content" runat="server">
  <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  </form>

  <div class="row">        
    <div class="col-6 p-5">
      <h2>JR On Duty</h2>
      <hr>
      <table class="table-response table table-hover">
        <thead>
          <tr>
            <td class="fw-bold">No</td>
            <td class="fw-bold">Nama</td>
            <td class="fw-bold">Tipe Lokasi</td>
            <td class="fw-bold">Nama Lokasi</td>
            <td class="fw-bold">Shift</td>
            <td class="fw-bold">Tanggal Efektif</td>
          </tr>
        </thead>
        <tbody id="tbody-summary"></tbody>
      </table>
    </div>    
  </div>

  <script type="text/javascript">
    $(document).ready(function () {
      $.ajax({
        type: "GET",
        url: "./Summary.aspx/GetDataOnDuty",
        contentType: "application/json; charset=utf-8",        
        success: function (data) {
          let html=``;
          const response = data["d"];
          let no = 0;
          response.forEach((e,i) => {            
            no++;
            html+=`
              <tr>
                <td>${no}</td>
                <td>${e.Nama}</td>
                <td>${e.TipeLokasi}</td>
                <td>${e.NamaLokasi}</td>
                <td>${e.NamaShift}</td>
                <td>${e.Periode}</td>
              </tr>`;              
          });
          $('#tbody-summary').html(html)
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          console.log(data);
        },
      });
    });
  </script>
</asp:content>
