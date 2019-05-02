using Aproplan.Api.Model.Actors;
using Aproplan.Api.Model.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aproplan.Api.Tests.Utilities.Models
{
    public class FormTemplateUtility
    {
        public static List<FormTemplate> GetFakeSimpleFormTemplates()
        {
            User user = UserUtility.CreateUser("john.smith@letsbuild.com", "John Smith");
            List<FormTemplate> fakeTemplates = new List<FormTemplate>
            {
                new FormTemplate{ Id = Guid.NewGuid(), Type = FormType.Quality, Subject = "Template 1", MustDisplayElementsCode = true, Creator = user },
                new FormTemplate{ Id = Guid.NewGuid(), Type = FormType.Safety, Subject = "Template 2", MustDisplayElementsCode = true, Creator = user },
                new FormTemplate{ Id = Guid.NewGuid(), Type = FormType.Quality, Subject = "Template 3", MustDisplayElementsCode = false, Creator = user },
                new FormTemplate{ Id = Guid.NewGuid(), Type = FormType.Safety, Subject = "Template 4", MustDisplayElementsCode = false, Creator = user},
                new FormTemplate{ Id = Guid.NewGuid(), Type = FormType.Environment, Subject = "Template 5", MustDisplayElementsCode = true, Creator = user }
            };
            return fakeTemplates;
        }
        
        public static FormTemplate GetFakeCompleteFormTemplate()
        {
            User user = UserUtility.CreateUser("john.smith@letsbuild.com", "John Smith");
            FormTemplate template = new FormTemplate { Id = Guid.NewGuid(), Type = FormType.Quality, Subject = "Template 1", MustDisplayElementsCode = true, Creator = user };

            template.SectionRules = new List<FormSectionRule>();
            template.SectionRules.Add(new FormSectionRule
            {
                Id = Guid.NewGuid(),
                Description = "Section 1",
                Code = "S1"
            });
            

            template.Questions = new List<FormQuestion>();
            template.Questions.Add(new FormQuestion
            {
                ItemType = FormItemType.PredefinedString,
                Title = "Question 1",
                Code = "S1.1",
                DisplayOrder = 1,
                Template = "{\"title\":\"Question 1\",\"description\":\"\", \"type\":\"string\", \"format\":null,\"availableChoices\":null,\"enum\":[\"OK\", \"NOK\"],\"max -length\":null,\"minimum\":null,\"maximum\":null,\"pattern\":null}",
                FormTemplateID = template.Id,
                SectionRuleId = template.SectionRules[0].Id,
            });
            template.Questions.Add(new FormQuestion
            {
                ItemType = FormItemType.PredefinedString,
                Title = "Question 2",
                Code = "S1.2",
                DisplayOrder = 2,
                Template = "{\"title\":\"Question 2\",\"description\":\"\", \"type\":\"string\", \"format\":null,\"availableChoices\":null,\"enum\":[\"OK\", \"NOK\"],\"max -length\":null,\"minimum\":null,\"maximum\":null,\"pattern\":null}",
                FormTemplateID = template.Id,
                SectionRuleId = template.SectionRules[0].Id,
                VisibleCondition = new FilterProperty
                {
                    ItemId = template.Questions[0].Id,
                    Value = "Yes"
                }
            });

            template.SectionRules.Add(new FormSectionRule
            {
                Id = Guid.NewGuid(),
                Description = "Section 2",
                Code = "S2",
                VisibleCondition = new FilterProperty
                {
                    ItemId = template.Questions[1].Id,
                    Value = "No"
                }
            });

            template.Questions.Add(new FormQuestion
            {
                ItemType = FormItemType.FreeText,
                Title = "Question 3",
                Code = "S2.1",
                DisplayOrder = 3,
                Template = "{\"title\":\"Question 3\",\"description\":\"\", \"type\":\"string\", \"format\":null,\"availableChoices\":null,\"max -length\":null,\"minimum\":null,\"maximum\":null,\"pattern\":null}",
                FormTemplateID = template.Id,
                SectionRuleId = template.SectionRules[0].Id                
            });


            return template;
        }
    }
}
