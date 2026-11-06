<%@ Page Language="C#" MasterPageFile="~/MasterLogin.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="formlogin" runat="server">
        <div class="modal fade" tabindex="-1" role="dialog" id="modal-forget-password">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title text-center">Forgot Password</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label for="email">Email address</label>
                                <asp:TextBox ID="txtemailsent" runat="server" CssClass="form-control" aria-describedby="emailHelp" placeholder="Email" AutoCompleteType="Email" />
                            </div>
                            <div class="form-group">
                                <label for="pass">NIK</label>
                                <asp:TextBox ID="txtnikuser" runat="server" CssClass="form-control" placeholder="NIK" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button runat="server" type="button" class="btn btn-primary" onserverclick="forgotPassword">Sent</button>
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="card">
            <div class="card-header text-center">
                <img src="Images/web/logo_web_franknco.png" class="rounded image-login">
            </div>
            <div class="card-body">
                <div class="form-group">
                    <label for="email">Email address</label>
                    <asp:TextBox ID="email" runat="server" CssClass="form-control" aria-describedby="emailHelp" placeholder="Email" AutoCompleteType="Email" required />
                    <asp:Label ID="lblemailwarning" runat="server" Text="Please input correctly." CssClass="form-text text-muted"></asp:Label>
                </div>
                <div class="form-group">
                    <label for="pass">Password</label>
                    <asp:TextBox ID="pass" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password" required />
                    <asp:Label ID="lblpasswordwarning" runat="server" CssClass="form-text text-danger" Visible="false"></asp:Label>
                </div>
                <!--<div class="form-check">
                    <input type="checkbox" class="form-check-input" id="checkRememberMe" />
                    <label class="form-check-label" for="checkRememberMe">Remember me</label>
                </div>-->
                <div class="form-text">
                    <label class="btn btn-link" onclick="onclickmodalpwd();">Forgot Password?</label>
                    <script type="text/javascript">
                        function onclickmodalpwd() {
                            $('#modal-forget-password').modal('show');
                        }
                    </script>
                </div>
            </div>
            <div class="card-footer">
                <button runat="server" class="btn btn-primary float-left" onserverclick="signIn">Sign In</button>
            </div>
        </div>
    </form>
</asp:Content>
