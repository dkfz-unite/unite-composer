﻿using System.Runtime.Serialization;

namespace Unite.Composer.Search.Services.Context.Enums;

public enum SpecimenType
{
    [EnumMember(Value = "Tissue")]
    Tissue = 1,

    [EnumMember(Value = "CellLine")]
    CellLine = 2,

    [EnumMember(Value = "Organoid")]
    Organoid = 3,

    [EnumMember(Value = "Xenograft")]
    Xenograft = 4
}
