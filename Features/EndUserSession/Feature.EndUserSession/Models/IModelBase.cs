using System;
using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Globalization;

namespace Feature.EndUserSession.DataTransferObjects
{
    public interface IModelBase : IComparable<IModelBase>, IEquatable<IModelBase>
    {
        Guid Id { get; set; }

        Language Language { get; set; }

        ItemUri Uri { get; set; }

        string DisplayName { get; set; }

        int Version { get; }

        string Path { get; set; }

        string FullPath { get; set; }

        string Name { get; set; }

        string Url { get; set; }

        Guid TemplateId { get; set; }

        string TemplateName { get; set; }

        IEnumerable<ModelBase> BaseChildren { get; set; }

        string Sortorder { get; set; }

        DateTime UpdatedDate { get; set; }

        string ChildContent { get; set; }

        ModelBase Parent { get; set; }

    }
}