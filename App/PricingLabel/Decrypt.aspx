<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Decrypt.aspx.cs" Inherits="_Default" %>

<form id="Form1" runat="server" class="form-inline">
    <div id="err" runat="server" visible="false">
    </div>
    <div class="text-muted">
        No. PLU :
    </div>
    <div class="form-group">
        <asp:textbox id="from" cssclass="form-control" runat="server" placeholder="Start" />
    </div>
    <div class="form-group">
        <div class="hidden-xs">&nbsp;</div>
        <div>
            <button id="go" runat="server" class="btn btn-primary" onserverclick="Decrypt">
                <i class="icon-search"></i>
                Decrypt
            </button>
        </div>
    </div>
</form>
