<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Repair_DataStoneAdd.aspx.cs" inherits="Page_Register_Repair_StoneAdd" %>

<%@ register src="MenuRepair.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Repair 
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<hr />
	<form runat="server" class="form-inline">
		<div class="form-group cb-multi">
            <asp:RadioButtonList ID="stoneindex" runat="server" RepeatColumns="2" AutoPostBack="true" OnSelectedIndexChanged="StoneChange" />
        </div>
        <hr />
        <div id="stone1a" runat="server" visible="false">
            <div class="text-muted">
                Parcel :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel101a" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Shape :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel102a" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Size :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel103a" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Colour :</asp:ListItem>
                    <asp:ListItem Value="">F</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel104a" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Clarity :</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div id="stone1b" runat="server" visible="false">
            <div class="text-muted">
                Parcel :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel101b" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Shape :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel102b" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Size :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel103b" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Colour :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel104b" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Clarity :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="clearfix"></div>
            <div class="form-separator hidden-xs"></div>
            <div class="form-group">
                <asp:DropDownList ID="parcel105b" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Cutting :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel106b" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Brand :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel107b" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Category :</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div id="stone2" runat="server" visible="false">
            <div class="text-muted">
                Parcel :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel201" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Shape :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel202" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Size :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel203" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Colour :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel204" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Clarity :</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div id="stone3" runat="server" visible="false">
            <div class="text-muted">
                Parcel :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel301" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Shape :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel302" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Size :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel303" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Colour :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel304" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Clarity :</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div id="stone4" runat="server" visible="false">
            <div class="text-muted">
                Parcel :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel401" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Shape :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel402" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Size :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel403" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Colour :</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div id="stone5" runat="server" visible="false">
            <div class="text-muted">
                Parcel :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel501" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Shape :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel502" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Size :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel503" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Colour :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel504" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Clarity :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="clearfix"></div>
            <div class="form-separator hidden-xs"></div>
            <div class="form-group">
                <asp:DropDownList ID="parcel505" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Cutting :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel506" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Brand :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:DropDownList ID="parcel507" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem>Category :</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
				<a id="cancel" runat="server" class="btn btn-default" href="Repair_DataStone.aspx">
					<i class="icon-remove"></i>
					Cancel
				</a>
			</div>
		</div>
		<div class="table-responsive">
			<asp:table id="tb" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
					<asp:tableheadercell width="200">Nama</asp:tableheadercell>
					<asp:tableheadercell width="200">Kode Parcel</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
</asp:content>

