﻿namespace Unite.Composer.Search.Services.Filters.Constants;

public static class DonorFilterNames
{
    private const string _prefix = "Donor";

    public static readonly string Id = $"{_prefix}.Id";
    public static readonly string ReferenceId = $"{_prefix}.ReferenceId";
    public static readonly string Diagnosis = $"{_prefix}.Diagnosis";
    public static readonly string Gender = $"{_prefix}.Gender";
    public static readonly string Age = $"{_prefix}.Age";
    public static readonly string VitalStatus = $"{_prefix}.VitalStatus";
    public static readonly string ProgressionStatus = $"{_prefix}.ProgressionStatus";
    public static readonly string Therapy = $"{_prefix}.Therapy";

    public static readonly string MtaProtected = $"{_prefix}.MtaProtected";
    public static readonly string WorkPackage = $"{_prefix}.WorkPackage";
}
