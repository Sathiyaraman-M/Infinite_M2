using System.ComponentModel;

namespace Infinite.Base.Enums;

public enum UploadType
{
    [Description(@"Images\Assets")]
    Product,

    [Description(@"Images\ProfilePictures")]
    ProfilePicture,

    [Description(@"Documents")]
    Document
}