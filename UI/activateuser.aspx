<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="activateuser.aspx.cs" Inherits="UI.ActivateUser" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="max-width: 50%; margin: auto">
        <table>
            <tr>
                <td>Thank you for joining eskrima!. We just need another step to validate you. 
                </td>
            </tr>
            <tr>
                <td>Please use the CODE sent to your email: <span id="emailID"></span>
                </td>
            </tr>

            <tr>
                <td>
                    <input type="text" placeholder="activation code here" id="txCode" />
                </td>
            </tr>
            <tr>
                <td>
                    <input type="button" value="Validate" id="btnActivate" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(function () {

            var userEmail = localStorage['currentEmail'];

            $('#emailID').html(userEmail);

            $('#btnActivate').click(function () {

                var activationCode = $("#txCode").val();

                $.ajax({
                    url: apiDomain + 'api/user/activate',
                    type: 'post',
                    data: { UserEmail: userEmail, ActivationCode: activationCode },
                    success: function (data) {
                        
                        if (data == 1) {
                            window.location.href = "login.aspx";
                            sessionStorage._currentEmail = userEmail;
                        }
                        else {
                            alert('User code invalid pls try again!');
                        }
                    }
                });
            });
        });
    </script>
</asp:Content>
