<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="UI.AddUser" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="max-width: 50%; margin: auto">
        <table data-bind="with: viewModel">
            <tr>
                <td>First Name: 
                </td>
            </tr>
            <tr>
                <td>
                    <input type="text" name="firstName" placeholder="Enter name here" data-bind="value: firstName" />
                </td>
            </tr>
            <tr>
                <td>Middle Name:
                </td>
            </tr>
            <tr>
                <td>
                    <input type="text" name="middleName" placeholder="Enter name here" data-bind="value: middleName" />
                </td>
            </tr>
            <tr>
                <td>Last Name:
                </td>
            </tr>
            <tr>
                <td>
                    <input type="text" name="lastName" placeholder="Enter name here" data-bind="value: lastName" />
                </td>
            </tr>
            <tr>
                <td>Email address:
                </td>
            </tr>
            <tr>
                <td>
                    <input type="text" name="emailAddress" placeholder="Enter name here" data-bind="value: emailAddress" />
                </td>
            </tr>
            <tr>
                <td>Password:
                </td>
            </tr>
            <tr>
                <td>
                    <input type="password" name="userPassword" placeholder="Enter password" data-bind="value: password" />
                </td>
            </tr>
            <tr>
                <td>
                    <input type="button" value="Add user" data-bind="click: $parent.AddUser">
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">
        
        function BuildUserInput() {

            var self = this;

            self.viewModel = {
                firstName: ko.observable("")
                , middleName: ko.observable("")
                , lastName: ko.observable("")
                , emailAddress: ko.observable("")
                , password: ko.observable("")
            };
            self.AddUser = function (body) {
                    $.ajax({
                        url: apiDomain + 'api/user/add',
                        type: 'POST',
                        data: self.viewModel,
                        success: function (data) {

                            if (data.IsUserExist) {
                                alert('Email ID already registered!');
                                return;
                            }

                            localStorage["currentEmail"] = data.EmailAddress;
                            window.location.href = "activateuser.aspx";
                        }
                    });
                }
        }
        ko.applyBindings(new BuildUserInput());
    </script>

</asp:Content>
