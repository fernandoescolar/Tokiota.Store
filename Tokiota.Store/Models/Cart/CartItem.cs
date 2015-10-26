namespace Tokiota.Store.Models.Cart
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    [Serializable]
    public class CartItem
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public decimal TotalPrice { get { return this.Price * this.Number; } }
    }
}