Betting.APICallService = Betting.APICallService || {};

Betting.APICallService = {

        executeAPIget: function (urlEndPoint) {
            return $.ajax({
                type: "GET",
                url: urlEndPoint
            })
        },
        testCall: function (msg) {
            alert(msg);
        }

};