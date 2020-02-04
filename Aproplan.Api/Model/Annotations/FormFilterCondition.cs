using Aproplan.Api.Http.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Model.Annotations
{
    public abstract partial class FormFilterCondition : Entity
    {
        public Guid? FormElementId { get; set; }


        public abstract FormFilterCondition Copy();

        public static void MapVisibleConditionQuestionToItemId(FormFilterCondition condition, Dictionary<Guid, Guid> mapQuestions)
        {
            if (condition != null)
            {
                FilterProperty propertyFilter = condition as FilterProperty;
                if (propertyFilter != null)
                {
                    if (mapQuestions.ContainsKey(propertyFilter.ItemId))
                    {
                        propertyFilter.ItemId = mapQuestions[propertyFilter.ItemId];
                    }
                }
                else
                {
                    FilterCombination filterCombination = condition as FilterCombination;
                    if (filterCombination != null)
                    {
                        MapVisibleConditionQuestionToItemId(filterCombination.LeftFilter, mapQuestions);
                        MapVisibleConditionQuestionToItemId(filterCombination.RightFilter, mapQuestions);
                    }
                }
            }
        }
    }
}
