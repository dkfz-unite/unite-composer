using Microsoft.AspNetCore.Mvc;
using Unite.Indices.Entities.Basic.Images.Constants;
using Unite.Indices.Entities.Basic.Specimens.Constants;

namespace Unite.Composer.Web.Controllers.Domain;

public abstract class DomainController : Controller
{
    private static readonly StringComparison _comparison = StringComparison.InvariantCultureIgnoreCase;


    protected async Task<IActionResult> OkAsync<T>(T resource)
    {
        var responce = Ok(resource);

        return await Task.FromResult(responce);
    }


    protected static string[] DetectImageType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return null;
        else if (type.Equals(ImageType.MR, _comparison))
            return [ImageType.MR];
        else if (type.Equals(ImageType.CT, _comparison))
            return [ImageType.CT];
        else
            throw new NotSupportedException($"Image type {type} is not supported.");
    }

    protected static string[] DetectSpecimenType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return null;
        else if (type.Equals(SpecimenType.Material, _comparison))
            return [SpecimenType.Material];
        else if (type.Equals(SpecimenType.Line, _comparison))
            return [SpecimenType.Line];
        else if (type.Equals(SpecimenType.Organoid, _comparison))
            return [SpecimenType.Organoid];
        else if (type.Equals(SpecimenType.Xenograft, _comparison))
            return [SpecimenType.Xenograft];
        else
            throw new NotSupportedException($"Specimen type {type} is not supported."); 
    }

    protected static Unite.Data.Entities.Images.Enums.ImageType ConvertImageType(string type)
    {
        if (string.Equals(type, ImageType.MR, _comparison))
            return Unite.Data.Entities.Images.Enums.ImageType.MR;
        else if (string.Equals(type, ImageType.CT, _comparison))
            return Unite.Data.Entities.Images.Enums.ImageType.CT;
        else
            throw new NotSupportedException($"Image type {type} is not supported.");
    }

    protected static Unite.Data.Entities.Specimens.Enums.SpecimenType ConvertSpecimenType(string type)
    {
        if (string.Equals(type, SpecimenType.Material, _comparison))
            return Unite.Data.Entities.Specimens.Enums.SpecimenType.Material;
        else if (string.Equals(type, SpecimenType.Line, _comparison))
            return Unite.Data.Entities.Specimens.Enums.SpecimenType.Line;
        else if (string.Equals(type, SpecimenType.Organoid, _comparison))
            return Unite.Data.Entities.Specimens.Enums.SpecimenType.Organoid;
        else if (string.Equals(type, SpecimenType.Xenograft, _comparison))
            return Unite.Data.Entities.Specimens.Enums.SpecimenType.Xenograft;
        else
            throw new NotSupportedException($"Specimen type {type} is not supported.");
    }
}
