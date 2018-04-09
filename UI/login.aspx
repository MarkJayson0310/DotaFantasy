<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="UI.login" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="max-width: 30%; margin: auto">
        <table data-bind="with: viewModel">
            <tr>
                <td>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                        <input id="email" type="text" class="form-control" name="email" placeholder="Email" data-bind="value: EmailAddress">
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                        <input id="password" type="password" class="form-control" name="password" placeholder="Password" data-bind="value: Password">
                    </div>
                </td>
            </tr>

            <tr>
                <td colspan="2">
                    <input type="button" value="Submit" class="btn btn-primary" data-bind="click: $parent.LoginUser" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" src="Scripts/js/login.js"></script>
</asp:Content>
