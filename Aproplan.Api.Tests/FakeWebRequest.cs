using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aproplan.Api.Tests
{
    public class FakeWebRequest: IWebRequestCreate
    {
        public static FakeWebRequest Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new FakeWebRequest();
                }
                return _instance;
            }
        }
        /// <summary>
        /// The web request.
        /// </summary>
        private static WebRequest nextRequest;

        /// <summary>
        /// Internally held lock object for multi-threading support.
        /// </summary>
        private static object lockObject = new object();

        public IReadOnlyList<Uri> UriCalled
        {

            get { return _uriCreated; }
        }

        /// <summary>
        /// Gets or sets the next request object.
        /// </summary>
        public static WebRequest NextRequest
        {
            get
            {
                return nextRequest;
            }

            set
            {
                lock (lockObject)
                {
                    nextRequest = value;
                }
            }
        }

        /// <summary>
        /// Creates the new instance of the CustomWebRequest.
        /// </summary>
        /// <param name="uri">The given Uri</param>
        /// <returns>An instantiated web request object requesting from the given Uri.</returns>
        public WebRequest Create(Uri uri)
        {
            _uriCreated.Add(uri);
            return nextRequest;
        }

        public void Reset()
        {
            _uriCreated.Clear();
        }

        /// <summary>
        /// Creates a Mock Http Web request
        /// </summary>
        /// <returns>The mocked HttpRequest object</returns>
        public static Mock<HttpWebRequest> CreateMockHttpWebRequestWithGivenResponseCode(HttpStatusCode httpStatusCode)
        {
            var response = new Mock<HttpWebResponse>(MockBehavior.Loose);
            response.Setup(c => c.StatusCode).Returns(httpStatusCode);

            var request = new Mock<HttpWebRequest>();
            request.Setup(s => s.GetResponse()).Returns(response.Object);
            NextRequest = request.Object;
            return request;
        }

        public static Mock<HttpWebRequest> CreateRequestWithResponse(string responseContent)
        {
            var response = new Mock<HttpWebResponse>(MockBehavior.Loose);
            var responseStream = new MemoryStream(Encoding.UTF8.GetBytes(responseContent));

            response.Setup(c => c.StatusCode).Returns(HttpStatusCode.OK);
            response.Setup(c => c.GetResponseStream()).Returns(responseStream);

            var request = new Mock<HttpWebRequest>();
            request.Setup(s => s.GetResponseAsync()).Returns(Task.FromResult((WebResponse)response.Object));

            var requestStream= new MemoryStream();
            request.Setup(s => s.GetRequestStream()).Returns(requestStream);
            NextRequest = request.Object;

            return request;
        }

        private FakeWebRequest()
        {

        }

        private List<Uri> _uriCreated = new List<Uri>();
        private static FakeWebRequest _instance;
    }
}
