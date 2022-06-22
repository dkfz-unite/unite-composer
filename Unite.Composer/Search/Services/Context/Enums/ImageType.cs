using System.Runtime.Serialization;

namespace Unite.Composer.Search.Services.Context.Enums;

public enum ImageType
{
    [EnumMember(Value = "MRI")]
    MRI = 1,

    [EnumMember(Value = "CT")]
    CT = 2
}
