using Unite.Composer.Search.Services.Context.Enums;

namespace Unite.Composer.Search.Services.Context
{
    public class ImageSearchContext
    {
        public ImageType? ImageType { get; set; }


        public ImageSearchContext()
        {
        }

        public ImageSearchContext(ImageType imageType) : base()
        {
            ImageType = imageType;
        }
    }
}
