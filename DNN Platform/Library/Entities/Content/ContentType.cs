﻿#region Copyright
// 
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2014
// by DotNetNuke Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion
#region Usings

using System;
using System.Data;
using System.Linq;

using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Modules;

#endregion

namespace DotNetNuke.Entities.Content
{
    /// <summary>
    /// This class exists solely to maintain compatibility between the original VB version
    /// which supported ContentType.ContentType and the c# version which doesn't allow members with
    /// the same naem as their parent type
    /// </summary>
    [Serializable]
    public abstract class ContentTypeMemberNameFixer
    {
        public string ContentType { get; set; }
    }

	/// <summary>
	/// Content type of a content item.
	/// </summary>
	/// <remarks>
	/// Content Types, simply put, are a way of telling the framework what module/functionality is associated with a Content Item. 
	/// Each product (ie. module) that wishes to allow categorization of data (via Taxonomy or Folksonomy) for it's content items
	///  will likely need to create its own content type. 
	/// </remarks>
    [Serializable]
    [TableName("ContentTypes")]
    [PrimaryKey("ContentTypeID")]
    [Cacheable(DataCache.ContentTypesCacheKey, DataCache.ContentTypesCachePriority, DataCache.ContentTypesCacheTimeOut)]
    public class ContentType : ContentTypeMemberNameFixer, IHydratable
    {
        private static ContentType _desktopModule;
        private static ContentType _module;
        private static ContentType _tab;

        private const string DesktopModuleContentTypeName = "DesktopModule";
        private const string ModuleContentTypeName = "Module";
        private const string TabContentTypeName = "Tab";

        public ContentType() : this(Null.NullString)
        {
        }

        public ContentType(string contentType)
        {
            ContentTypeId = Null.NullInteger;
            ContentType = contentType;
            PortalID = Null.NullInteger;
            IsStructured = Null.NullBoolean;
        }

        [IgnoreColumn]
        public static ContentType DesktopModule
	    {
	        get
	        {
	            return _desktopModule ?? (_desktopModule = new ContentTypeController().GetContentTypes().FirstOrDefault(type => type.ContentType ==  DesktopModuleContentTypeName));
	        }
	    }

        [IgnoreColumn]
        public static ContentType Module
	    {
	        get
	        {
	            return _module ?? (_module = new ContentTypeController().GetContentTypes().FirstOrDefault(type => type.ContentType ==  ModuleContentTypeName));
	        }
	    }

        [IgnoreColumn]
        public static ContentType Tab 
        {
            get
            {
                return _tab ?? (_tab = new ContentTypeController().GetContentTypes().FirstOrDefault(type => type.ContentType == TabContentTypeName));
            }
        }

        /// <summary>
		/// Gets or sets the content type id.
		/// </summary>
		/// <value>
		/// The content type id.
		/// </value>
        public int ContentTypeId { get; set; }

        /// <summary>
        /// Gets or sets whether the Content Type is structured
        /// </summary>
        /// <value>
        /// A flag that indicates whether the Content Type is structured.
        /// </value>
        public bool IsStructured { get; set; }

        /// <summary>
        /// Gets or sets the portal id for the Content Type
        /// </summary>
        /// <value>
        /// The portal id.
        /// </value>
        public int PortalID { get; set; }

        public override string ToString()
        {
            return ContentType;
        }

        [Obsolete("Deprecated in DNN 8.0.0.  ContentTypeController methods use DAL2 so IHydratable is no longer needed")]
        [IgnoreColumn]
        public int KeyID
        {
            get { return ContentTypeId; }
            set { ContentTypeId = value; }
        }

        [Obsolete("Deprecated in DNN 8.0.0.  ContentTypeController methods use DAL2 so IHydratable is no longer needed")]
        public void Fill(IDataReader dr)
        {
            ContentTypeId = Null.SetNullInteger(dr["ContentTypeID"]);
            ContentType = Null.SetNullString(dr["ContentType"]);
        }
    }
}