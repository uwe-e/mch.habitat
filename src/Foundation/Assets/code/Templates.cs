﻿namespace Sitecore.Foundation.Assets
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct RenderingAssets
        {
            public static readonly ID ID = new ID("{7CEAC341-B953-4C69-B907-EE44302BF6AE}");

            public struct Fields
            {
                public static readonly ID ScriptFiles = new ID("{E514A1EB-DDBA-44F7-8528-82CA2280F778}");
                public static readonly ID StylingFiles = new ID("{4867D192-326A-4AA4-81EF-EA430E224AFF}");
                public static readonly ID InlineScript = new ID("{11421560-0BCB-403A-B099-8595C34341FD}");
                public static readonly ID InlineStyling = new ID("{FD0DEC96-B220-4196-B544-68B11EEE727A}");
            }
        }

        public struct PageAssets
        {
            public static readonly ID ID = new ID("{91962B60-25F6-428F-8D10-02AA1E49D6A5}");

            public struct Fields
            {
                public static readonly ID JavascriptCodeTop = new ID("{D79D9DDD-2774-42F4-94C3-50C892F6E13D}");
                public static readonly ID JavascriptCodeBottom = new ID("{B3BA9EA9-D0A1-49DF-9F4B-28FA5D353DC8}");
                public static readonly ID CssCode = new ID("{06A96EFC-F2E5-45C3-A7DC-4DDDFA366CC0}");
                public static readonly ID InheritAssets = new ID("{F19E8A50-9950-4861-9E66-9598A1898E71}");
            }
        }

        public struct HasTheme
        {
            public static readonly ID ID = new ID("{5B6F8720-3A93-4DA1-92A0-C3E85E01219A}");

            public struct Fields
            {
                public static readonly ID Theme = new ID("{53B5AF0A-265F-4E60-B2B2-4576CE0BECCF}");
            }
        }

        public struct AssetFolder
        {
            public static readonly ID ID = new ID("{4ACE1EA6-EC7D-433C-A491-30B373709F3B}");
        }

        public struct AssetBase
        {
            public struct Fields
            {
                public static readonly ID Comment = new ID("{7C944A28-228E-4F11-B751-402F90EC6190}");
                public static readonly ID InheritAsset = new ID("{60B386D2-18F4-430E-A603-863DE0104AB1}");
            }
        }
        public struct ScriptAsset
        {
            public static readonly ID ID = new ID("{A1989802-564A-4680-85F8-D7E397EE4E80}");
            public struct Fields
            {
                public static readonly ID Comment = AssetBase.Fields.Comment;
                public static readonly ID InheritAsset = AssetBase.Fields.InheritAsset;
                public static readonly ID Src = new ID("{A27DC786-3472-49CE-8398-A490413EC63A}");
                public static readonly ID Code = new ID("{7DC02058-6DE9-4A18-ACFD-E46131A99D48}");
                public static readonly ID ScriptLocation = new ID("{C1F46FB2-8124-4C67-8034-3A4E9F53F3F1}");
            }
        }
        public struct StyleAsset
        {
            public static readonly ID ID = new ID("{43705D9E-9039-4E6D-8BB1-9CE45501DD37}");
            public struct Fields
            {
                public static readonly ID Comment = AssetBase.Fields.Comment;
                public static readonly ID InheritAsset = AssetBase.Fields.InheritAsset;
                public static readonly ID Href = new ID("{CD784129-580B-4BB9-8770-085243937369}");
                public static readonly ID InlineStyle = new ID("{D9ED0C12-D1CF-411D-AACC-AA98D5CB35A8}");
            }
        }
        public struct RawAssetBase
        {
            public static readonly ID ID = new ID("{AABACA43-875A-4831-8A3C-5156AD465F36}");
            public struct Fields
            {
                public static readonly ID Text = new ID("{86C69004-9D89-4777-8A14-249AC8A72F5D}");
            }
        }
        public struct PlainTextAsset
        {
            public static readonly ID ID = new ID("{410A5B40-69BE-4BCD-A11B-7985047458B6}");
            public struct Fields
            {
                public static readonly ID Comment = AssetBase.Fields.Comment;
                public static readonly ID InheritAsset = AssetBase.Fields.InheritAsset;
                public static readonly ID Text = RawAssetBase.Fields.Text;
            }
        } 
    }
}