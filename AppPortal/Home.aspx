<%@ page language="C#" masterpagefile="~/MasterHome.master" autoeventwireup="true" codefile="Home.aspx.cs" inherits="Home" %>

<asp:content contentplaceholderid="content" runat="server">
	<div class="row" style="background: linear-gradient(to bottom right, white, transparent); padding:15px 0;">
        <div class="hidden-xs hidden-sm col-md-3">
            <div class="row">
	            <div class="col-xs-12 col-sm-4"><img runat="server" id="img_1" src="/Images/web/logo_web_cmk_2017.jpg" class="img-responsive" border="0" style="max-height:60px; margin:0 auto;" visible="false" /></div>
                <div class="col-xs-12 col-sm-4"><img runat="server" id="img_2" src="/Images/web/logo_web_cmk_2017.jpg" class="img-responsive" border="0" style="max-height:60px; margin:0 auto;" visible="false" /></div>
                <div class="col-xs-12 col-sm-4"><img runat="server" id="img_3" src="/Images/web/logo_web_cmk_2017.jpg" class="img-responsive" border="0" style="max-height:60px; margin:0 auto;" visible="false" /></div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-12 col-md-9">
            <div class="row">
                <div class="col-xs-12 col-sm-3 col-md-3">
			        <p>
				        <span class="text-muted">User Account</span><br />
				        <span class="text-lg" id="nama" runat="server" />
			        </p>
		        </div>
                <div class="col-xs-12 col-sm-3 col-md-3">
			        <div class="button-left" runat="server" id="div_IT" visible="false">
				        <p>
                            <a href="/HomeIT.aspx" class="btn btn-default" type="button" style="color:red;">
                                <span class="glyphicon glyphicon-fire" aria-hidden="true"></span>&nbsp;Warning <span class="badge" runat="server" style="background-color:red;" id="span_badge"></span> 
                            </a>
				        </p>
                    </div>
		        </div>
		        <div class="col-xs-12 col-sm-3 col-md-3">
			        <div class="button-left">
				        <p>
					        <a href="/HomeProfile.aspx" class="btn btn-default">
						        <i class="icon-user icon-large"></i>
						        Profile
					        </a>
				        </p>
                        <p id="gantipass" runat="server">
                            <span class="text-validator">Update Password Berhasil</span>
                        </p>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-3 col-md-3">
                    <div class="button-left">
				        <p>
					        <a href="/?so=1" class="btn btn-default text-center">
						        <i class="icon-signout icon-large"></i>
						        Sign-Out
					        </a>
				        </p>
			        </div>
                </div>
            </div>
        </div>
		
	</div>
	<form id="Form2" runat="server">
	<div id="ketentuan" runat="server">
		<h3 class="text-info">Ketentuan Pengguna</h3>
		<ol>
			<li>Sistem Aplikasi dan Data yang terkandung di dalamnya adalah <strong>MILIK PERUSAHAAN</strong> dan bersifat <strong>RAHASIA</strong>.</li>
			<li>Sistem Aplikasi adalah <strong>FASILITAS</strong> yang disediakan dari perusahaan kepada Anda untuk
				membantu Anda dalam menjalankan tugas sehari-hari. Sistem Aplikasi juga berfungsi
				sebagai sarana komunikasi, kontrol dan koordinasi antar bagian. Oleh sebab itu,
				partisipasi Anda sebagai Pengguna dari Sistem Aplikasi sifatnya adalah <strong>WAJIB TANPA
				KECUALI</strong>.
			</li>
			<li>Harap diperhatikan bahwa <strong>Username dan Password</strong> adalah mekanisme Sistem Aplikasi
				untuk melakukan proses identifikasi dan merupakan bagian dari skema pertanggungjawaban.
				Setiap tindakan dan kegiatan Anda di dalam Sistem Aplikasi akan selalu dimonitor
				oleh Perusahaan. Oleh sebab itu, simpan dan <strong>RAHASIAKAN</strong> selalu Password Anda.
			</li>
			<li>Tindakan berikut dikategorikan sebagai pelanggaran hukum pidana dan akan ditindak-lanjuti
				dengan <u>Pemutusan Hubungan Kerja</u> dan proses hukum yang berlaku :
				<ul>
					<li>Pencurian dan Perusakan Data dengan unsur kesengajaan.</li>
					<li>Pencurian dan Perusakan Soft-Copy Sistem Aplikasi dengan unsur kesengajaan.</li>
					<li>Merusak keseluruhan dan atau sebagian dari Sistem Aplikasi dengan unsur kesengajaan.</li>
					<li>Pencurian dan Perusakan Hardware Komputer, Hardware Jaringan dan perangkat penunjang lainnya.</li>
					<li>Menggunakan Sistem Aplikasi untuk mendapatkan keuntungan pribadi.</li>
				</ul>
			</li>
			<li>Tindakan berikut dikategorikan sebagai pelanggaran dan akan ditindak-lanjuti
				dengan pemberian <u>Surat Peringatan</u> sesuai ketentuan :
				<ul>
					<li>Memberikan Akses kepada Pihak Luar dengan sepengetahuan Anda.</li>
					<li>Meninggalkan Workstation Anda tanpa melakukan proses Sign-Out yang menyebabkan Pihak Luar dapat mengakses Sistem Aplikasi.</li>
					<li>Instalasi Aplikasi Virus dan sejenisnya ke dalam Sistem Aplikasi, baik sengaja maupun tidak.</li>
					<li>Masuk ke dalam Aplikasi dengan menggunakan Username rekan kerja Anda dan tanpa sepengetahuan Atasan Langsung.</li>
					<li>Mengetahui terjadinya tindakan pelanggaran dan tidak melaporkannya kepada Atasan Langsung.</li>
				</ul>
			</li>
		</ol>
			
            <asp:LinkButton ID="setuju" 
                            runat="server" 
                            CssClass="btn btn-primary"    
                            OnClick="setuju_Click">
                <span aria-hidden="true" class="glyphicon glyphicon-ok"></span> Saya Setuju
            </asp:LinkButton>
		
        <br /><br />
	</div>
    <div id="usernumber" runat="server">
		<h3 class="text-info">Update Login System</h3>
        <h4>Pembaharuan dalam Sistem Login Aplikasi (9 Oktober 2017)</h4>
        Seluruh user login aplikasi diwajibkan untuk :
        <ul>
            <li>menginput NIK (Nomor Induk Karyawan).</li>
			<li>menginput tanggal lahir (yang terdaftar di data personalia HRD) sebagai konfirmasi NIK</li>
		</ul>
        Catatan tambahan :
        <ul>
            <li>Silahkan menginput NIK dan tanggal lahir <span style="color:red;">hanya dengan menggunakan angka</span></li>
		</ul>
        <hr />
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="row">
                    <div class="col-xs-12 col-sm-4 col-md-3">
                        <div class="form-group width-100">
			                <label>
				                NIK
			                </label>
			                <asp:textbox id="txt_usernumber" runat="server" MaxLength="6" cssclass="form-control text-center" />
		                </div>
                    </div>
                    <div class="col-xs-12 col-sm-4 col-md-3">
		                <div class="form-group width-100">
			                <label>
				                Tanggal Lahir
			                </label>
			                <asp:textbox id="txt_lahir_tgl" runat="server" MaxLength="2" cssclass="form-control text-center" />
		                </div>
                    </div>
                    <div class="col-xs-12 col-sm-4 col-md-3">
		                <div class="form-group width-100">
			                <label>
				                Bulan Lahir
			                </label>
			                <asp:textbox id="txt_lahir_bln" runat="server" MaxLength="2" cssclass="form-control text-center" />
		                </div>
                    </div>
                    <div class="col-xs-12 col-sm-4 col-md-3">
		                <div class="form-group width-100">
			                <label>
				                Tahun Lahir
			                </label>
			                <asp:textbox id="txt_lahir_thn" runat="server" MaxLength="4" cssclass="form-control text-center" />
		                </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <div class="button-inline-noborder" runat="server" id="div_search" visible="false">
			                <button id="btn_search" runat="server" class="btn btn-primary" onserverclick="btn_search_Click">
				                <i class="icon-search"></i>
				                Search
			                </button>
		                </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <p class="text-red" id="error" runat="server" visible="false">
		            <asp:Literal runat="server" ID="errMsg" />
	            </p>
		        <table class="table-file">
		            <tr>
			            <td class="text-label">Nama</td>
			            <td>:</td>
			            <td id="txt_usernama" runat="server" />
		            </tr>
		            <tr>
			            <td class="text-label">Jabatan</td>
			            <td>:</td>
			            <td id="txt_userjabatan" runat="server" />
		            </tr>
		            <tr>
			            <td class="text-label">Status</td>
			            <td>:</td>
			            <td id="txt_userstatus" runat="server" />
		            </tr>
	            </table>
                <br />
                <div class="button-inline-noborder" runat="server" id="div_save" visible="false">
			        <button id="btn_save" runat="server" class="btn btn-success" onserverclick="btn_save_Click">
				        <i class="icon-ok"></i>
				        Save
			        </button>
		        </div>
            </div>

            
        </div>
        
	    
	<hr />
	</div>
	</form>
	<div id="module" runat="server" class="row">
		<div id="div1" runat="server" class="col-xs-12 col-sm-3 col-md-3" />
		<div id="div2" runat="server" class="col-xs-12 col-sm-3 col-md-3" />
		<div id="div3" runat="server" class="col-xs-12 col-sm-3 col-md-3" />
		<div id="div4" runat="server" class="col-xs-12 col-sm-3 col-md-3" />
	</div>
	<div id="footer">
		<p class="text-muted" style="text-align:center;">
			PT Central Mega Kencana &copy; 2014
		</p>
	</div>
</asp:content>
