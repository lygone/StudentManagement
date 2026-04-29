using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

class SeedData
{
    static readonly string[] Surnames = {"王","李","张","刘","陈","杨","赵","黄","周","吴",
        "徐","孙","胡","朱","高","林","何","郭","马","罗","梁","宋","郑","谢","韩","唐",
        "冯","于","董","萧","程","曹","袁","邓","许","傅","沈","曾","彭","吕","苏","卢",
        "蒋","蔡","贾","丁","魏","薛","叶","阎","余","潘","杜","戴","夏","钟","汪","田","任","姜"};

    static readonly string[] MaleNames = {"伟","强","磊","洋","勇","军","杰","涛","明","超",
        "华","浩","鑫","鹏","飞","亮","平","辉","龙","峰","刚","斌","宇","轩","文","博",
        "毅","恒","瑞","佳","子","志","国","建","海","宁","康","健","翔","然","睿","晨",
        "阳","旭","天","乐","逸","诚","达","新","力","学","德","成","安","宏","世","思","远","昌"};

    static readonly string[] FemaleNames = {"芳","敏","静","丽","婷","雪","琳","玲","慧","洁",
        "娜","秀","娟","英","华","萍","红","梅","兰","霞","艳","燕","晶","瑶","佳","怡",
        "倩","蓉","莉","颖","文","艺","欣","思","婉","薇","蕾","菲","妍","璐","梦","云",
        "雨","月","诗","灵","曼","丹","琪","晓","君","昕","彤","凝","若","欢","巧","真"};

    static readonly string[] Grades = {"一年级","二年级","三年级","四年级","五年级","六年级",
        "初一","初二","初三","高一","高二","高三"};

    static void HashPassword(string password, out string hash, out string salt)
    {
        byte[] saltBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(saltBytes);
        byte[] hashBytes;
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000, HashAlgorithmName.SHA256))
            hashBytes = pbkdf2.GetBytes(32);
        hash = Convert.ToBase64String(hashBytes);
        salt = Convert.ToBase64String(saltBytes);
    }

    class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Grade { get; set; }
        public double Score { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    static void Main()
    {
        var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Students.db");
        if (File.Exists(dbPath)) { File.Delete(dbPath); Console.WriteLine("[OK] Old database deleted"); }

        Console.WriteLine("========================================");
        Console.WriteLine("  StudentManagement - Seed Data");
        Console.WriteLine("========================================");

        using (var db = new LiteDatabase(string.Format("Filename={0};Connection=direct", dbPath)))
        {
            Console.WriteLine("[1/3] Creating admin...");
            var users = db.GetCollection<User>("users");
            string hash, salt;
            HashPassword("199610", out hash, out salt);
            var admin = new User
            {
                Username = "lygone",
                PasswordHash = hash,
                Salt = salt,
                Role = "管理员",
                CreatedAt = DateTime.Now
            };
            users.Insert(admin);
            Console.WriteLine("      admin: lygone / 199610");

            Console.WriteLine("[2/3] Generating 1000 students...");
            var students = db.GetCollection<Student>("students");
            var batch = new List<Student>();
            var rng = new Random();

            for (int i = 0; i < 1000; i++)
            {
                bool isMale = rng.Next(2) == 0;
                string name = Surnames[rng.Next(Surnames.Length)] +
                    (isMale ? MaleNames[rng.Next(MaleNames.Length)] : FemaleNames[rng.Next(FemaleNames.Length)]);
                var now = DateTime.Now.AddDays(-rng.Next(0, 365)).AddHours(-rng.Next(0, 24));
                var s = new Student
                {
                    Name = name,
                    Age = rng.Next(6, 20),
                    Grade = Grades[rng.Next(Grades.Length)],
                    Score = Math.Round(rng.NextDouble() * 60 + 40, 1),
                    CreatedAt = now,
                    UpdatedAt = now
                };
                batch.Add(s);

                if (batch.Count >= 100) { students.InsertBulk(batch); batch.Clear(); }
                if ((i + 1) % 100 == 0) Console.WriteLine("      {0}/1000", i + 1);
            }
            if (batch.Count > 0) students.InsertBulk(batch);
            Console.WriteLine("      Done: 1000 students");

            Console.WriteLine("[3/3] Verifying...");
            var count = students.Count();
            double total = 0; int cnt = 0;
            var gradeDict = new Dictionary<string, int>();
            foreach (var s in students.FindAll())
            {
                total += s.Score; cnt++;
                if (!gradeDict.ContainsKey(s.Grade)) gradeDict[s.Grade] = 0;
                gradeDict[s.Grade]++;
            }
            double avg = cnt > 0 ? Math.Round(total / cnt, 2) : 0;

            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("  Seed completed!");
            Console.WriteLine("========================================");
            Console.WriteLine("  Admin   : lygone / 199610");
            Console.WriteLine("  Students: {0}", count);
            Console.WriteLine("  AvgScore: {0}", avg);
            Console.Write("  Grades  : ");
            foreach (var kv in gradeDict) Console.Write("{0}({1}) ", kv.Key, kv.Value);
            Console.WriteLine();
            Console.WriteLine("  DB path : {0}", dbPath);
            Console.WriteLine();
            Console.WriteLine("  Run bin\\Debug\\StudentManagement.exe");
            Console.WriteLine("========================================");
        }
    }
}
