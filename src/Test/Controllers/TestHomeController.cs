using NUnit.Framework;
using Shouldly;
using Web.Controllers;
using Web.Repositories;

namespace Test.Controllers
{
    [TestFixture]
    public class TestHomeController : ControllerFixture<HomeController>
    {
        ISessionRepository sessionRepository;

        protected override HomeController Create()
        {
            sessionRepository = Mock<ISessionRepository>();
            return new HomeController(sessionRepository);
        }

        [Test]
        public void RendersHomeView()
        {
            var viewResult = controller.Index(null);

            viewResult.ViewName.ShouldBe("");
        }
    }
}
