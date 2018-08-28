using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aproplan.Api;
using Aproplan.Api.Http;
using Aproplan.Api.Http.Utils;
using Aproplan.Api.Model;
using Aproplan.Api.Model.Documents;
using Aproplan.Api.Model.Projects;

namespace TestSdkConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ApiRequest request = new ApiRequest("sergio.ristagno@aproplan.com", "aproplan", new Guid("EFCF4D2E-70BD-4BCE-B81B-CBED37F6F7F3"), "13", "https://app-tst.aproplan.com/");

            request.Login().GetAwaiter().GetResult();
            //string res = Aproplan.Api.Api.Request("http://localhost:39299/rest/countries?v=13&requesterid=EFCF4D2E-70BD-4BCE-B81B-CBED37F6F7F3", Aproplan.Api.ApiMethod.Get, null).GetAwaiter().GetResult();

            Console.WriteLine(request.CurrentUser.Alias);
            var projects = request.GetEntityList<Project>(null, new PathToLoad("Creator")).GetAwaiter().GetResult();
            Console.WriteLine(projects.Count);
            Console.WriteLine(request.GetEntityCount<Project>().GetAwaiter().GetResult());

            var ids = request.GetEntityIds<Project>().GetAwaiter().GetResult();

            projects = request.GetEntityByIds<Project>(ids.GetRange(0, 3).ToArray()).GetAwaiter().GetResult();
            var tokenInfo = request.RenewToken().GetAwaiter().GetResult();
            Console.WriteLine(projects.Count);

            DocumentService svd = new DocumentService(request);
            //Document d = svd.UploadNewDocument(@"D:\APROPLAN\temp\planTest\NTK_CL_NIV00_20120412_PL48_J.pdf", Guid.Parse("E6657B38-2EA8-4BAB-978A-2DDFFC79B767")).GetAwaiter().GetResult();
            //Thread.Sleep(6000);
           // Document version = svd.UploadVersion(d.Id, @"D:\APROPLAN\temp\planTest\NTK_CL_NIV00_20120417_PL48_K.pdf").GetAwaiter().GetResult();
            var d = svd.JoinSourceToDocument(@"D:\APROPLAN\temp\planTest\182_02_BO_GE_BLOK_K_AS.dwg",  Guid.Parse("27dcc66c-329f-4f70-af9a-e953198ca042")).GetAwaiter().GetResult();

            Console.WriteLine(d.Name);

            Console.ReadLine();
        }
    }
}
