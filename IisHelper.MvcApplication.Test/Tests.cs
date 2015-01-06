using System.Net;
using Xunit;

namespace IisHelper.MvcApplication.Test
{
    public class Tests : IUseFixture<IisExpressFixture>
    {
        #region Tests
// ReSharper disable PossibleNullReferenceException

        [Fact]
        public void CanAccessRoot()
        {
            var request = WebRequest.Create("http://localhost:53127");
            var response = request.GetResponse() as HttpWebResponse;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void CanAccessWebApi()
        {
            var request = WebRequest.Create("http://localhost:53127/api/values");
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
