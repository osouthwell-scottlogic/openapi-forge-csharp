using System;
using System.Collections.Generic;

namespace OpenApiForge
{
    public class Order
    {
        // e.g. 10
        public long? id { get; set; }

        // e.g. 198772
        public long? petId { get; set; }

        // e.g. 7
        public int? quantity { get; set; }

        public DateTime? shipDate { get; set; }

        // Order Status
        // e.g. approved
        public string status { get; set; }

        public bool? complete { get; set; }
    }

    public class Customer
    {
        // e.g. 100000
        public long? id { get; set; }

        // e.g. fehguy
        public string username { get; set; }

        public Address[] address { get; set; }
    }

    public class Address
    {
        // e.g. 437 Lytton
        public string street { get; set; }

        // e.g. Palo Alto
        public string city { get; set; }

        // e.g. CA
        public string state { get; set; }

        // e.g. 94301
        public string zip { get; set; }
    }

    public class Category
    {
        // e.g. 1
        public long? id { get; set; }

        // e.g. Dogs
        public string name { get; set; }
    }

    public class User
    {
        // e.g. 10
        public long? id { get; set; }

        // e.g. theUser
        public string username { get; set; }

        // e.g. John
        public string firstName { get; set; }

        // e.g. James
        public string lastName { get; set; }

        // e.g. john@email.com
        public string email { get; set; }

        // e.g. 12345
        public string password { get; set; }

        // e.g. 12345
        public string phone { get; set; }

        // User Status
        // e.g. 1
        public int? userStatus { get; set; }
    }

    public class Tag
    {
        public long? id { get; set; }

        public string name { get; set; }
    }

    public class Pet
    {
        // e.g. 10
        public long? id { get; set; }

        // e.g. doggie
        public string name { get; set; }

        public Category category { get; set; }

        public string[] photoUrls { get; set; }

        public Tag[] tags { get; set; }

        // pet status in the store
        public string status { get; set; }
    }

    public class ApiResponse
    {
        public int? code { get; set; }

        public string type { get; set; }

        public string message { get; set; }
    }
}