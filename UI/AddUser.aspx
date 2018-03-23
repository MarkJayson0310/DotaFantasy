<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AddUser.aspx.cs" Inherits="UI.AddUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
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

    <script type="text/javascript" src="Scripts/js/knockout-3.4.2.js"></script>
    <script type="text/javascript" src="Scripts/js/knockout-mapping.js"></script>
    <script type="text/javascript" src="Scripts/js/jquery-3.3.1.min.js"></script>
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
                        url: 'http://localhost:51322/api/user/add',
                        type: 'post',
                        data: self.viewModel
                    });
                    console.log(self.viewModel);
                }
        }
        ko.applyBindings(new BuildUserInput());
    </script>

</asp:Content>
