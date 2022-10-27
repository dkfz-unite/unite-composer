using System.Runtime.Serialization;

namespace Unite.Composer.Search.Services.Context.Enums;

public enum VariantType
{
    [EnumMember(Value = "SSM")]
    SSM = 1,

    [EnumMember(Value = "CNV")]
    CNV = 2,

    [EnumMember(Value = "SV")]
    SV = 3
}
