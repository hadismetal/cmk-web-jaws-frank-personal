<%@ page language="C#" Debug="true" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="ClosingStock.aspx.cs" inherits="Page_Approval_DJ" %>

<asp:content contentplaceholderid="h1" runat="server">
	Closing Stock
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div id="zero" runat="server" visible="false">
		<i class="icon-ok icon-5x pull-left text-success"></i>
		<h4>Proses Selesai</h4>
		<p class="text-muted">
			Semua data / transaksi dalam kondisi sudah approval.
		</p>
		<div class="clearfix"></div>
		<hr />
	</div>
	<div id="form_area" runat="server">
		<form runat="server" class="form-inline">
            
            
            
            <div>
                
                <div class="form-group">
                    <div class="text-muted">
                        Bulan
                    </div>
                    <asp:DropDownList ID="ddlbulan" runat="server" CssClass="form-control" Width="175">
                    
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <div class="text-muted">
                        Tahun
                    </div>
                    <asp:DropDownList ID="ddltahun" runat="server" CssClass="form-control" Width="175">
                    
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <div class="hidden-xs">&nbsp;</div>
                    <div>
                        <asp:LinkButton ID="Display" 
                                    runat="server" 
                                    CssClass="btn btn-primary"    
                                    OnClick="Display_Click">
                            <span aria-hidden="true" class="glyphicon glyphicon-search"></span> 
                            Display
                        </asp:LinkButton>
                    
                    </div>
                </div>
            </div>
            <div style="padding-top:15px;">
                
                <div class="form-group">
                    <div class="text-muted">
                        Keterangan
                    </div>
                    <asp:TextBox runat="server" ID="txtketerangan" Width="300" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <div class="hidden-xs">&nbsp;</div>
                    <div>
                        <asp:LinkButton ID="Closing" 
                                    runat="server" 
                                    CssClass="btn btn-info"    
                                    OnClick="Closing_Click">
                            <span aria-hidden="true" class="glyphicon glyphicon-hand-down"></span> 
                            Closing
                        </asp:LinkButton>

                    </div>
                </div>
            </div>
		</form>
	</div>
    <div>
        <br />
        <asp:Literal ID="err" runat="server" />
    </div>
    <div>
        <br />
        <asp:Table runat="server" ID="tbl" cssclass="table-list" />
    </div>
</asp:content>

