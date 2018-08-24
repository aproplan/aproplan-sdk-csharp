using System;
using System.Collections.Generic;
using System.Text;

namespace Aproplan.Api.Http.Utils
{
    public enum FilterComparisonType
    {
        Eq, NotEq, Gt, Ge, Lt, Le, In
    }

    public enum FilterUnaryType
    {
        IsFalse, IsTrue, IsNull, IsNotNull
    }

    public abstract class Filter
    {
        public static Filter operator &(Filter a, Filter b)
        {
            return And(a, b);
        }
        public static Filter operator |(Filter a, Filter b)
        {
            return Or(a, b);
        }

        // Static constructor
        public static Filter And(Filter a, Filter b) { return new CombinationFilter(a, b, FilterCombinationType.And); }
        public static Filter Or(Filter a, Filter b) { return new CombinationFilter(a, b, FilterCombinationType.Or); }
        public static Filter Eq(string propertyPath, object value) { return new ComparisonFilter(FilterComparisonType.Eq, value, propertyPath); }
        public static Filter NotEq(string propertyPath, object value) { return new ComparisonFilter(FilterComparisonType.NotEq, value, propertyPath); }
        public static Filter Gt(string propertyPath, object value) { return new ComparisonFilter(FilterComparisonType.Gt, value, propertyPath); }
        public static Filter Ge(string propertyPath, object value) { return new ComparisonFilter(FilterComparisonType.Ge, value, propertyPath); }
        public static Filter Lt(string propertyPath, object value) { return new ComparisonFilter(FilterComparisonType.Lt, value, propertyPath); }
        public static Filter Le(string propertyPath, object value) { return new ComparisonFilter(FilterComparisonType.Le, value, propertyPath); }
        public static Filter In(string propertyPath, object[] value) { return new ComparisonFilter(FilterComparisonType.In, value, propertyPath); }
        public static Filter IsFalse(string propertyPath) { return new UnaryFilter(FilterUnaryType.IsFalse, propertyPath); }
        public static Filter IsTrue(string propertyPath) { return new UnaryFilter(FilterUnaryType.IsTrue, propertyPath); }
        public static Filter IsNull(string propertyPath) { return new UnaryFilter(FilterUnaryType.IsNull, propertyPath); }
        public static Filter IsNotNull(string propertyPath) { return new UnaryFilter(FilterUnaryType.IsNotNull, propertyPath); }
    }
}
