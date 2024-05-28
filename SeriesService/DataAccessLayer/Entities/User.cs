using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class User
    {
        public string UserId { get; set; }
        public ICollection<CommentLike> CommentLikes { get; set; }
        public ICollection<CommentDislike> CommentDislikes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
