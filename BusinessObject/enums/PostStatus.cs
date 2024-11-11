using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.enums
{
    public enum PostStatus
    {
        PENDING,
        ACTIVE,
        CLOSED
    }

    public static class PostStatusExtensions
    {
        public static string ToFriendlyString(this PostStatus status)
        {
            return status.ToString().ToLowerInvariant() switch
            {
                "pending" => "PENDING",
                "active" => "ACTIVE",
                "closed" => "CLOSED",
                _ => status.ToString()
            };
        }
    }

}
