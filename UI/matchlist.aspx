<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="matchlist.aspx.cs" Inherits="UI.matchlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="max-width: 70%; margin: auto">
        <table>
            <%--<tr>
                <td colspan="2">
                    <table>
                        <tr>
                            <td>Banner here...
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>Sign in:
                                        </td>
                                        <td>
                                            <input type="text" placeholder="enter user name" />
                                        </td>
                                        <td>
                                            <input type="text" placeholder="enter password" />
                                        </td>
                                        <td>
                                            <input type="button" value="GO" />
                                        </td>
                                        <td>New user?
                                        </td>
                                        <td>
                                            <a href="signup.html">Click here</a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>--%>
            <tr>
                <td>Total Points: 
                </td>
                <td id="totalPoints"></td>
            </tr>
            <tr>
                <td>Available points for this tournament: 
                </td>
                <td id="tournamentPoints"></td>
            </tr>
            <tr>
                <td colspan="2">
                    <table style="border: solid; list-style-type: none; padding: 2px" data-bind="foreach: matches">

                        <tr>
                            <td data-bind="text: MatchDate"></td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr style="text-align: left">
                                                    <td data-bind="text: TeamOne"></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <input type="text" name="betTNC" data-bind="value: TeamOneBet" placeholder="place bet points here..." />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table>
                                                <tr style="text-align: center">
                                                    <td>vs
                                                    </td>
                                                </tr>
                                                <tr style="text-align: center">
                                                    <td>:
                                                    </td>

                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table>
                                                <tr style="text-align: right">
                                                    <td data-bind="text: TeamTwo"></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <input type="text" name="betTNC" placeholder="place bet points here..." data-bind="value: TeamTwoBet" />
                                                    </td>
                                                    <td>
                                                        <input type="button" value="Lock in bet" data-bind="click: ($data.TeamOneBet > 0 && $data.TeamTwoBet > 0) ? alert('Please choose 1 side to bet only') : $parent.SaveMatchBet.bind($data)" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>

            </tr>
        </table>
    </div>

    <script type="text/javascript">

        var currentEmail = localStorage['currentEmail'] || 'N/A';
        var currentUserID = localStorage['currentUserID'] || 'N/A';
        console.log(currentEmail + '-' + currentUserID);

        var tournamentID = 1;

        var userPointsDetails = [];

        ////load tournaments list
        var _self = this;
        _self.UserPointsDetails = ko.observableArray([]);

        $.get('http://localhost:51322/api/betpoints/user/' + currentUserID,
            function (data) {
                $.each(data, function (index, value) {
                    userPointsDetails.push(value);
                });

                var totalPoints = userPointsDetails[0].TotalPoints ? userPointsDetails[0].TotalPoints : 0
                var tournamentPoints = userPointsDetails[0].AvailableMatchPoints ? userPointsDetails[0].AvailableMatchPoints : 0

                $('#totalPoints').html(totalPoints);
                $('#tournamentPoints').html(tournamentPoints);

            }).fail(function () {
                console.log('error');
            });
        //end load

        ///
        var availableMatches = [];
        $.post('http://localhost:51322/api/match/tournament/' + tournamentID, { UserID: currentUserID, UserName: "Bettor 1" },
            function (data) {
                $.each(data, function (index, value) {
                    availableMatches.push(value);
                });
                function BuildTable() {

                    var self = this;
                    self.matches = ko.observableArray(availableMatches);
                    self.SaveMatchBet = function (body) {

                        if (body.TeamOneBet > 0) {
                            body.PlaceBet = body.TeamOneBet;
                            body.TeamID = body.TeamOneID
                        }
                        else {
                            body.PlaceBet = body.TeamTwoBet;
                            body.TeamID = body.TeamTwoID
                        }

                        $.ajax({
                            url: 'http://localhost:51322/api/add/bet',
                            type: 'POST',
                            data: body,
                            success: function (data) {
                                window.location.href = "http://localhost:64057/matchlist.aspx";
                            }
                        });
                    }
                }
                ko.applyBindings(new BuildTable());

            }).fail(function () {
                //console.log("error");
            });
        ///

    </script>
</asp:Content>
