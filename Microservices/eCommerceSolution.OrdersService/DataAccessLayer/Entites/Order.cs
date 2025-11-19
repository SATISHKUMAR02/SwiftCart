using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccessLayer.Entites
{
    public class Order
    {
        [BsonId]

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid _id { get; set; }
        
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid OrderID { get; set; } // this value will be stored as a string

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public Guid UserID { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public DateTime OrderDate { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.Double)]

        public decimal TotalBill { get; set; }

        public List<OrderItem> OrderItems { get; set; }  = new List<OrderItem>();


    }
}
