using System.Net;
using Xunit;

namespace IisHelper.MvcApplication.Test
{
    public class Tests : IUseFixture<IisExpressFixture>
    {
        private const string BaseAdress = "http://localhost:53127/";

        #region Tests
// ReSharper disable PossibleNullReferenceException

        [Fact]
        public void CanAccessRoot()
        {
            WebRequest request = WebRequest.Create(BaseAdress);
            var response = request.GetResponse() as HttpWebResponse;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void CanAccessWebApi()
        {
            WebRequest request = WebRequest.Create(BaseAdress + "api/values");
            var response = request.GetResponse() as HttpWebResponse;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

// ReSharper restore PossibleNullReferenceException
        #endregion // Tests

        #region IUseFixture<IisExpressFixture>

        public void SetFixture(IisExpressFixture data)
        {
            IisExpress iisExpress = data.IisExpress;

            if (!iisExpress.IsRunning)
            {
                data.IisExpress.ConfigPath = "IisHelper.MvcApplication.Test.config";

                data.IisExpress.Start("IisHelper.MvcApplication");
            }
        }

        #endregion // IUseFixture<IisExpressFixture>
    }
}
