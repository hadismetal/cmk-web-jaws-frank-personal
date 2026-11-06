<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Page_Register_WhitelistTradeIn" %>


<asp:Content ContentPlaceHolderID="h1" runat="server">
    Whitelist TradeIn
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <div class="row">
            <div class="col-lg-4">
                <asp:Label ID="label_message_image" runat="server" Width="100%"></asp:Label>
                <asp:Label ID="label_message" runat="server" Width="100%"></asp:Label>
            </div>
        </div>
        <form runat="server" class="form-inline">
            <div class="row" style="position: relative;">
                <div class="col-lg-4">
                    <div class="col-lg-6" style="z-index: 999; position: relative;">
                        <div class="form-group" style="width: 100%;">
                            <div class="text-muted">
                                Tipe Lokasi :
                            </div>
                            <asp:DropDownList ID="ddl_tipelokasi" runat="server" CssClass="form-control" style="position: relative;" AutoPostBack="true" OnSelectedIndexChanged="OnChange_TipeLokasi">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-6" style="z-index: 999">
                        <div class="form-group" style="width: 100%;">
                            <div class="text-muted">
                                Lokasi :
                            </div>
                            <asp:DropDownList ID="ddl_lokasi" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-12" style="z-index: 100">
                        <div class="form-group" style="width: 100%; margin-top: 20px;">
                            <div class="text-muted">
                                Tipe Produk :
                            </div>
                            <asp:DropDownList ID="ddl_tipeproduk" runat="server" CssClass="form-control">
                                <asp:ListItem Value="dj">Diamond Jewellery</asp:ListItem>
                                <asp:ListItem Value="pg">Plain Gold</asp:ListItem>
                                <asp:ListItem Value="gj">Gold Jewellery</asp:ListItem>
                                <asp:ListItem Value="ld">Loose Diamond</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-12">
                        <div class="form-group" style="width: 100%; margin-top: 20px;">
                            <div class="text-muted">
                                PLU :
                            </div>
                            <asp:TextBox ID="txt_plu" runat="server" CssClass="form-control" placeholder="..." />
                        </div>
                    </div>
                    <div class="col-lg-12" style="z-index: 999">
                        <div class="form-group" style="width: 100%; margin-top: 20px;">
                            <div class="text-muted">
                                Jenis Alasan :
                            </div>
                            <asp:DropDownList ID="ddl_reason" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-12" >
                        <div class="form-group" style="width: 100%; margin-top: 20px;">
                            <div class="text-muted">
                                Keterangan Alasan Pengajuan Whitelist :
                            </div>
                            <asp:TextBox TextMode="MultiLine" ID="txt_keterangan" Rows="5" runat="server" CssClass="form-control" placeholder="..." />
                        </div>
                    </div>
                     <div class="col-lg-12" >
                        <div class="form-group" style="width: 100%; margin-top: 20px;">
                            <div class="text-muted">
                                Attachment :
                            </div>
                            <asp:fileupload id="imagePicture" runat="server" AllowMultiple="true" />
                            <small style="font-weight: bold; color: #696969">
                                *Berkas yang diterima terdiri atas format .pdf, .png, .jpeg, dan .jpg, serta dapat dikirim lebih dari satu file.
                            </small>
                        </div>
                    </div>
                    <div class="col-lg-12">
                        <div class="form-group">
                            <div class="hidden-xs">&nbsp;</div>
                            <div>
                                <button runat="server" class="btn btn-primary" onserverclick="refreshPage">Submit</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </div>
</asp:Content>


