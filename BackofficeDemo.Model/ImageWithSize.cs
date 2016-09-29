using System;
using BackofficeDemo.MongoBase;

namespace BackofficeDemo.Model
{
    public class ImageWithSize: Entity
    {
        public string ImageId { get; set; }

        public Guid ImageSizeGuid { get; set; }
    }
}
