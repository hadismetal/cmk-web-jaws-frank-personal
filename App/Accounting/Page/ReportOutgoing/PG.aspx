<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="PG.aspx.cs" inherits="PG" %>

<asp:content contentplaceholderid="h1" runat="server">
	Plain Gold
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Report Outgoing
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	
    <form runat="server" class="form-inline">
        
        <table class="table-form">
			<tr>
				<td>Periode</td>
				<td>:</td>
				<td>
					<asp:textbox id="tgl" runat="server" cssclass="form-control text-center" width="110" />
					<script>
					    $(document).ready(function () { $('#content_tgl').datepicker({ format: 'dd-mm-yyyy' }); });
					</script>
				</td>
				<td>
					<asp:requiredfieldvalidator id="tgl_RF" runat="server" />
					<span id="tgl_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
                <td>s/d&nbsp;&nbsp;</td>
				<td>
					<asp:textbox id="tgl2" runat="server" cssclass="form-control text-center" width="110" />
					<script>
					    $(document).ready(function () { $('#content_tgl2').datepicker({ format: 'dd-mm-yyyy' }); });
					</script>
				</td>
				<td>
					<asp:requiredfieldvalidator id="tgl2_RF" runat="server" />
					<span id="tgl2_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
                <td>
                    <asp:TextBox ID="kw" runat="server" CssClass="form-control text-center" Width="300" placeholder="No. Dokumen" />
                </td>
			</tr>
		</table>
		<div class="form-group">
            <div class="text-muted">
                Company Brand :
            </div>
            <asp:DropDownList ID="brand" runat="server" CssClass="form-control" Width="200">
                <asp:ListItem Value="-1">All</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
               Location Type (From)
            </div>
            <asp:DropDownList ID="tipelokasifrom" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasi" AutoPostBack="true">
                
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Location (From)
            </div>
            <asp:DropDownList ID="lokasifrom" runat="server" CssClass="form-control" Width="175">
                
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
               Location Type (To)
            </div>
            <asp:DropDownList ID="tipelokasito" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasiTo" AutoPostBack="true">
                
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Location (To)
            </div>
            <asp:DropDownList ID="lokasito" runat="server" CssClass="form-control" Width="175">
                
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Status Outgoing :
            </div>
            <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="200">
                <asp:ListItem Value="-1">All</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="Button1" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                    <span class="glyphicon glyphicon-search" aria-hidden="true"></span>
                    Display
                </button>
                <button id="Button2" runat="server" class="btn btn-primary" onserverclick="Export">
                    <span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span>
                    Export
                </button>
                <button id="Button3" runat="server" class="btn btn-primary" onserverclick="Print">
                    <span class="glyphicon glyphicon-print" aria-hidden="true"></span>
                    Print
                </button>
            </div>
        </div>
		<div class="table-responsive">
			<asp:table id="tb" runat="server" cssclass="table-list">
				
			</asp:table>
		</div>
	</form>
</asp:content>

