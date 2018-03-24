<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="matchlist.aspx.cs" Inherits="UI.matchlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="max-width: 70%; margin: auto">
        <table>
            <tr>
                <td>Total Points: <span id="totalPoints" style="color: blue" data-bind="text: totalPoints" />
                </td>
            </tr>
            <tr>
                <td>Available points for this tournament: <span id="tournamentPoints" style="color: green" data-bind="text: tournamentPoints" />
                </td>
            </tr>
            <tr>
                <td>
                    <input type="button" value="Register bet for this tournament" data-bind="style: { visibility: tournamentPoints === 0 ? 'hidden' : 'visible' }, click: RegisterTournamentBet" />
                </td>
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
                                                        <input type="button" value="Lock in bet" data-bind="click: $parent.SaveMatchBet.bind($data)" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
        </table>
    </div>

    <script type="text/javascript">

        var currentEmail = localStorage['currentEmail'] || 'N/A';
        var currentUserID = localStorage['currentUserID'] || 'N/A';
        //console.log(currentEmail + '-' + currentUserID);

        var currentTournamentID = 1;

        var userPointsDetails = [];
        var totalpoints = 0;
        var tournamentpoints = 0;

        ////load tournaments list
        var _self = this;
        _self.UserPointsDetails = ko.observableArray([]);

        $.get('http://localhost:51322/api/betpoints/user/' + currentUserID,
            function (data) {
                $.each(data, function (index, value) {
                    userPointsDetails.push(value);
                });

                if (userPointsDetails.length > 0) {
                    totalpoints = userPointsDetails[0].TotalPoints;
                    tournamentpoints = userPointsDetails[0].TournamentPoints;
                }

                //$('#totalPoints').html(totalpoints);
                //$('#tournamentPoints').html(tournamentpoints);

            }).fail(function () {
                console.log('error');
            });
        //end load

        ///
        var availableMatches = [];
        $.post('http://localhost:51322/api/match/tournament/' + currentTournamentID, { UserID: currentUserID, UserName: "Bettor 1" },
            function (data) {
                $.each(data, function (index, value) {
                    availableMatches.push(value);
                });
                function BuildTable() {

                    var self = this;
                    self.matches = ko.observableArray(availableMatches);
                    self.totalPoints = ko.observable(totalpoints);
                    self.tournamentPoints = ko.observable(tournamentpoints);

                    self.SaveMatchBet = function (body) {

                        var teamOneBet = parseInt(body.TeamOneBet) || 0;
                        var teamTwoBet = parseInt(body.TeamTwoBet) || 0;

                        if (teamOneBet == 0 && teamTwoBet == 0) {
                            alert('Place your bet first!');
                            return;
                        }

                        if (teamOneBet > 0 && teamTwoBet > 0) {
                            alert('Choose 1 side only');
                            return;
                        }

                        body.TournamentID = currentTournamentID;
                        if (teamOneBet > 0) {

                            if (tournamentpoints < teamOneBet) {
                                alert('bet points not sufficient');
                                return;
                            }
                            body.PlaceBet = teamOneBet;
                            body.TeamID = body.TeamOneID
                        }
                        else {

                            if (tournamentpoints < teamTwoBet) {
                                alert('bet points not sufficient');
                                return;
                            }
                            body.PlaceBet = teamTwoBet;
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
                    //console.log(tournamentpoints);
                    self.RegisterTournamentBet = function () {

                        if (totalpoints == 0) {
                            alert('No available points to register');
                            return;
                        }

                        $.post('http://localhost:51322/api/registerbet', { userID: currentUserID, tournamentID: currentTournamentID, TournamentPoints: 50 })
                            .done(function () {
                                alert('Your ready to bet for this tournament!');
                            })
                            .fail(function () {
                                alert('Something went wrong with your request');
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
