<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="UI.login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="max-width: 30%; margin: auto">
        <table data-bind="with: viewModel">
            <tr>
                <td>
                    <input type="text" placeholder="Registered email" data-bind="value: EmailAddress"/>
                </td>
            </tr>
            <tr>
                <td>
                    <input type="password" placeholder="Password" data-bind="value: Password"/>
                </td>
            </tr>
           
            <tr>
                <td colspan="2">
                    <input type="button" value="Submit" data-bind="click: $parent.LoginUser"/>
                </td>
            </tr>
        </table>
    </div>
     
    <script type="text/javascript">
        function LoginUserInput() {

            var self = this;

            self.viewModel = {
                EmailAddress: ko.observable("")
                , Password: ko.observable("")
            };
            self.LoginUser = function (body) {
                $.ajax({
                    url: apiDomain + 'api/user/login',
                    type: 'post',
                    data: self.viewModel,
                    success: function (data) {
                        
                        localStorage["currentEmail"] = data.EmailAddress;
                        localStorage["currentUserID"] = data.UserID;

                        window.location.href = "matchlist.aspx";
                    }
                });
            }
        }
        ko.applyBindings(new LoginUserInput());
    </script>
</asp:Content>
