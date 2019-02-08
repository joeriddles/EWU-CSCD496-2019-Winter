using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.ViewModels;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public void PostUser_NoFirstName_ReturnsBadRequest()
        {
            var controller = new UserController(new TestableUserService(), Mapper.Instance);
            ActionResult<UserViewModel> result = controller.Post(new UserInputViewModel {LastName = "Smith"}) as ActionResult;
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }
    }
}
