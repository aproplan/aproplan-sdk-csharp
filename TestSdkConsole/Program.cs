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
using Aproplan.Api.Model.Documents;
using Aproplan.Api.Model.IdentificationFiles;
using Aproplan.Api.Model.Projects;

namespace TestSdkConsole
{
    class Program
    {
        static void Main(string[] args)
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
        }
    }
}
