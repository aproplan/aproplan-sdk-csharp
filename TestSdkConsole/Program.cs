using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aproplan.Api;
using Aproplan.Api.Http;
using Aproplan.Api.Http.Services;
using Aproplan.Api.Http.Utils;
using Aproplan.Api.Model;
using Aproplan.Api.Model.Annotations;
using Aproplan.Api.Model.Documents;
using Aproplan.Api.Model.IdentificationFiles;
using Aproplan.Api.Model.List;
using Aproplan.Api.Model.Projects;
using Aproplan.Api.Model.Projects.Config;

namespace TestSdkConsole
{
    class Program
    {
        private static ApiRequest Api;
        static void Main(String[] args)
        {
            // You need to put your token to use the APROPLAN api, you can request one here: https://www.aproplan.com/integrations
            Api = new ApiRequest(new Guid("YOUR-API-REQUESTER-ID"), "https://api-tst.aproplan.com/");

            Task t = mainLoop();
            t.Wait(); 
        }

        private static async Task mainLoop()
        {
            showMenu(); 

            String choice = "";
            while (!(choice = Console.ReadLine()).Contains("exit"))
            {
                try
                {

                    switch (choice.Split(new[] { ' '}, StringSplitOptions.RemoveEmptyEntries).First())
                    {
                        case "?":
                            showMenu();
                            break; 
                        case "login":
                            await Api.Login("sergio.ristagno@aproplan.com", "aproplan");
                            Console.WriteLine("success"); 
                            break;
                        case "logout":
                            Api.Logout();
                            break;
                        case "projects": 
                            foreach(Project p in await Api.GetEntityList<Project>())
                            {
                                Console.WriteLine(p.Id + " " + p.Name);
                            }
                            break;
                        case "lists":
                            foreach (Meeting p in await Api.GetEntityList<Meeting>())
                            {
                                Console.WriteLine(p.Id + " " + p.Title);
                            }
                            break;
                        case "randompoint_create":
                            Project pointProject = (await Api.GetEntityList<Project>())[0]; 
                            Meeting list = (await Api.GetEntityList<Meeting>(filter: Filter.Eq("Project.Id", pointProject.Id)))[0];
                            NoteProjectStatus status = (await Api.GetEntityList<NoteProjectStatus>(filter: Filter.Eq("Project.Id", pointProject.Id)))[0];


                            Note newNote = new Note
                            {
                                Id = Guid.NewGuid(), 
                                Subject = "Sample subject", 
                                Meeting = list, 
                                Project = pointProject, 
                                From = Api.CurrentUser, 
                                Status = status
                            };

                            Note createdNote = await Api.CreateEntity<Note>(newNote);
                            Console.WriteLine("Point created");
                            Console.WriteLine("  " + createdNote.EntityCreationDate);
                            Console.WriteLine("  " + createdNote.EntityCreationUser);
                            break;
                        case "randomform_create":
                            Project formProject = (await Api.GetEntityList<Project>())[0];
                            Meeting formList = (await Api.GetEntityList<Meeting>(filter: Filter.Eq("Project.Id", formProject.Id)))[0];
                            FormTemplate template = (await Api.GetEntityList<FormTemplate>(pathToLoad: new PathToLoad("Questions,SectionRules,Language")))[0];

                            Form newForm = new Form
                            {
                                Id = Guid.NewGuid(),
                                Subject = "Sample subject",
                                Meeting = formList,
                                Project = formProject,
                                From = Api.CurrentUser,
                                TemplateId = template.Id 
                            };


                            FormSection[] sections = new FormSection[template.SectionRules.Count];
                            for (int i = 0; i < template.SectionRules.Count; i++)
                            {
                                sections[i] = template.SectionRules[i].ToSection(newForm.Id);
                            }
                            newForm.Sections = sections.ToList(); 


                            FormItem[] items = new FormItem[template.Questions.Count];
                            for (int i = 0; i < template.Questions.Count; i++)
                            {
                                items[i] = template.Questions[i].ToFormItem(newForm.Id, newForm.Sections);
                            }
                            newForm.Items = items.ToList();

                            newForm.Language = template.Language; 

                            Form createdForm = await Api.CreateEntity<Form>(newForm);
                            Console.WriteLine("Form created");
                            Console.WriteLine("  " + createdForm.EntityCreationDate);
                            Console.WriteLine("  " + createdForm.EntityCreationUser);
                            break;
                        default:
                            Console.WriteLine("Unknown command...");
                            showMenu();
                            break; 
                    }
                }
                catch(ApiException e)
                {
                    Console.WriteLine("API Exception raised: ");
                    Console.WriteLine("  " + e.Code);
                    Console.WriteLine("  " + e.ErrorId);
                    Console.WriteLine("  " + e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Other exception raised: ");
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine(""); 
                Console.WriteLine("----------------------------------");
            }
        }

        private static void showMenu()
        {
            Console.WriteLine("Type any of the following commands");
            Console.WriteLine("   ?         - Show this help"); 
            Console.WriteLine("   exit      - quit program"); 
            Console.WriteLine("   login     - Log into aproplan"); 
            Console.WriteLine("   logout    - Log out of aproplan"); 
            Console.WriteLine("   projects  - Display my projects"); 
            Console.WriteLine("   lists     - Display my lists"); 
            Console.WriteLine("   randompoint_create - Create a random point");
            Console.WriteLine("   randomform_create - Create a random form");
            Console.WriteLine(""); 
            Console.WriteLine(""); 
        }

        /*static void Main(string[] args)
        {
            using (ApiRequest request = new ApiRequest(new Guid("EFCF4D2E-70BD-4BCE-B81B-CBED37F6F7F3"), "https://api-tst.aproplan.com/"))
            {
                try
                {
                    var countries = request.GetEntityList<Country>().GetAwaiter().GetResult();

                    Console.WriteLine(request.Login("sergio.ristagno@aproplan.com", "aproplan").GetAwaiter().GetResult());
                    //string res = Aproplan.Api.Api.Request("http://localhost:39299/rest/countries?v=13&requesterid=EFCF4D2E-70BD-4BCE-B81B-CBED37F6F7F3", Aproplan.Api.ApiMethod.Get, null).GetAwaiter().GetResult();

                    //Project project = new Project();
                    //project.Name = "SDK project";
                    //project.Code = "SDK";
                    //project.Address = "Chaussée de bruxelles 315A";
                    //project.City = "La Hulpe";
                    //project.Country = countries.FirstOrDefault(x => x.Iso2 == "BE");
                    //project = request.CreateEntity(project, new Dictionary<string, string> { { "defaultfolder", "true" } }).GetAwaiter().GetResult();



                    //Console.WriteLine(project.Id + " - " + project.Name);

                    var listProj = request.GetEntityList<Project>(null, Filter.IsTrue("IsActive")).GetAwaiter().GetResult();
                    foreach(var project in listProj)
                        Console.WriteLine(project.Id + " - " + project.Name);

                    //Document d = svd.UploadNewDocument(@"D:\APROPLAN\temp\planTest\NTK_CL_NIV00_20120412_PL48_J.pdf", Guid.Parse("E6657B38-2EA8-4BAB-978A-2DDFFC79B767")).GetAwaiter().GetResult();
                    //Thread.Sleep(6000);
                    // Document version = svd.UploadVersion(d.Id, @"D:\APROPLAN\temp\planTest\NTK_CL_NIV00_20120417_PL48_K.pdf").GetAwaiter().GetResult();
                    //  var d = svd.UploadSourceToDocument(@"D:\APROPLAN\temp\planTest\182_02_BO_GE_BLOK_K_AS.dwg",  d).GetAwaiter().GetResult();

                    //    Console.WriteLine(d.Name);
                    ProjectService psv = new ProjectService(request);
                    var idLevels = psv.GetFoldersIdHierarchy(Guid.Parse("9527fc61-f695-4f48-8763-e5a14e5300f2"), "Custom").GetAwaiter().GetResult();
                    foreach (var idLevel in idLevels)
                        Console.WriteLine(idLevel.Id + " - " + idLevel.Level);

                    request.RenewToken().GetAwaiter();

                }
                catch(ApiException ex)
                {
                    Console.WriteLine(ex.Message + " - " + (ex.Code == null ? "-[No code]": ex.Code));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
        }*/
    }
}
