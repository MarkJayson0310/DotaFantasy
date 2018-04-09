Betting.Main = {};

Betting.Main.dataFields = function () {

    var _self = this;

    _self.Matches = {
        "TeamOneID": ko.observable(null),
        "TeamOne": ko.observable(null),
        "TeamTwoID": ko.observable(null),
        "TeamTwo": ko.observable(null),
        "MatchSchedule": ko.observable(null),
        "TeamOneBet": ko.observable(null),
        "TeamTwoBet": ko.observable(null),
    }

    _self.dataFields = {
        "TournamentID": ko.observable(null),
        "TournamentName": ko.observable(""),
        "TournamentDate": ko.observable(""),
        "TournamentPoints": ko.observable(null),
        "AvailablePoints": ko.observable(null),
        "Matches": ko.observableArray(_self.Matches)
    }
}

//Betting.Main.UserBet = function (tournamentid) {

//    var _self = this;

//    //_self.
//}