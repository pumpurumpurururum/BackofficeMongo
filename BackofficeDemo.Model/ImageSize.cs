using BaseCms.Common.Attributes;
using BackofficeDemo.Model.Metadata;
using BackofficeDemo.MongoBase;

namespace BackofficeDemo.Model
{

    [CmsMetadataType(typeof(ImageSizeMetadata),
    CollectionTitleSingular = "Image Size",
    CollectionTitlePlural = "Image Sizes",
    CustomEditView = "EditWithTabs",
    TabsWithBlocks = "Main;Tech Info")]
    public class ImageSize : Entity
    {
        public string Key { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
