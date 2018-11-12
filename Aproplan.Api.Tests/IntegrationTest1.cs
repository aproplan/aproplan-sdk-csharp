using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aproplan.Api.Http;
using Aproplan.Api.Model.Actors;
using Aproplan.Api.Model.Annotations;
using Aproplan.Api.Model.IdentificationFiles;
using Aproplan.Api.Model.List;
using Aproplan.Api.Model.Projects;
using Aproplan.Api.Model.Projects.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aproplan.Api.Tests
{
    [TestClass]
    public class IntegrationTest1
    {
        private ApiRequest Api;
        private User LoggedUser;
        private List<Project> Projects;
        private List<Meeting> Lists;
        private List<Note> Points;
        private List<NoteProjectStatus> Status;

        private Note NewPoint; 

        [TestMethod]
        public async Task IntegrationTest()
        {
            await PublicRequest();
            await Login();
            await GetPoint();
            await CreatePoint();
            await EditPoint();
            Logout();
        }

        #region Internal methods

        /// <summary>
        /// Make some unauthenticated requests
        /// </summary>
        /// <returns></returns>
        public async Task PublicRequest()
        {
            ApiRequest req = new ApiRequest(Config.RequesterId);
            var languages = (await req.GetEntityList<Language>());
            Assert.IsTrue(languages.Count > 0);
        }

        /// <summary>
        /// Log into the API
        /// </summary>
        /// <returns></returns>
        public async Task Login()
        {
            // Log in 
            String alias = "acl+demo@aproplan.com";
            String password = "aproplan";
            Api = new ApiRequest(alias, password, Config.RequesterId, "https://app.aproplan.com");
            LoggedUser = await Api.Login();
            Assert.AreEqual(LoggedUser.Alias, alias);
        }

        /// <summary>
        /// Get some data about logged in user
        /// </summary>
        /// <returns></returns>
        public async Task GetPoint()
        {

            // Retrieve data
            Points = (await Api.GetEntityList<Note>());
            Assert.IsTrue(Points.Count > 0);

            Projects = (await Api.GetEntityList<Project>());
            Assert.IsTrue(Projects.Count > 0);

            Status = (await Api.GetEntityList<NoteProjectStatus>(pathToLoad: new Http.Utils.PathToLoad("Project")));
            Assert.IsTrue(Status.Count > 0);

            Lists = (await Api.GetEntityList<Meeting>(pathToLoad: new Http.Utils.PathToLoad("Project")));
            Assert.IsTrue(Lists.Count > 0);
        }

        /// <summary>
        /// Create a note and check it worked
        /// </summary>
        /// <returns></returns>
        public async Task CreatePoint()
        {
            NewPoint = await Api.CreateEntity<Note>(new Note
            {
                Subject = "created",
                Id = Guid.NewGuid(),
                Project = Projects[0],
                Meeting = Lists.Find(x => x.Project.Id == Projects[0].Id),
                Status = Status.Find(x => x.Project.Id == Projects[0].Id),
                From = LoggedUser
            });

            Assert.IsNotNull(NewPoint.EntityCreationDate);

            var points2 = (await Api.GetEntityList<Note>());
            Assert.AreEqual(points2.Count, Points.Count + 1);
        }

        /// <summary>
        /// Edit a point & check the changes
        /// </summary>
        /// <returns></returns>
        public async Task EditPoint()
        {
            NewPoint.Subject = "Subject edited";
            NewPoint.Status = Status.FindLast(x => x.Project.Id == Projects[0].Id);
            NewPoint.ModifiedProperties = new[] { "Subject", "Status" }; 
            NewPoint = await Api.UpdateEntity<Note>(NewPoint);
            NewPoint = await Api.GetEntityById<Note>(NewPoint.Id, Projects[0].Id, new Http.Utils.PathToLoad("Project,Status,Meeting,From,ProcessStatusHistories")); 

            Assert.AreEqual(NewPoint.Subject, "Subject edited");
            Assert.IsTrue(NewPoint.ProcessStatusHistories.Count > 1);
        }

        public void Logout()
        {
            Api.Logout();
        }

        #endregion 
    }
}
