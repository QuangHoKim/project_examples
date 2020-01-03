using System;

namespace Vnn88.DataAccess.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public string UserName { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
