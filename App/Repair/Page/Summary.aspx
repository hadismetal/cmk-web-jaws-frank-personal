<%@ page language="C#" masterpagefile="~/MasterPage.master"
autoeventwireup="true" codefile="Summary.aspx.cs" inherits="Page_Summary" %>

<asp:content contentplaceholderid="h1" runat="server"> Summary </asp:content>

<asp:content contentplaceholderid="content" runat="server">
  <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  </form>

  <div class="row">
    <div class="col-6 p-5">
      <table class="table-response table table-hover">
        <thead>
          <tr>
            <td class="fw-bold">Status</td>
            <td class="fw-bold">Jumlah</td>
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
        url: "./Summary.aspx/GetSummary",
        contentType: "application/json; charset=utf-8",
        data: { TipeLokasi: 9, IDLokasi: 0 },
        success: function (data) {
          const response = data["d"];

          response.forEach(({ StatusName, Jumlah, Url }) => {
            $("#tbody-summary").append(`
			<tr>
				<td>${StatusName}</td>
				<td><a target="_blank" href="${Url}">${Jumlah}</a></td>
			</tr>
		 `);
          });
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
