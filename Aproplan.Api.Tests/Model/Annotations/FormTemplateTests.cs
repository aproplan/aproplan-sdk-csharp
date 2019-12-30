using Aproplan.Api.Model.Annotations;
using Aproplan.Api.Tests.Utilities.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aproplan.Api.Tests.Model.Annotations
{
    public class FormTemplateTests
    {
        [TestCase]
        public void SimpleToFormTestOk()
        {
            FormTemplate template = FormTemplateUtility.GetFakeCompleteFormTemplate();

            Form form = template.ToForm();
            Assert.AreEqual(template.Id, form.TemplateId);
            Assert.AreEqual(template.Type, form.Type);
            Assert.AreEqual(template.Subject, form.Subject);
            Assert.AreEqual(template.MustDisplayElementsCode, form.MustDisplayElementsCode);
            Assert.IsNull(form.From);
            Assert.AreEqual(template.SectionRules.Count, form.Sections.Count);
            Assert.AreEqual(template.Questions.Count, form.Items.Count);
            Assert.AreEqual(template.IsSignatureAllowed, form.IsSignatureAllowed);

            int i = 0;
            template.SectionRules.ForEach((section) =>
            {
                Assert.AreEqual(section.Description, form.Sections[i].Description);
                Assert.AreEqual(section.Code, form.Sections[i].Code);
                Assert.AreEqual(section.DisplayOrder, form.Sections[i].DisplayOrder);
                Assert.AreEqual(section.Title, form.Sections[i].Title);
                CheckVisibleConditionEquality(section.VisibleCondition, form.Sections[i].VisibleCondition, template, form);
                i++;
            });

            i = 0;

            template.Questions.ForEach((question) =>
            {
                Assert.AreEqual(question.Template, form.Items[i].Template);
                Assert.AreEqual(question.ItemType, form.Items[i].ItemType);
                Assert.AreEqual(question.Code, form.Items[i].Code);
                Assert.AreEqual(question.Title, form.Items[i].Title);
                Assert.AreEqual(question.Description, form.Items[i].Description);
                Assert.AreEqual(question.ConformityRules, form.Items[i].ConformityRules);
                Assert.AreEqual(question.AllowAttachComment, form.Items[i].AllowAttachComment);
                Assert.AreEqual(question.AllowAttachPicture, form.Items[i].AllowAttachPicture);
                Assert.AreEqual(form.Id, form.Items[i].FormId);
                CheckVisibleConditionEquality(question.VisibleCondition, form.Items[i].VisibleCondition, template, form);
                i++;
            });
        }


        private static void CheckVisibleConditionEquality(FormFilterCondition templateCondition, FormFilterCondition formCondition, FormTemplate template, Form form)
        {
            if(templateCondition == null)
            {
                Assert.IsNull(formCondition);
                return;
            }

            Assert.AreEqual(templateCondition.GetType(), formCondition.GetType());
            FilterProperty propertyFilter = templateCondition as FilterProperty;  
            if(propertyFilter != null)
            {
                FilterProperty formProperty = (FilterProperty)formCondition;
                Assert.AreEqual(propertyFilter.IsConform, formProperty.IsConform);
                Assert.AreEqual(propertyFilter.Value, formProperty.Value);
                Assert.AreEqual(propertyFilter.NotApplicable, formProperty.NotApplicable);
                Guid itemId = form.Items.Find((item) => item.QuestionId == propertyFilter.ItemId).Id;
                Assert.AreEqual(itemId, formProperty.ItemId);
            }
            else
            {
                FilterCombination combination = (FilterCombination)templateCondition;
                FilterCombination formComb = (FilterCombination)formCondition;
                Assert.AreEqual(combination.Type, formComb.Type);
                CheckVisibleConditionEquality(combination.LeftFilter, formComb.LeftFilter, template, form);
                CheckVisibleConditionEquality(combination.RightFilter, formComb.RightFilter, template, form);
            }
        }
    }
}
