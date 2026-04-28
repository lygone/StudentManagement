using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement
{
    public class DatabaseService
    {
        private readonly string connectionString = @"Filename=Students.db;Connection=direct";

        // ---------- 同步写操作 ----------
        public int AddStudent(Student student)
        {
            using (var db = new LiteDatabase(connectionString))
            {
                var col = db.GetCollection<Student>("students");
                return col.Insert(student);
            }
        }

        public bool UpdateStudent(Student student)
        {
            using (var db = new LiteDatabase(connectionString))
            {
                var col = db.GetCollection<Student>("students");
                return col.Update(student);
            }
        }

        public bool DeleteStudent(int id)
        {
            using (var db = new LiteDatabase(connectionString))
            {
                var col = db.GetCollection<Student>("students");
                return col.Delete(id);
            }
        }

        public int DeleteStudents(List<int> ids)
        {
            using (var db = new LiteDatabase(connectionString))
            {
                var col = db.GetCollection<Student>("students");
                return col.DeleteMany(s => ids.Contains(s.Id));
            }
        }

        public int UpdateGradeForStudents(List<int> ids, string newGrade)
        {
            using (var db = new LiteDatabase(connectionString))
            {
                var col = db.GetCollection<Student>("students");
                var students = col.Find(s => ids.Contains(s.Id)).ToList();
                foreach (var s in students)
                {
                    s.Grade = newGrade;
                    s.UpdatedAt = System.DateTime.Now;
                    col.Update(s);
                }
                return students.Count;
            }
        }

        // ---------- 异步查询 ----------
        public Task<List<Student>> GetAllStudentsAsync()
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(connectionString))
                {
                    var col = db.GetCollection<Student>("students");
                    return col.FindAll().ToList();
                }
            });
        }

        public Task<List<string>> GetAllGradesAsync()
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(connectionString))
                {
                    var col = db.GetCollection<Student>("students");
                    return col.FindAll().Select(s => s.Grade).Distinct().ToList();
                }
            });
        }

        // ---------- 筛选 + 分页 ----------
        public Task<List<Student>> FilterStudentsAsync(string keyword = null, string grade = null,
            int? ageMin = null, int? ageMax = null, double? scoreMin = null, double? scoreMax = null,
            int? pageSize = null, int? pageIndex = null)
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(connectionString))
                {
                    var col = db.GetCollection<Student>("students");
                    var all = col.FindAll();
                    if (!string.IsNullOrEmpty(keyword))
                        all = all.Where(s => s.Name.ToLower().Contains(keyword.ToLower()));
                    if (!string.IsNullOrEmpty(grade))
                        all = all.Where(s => s.Grade == grade);
                    if (ageMin.HasValue)
                        all = all.Where(s => s.Age >= ageMin.Value);
                    if (ageMax.HasValue)
                        all = all.Where(s => s.Age <= ageMax.Value);
                    if (scoreMin.HasValue)
                        all = all.Where(s => s.Score >= scoreMin.Value);
                    if (scoreMax.HasValue)
                        all = all.Where(s => s.Score <= scoreMax.Value);

                    var result = all.OrderBy(s => s.Id); // 默认排序
                    if (pageSize.HasValue && pageIndex.HasValue)
                    {
                        int skip = pageIndex.Value * pageSize.Value;
                        return result.Skip(skip).Take(pageSize.Value).ToList();
                    }
                    return result.ToList();
                }
            });
        }

        // ---------- 统计 ----------
        public Task<(int total, double avgAge, double avgScore, Dictionary<string, int> gradeCount)>
            GetStatisticsAsync()
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(connectionString))
                {
                    var col = db.GetCollection<Student>("students");
                    var all = col.FindAll().ToList();
                    int total = all.Count;
                    double avgAge = total > 0 ? all.Average(s => s.Age) : 0;
                    double avgScore = total > 0 ? all.Average(s => s.Score) : 0;
                    var gradeCount = all.GroupBy(s => s.Grade)
                                        .ToDictionary(g => g.Key, g => g.Count());
                    return (total, avgAge, avgScore, gradeCount);
                }
            });
        }

        // 获取总记录数（用于分页计算）
        public Task<int> GetTotalCountAsync(string keyword = null, string grade = null,
            int? ageMin = null, int? ageMax = null, double? scoreMin = null, double? scoreMax = null)
        {
            return Task.Run(() =>
            {
                using (var db = new LiteDatabase(connectionString))
                {
                    var col = db.GetCollection<Student>("students");
                    var all = col.FindAll();
                    if (!string.IsNullOrEmpty(keyword))
                        all = all.Where(s => s.Name.ToLower().Contains(keyword.ToLower()));
                    if (!string.IsNullOrEmpty(grade))
                        all = all.Where(s => s.Grade == grade);
                    if (ageMin.HasValue)
                        all = all.Where(s => s.Age >= ageMin.Value);
                    if (ageMax.HasValue)
                        all = all.Where(s => s.Age <= ageMax.Value);
                    if (scoreMin.HasValue)
                        all = all.Where(s => s.Score >= scoreMin.Value);
                    if (scoreMax.HasValue)
                        all = all.Where(s => s.Score <= scoreMax.Value);
                    return all.Count();
                }
            });
        }

        // ---------- 用户管理 ----------
        public bool AddUser(string username, string password, string role = "管理员")
        {
            using (var db = new LiteDatabase(connectionString))
            {
                var col = db.GetCollection<User>("users");
                // 检查用户名是否已存在
                if (col.Exists(u => u.Username == username))
                    return false;

                var (hash, salt) = PasswordHelper.HashPassword(password);
                var user = new User
                {
                    Username = username,
                    PasswordHash = hash,
                    Salt = salt,
                    Role = role,
                    CreatedAt = DateTime.Now
                };
                col.Insert(user);
                return true;
            }
        }

        public User AuthenticateUser(string username, string password)
        {
            using (var db = new LiteDatabase(connectionString))
            {
                var col = db.GetCollection<User>("users");
                var user = col.FindOne(u => u.Username == username);
                if (user != null && PasswordHelper.VerifyPassword(password, user.Salt, user.PasswordHash))
                    return user;
                return null;
            }
        }

        public List<User> GetAllUsers()
        {
            using (var db = new LiteDatabase(connectionString))
            {
                return db.GetCollection<User>("users").FindAll().ToList();
            }
        }

        public bool DeleteUser(int userId)
        {
            using (var db = new LiteDatabase(connectionString))
            {
                return db.GetCollection<User>("users").Delete(userId);
            }
        }

        public bool HasAnyUser()
        {
            using (var db = new LiteDatabase(connectionString))
            {
                return db.GetCollection<User>("users").Exists(_ => true);
            }
        }
    }
}