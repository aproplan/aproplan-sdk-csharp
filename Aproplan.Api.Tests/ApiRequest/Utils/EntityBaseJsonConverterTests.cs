using System;
using Aproplan.Api.Http.Utils;
using Aproplan.Api.Model.Annotations;
using Aproplan.Api.Model.Projects.Config;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Aproplan.Api.Tests
{
    [TestFixture]
    public class EntityBaseJsonConverterTests
    {
        [TestCase]
        public void ShouldConvertFilterPropertyJsonToCorrectEntity()
        {
            Guid id = Guid.NewGuid();

            //FormFilterProperty
            string json = @"{
                ""EntityDiscriminator"": ""FilterProperty"",
                ""IsConform"": true,
                ""Value"": ""This is a test"",
                ""ItemId"": """ + id + @"""
            }";

            var filterCondition = JsonConvert.DeserializeObject<FormFilterCondition>(json);
            Assert.IsTrue(filterCondition is FilterProperty);
            var filterProperty = filterCondition as FilterProperty;
            Assert.IsTrue(filterProperty.IsConform);
            Assert.AreEqual("This is a test", filterProperty.Value);
            Assert.AreEqual(id, filterProperty.ItemId);
        }
        
        [TestCase]
        public void ShouldConvertFilterCombinationJsonToCorrectEntity()
        {
            Guid id = Guid.NewGuid();

            //FormFilterCondition
            string json = @"{
                ""EntityDiscriminator"": ""FilterCombination"",
                ""Type"": ""Or"",
                ""RightFilter"": {
                    ""EntityDiscriminator"": ""FilterProperty"",
                    ""IsConform"": true,
                    ""Value"": ""This is a test right"",
                    ""ItemId"": """ + id + @"""
                },
                ""LeftFilter"": {
                    ""EntityDiscriminator"": ""FilterProperty"",
                    ""IsConform"": true,
                    ""Value"": ""This is a test left"",
                    ""ItemId"": """ + id + @"""
                }
            }";

            var filterCondition = JsonConvert.DeserializeObject<FormFilterCondition>(json);
            Assert.IsTrue(filterCondition is FilterCombination);
            var filterCombination = filterCondition as FilterCombination;
            Assert.AreEqual(FormFilterType.Or, filterCombination.Type);
            Assert.AreEqual("This is a test right", ((FilterProperty) filterCombination.RightFilter).Value);
            Assert.AreEqual("This is a test left", ((FilterProperty) filterCombination.LeftFilter).Value);
        }

        [TestCase]
        public void ShouldConvertNoteJsonToCorrectEntityFromNoteBase()
        {
            var note = new Note
            {
                Id = Guid.NewGuid(),
                Subject = "This is my note", 
                Code = "1.01",
                Status = new NoteProjectStatus
                {
                    Code = "INPROGRESS", Color = "452147", Name = "In progress", IsTodo = true
                }
            };

            NoteBase noteBase = note;
            string json = JsonConvert.SerializeObject(noteBase);

            Assert.AreEqual(json,
                "{\"EntityDiscriminator\":\"Note\",\"Status\":{\"IsDisabled\":false,\"Code\":\"INPROGRESS\",\"Name\":\"In progress\",\"DisplayOrder\":0,\"IsOnlyUsedByMeetingManager\":false,\"Color\":\"452147\",\"IsTodo\":true,\"IsDone\":false,"+
                    "\"DoneAction\":false,\"IsBlocked\":false,\"Id\":\""+ note.Status.Id +"\",\"EntityVersion\":0,\"Deleted\":false,\"EntityCreationDate\":\"0001-01-01T00:00:00\",\"EntityModificationDate\":\"0001-01-01T00:00:00\",\"EntityCreationUser\":\"00000000-0000-0000-0000-000000000000\"}" +
                    ",\"ProblemLocation\":0,\"Date\":\"0001-01-01T00:00:00\",\"ModificationDate\":\"0001-01-01T00:00:00\",\"Subject\":\"" + noteBase.Subject + "\",\"IsArchived\":false,\"IsUrgent\":false,\"MeetingNumSeq\":0,\"ProjectNumSeq\":0,\"Code\":\"1.01\",\"IsReadOnly\":false,\"HasAttachment\":false," +
                    "\"Id\":\""+ noteBase.Id +"\",\"EntityVersion\":0,\"Deleted\":false,\"EntityCreationDate\":\"0001-01-01T00:00:00\",\"EntityModificationDate\":\"0001-01-01T00:00:00\",\"EntityCreationUser\":\"00000000-0000-0000-0000-000000000000\"}");
        }
        
        [TestCase]
        public void ShouldConvertFormJsonToCorrectEntityFromNoteBase()
        {
            var form = new Form
            {
                Id = Guid.NewGuid(),
                Subject = "This is my form", 
                Code = "1.01",
                Status = FormStatus.InProgress
            };

            NoteBase noteBase = form;
            string json = JsonConvert.SerializeObject(noteBase);

            Assert.AreEqual(json, "{\"EntityDiscriminator\":\"Form\",\"Status\":1,\"IsConform\":false,\"Type\":0,\"TemplateId\":\"00000000-0000-0000-0000-000000000000\",\"MustDisplayElementsCode\":false,\"IsSignatureAllowed\":false,\"NbNonConformities\":0," + 
                                  "\"NbPointsOnForm\":0,\"Date\":\"0001-01-01T00:00:00\",\"ModificationDate\":\"0001-01-01T00:00:00\",\"Subject\":\"" + noteBase.Subject + "\",\"IsArchived\":false,\"IsUrgent\":false,\"MeetingNumSeq\":0,\"ProjectNumSeq\":0,\"Code\":\"1.01\","+
                                  "\"IsReadOnly\":false,\"HasAttachment\":false,\"Id\":\"" + noteBase.Id +"\",\"EntityVersion\":0,\"Deleted\":false,\"EntityCreationDate\":\"0001-01-01T00:00:00\",\"EntityModificationDate\":\"0001-01-01T00:00:00\","+
                                  "\"EntityCreationUser\":\"00000000-0000-0000-0000-000000000000\"}");
        }

        [TestCase]
        public void ShouldConvertEntityToCorrectJson()
        {
            Guid leftId = Guid.NewGuid();
            Guid rightId = Guid.NewGuid();
            
            var left = new FilterProperty
            {
                IsConform = true,
                Value = "This is a test left",
                ItemId = leftId
            };
            var right = new FilterProperty
            {
                IsConform = true,
                Value = "This is a test right",
                ItemId = rightId
            };
            var combination = new FilterCombination
            {
                RightFilter = right,
                LeftFilter = left,
                Type = FormFilterType.And
            };

            string jsonTest = JsonConvert.SerializeObject(combination);
            Assert.AreEqual("{\"EntityDiscriminator\":\"FilterCombination\",\"Type\":0,\"RightFilter\":{\"EntityDiscriminator\":\"FilterProperty\",\"IsConform\":true,\"Value\":\"This is a test right\",\"NotApplicable\":false,\"ItemId\":\"" + 
                            rightId + "\",\"Id\":\"" + right.Id + "\",\"EntityVersion\":0,\"Deleted\":false,\"EntityCreationDate\":\"0001-01-01T00:00:00\",\"EntityModificationDate\":\"0001-01-01T00:00:00\",\"EntityCreationUser\":\"00000000-0000-0000-0000-000000000000\"}," + 
                            "\"LeftFilter\":{\"EntityDiscriminator\":\"FilterProperty\",\"IsConform\":true,\"Value\":\"This is a test left\",\"NotApplicable\":false,\"ItemId\":\""+ leftId + 
                            "\",\"Id\":\"" + left.Id + "\",\"EntityVersion\":0,\"Deleted\":false,\"EntityCreationDate\":\"0001-01-01T00:00:00\",\"EntityModificationDate\":\"0001-01-01T00:00:00\",\"EntityCreationUser\":\"00000000-0000-0000-0000-000000000000\"},"+
                            "\"Id\":\"" + combination.Id + "\",\"EntityVersion\":0,\"Deleted\":false,\"EntityCreationDate\":\"0001-01-01T00:00:00\",\"EntityModificationDate\":\"0001-01-01T00:00:00\",\"EntityCreationUser\":\"00000000-0000-0000-0000-000000000000\"}", jsonTest);

        }
    }
}