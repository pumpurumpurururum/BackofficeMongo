using System;
using BackofficeDemo.MongoBase;
using BackofficeDemo.Model.Enums;

namespace BackofficeDemo.Model
{
    public class OrderStatusHistory : Entity
    {
        public Guid OrderGuid { get; set; }

        public OrderStatus Status { get; set; }

        public string SmsToCustomerDetails { get; set; }

        public string SmsToCustomerMessageId { get; set; }

        public bool SmsToCustomerRecieved { get; set; }

        public string SmsToCustomerError { get; set; }

        public string SmsToPartherDetails { get; set; }

        public string SmsToPartherId { get; set; }

        public bool SmsToPartherRecieved { get; set; }

        public string EmailToCustomerDetails { get; set; }

        public string EmailToCustomerError { get; set; }

        public string EmailToPartnerDetails { get; set; }

        public string EmailToPartnerError { get; set; }

        
    }
}
