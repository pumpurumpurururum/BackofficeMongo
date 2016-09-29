using System;
using System.Collections.Generic;
using System.Linq;
using BackofficeDemo.Model.Enums;
using BackofficeDemo.Model.Interfaces;
using BackofficeDemo.MongoBase;
using MongoDB.Bson.Serialization.Attributes;
using BaseCms.Manager.IconsForMenu;

namespace BackofficeDemo.Model
{
    public class Order : Entity, ICustomerInfo
    {
        public Guid? CustomerGuid { get; set; }

        public Guid PartnerGuid { get; set; }

        public OrderStatus? Status { get; set; }

        public int SeId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
       
        public string PhoneNumber { get; set; }
        public string PhoneNumberToDial { get; set; }

        public string Address { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public string PartnerName { get; set; }
        public string PartnerAlias { get; set; }

        public string OperatorComments { get; set; }
        
        public PaymentType PaymentType { get; set; }

        public DeliveryTimeType DeliveryTimeType { get; set; }

        public DateTime? DeliveryTime { get; set; }

        public Decimal? ChangeFrom { get; set; }

        public string Comments { get; set; }
        public string ProcessingComments { get; set; }
        //todo: перименовать (тело письма)
        public String SentEmail { get; set; }

        #region Payment
        public bool OnlinePaymentPaid { get; set; }

        public DateTime? OnlinePaymentDateTime { get; set; }

        public string OnlinePaymentTokenId { get; set; }

        public string OnlinePaymentTokenEmail { get; set; }

        public string OnlinePaymentChargeId { get; set; }
        #endregion

        [BsonIgnore]
        public DateTime? DeliveryTimeLocal => DeliveryTime?.ToLocalTime();

        #region ShoppingCart

        public int BonusPercent { get; set; }

        public List<ShoppingCartItem> Items { get; set; }

        public decimal DeliveryRate { get; set; }

        public decimal DeliveryFreeAfter { get; set; }

        [BsonIgnore]
        public decimal DeliveryPrice => NetPrice > DeliveryFreeAfter ? 0 : DeliveryRate;

        [BsonIgnore]
        public Decimal BonusRecieved => (NetPrice/100)*BonusPercent;

        [BsonIgnore]
        public decimal Taxes { get; set; }

        [BsonIgnore]
        public decimal TaxesPrice => (Taxes / 100) * NetPrice;

        [BsonIgnore]
        public decimal NetPrice => Items?.Sum(item => item.Price) ?? 0;

        [BsonIgnore]
        public decimal Price => NetPrice + TaxesPrice + DeliveryPrice;

        [BsonIgnore]
        public decimal PayPrice => Price - BonusApplied;


        public decimal BonusApplied { get; set; }

        [BsonIgnore]
        public int PriceInCents => (int)(PayPrice * 100);

        [BsonIgnore]
        public int Quantity => Items?.Sum(item => item.Quantity) ?? 0;

        public decimal MinimumPrice { get; set; }

        public OrderSource Source { get; set; }

        #endregion

        public string ShoppingCartHtml { get; set; }

        [BsonIgnore]
        public string DeliveryDescription => (DeliveryTimeType == DeliveryTimeType.Asap) ? "As soon as possible" : (DeliveryTimeLocal?.ToString("f") ?? "");

        [BsonIgnore]
        public string PaymentTypeDescription => PaymentType.ToDescriptionString();

        public LikeFlag? LikeFlag { get; set; }

        public int NumberOfPersons { get; set; }

    }
}

