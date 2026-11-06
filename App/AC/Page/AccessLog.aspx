<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="AccessLog.aspx.cs" inherits="Page_AccessLog" %>

<asp:content contentplaceholderid="h1" runat="server">
	Access Log
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server">
		<asp:calendar id="tgl" runat="server"
			titlestyle-forecolor="White"
			titlestyle-backcolor="Gray"
			nextprevstyle-forecolor="WhiteSmoke"
			nextprevformat="FullMonth"
			daynameformat="Full"
			todaydaystyle-backcolor="LightBlue"
			weekenddaystyle-forecolor="Red"
			othermonthdaystyle-forecolor="Silver"
			onselectionchanged="fillTable" />
	</form>
	<br />
	<asp:table id="tb" runat="server" cssclass="table-list">
		<asp:tableheaderrow>
			<asp:tableheadercell width="90" />
			<asp:tableheadercell width="150">Aktivitas</asp:tableheadercell>
			<asp:tableheadercell width="150" />
		</asp:tableheaderrow>
	</asp:table>
</asp:content>

