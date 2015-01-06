using System;
using Xunit;

namespace IisHelper.MvcApplication.Test
{
    /// <summary>
    /// XUnit.Net fixture class that exposes the <see cref="IisExpress"/> helper class 
    /// to a test class that implements <see cref="IUseFixture{IisExpressFixture}"/>.
    /// Shuts down the IisExpress instance on disposal.
    /// </summary>
    /// <remarks>
    /// Instantiated just before the first test is run, and the same instance is shared by all tests in the class. 
    /// Implements <see cref="IDisposable"/>, therefore it will be disposed of after the last test is run.
    /// <para>Equivalent to <c>TestFixtureSetUp/TearDown</c> methods in NUnit, or <c>ClassInitialize/Cleanup</c> 
    /// in MSTest respectively. </para>
    /// </remarks>
    public class IisExpressFixture : IDisposable
    {
        private readonly IisExpress _iisExpress = new IisExpress();

        public IisExpress IisExpress
        {
            get { return _iisExpress; }
        }

        public void Dispose()
        {
            _iisExpress.Stop();
        }
    }
}