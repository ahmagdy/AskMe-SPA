using Microsoft.AspNetCore.Identity;
using System;

namespace Aspcorespa.Models
{
    public class UserEntity : IdentityUser
    {
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string ImageUrl { get; set; }

        public virtual DateTime CreatedAt { get; set; }


    }
}
