using System;
using System.Collections.Generic;
using System.Linq;
using Procurement.ViewModel.Filters;

public class AndFilter : IFilter
{
    public FilterGroup Group
    {
        get { return FilterGroup.Default; }
    }

    List<IFilter> filters;

    public AndFilter(params IFilter[] filters)
    {
        this.filters = new List<IFilter>();
        this.filters.AddRange(filters);
    }

    public bool CanFormCategory
    {
        get { return false; }
    }

    public string Keyword
    {
        get { return string.Join(Environment.NewLine, filters.Select(f => f.Keyword).ToArray()); }
    }

    public string Help
    {
        get { return string.Join(Environment.NewLine, filters.Select(f => f.Help).ToArray()); }
    }

    public bool Applicable(POEApi.Model.Item item)
    {
        return filters.All(filter => filter.Applicable(item));
    }
}