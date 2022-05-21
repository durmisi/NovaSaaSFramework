using System;

namespace Nova.SaaS.Admin.Api.Data.Models
{
    public class Value
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }
    }
}