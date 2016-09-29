using System;
using BackofficeDemo.Model.Enums;
using BackofficeDemo.MongoBase;

namespace BackofficeDemo.Model
{
    public class Review: Entity
    {
        public Guid PartnerGuid { get; set; }
        public Guid? OrderGuid { get; set; }
        public string Text { get; set; }
        public Guid? CustomerGuid { get; set; }
        public bool Approved { get; set; }
        public LikeFlag? LikeFlag { get; set; }
    }
}
