using System;

namespace StudentManagement
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
        public string Role { get; set; } = "管理员"; // 默认角色
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}