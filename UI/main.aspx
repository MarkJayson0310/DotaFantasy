<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="UI.Betting" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
        <table data-bind="foreach: TournamentMatch">
            <tr>
                <td data-bind="text: TournamentName"></td>
                <td data-bind="text: TournamentDate"></td>
            </tr>
            <tr>
                <td>
                    <table data-bind="foreach: Matches">
                        <tr>
                            <td data-bind="text: TeamOne"></td>
                            <td>VS:
                            </td>
                            <td data-bind="text: TeamTwo"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <input type="button" value="go to matches" data-bind="click: $parent.GoToMatchList.bind($data.TournamentID)" />
                </td>
            </tr>
        </table>
    
    <script type="text/javascript">


        var tournamentMatches = [];
        $.get(
            apiDomain + 'api/landing',
            function (data) {
                $.each(data, function (index, value) {
                    tournamentMatches.push(value);
                });

                function BuildMatchList() {

                    var self = this;
                    self.TournamentMatch = ko.observableArray(tournamentMatches);
                    self.GoToMatchList = function (id) {
                        sessionStorage['_currentTournamentID'] = id.TournamentID;
                        window.location.href = "matchlist.aspx";
                    }
                }
                ko.applyBindings(new BuildMatchList());

            }).fail(function () {
                console.log('error');
            });


    </script>
</asp:Content>
