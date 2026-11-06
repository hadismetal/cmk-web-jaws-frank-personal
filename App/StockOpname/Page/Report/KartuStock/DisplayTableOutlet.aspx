<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DisplayTableOutlet.aspx.cs" inherits="Page_DisplayTableOutlet" %>

<asp:content contentplaceholderid="h1" runat="server">
	Display Table Outlet
</asp:content>

<asp:content contentplaceholderid="content" runat="server">

    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">            
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Location :
                </div>
                <asp:DropDownList ID="location" runat="server" CssClass="form-control" Width="175">                    
                </asp:DropDownList>
                <button type="button" id="btn-display" class="btn btn-primary" style="margin-left: 10px;">
                        <i class="icon-search"></i>
                        Display
                    </button>                    
            </div>                        
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>                                    
        </form>
        <div class="table-responsive">
            <table ID="tb" class="table-list table-striped text-sm">
                <thead>
                    <tr>
                        <th Width="40">No.</th>
                        <th Width="150">Nama Lokasi</th>
                        <th Width="150">Display Table</th>
                        <th Width="80">Status</th>
                        <th Width="150">Tanggal Buat</th>
                        <th Width="150">Operator</th>                    
                        <th Width="150">Action</th>                    
                    </tr>
                </thead>
                <tbody id="table_body"></tbody>
            </table>
                
        </div>
        <asp:Literal ID="paging_text" runat="server" />
    </div>

    <div class="modal fade" id="modalUpdate" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="modalUpdateLabel" aria-hidden="true">
		<div class="modal-dialog">
			<div class="modal-content">
			<div class="modal-header">
				<h5 class="modal-title" id="modalUpdateHeader"></h5>
				<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
			</div>
			<div class="modal-body">				
				
			</div>
			<div class="modal-footer">
				
			</div>
			</div>
		</div>
	</div>
    
    <script>
        $(document).ready(function () {            
            $('#btn-display').click(function(){
                const IDLokasi = $('#<%=location.ClientID %>').val();						                
                console.log(IDLokasi)
                $.ajax({
                    type: "GET",
                    url: "DisplayTableOutlet.aspx/GetData",
                    data: { IDLokasi: IDLokasi},
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {                        
                        var data = response.d;
                        let html = ``;
                        let no = 0 ;
                        let button = ``;
                        data.forEach((e,i) => {
                            if (e.Status == 1) {
                                button = `<a href="#" class="btn btn-sm btn-warning" data-bs-toggle="modal" data-bs-target="#modalUpdate" data-id="${e.ID}" data-tipe="update" data-nama="${e.NamaDisplay}">
                                    <i class="icon-pen"></i> Update
                                </a>
                                <a href="#" class="btn btn-sm btn-outline-danger" data-bs-toggle="modal" data-bs-target="#modalUpdate" data-id="${e.ID}" data-tipe="delete" data-nama="${e.NamaDisplay}">
                                    <i class="icon-pen"></i> Non-Aktifkan
                                </a>`;
                            } else {
                                button = `<a href="#" class="btn btn-sm btn-success" data-bs-toggle="modal" data-bs-target="#modalUpdate" data-id="${e.ID}" data-tipe="aktif" data-nama="${e.NamaDisplay}">
                                    <i class="icon-pen"></i> Aktifkan
                                </a>
                                `;
                            }

                            no++;
                            html += `                        
                                <tr>
                                    <td>${no}</td>
                                    <td>${e.NamaLokasi}</td>
                                    <td>${e.NamaDisplay}</td>
                                    <td>${e.Status == 1 ? '<badge class="badge bg-success">Aktif</badge>' : '<badge class="badge bg-secondary">Tidak Aktif</badge>'}</td>
                                    <td>${e.Created_at}</td>
                                    <td>${e.Created_by}</td>
                                    <td class="text-center">                                        
                                    ${button}
                                    </td>   
                                </tr>
                            `;
                        });
                        
                        $('#table_body').html(html);

                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            });
        
            $('#modalUpdate').on('show.bs.modal', function (event) {
                var button = $(event.relatedTarget); // Button that triggered the modal
                var id = button.data('id'); // Extract info from data-* attributes
                var nama = button.data('nama');                
                var tipe = button.data('tipe');
                let html = ``;
                if (tipe === "update") {
                    html = `<div class="form-group">
                        <label for="namaDisplay">Nama Display:</label>
                        <input type="text" value="${nama}" id="namaDisplay" class="form-control">
                    </div>`;
                }else if (tipe === "delete") {
                    html = `<p>Apakah kamu yakin ingin menonaktifkan display table <strong>${nama}</strong> ?</p>`;
                }else if (tipe == 'aktif') {
                    html = `<p>Apakah kamu yakin ingin mengaktifkan display table <strong>${nama}</strong> ?</p>`;                    
                }
                $('#modalUpdateHeader').html(`Data Display Table ${nama}`);
                $('#modalUpdate .modal-body').html(html);

                $('#modalUpdate .modal-footer').html(`
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-${tipe === "update" ? "warning" : tipe == "aktif" ? "success" : "danger"}" data-id="${id}" data-nama="${nama}" data-tipe="${tipe}" id="modalSaveBtn">${tipe === "update" ? 'Update' : tipe == "aktif" ? 'Aktifkan' : 'Non-Aktif'}</button>
                `);

                var modal = $(this);            
            });


            $(document).on('click', '#modalSaveBtn', function (e) {
				$(this).attr('disabled', "disabled")
				e.preventDefault();
				const div = $(this);	
				const ID = div.data('id');				
				const tipe = div.data('tipe');
                const namaOld = div.data('nama');
                const namaBaru = $('#namaDisplay').val();                

				if (namaBaru == namaOld && tipe == 'update') {
					toastr.warning('Tidak ada perubahan pada data tersebut!!')
					$(this).removeAttr('disabled','disabled')
					return;
				}else if (namaBaru == namaOld && tipe == 'update') {
                    toastr.warning('Nama Display tidak boleh kosong!');
                    $(this).removeAttr('disabled','disabled')
                    return;
                }else{				
					$.ajax({					
						type: "POST",
						url: "./DisplayTableOutlet.aspx/UpdateData",
						data: JSON.stringify({ ID: ID, tipe:tipe, NamaDisplay: namaBaru?namaBaru:'' }),
						contentType: "application/json; charset=utf-8",
						dataType: "json",
						success: function (response) {                    
							let data = response.d;                            
                            if (data.Status == 200) {
                                toastr.success(data.Message);                                
                                $('#btn-display').click(); // Refresh table
                                $('#modalUpdate').modal('hide');
                            } else {
                                toastr.error(data.Message);                                
                            }
							
						},
						error: function (xhr, status, error) {                    
							toastr.error(`Error saving : ${error}`);
							$(this).removeAttr('disabled', "disabled")							
						}
					});				
				}			
			})
        });
        
    </script>
</asp:content>

