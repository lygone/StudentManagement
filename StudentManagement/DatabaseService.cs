using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement
{
    public class DatabaseService
    {
        private static readonly Lazy<LiteDatabase> _db = new Lazy<LiteDatabase>(() =>
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Students.db");
            return new LiteDatabase($"Filename={dbPath};Connection=direct");
        });

        private LiteDatabase Database => _db.Value;

        private ILiteCollection<Student> Students => Database.GetCollection<Student>("students");
        private ILiteCollection<User> Users => Database.GetCollection<User>("users");

        // ========== 安全停机 ==========
        public static void Shutdown()
        {
            if (_db.IsValueCreated)
                _db.Value.Dispose();
        }

        // ========== Student CRUD ==========
        public Student GetStudentById(int id)
        {
            return Students.FindById(id);
        }

        public int AddStudent(Student student)
        {
            return Students.Insert(student);
        }

        public int AddStudents(List<Student> students)
        {
            return Students.InsertBulk(students);
        }

        public bool UpdateStudent(Student student)
        {
            student.UpdatedAt = DateTime.Now;
            return Students.Update(student);
        }

        public bool DeleteStudent(int id)
        {
            return Students.Delete(id);
        }

        public int DeleteStudents(List<int> ids)
        {
            return Students.DeleteMany(s => ids.Contains(s.Id));
        }

        public int UpdateGradeForStudents(List<int> ids, string newGrade)
        {
            var students = Students.Find(s => ids.Contains(s.Id)).ToList();
            foreach (var s in students)
            {
                s.Grade = newGrade;
                s.UpdatedAt = DateTime.Now;
                Students.Update(s);
            }
            return students.Count;
        }

        // ========== 公共筛选逻辑 ==========
        private ILiteQueryable<Student> BuildFilterQuery(string keyword, string grade,
            int? ageMin, int? ageMax, double? scoreMin, double? scoreMax)
        {
            var query = Students.Query();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(s => s.Name.Contains(keyword));
            if (!string.IsNullOrEmpty(grade))
                query = query.Where(s => s.Grade == grade);
            if (ageMin.HasValue)
                query = query.Where(s => s.Age >= ageMin.Value);
            if (ageMax.HasValue)
                query = query.Where(s => s.Age <= ageMax.Value);
            if (scoreMin.HasValue)
                query = query.Where(s => s.Score >= scoreMin.Value);
            if (scoreMax.HasValue)
                query = query.Where(s => s.Score <= scoreMax.Value);
            return query;
        }

        // ========== 异步查询 ==========
        public Task<List<Student>> GetAllStudentsAsync()
        {
            return Task.Run(() => Students.FindAll().ToList());
        }

        public Task<List<string>> GetAllGradesAsync()
        {
            return Task.Run(() => Students.FindAll().Select(s => s.Grade).Distinct().ToList());
        }

        public Task<List<Student>> FilterStudentsAsync(string keyword = null, string grade = null,
            int? ageMin = null, int? ageMax = null, double? scoreMin = null, double? scoreMax = null,
            int? pageSize = null, int? pageIndex = null)
        {
            return Task.Run(() =>
            {
                var query = BuildFilterQuery(keyword, grade, ageMin, ageMax, scoreMin, scoreMax);
                var result = query.OrderBy(s => s.Id);
                if (pageSize.HasValue && pageIndex.HasValue)
                {
                    int skip = pageIndex.Value * pageSize.Value;
                    return result.Skip(skip).Limit(pageSize.Value).ToList();
                }
                return result.ToList();
            });
        }

        public Task<int> GetTotalCountAsync(string keyword = null, string grade = null,
            int? ageMin = null, int? ageMax = null, double? scoreMin = null, double? scoreMax = null)
        {
            return Task.Run(() =>
            {
                var query = BuildFilterQuery(keyword, grade, ageMin, ageMax, scoreMin, scoreMax);
                return query.Count();
            });
        }

        public Task<(int total, double avgAge, double avgScore, Dictionary<string, int> gradeCount)>
            GetStatisticsAsync(string keyword = null, string grade = null,
            int? ageMin = null, int? ageMax = null, double? scoreMin = null, double? scoreMax = null)
        {
            return Task.Run(() =>
            {
                var query = BuildFilterQuery(keyword, grade, ageMin, ageMax, scoreMin, scoreMax);
                var all = query.ToList();
                int total = all.Count;
                double avgAge = total > 0 ? all.Average(s => s.Age) : 0;
                double avgScore = total > 0 ? all.Average(s => s.Score) : 0;
                var gradeCount = all.GroupBy(s => s.Grade)
                                    .ToDictionary(g => g.Key, g => g.Count());
                return (total, avgAge, avgScore, gradeCount);
            });
        }

        public Task<Dictionary<string, double>> GetScoreDistributionAsync()
        {
            return Task.Run(() =>
            {
                var all = Students.FindAll().ToList();
                var dist = new Dictionary<string, double>
                {
                    ["0-59"] = 0,
                    ["60-69"] = 0,
                    ["70-79"] = 0,
                    ["80-89"] = 0,
                    ["90-100"] = 0
                };
                foreach (var s in all)
                {
                    if (s.Score < 60) dist["0-59"]++;
                    else if (s.Score < 70) dist["60-69"]++;
                    else if (s.Score < 80) dist["70-79"]++;
                    else if (s.Score < 90) dist["80-89"]++;
                    else dist["90-100"]++;
                }
                return dist;
            });
        }

        public Task<(int today, int thisMonth, double maxScore, string maxName, string maxGrade, double minScore, string minName, string minGrade)> GetQuickStatsAsync()
        {
            return Task.Run(() =>
            {
                var all = Students.FindAll().ToList();
                var now = DateTime.Now;
                int today = all.Count(s => s.CreatedAt.Date == now.Date);
                int thisMonth = all.Count(s => s.CreatedAt.Year == now.Year && s.CreatedAt.Month == now.Month);
                double maxScore = 0; string maxName = ""; string maxGrade = "";
                double minScore = 0; string minName = ""; string minGrade = "";
                if (all.Count > 0)
                {
                    var top = all.OrderByDescending(s => s.Score).First();
                    var bottom = all.OrderBy(s => s.Score).First();
                    maxScore = top.Score; maxName = top.Name; maxGrade = top.Grade;
                    minScore = bottom.Score; minName = bottom.Name; minGrade = bottom.Grade;
                }
                return (today, thisMonth, maxScore, maxName, maxGrade, minScore, minName, minGrade);
            });
        }

        // ========== User管理 ==========
        public bool AddUser(string username, string password, string role)
        {
            var col = Users;
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

        public User AuthenticateUser(string username, string password)
        {
            var col = Users;
            var user = col.FindOne(u => u.Username == username);
            if (user == null) return null;

            if (PasswordHelper.VerifyPassword(password, user.Salt, user.PasswordHash))
                return user;

            if (PasswordHelper.VerifyLegacyPassword(password, user.Salt, user.PasswordHash))
            {
                var (newHash, newSalt) = PasswordHelper.HashPassword(password);
                user.PasswordHash = newHash;
                user.Salt = newSalt;
                col.Update(user);
                return user;
            }

            return null;
        }

        public List<User> GetAllUsers()
        {
            return Users.FindAll().ToList();
        }

        public bool DeleteUser(int userId)
        {
            return Users.Delete(userId);
        }

        public bool UpdateUserPassword(int userId, string newPassword)
        {
            var user = Users.FindById(userId);
            if (user == null) return false;
            var (hash, salt) = PasswordHelper.HashPassword(newPassword);
            user.PasswordHash = hash;
            user.Salt = salt;
            return Users.Update(user);
        }

        public bool VerifyUserPassword(int userId, string password)
        {
            var user = Users.FindById(userId);
            if (user == null) return false;
            return PasswordHelper.VerifyPassword(password, user.Salt, user.PasswordHash)
                || PasswordHelper.VerifyLegacyPassword(password, user.Salt, user.PasswordHash);
        }

        public bool HasAnyUser()
        {
            return Users.Exists(_ => true);
        }
    }
}
