using Aproplan.Api.Http;
using Aproplan.Api.Http.Utils;
using Aproplan.Api.Model.Annotations;
using Aproplan.Api.Tests.Utilities;
using Aproplan.Api.Tests.Utilities.Models;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Aproplan.Api.Tests
{
    public class GetEntitiesTests
    {
        ApiRequest request;
        [SetUp]
        public void SetupCase()
        {
            request = AproplanApiUtility.CreateRequester();

            WebRequest.RegisterPrefix(request.ApiRootUrl, FakeWebRequest.Instance);
            UserUtility.MakeLogin(request);
        }

        [TestCase]
        public void GetEntityByIdOK()
        {
            var projectId = Guid.NewGuid();
            Note note = NoteUtility.GetFakeSimpleNotes()[0];

            string content = JsonConvert.SerializeObject(new List<Note> { note }, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content);
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();

            Note resultNote = request.GetEntityById<Note>(note.Id, projectId).GetAwaiter().GetResult();

            string filter = string.Format("Filter.Eq(Id,{0})", note.Id);

            
            string expectedUrl = AproplanApiUtility.BuildRestUrl(request.ApiRootUrl, "notes", request.ApiVersion, request.RequesterId, request.TokenInfo.Token);
            expectedUrl += "&filter=" + AproplanApiUtility.EncodeUrl(filter) + "&projectid=" + projectId;
            
            Assert.AreEqual(expectedUrl, FakeWebRequest.Instance.UriCalled[0].ToString());

            Assert.AreEqual(note.Id, resultNote.Id);

            mockWebRequest.Verify();
        }

        [TestCase]
        public void GetEntityByIdWithoutResultOK()
        {
            string content = JsonConvert.SerializeObject(new List<Note>(), new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content);
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();

            var id = Guid.NewGuid();
            Note resultNote = request.GetEntityById<Note>(id).GetAwaiter().GetResult();

            Assert.IsNull(resultNote);
            mockWebRequest.Verify();
        }

        [TestCase]
        public void GetEntityByIdsWithSeveralValOK()
        {
            var projectId = Guid.NewGuid();
            List<Note> notes = NoteUtility.GetFakeSimpleNotes();

            string content = JsonConvert.SerializeObject(notes, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content);
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();

            Guid[] ids = notes.Select((n => n.Id)).ToArray();
            List<Note> resultNotes = request.GetEntityByIds<Note>(ids, projectId).GetAwaiter().GetResult();

            string filter = Filter.In("Id", ids).ToString();


            string expectedUrl = AproplanApiUtility.BuildRestUrl(request.ApiRootUrl, "notes", request.ApiVersion, request.RequesterId, request.TokenInfo.Token);
            expectedUrl += "&filter=" + AproplanApiUtility.EncodeUrl(filter) + "&projectid=" + projectId;

            Assert.AreEqual(expectedUrl, FakeWebRequest.Instance.UriCalled[0].ToString());

            Assert.AreEqual(notes.Count, resultNotes.Count);
            for (int i = 0; i < notes.Count; i++) {
                var expectedNote = notes[i];
                Assert.AreEqual(expectedNote.Id, resultNotes[i].Id);
            }

            mockWebRequest.Verify();
        }

        [TestCase]
        public void GetEntityByIdsWithOneValOK()
        {
            var projectId = Guid.NewGuid();
            Note note = NoteUtility.GetFakeSimpleNotes()[0];

            string content = JsonConvert.SerializeObject(new List<Note> { note }, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content);
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();

            Guid[] ids = new Guid[] { note.Id };
            List<Note> resultNotes = request.GetEntityByIds<Note>(ids, projectId).GetAwaiter().GetResult();

            string filter = Filter.Eq("Id", note.Id).ToString();


            string expectedUrl = AproplanApiUtility.BuildRestUrl(request.ApiRootUrl, "notes", request.ApiVersion, request.RequesterId, request.TokenInfo.Token);
            expectedUrl += "&filter=" + AproplanApiUtility.EncodeUrl(filter) + "&projectid=" + projectId;

            Assert.AreEqual(expectedUrl, FakeWebRequest.Instance.UriCalled[0].ToString());

            Assert.AreEqual(1, resultNotes.Count);
            Assert.AreEqual(note.Id, resultNotes[0].Id);
            

            mockWebRequest.Verify();
        }

        [TestCase]
        public void GetEntityByIdsWithQueryParamsOK()
        {
            var projectId = Guid.NewGuid();
            Note note = NoteUtility.GetFakeSimpleNotes()[0];

            string content = JsonConvert.SerializeObject(new List<Note> { note }, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content);
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();

            Guid[] ids = new Guid[] { note.Id };
            List<Note> resultNotes = request.GetEntityByIds<Note>(ids, projectId, null, new Dictionary<string, string> { { "testparam", "john" } }).GetAwaiter().GetResult();

            string filter = Filter.Eq("Id", note.Id).ToString();


            string expectedUrl = AproplanApiUtility.BuildRestUrl(request.ApiRootUrl, "notes", request.ApiVersion, request.RequesterId, request.TokenInfo.Token);
            expectedUrl += "&filter=" + AproplanApiUtility.EncodeUrl(filter) + "&projectid=" + projectId + "&testparam=john";

            Assert.AreEqual(expectedUrl, FakeWebRequest.Instance.UriCalled[0].ToString());

            Assert.AreEqual(1, resultNotes.Count);
            Assert.AreEqual(note.Id, resultNotes[0].Id);


            mockWebRequest.Verify();
        }

        [TestCase]
        public void GetEntityCountOK()
        {
            var projectId = Guid.NewGuid();
            string content = JsonConvert.SerializeObject(new List<Note>(), new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse("125");
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();

            var id = Guid.NewGuid();
            Filter filter = Filter.IsNotNull("Subject");

            int cpt = request.GetEntityCount<Note>(projectId, filter).GetAwaiter().GetResult();

            string expectedUrl = AproplanApiUtility.BuildRestUrl(request.ApiRootUrl, "notecount", request.ApiVersion, request.RequesterId, request.TokenInfo.Token);
            expectedUrl += "&filter=" + AproplanApiUtility.EncodeUrl(filter.ToString()) + "&projectid=" + projectId;

            Assert.AreEqual(expectedUrl, FakeWebRequest.Instance.UriCalled[0].ToString());
            Assert.AreEqual(125, cpt);
            mockWebRequest.Verify();
        }

        [TestCase]
        public void GetEntityIdsOK()
        {
            var projectId = Guid.NewGuid();
            List<Guid> expectedGuid = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            string content = JsonConvert.SerializeObject(expectedGuid, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });
            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content);
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();

            var id = Guid.NewGuid();
            Filter filter = Filter.IsNotNull("Subject");

            List<Guid> guids = request.GetEntityIds<Note>(projectId, filter).GetAwaiter().GetResult();

            string expectedUrl = AproplanApiUtility.BuildRestUrl(request.ApiRootUrl, "notesids", request.ApiVersion, request.RequesterId, request.TokenInfo.Token);
            expectedUrl += "&filter=" + AproplanApiUtility.EncodeUrl(filter.ToString()) + "&projectid=" + projectId;

            Assert.AreEqual(expectedUrl, FakeWebRequest.Instance.UriCalled[0].ToString());
            Assert.AreEqual(expectedGuid.Count, 2);
            Assert.AreEqual(expectedGuid[0], guids[0]);
            Assert.AreEqual(expectedGuid[1],guids[1]);
            mockWebRequest.Verify();
        }

    }
}
