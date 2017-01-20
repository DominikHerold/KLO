using System.Collections.Generic;

using FluentAssertions;

using NUnit.Framework;

namespace Klo.Core.Tests
{
    [TestFixture]
    public class WebClientWrapperTests
    {
        private WebClientWrapper _sut;

        private string _result;

        private string _address;

        private Dictionary<string, string> _optionalHeaders;
        
        [SetUp]
        public void given_the_sut()
        {
            _sut = new WebClientWrapper();
            _address = "http://192.168.23.171:8080/api/respones.json";
            _optionalHeaders = new Dictionary<string, string> { { "Content-Type", "application/xml" } };
        }

        private void because_of_execution()
        {
            _result = _sut.DownloadString(_address, _optionalHeaders);
        }

        [Test]
        public void it_should_return_correct_string()
        {
            because_of_execution();

            _result.Should().Contain("isLightOn");
        }
    }
}
