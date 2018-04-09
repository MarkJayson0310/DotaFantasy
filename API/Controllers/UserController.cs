using System;
using System.Web.Http;
using DataRepository;
using DataModel;

namespace API.Controllers
{
    public class UserController : ApiController
    {
        [Route("api/user/add")]
        [HttpPost]
        public dynamic AddNewUser(UserModel newuser)
        {
            UserRepository _repo = new UserRepository();
            return _repo.AddNewUser(newuser);
        }

        [Route("api/user/login")]
        [HttpPost]
        public dynamic LoginUser(UserModel newuser)
        {
            UserRepository _repo = new UserRepository();
            return _repo.LogInUser(newuser);
        }

        [Route("api/user/activate")]
        [HttpPost]
        public dynamic ValidateUserActivation(ActivateUserModel activate)
        {
            UserRepository _repo = new UserRepository();
            return _repo.ValidateUserActivation(activate);
        }
    }
}