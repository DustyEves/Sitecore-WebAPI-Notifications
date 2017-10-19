using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Converters;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Mvc.Presentation;


namespace Feature.EndUserSession.DataTransferObjects
{
    [SitecoreType(TemplateId = "{1930BBEB-7805-471A-A3BE-4858AC7CF696}", AutoMap = true)]    
    public class ModelBase : IModelBase
    {
        [SitecoreIgnore]
        public Rendering Rendering
        {
            get
            {
                if (RenderingContext.CurrentOrNull == null) return null;

                return RenderingContext.Current.Rendering;
            }
        }

        [SitecoreId, IndexField("_group")]
        public virtual Guid Id { get; set; }

        [SitecoreInfo(SitecoreInfoType.Language), IndexField("_language")]
        public virtual Language Language { get; set; }

        [TypeConverter(typeof(IndexFieldItemUriValueConverter)), XmlIgnore, IndexField("_uniqueid")]
        public virtual ItemUri Uri { get; set; }

        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string DisplayName { get; set; }

        [SitecoreInfo(SitecoreInfoType.Version), IndexField("version")]
        public virtual int Version { get; set; }

        [SitecoreInfo(SitecoreInfoType.Path), IndexField("_path")]
        public virtual string Path { get; set; }

        [IndexField("_fullpath")]
        public virtual string FullPath { get; set; }

        [SitecoreInfo(SitecoreInfoType.Name), IndexField("title")]
        public virtual string Name { get; set; }

        [SitecoreInfo(SitecoreInfoType.Url, UrlOptions = SitecoreInfoUrlOptions.LanguageEmbeddingNever), IndexField("urllink")]
        public virtual string Url { get; set; }

        [SitecoreInfo(SitecoreInfoType.TemplateId), IndexField("_template")]
        public virtual Guid TemplateId { get; set; }

        [SitecoreInfo(SitecoreInfoType.TemplateName), IndexField("_templatename")]
        public virtual string TemplateName { get; set; }

        [SitecoreChildren(IsLazy = true, InferType = true)]
        public virtual IEnumerable<ModelBase> BaseChildren { get; set; }

        [SitecoreField("__Sortorder")]
        public virtual string Sortorder { get; set; }

        [SitecoreField("_updated"), IndexField("_updated")]
        public virtual DateTime UpdatedDate { get; set; }

        [IndexField("childcontent")]
        public virtual string ChildContent { get; set; }

        [SitecoreParent(InferType = true)]
        public virtual ModelBase Parent { get; set; }

        [IndexField("parentid")]        
        public Guid ParentId { get; set; }
        

        //public string GetInnerPlaceholderRenderingParameter()
        //{
        //    if (Rendering != null && Rendering.RenderingItem != null & Rendering.RenderingItem.InnerItem != null)
        //    {
        //        Sitecore.Data.Items.Item itm = Rendering.RenderingItem.InnerItem;
        //        if (itm.Fields[SitecoreConstants.RenderingOptions.InnerPlaceholderNameFieldId] != null)
        //            return itm.Fields[SitecoreConstants.RenderingOptions.InnerPlaceholderNameFieldId].Value;
        //    }
        //    //if (Rendering != null && !string.IsNullOrEmpty(Rendering.RenderingItem.Parameters))
        //    //{
        //    //    NameValueCollection parameters = WebUtil.ParseUrlParameters(Rendering.RenderingItem.Parameters);
        //    //    string val = parameters["Placeholder Name"];
        //    //    if (!string.IsNullOrEmpty(val))
        //    //    {
        //    //        return val;
        //    //    }
        //    //}
        //    return string.Empty;
        //}

        public IEnumerable<T> GetSelectChildren<T>() where T : class
        {
            return BaseChildren != null ? BaseChildren.Where(a => a is T).Cast<T>().ToList() : new List<T>();
        }

        public int CompareTo(IModelBase other)
        {
            Assert.IsNotNull(other, typeof(IModelBase));
            return string.Compare(Name, other.Name, StringComparison.CurrentCulture);
        }

        public bool Equals(IModelBase other)
        {
            return Id.Equals(other.Id) && Version == other.Version && Language == other.Language;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IModelBase)) return false;
            return Equals((IModelBase)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}", DisplayName, TemplateName, Id);
        }
    }
}
