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

                if (data.IsUserExist) {

                    sessionStorage._currentEmail = data.EmailAddress;
                    sessionStorage._currentID = data.UserID;
                    sessionStorage._currentName = data.FirstName;

                    if (data.IsVerified == 1) {
                        window.location.href = "main.aspx";
                    }
                    else {
                        window.location.href = "activateuser.aspx";
                    }
                }
                else {
                    alert('Email account not registered!');
                }
            }
        });
    }
}
ko.applyBindings(new LoginUserInput());