<%@ page language="C#" masterpagefile="~/MasterLogin.master" autoeventwireup="true" codefile="Default.aspx.cs" inherits="_Default" %>

<asp:content contentplaceholderid="logo" runat="server">
    <div class="row">
	    <div class="col-xs-12 col-sm-12"><img runat="server" id="img_1" src="/Images/web/logo_web_cmk_2017.jpg" class="img-responsive" border="0" style="max-height:100px; margin:0 auto;" visible="false" /></div>
        <div class="col-xs-12 col-sm-12">
            <div class="col-xs-12 col-sm-3"></div>
            <div class="col-xs-12 col-sm-3"><img runat="server" id="img_2" src="/Images/web/logo_web_cmk_2017.jpg" class="img-responsive" border="0" style="max-height:100px; margin:0 auto;" visible="false" /></div>
            <div class="col-xs-12 col-sm-3"><img runat="server" id="img_3" src="/Images/web/logo_web_cmk_2017.jpg" class="img-responsive" border="0" style="max-height:100px; margin:0 auto;" visible="false" /></div>
            <div class="col-xs-12 col-sm-3"></div>
        </div>
        
    </div>
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form id="Form1" runat="server">
    <div class="row">
        <div class="col-xs-12">
	        <div id="debug" runat="server">
		        <i class="icon-warning-sign icon-5x pull-left"></i>
		        <h4>Security Disabled</h4>
		        <p class="text-muted">
			        Aplikasi dalam kondisi tanpa mekanisme proteksi halaman.
		        </p>
		        <div class="clearfix"></div>
		        <hr style="margin-bottom:0;" />
	        </div>
	    </div>
	</div>
        <div class="row">
            <div class="col-xs-12" style="padding-top:15px;">
                <div class="text-center hidden-xs">
                    <h2 style="font-family: 'Helvetica Neue', Arial, Helvetica, sans-serif; margin:0px;">
                        Jewellery Application Web Based System
                    </h2>
                </div>
                <div runat="server" id="div_mode" style="margin:0 auto; text-align:center;">
				    <span class="label label-danger"><asp:Literal runat="server" ID="lit_mode" /></span>
			    </div>
                <div class="text-center hidden-xs">
		            <hr />
                </div>
            </div>

        </div>
    <div class="row">
        <div class="col-xs-12 col-sm-2 col-md-2">

        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <%--<div class="col-xs-12 col-sm-4 col-md-4">
                <img src="Images/logo.jpg" border="0" class="img-responsive hidden-xs" style="border:none; max-height:140px; margin:0 auto;" />
                
            </div>--%>
            <div class="col-xs-12 col-sm-2 col-md-3">

            </div>
            <div class="col-xs-12 col-sm-8 col-md-6">
			    
			
                <div class="form-group">
				    <label>
					    Email
				    </label>
				    <asp:requiredfieldvalidator id="email_RF" runat="server" />
				    <div class="input-group input-group-md">
					    <span class="input-group-addon"><i class="icon-user"></i></span>
					    <asp:textbox id="email" runat="server" cssclass="form-control" spellcheck="false" />
				    </div>
			    </div>
			    <div class="form-group">
				    <label>
					    Password
				    </label>
				    <asp:requiredfieldvalidator id="pass_RF" runat="server" />
				    <span id="pass_CAPS" class="text-warning" style="display: none;">
					    <i class="icon-warning-sign"></i>
					    Caps-Lock
				    </span>
				    <div class="input-group input-group-md">
					    <span class="input-group-addon"><i class="icon-lock"></i></span>
					    <asp:textbox id="pass" runat="server" cssclass="form-control" textmode="Password" />
                        <span class="input-group-btn">
                            <button id="signin" runat="server" class="btn btn-primary" onserverclick="signIn">
				                <i class="icon-signin"></i>
				                Sign-In
			                </button>
                        </span>
				    </div>
			    </div>
			    <p>
				    <a href="DefaultPass.aspx">Lupa Password ?</a>
			    </p>
			    
			    <div id="gagal" runat="server">
				    <p class="text-warning">
					    <strong>Otorisasi Gagal</strong>
				    </p>
				    <p id="alasan" runat="server" />
			    </div>
            </div>
            
            <div class="col-xs-12 col-sm-2 col-md-3">

            </div>
        </div>
        <div class="col-xs-12 col-sm-2 col-md-2">

        </div>


        
        <%--<div class="col-xs-12 col-sm-12 col-md-12">
            <br />
            <div class="alert alert-danger" role="alert" visible="false" runat="server" id="div_announcement_1">
                <b>Perhatian !!</b><br />
                Kepada seluruh pengguna user login JAWS,<br />
                Sehubungan dengan adanya instruksi dari HRD terkait perubahan email pribadi non-corporate menjadi email corporate, dimohon untuk mengirimkan list dengan format :<br />
                - NIK (wajib)<br />
                - Nama<br />
                - Email lama (non-corporate)<br />
                - Email baru (corporate)<br />
                <br />
                ke email :<br />
                <b>it.support@centralmegakencana.com</b><br />
                <br />
                Contoh email corporate yang diperbolehkan yaitu berakhiran :<br />
                <b>centralmegakencana.com</b><br />
                <b>frankncojewellery.com</b><br />
                <b>mondialjeweler.com</b><br />
                <b>palacejeweler.com</b><br />
                <b>miss-mondial.com</b><br />
                <br />
                Semua penggunaan email diluar email corporate (selain yang disebutkan diatas) akan segera dinonaktifkan tanpa terkecuali per tanggal <b>9 Okt 2017</b>. Dimohon kepada seluruh pengguna login JAWS yang masih belum memberikan list sesuai format diatas untuk menginfokan segera ke bagian IT Support paling lambat seminggu sebelum tanggal eksekusi.<br />
                <br />
                Regards,<br />
                <b>Team IT Support</b>
            </div>
        </div>--%>
    </div>
    <div class="row">
        <div class="col-xs-12">
	        <div id="footer">
		        <p class="text-muted" style="text-align:center;">
			        PT Central Mega Kencana &copy; 2014
		        </p>
	        </div>
	    </div>
	</div>
    </form>
</asp:content>
