<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FAQ.aspx.cs" Inherits="Page_FAQ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" Runat="Server">
    FAQ
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" Runat="Server">
    Outlet Sales
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" Runat="Server">
<div class ="container">
<div class="row">
    <form runat="server">
        <asp:ScriptManager EnablePartialRendering="true" ID="ScriptManager1" runat="server"></asp:ScriptManager>
       <%-- <div class="col-xs-12 col-sm-5 col-md-4">
  
            <div class="input-group">
              <input type="text" class="form-control" placeholder="Search for ..." id="tsearch" runat="server"/>
              <span class="input-group-btn">
                <button class="btn btn-info" type="button" id="bsearch" runat="server" onclick="Search">
                    <i class="icon-search"></i>
                </button>
              </span>
            </div>

      
            <p id="listqa" runat="server" class="well">
             1. Payment <br />
                 <a>  1. Bagaimana ----</a><br />
                 <a>  2. Bagaimana ----</a><br />
              2.  Discount <br />
                 <a>  2.1. Bagaimana ----</a><br />
                 <a>  2.2. Bagaimana ----</a><br />
            </p>
          
                   
        </div>--%>

        <div class="col-xs-12 col-sm-7 col-md-8">
           <p id="content1" runat="server">
               <div class="panel panel-info">
                    <div class="panel-heading">
                         <h3 class="panel-title">
                             <asp:Label ID="lquestion" runat="server" Font-Bold="true" Font-Size="Large" Text="1. Bagaimana jika lupa password atau account terblokir karena salah password 3x?"></asp:Label>
                         </h3>
                     </div>
                    <div class="panel-body">
                    <asp:Label ID="lanswer" runat="server" Text=" = Klik tombol lupa password pada halaman login, 
                        isi alamat email yang biasa digunakan untuk login kemudian buka email tersebut. 
                        Buka email dari noreply.jaws yang berisi kode untuk reset password.  
                        Masukkan kode tersebut beserta password baru anda. 
                        Kemudian login kembali untuk masuk ke halaman menu. "></asp:Label> 
                    </div>
                </div>
           </p>
        </div>

        <div class="col-xs-12 col-sm-7 col-md-8">
           <p id="content2" runat="server">
               <div class="panel panel-info">
                    <div class="panel-heading">
                         <h3 class="panel-title">
                             <asp:Label ID="Label1" runat="server" Font-Bold="true" Font-Size="Large"
                              Text="2. Bagaimana jika terjadi kesalahan pada saat pembuatan invoice, trade in dan resell ?">
                             </asp:Label>
                         </h3>
                     </div>
                    <div class="panel-body">
                    <asp:Label ID="Label2" runat="server" Text=" = Jika terjadi kesalahan pembuatan invoice, trade in dan resell void transaksi tersebut di menu report, void transaksi ini hanya bisa dilakukan pada hari yang sama."></asp:Label> 
                    </div>
                </div>
           </p>
        </div>

        <div class="col-xs-12 col-sm-7 col-md-8">
           <p id="P1" runat="server">
               <div class="panel panel-info">
                    <div class="panel-heading">
                         <h3 class="panel-title">
                             <asp:Label ID="Label3" runat="server" Font-Bold="true" Font-Size="Large"
                              Text="3. Bagaimana jika kesalahan penginputan transaksi sudah lewat hari ?">
                             </asp:Label>
                         </h3>
                     </div>
                    <div class="panel-body">
                    <asp:Label ID="Label4" runat="server" Text=" = Semua kesalahan transaksi yang sudah lewat hari harus direquest kepada pihak accounting untuk perbaikannya.  "></asp:Label> 
                    </div>
                </div>
           </p>
        </div>

        <div class="col-xs-12 col-sm-7 col-md-8">
           <p id="P2" runat="server">
               <div class="panel panel-info">
                    <div class="panel-heading">
                         <h3 class="panel-title">
                             <asp:Label ID="Label5" runat="server" Font-Bold="true" Font-Size="Large"
                              Text="4. Bagaimana jika ada barang yang belum terdaftar ke dalam Promo atau souvenir?">
                             </asp:Label>
                         </h3>
                     </div>
                    <div class="panel-body">
                    <asp:Label ID="Label6" runat="server" Text=" = Untuk barang-barang yang belum terdaftar ke dalam promo maupun souvenir bisa langsung menghubungi bagian SK untuk konfirmasi maupun minta didaftarkan ke dalam promo / souvenir. "></asp:Label> 
                    </div>
                </div>
           </p>
        </div>

        <div class="col-xs-12 col-sm-7 col-md-8">
           <p id="P3" runat="server">
               <div class="panel panel-info">
                    <div class="panel-heading">
                         <h3 class="panel-title">
                             <asp:Label ID="Label7" runat="server" Font-Bold="true" Font-Size="Large"
                              Text="5. Bagaimana jika ada barang yang tidak bisa dibuat ke dalam transaksi POS ? ">
                             </asp:Label>
                         </h3>
                     </div>
                    <div class="panel-body">
                    <asp:Label ID="Label8" runat="server" Text=" = Cek stok pada outlet inventory, lihat posisi barang ada di lokasi mana, status stock harus ”Jual”, pastikan pcs nya 1. Cek report sales oder apakah ada inputan yang belum selesai (draft), jika ada delete transaksi draft tersebut. jika masih tidak bisa diinput silahkan langsung menghubungi IT support."></asp:Label> 
                    </div>
                </div>
           </p>
        </div>

        <div class="col-xs-12 col-sm-7 col-md-8">
           <p id="P4" runat="server">
               <div class="panel panel-info">
                    <div class="panel-heading">
                         <h3 class="panel-title">
                             <asp:Label ID="Label9" runat="server" Font-Bold="true" Font-Size="Large"
                              Text="6. Bagaimana jika ada voucher yang dibawa customer ketika mau diinput ke dalam POS tidak terdaftar ? ">
                             </asp:Label>
                         </h3>
                     </div>
                    <div class="panel-body">
                    <asp:Label ID="Label10" runat="server" Text=" = Untuk voucher yang tidak bisa diinput bisa langsung konfirmasi ke pihak marketing masing-masing. "></asp:Label> 
                    </div>
                </div>
           </p>
        </div>

        <div class="col-xs-12 col-sm-7 col-md-8">
           <p id="P5" runat="server">
               <div class="panel panel-info">
                    <div class="panel-heading">
                         <h3 class="panel-title">
                             <asp:Label ID="Label11" runat="server" Font-Bold="true" Font-Size="Large"
                              Text="7. Bagaimana jika ASP barang tidak sesuai ketika cetak invoice ? ">
                             </asp:Label>
                         </h3>
                     </div>
                    <div class="panel-body">
                    <asp:Label ID="Label12" runat="server" Text=" = Untuk ASP yang salah pada cetakan, dimohon untuk buka informasi barang di menu product. Cek apakah product category barang dengan ASP yang berlaku sudah sesuai. Jika berbeda bisa mengkonfirmasi ke pihak SK. Jika sudah sama dan cetakan masih keluar berbeda dari ketentuan bisa langsung menghubungi pihak IT support.  "></asp:Label> 
                    </div>
                </div>
           </p>
        </div>

        <div class="col-xs-12 col-sm-7 col-md-8">
           <p id="P6" runat="server">
               <div class="panel panel-info">
                    <div class="panel-heading">
                         <h3 class="panel-title">
                             <asp:Label ID="Label13" runat="server" Font-Bold="true" Font-Size="Large"
                              Text="8. Bagaimana jika ada kelebihan nilai DP (DP lebih besar daripada barang yang dibeli) ? ">
                             </asp:Label>
                         </h3>
                     </div>
                    <div class="panel-body">
                    <asp:Label ID="Label14" runat="server" Text=" = Saat halaman pembayaran bisa dikeluarkan melalui cash out atau transfer out kelebihan nominal tersebut untuk kemudian dikembalikan kepada customer atau dibuatkan DP baru.  "></asp:Label> 
                    </div>
                </div>
           </p>
        </div>

        <div class="col-xs-12 col-sm-7 col-md-8">
           <p id="P7" runat="server">
               <div class="panel panel-info">
                    <div class="panel-heading">
                         <h3 class="panel-title">
                             <asp:Label ID="Label15" runat="server" Font-Bold="true" Font-Size="Large"
                              Text="9. Bagaimana jika DP tidak bisa dipilih di halaman pembayaran ? ">
                             </asp:Label>
                         </h3>
                     </div>
                    <div class="panel-body">
                    <asp:Label ID="Label16" runat="server" Text=" = Kemungkinan ada perbedaan kode customer yang digunakan ketika DP dan ketika pelunasan. Jika kode customer yang digunakan untuk DP tidak ada (terhapus sewaktu penggabungan kode customer) bisa langsung contact IT Support. "></asp:Label> 
                    </div>
                </div>
           </p>
        </div>

        <div class="col-xs-12 col-sm-7 col-md-8">
           <p id="P8" runat="server">
               <div class="panel panel-info">
                    <div class="panel-heading">
                         <h3 class="panel-title">
                             <asp:Label ID="Label17" runat="server" Font-Bold="true" Font-Size="Large"
                              Text="10. Bagaimana Jika ketika trade in atau resell kode barang tidak bisa di display ? ">
                             </asp:Label>
                         </h3>
                     </div>
                    <div class="panel-body">
                    <asp:Label ID="Label18" runat="server" Text=" = Cek kembali kode customer yang digunakan waktu membeli barang, apakah sudah sama dengan yang akan digunakan saat trade in atau resell. Cek report di outlet inventory – stock ledger apakah barang tsb dibeli melalui speedware atau JAWS. Jika dibeli melalui speedware silahkan untuk langsung di add PLU sesuai jenis barang nya. Jika kode customer sudah tidak ada (terhapus karena merge customer oleh pihak CRM) bisa langsung menghubungi IT support. "></asp:Label> 
                    </div>
                </div>
           </p>
        </div>

    </form>
    <div class="col-xs-12 col-sm-5 col-md-4">

    </div>
</div>
</div>

</asp:Content>

