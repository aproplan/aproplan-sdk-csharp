using Aproplan.Api.Http;
using Aproplan.Api.Http.Services;
using Aproplan.Api.Model.Annotations;
using Aproplan.Api.Model.Projects;
using Aproplan.Api.Tests.Utilities;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aproplan.Api.Tests.Services
{
    [TestFixture]
    public class SyncServiceTests
    {
        [SetUp]
        public void SetupCase() {
            FakeWebRequest.Instance.Reset();
        }
        [TestCase]
        public void SyncProjectsOK()
        {
            Mock<ApiRequest> mockApi = AproplanApiUtility.CreateMockRequester();


            WebRequest.RegisterPrefix(AproplanApiUtility.ROOT_URL, FakeWebRequest.Instance);

            SyncService syncService = new SyncService(mockApi.Object);

            List<Project> fakeProjects = GetFakeProjects();

            string content = JsonConvert.SerializeObject(fakeProjects.GetRange(0, 2), new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content, new Dictionary<string, string> { { "SyncTimestamp", "stamp1;projectid=3" } });
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();

            SyncResult<Project> result = syncService.SyncProjects(null).GetAwaiter().GetResult();

            UriBuilder uriBuilder = new UriBuilder("https://api.aproplan.com/rest/projectsync");

            string expectedUrl = AproplanApiUtility.BuildRestUrl(mockApi.Object.ApiRootUrl, "projectsync", mockApi.Object.ApiVersion, mockApi.Object.RequesterId);
            Assert.AreEqual(expectedUrl, FakeWebRequest.Instance.UriCalled[0].ToString());
            Assert.AreEqual(2, result.Data.Count);
            Assert.AreEqual(fakeProjects[0].Id, result.Data[0].Id);
            Assert.AreEqual(fakeProjects[1].Id, result.Data[1].Id);
            Assert.AreEqual("stamp1;projectid=3", result.ContinuationToken);
            mockWebRequest.Verify();
        }

        [TestCase]
        public void SyncAllProjectsOK()
        {

            Mock<ApiRequest> mockApi = AproplanApiUtility.CreateMockRequester();

            WebRequest.RegisterPrefix(AproplanApiUtility.ROOT_URL, FakeWebRequest.Instance);
            SyncService syncService = new SyncService(mockApi.Object);

            List<Project> fakeProjects = GetFakeProjects();
            int index = 0;
            List<Project> resultToReturn = fakeProjects.GetRange(index, 2);
            string content = JsonConvert.SerializeObject(resultToReturn, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            string stamp = "stamp" + index + ";projectid=" + index;
            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content, new Dictionary<string, string> { { "SyncTimestamp", stamp } });
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();
            
            string lastStamp = stamp;
            string baseUrl = AproplanApiUtility.BuildRestUrl(mockApi.Object.ApiRootUrl, "projectsync", mockApi.Object.ApiVersion, mockApi.Object.RequesterId);
            SyncResult<Project> result = syncService.SyncAllProjects(null, (SyncResult<Project> r, ref bool cancel) => {
                string expectedUrl = baseUrl;
                if (index > 0)
                    expectedUrl = baseUrl + "&timestamp=" + Uri.EscapeDataString(lastStamp);
                Assert.AreEqual(expectedUrl, FakeWebRequest.Instance.UriCalled[FakeWebRequest.Instance.UriCalled.Count - 1].ToString());
                Assert.AreEqual(stamp, r.ContinuationToken);
                Assert.AreEqual(resultToReturn.Count, r.Data.Count);
                for(var i = 0; i < resultToReturn.Count; i++)
                {
                    Assert.AreEqual(resultToReturn[i].Id, r.Data[i].Id);
                }

                lastStamp = stamp;
                index += 2;
                int cpt = fakeProjects.Count - index;

                resultToReturn = cpt > 0 ? fakeProjects.GetRange(index, cpt < 2 ? 1 : 2) : new List<Project>();
                content = JsonConvert.SerializeObject(resultToReturn, new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                });
                if (resultToReturn.Count > 0)
                    stamp = "stamp" + index + ";projectid=" + index;
                else
                    stamp = null;
                mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content, new Dictionary<string, string> { { "SyncTimestamp", stamp } });
                mockWebRequest.SetupSet(req => req.Method = "GET").Verifiable();
                

                
            }).GetAwaiter().GetResult();

            

            Assert.AreEqual(4, FakeWebRequest.Instance.UriCalled.Count);
            Assert.AreEqual(fakeProjects.Count, result.Data.Count);
            
            Assert.AreEqual(true, string.IsNullOrEmpty(result.ContinuationToken));

            mockWebRequest.Verify();
        }

        [TestCase]
        public void SyncNotesOK()
        {
            Mock<ApiRequest> mockApi = AproplanApiUtility.CreateMockRequester();
            Guid projectId = Guid.NewGuid();

            WebRequest.RegisterPrefix(AproplanApiUtility.ROOT_URL, FakeWebRequest.Instance);

            SyncService syncService = new SyncService(mockApi.Object);

            List<Note> fakeNotes = GetFakeNotes();

            string content = JsonConvert.SerializeObject(fakeNotes.GetRange(0, 2), new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content, new Dictionary<string, string> { { "SyncTimestamp", "stamp1;noteid=3" } });
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();

            SyncResult<Note> result = syncService.SyncNotes(projectId, null).GetAwaiter().GetResult();


            string expectedUrl = AproplanApiUtility.BuildRestUrl(mockApi.Object.ApiRootUrl, "notesync", mockApi.Object.ApiVersion, mockApi.Object.RequesterId);
            expectedUrl += "&projectid=" + projectId;
            Assert.AreEqual(expectedUrl, FakeWebRequest.Instance.UriCalled[0].ToString());
            Assert.AreEqual(2, result.Data.Count);
            Assert.AreEqual(fakeNotes[0].Id, result.Data[0].Id);
            Assert.AreEqual(fakeNotes[1].Id, result.Data[1].Id);
            Assert.AreEqual("stamp1;noteid=3", result.ContinuationToken);
            mockWebRequest.Verify();
        }


        [TestCase]
        public void SyncAllNotesOK()
        {

            Guid projectId = Guid.NewGuid();
            Mock<ApiRequest> mockApi = AproplanApiUtility.CreateMockRequester();

            WebRequest.RegisterPrefix(AproplanApiUtility.ROOT_URL, FakeWebRequest.Instance);
            SyncService syncService = new SyncService(mockApi.Object);

            List<Note> fakeNotes = GetFakeNotes();
            int index = 0;
            List<Note> resultToReturn = fakeNotes.GetRange(index, 2);
            string content = JsonConvert.SerializeObject(resultToReturn, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            string stamp = "stamp" + index + ";noteid=" + index;
            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content, new Dictionary<string, string> { { "SyncTimestamp", stamp } });
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();

            string lastStamp = stamp;
            string baseUrl = AproplanApiUtility.BuildRestUrl(mockApi.Object.ApiRootUrl, "notesync", mockApi.Object.ApiVersion, mockApi.Object.RequesterId);
            baseUrl += "&projectid=" + projectId;
            SyncResult<Note> result = syncService.SyncAllNotes(projectId, null, (SyncResult<Note> r, ref bool cancel) => {
                string expectedUrl = baseUrl;
                if (index > 0)
                    expectedUrl = baseUrl + "&timestamp=" + Uri.EscapeDataString(lastStamp);
                Assert.AreEqual(expectedUrl, FakeWebRequest.Instance.UriCalled[FakeWebRequest.Instance.UriCalled.Count - 1].ToString());
                Assert.AreEqual(stamp, r.ContinuationToken);
                Assert.AreEqual(resultToReturn.Count, r.Data.Count);
                for (var i = 0; i < resultToReturn.Count; i++)
                {
                    Assert.AreEqual(resultToReturn[i].Id, r.Data[i].Id);
                }

                lastStamp = stamp;
                index += 2;
                int cpt = fakeNotes.Count - index;

                resultToReturn = cpt > 0 ? fakeNotes.GetRange(index, cpt < 2 ? 1 : 2) : new List<Note>();
                content = JsonConvert.SerializeObject(resultToReturn, new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                });
                if (resultToReturn.Count > 0)
                    stamp = "stamp" + index + ";noteid=" + index;
                else
                    stamp = null;
                mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content, new Dictionary<string, string> { { "SyncTimestamp", stamp } });
                mockWebRequest.SetupSet(req => req.Method = "GET").Verifiable();



            }).GetAwaiter().GetResult();



            Assert.AreEqual(4, FakeWebRequest.Instance.UriCalled.Count);
            Assert.AreEqual(fakeNotes.Count, result.Data.Count);

            Assert.AreEqual(true, string.IsNullOrEmpty(result.ContinuationToken));

            mockWebRequest.Verify();
        }

        [TestCase]
        public void SyncAllNotesCancelledOK()
        {

            Guid projectId = Guid.NewGuid();
            Mock<ApiRequest> mockApi = AproplanApiUtility.CreateMockRequester();

            WebRequest.RegisterPrefix(AproplanApiUtility.ROOT_URL, FakeWebRequest.Instance);
            SyncService syncService = new SyncService(mockApi.Object);

            List<Note> fakeNotes = GetFakeNotes();
            int index = 0;
            List<Note> resultToReturn = fakeNotes.GetRange(index, 2);
            string content = JsonConvert.SerializeObject(resultToReturn, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            string stamp = "stamp" + index + ";noteid=" + index;
            Mock<HttpWebRequest> mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content, new Dictionary<string, string> { { "SyncTimestamp", stamp } });
            mockWebRequest.SetupSet(r => r.Method = "GET").Verifiable();

            string lastStamp = stamp;
            string baseUrl = AproplanApiUtility.BuildRestUrl(mockApi.Object.ApiRootUrl, "notesync", mockApi.Object.ApiVersion, mockApi.Object.RequesterId);
            baseUrl += "&projectid=" + projectId;
            SyncResult<Note> result = syncService.SyncAllNotes(projectId, null, (SyncResult<Note> r, ref bool cancel) => {
                string expectedUrl = baseUrl;
                if (index > 0)
                    expectedUrl = baseUrl + "&timestamp=" + Uri.EscapeDataString(lastStamp);
                Assert.AreEqual(expectedUrl, FakeWebRequest.Instance.UriCalled[FakeWebRequest.Instance.UriCalled.Count - 1].ToString());
                Assert.AreEqual(stamp, r.ContinuationToken);
                Assert.AreEqual(resultToReturn.Count, r.Data.Count);
                for (var i = 0; i < resultToReturn.Count; i++)
                {
                    Assert.AreEqual(resultToReturn[i].Id, r.Data[i].Id);
                }

                lastStamp = stamp;
                index += 2;
                int cpt = fakeNotes.Count - index;

                resultToReturn = cpt > 0 ? fakeNotes.GetRange(index, cpt < 2 ? 1 : 2) : new List<Note>();
                content = JsonConvert.SerializeObject(resultToReturn, new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                });
                if (resultToReturn.Count > 0)
                    stamp = "stamp" + index + ";noteid=" + index;
                else
                    stamp = null;
                mockWebRequest = FakeWebRequest.CreateRequestWithResponse(content, new Dictionary<string, string> { { "SyncTimestamp", stamp } });
                
                cancel = true;



            }).GetAwaiter().GetResult();



            Assert.AreEqual(1, FakeWebRequest.Instance.UriCalled.Count);
            Assert.AreEqual(2, result.Data.Count);

            Assert.AreEqual(lastStamp, result.ContinuationToken);

            mockWebRequest.Verify();
        }

        public List<Project> GetFakeProjects()
        {
            List<Project> fakeProjects = new List<Project>
            {
                new Project{ Id = Guid.NewGuid(), Code = "PR1", Name = "Project 1" },
                new Project{ Id = Guid.NewGuid(), Code = "PR2", Name = "Project 2" },
                new Project{ Id = Guid.NewGuid(), Code = "PR3", Name = "Project 3" },
                new Project{ Id = Guid.NewGuid(), Code = "PR4", Name = "Project 4" },
                new Project{ Id = Guid.NewGuid(), Code = "PR5", Name = "Project 5" }
            };
            return fakeProjects;
        }

        public List<Note> GetFakeNotes()
        {
            List<Note> fakePoints= new List<Note>
            {
                new Note{ Id = Guid.NewGuid(), Code = "PR1", Subject = "Point 1" },
                new Note{ Id = Guid.NewGuid(), Code = "PR2", Subject = "Point 2" },
                new Note{ Id = Guid.NewGuid(), Code = "PR3", Subject = "Point 3" },
                new Note{ Id = Guid.NewGuid(), Code = "PR4", Subject = "Point 4" },
                new Note{ Id = Guid.NewGuid(), Code = "PR5", Subject = "Point 5" }
            };
            return fakePoints;
        }
    }
}
