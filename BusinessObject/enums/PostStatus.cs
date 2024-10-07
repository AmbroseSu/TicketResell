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
        OPENING,
        REJECTED,
        CLOSED
    }

    public static class PostStatusExtensions
    {
        public static string ToFriendlyString(this PostStatus status)
        {
            return status.ToString().ToLowerInvariant() switch
            {
                "pending" => "PENDING",
                "opening" => "OPENING",
                "rejected" => "REJECTED",
                "closed" => "CLOSED",
                _ => status.ToString()
            };
        }
    }

}
