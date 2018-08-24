using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aproplan.Api;
using Aproplan.Api.Http;
using Aproplan.Api.Http.Utils;
using Aproplan.Api.Model;
using Aproplan.Api.Model.Projects;

namespace TestSdkConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ApiRequest request = new ApiRequest("sergio.ristagno@aproplan.com", "aproplan", new Guid("EFCF4D2E-70BD-4BCE-B81B-CBED37F6F7F3"), "13", "https://app-tst.aproplan.com/");

            request.Login().GetAwaiter().GetResult();
            //string res = Aproplan.Api.Api.Request("https://app-tst.aproplan.com/rest/countries?v=13&requesterid=EFCF4D2E-70BD-4BCE-B81B-CBED37F6F7F3", Aproplan.Api.ApiMethod.Get, null).GetAwaiter().GetResult();
            
            Console.WriteLine(request.CurrentUser.Alias);
            var projects = request.GetEntityList<Project>(null, new PathToLoad("Creator")).GetAwaiter().GetResult();
            Console.WriteLine(projects.Count);
            Console.ReadLine();
        }
    }
}
