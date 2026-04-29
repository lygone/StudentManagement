# 学生管理系统 - 初始化测试数据脚本
# 运行后将自动创建 Students.db，包含:
#   管理员: lygone / 199610
#   1000 个随机学生数据
#
# 用法: 在项目根目录右键 → "使用 PowerShell 运行"，或在终端执行:
#       powershell -ExecutionPolicy Bypass -File init-data.ps1

$ErrorActionPreference = "Stop"
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$dbPath = Join-Path $scriptDir "StudentManagement\bin\Debug\Students.db"
$liteDbPath = Join-Path $scriptDir "packages\LiteDB.5.0.21\lib\net45\LiteDB.dll"

# 确保目录存在
$dbDir = Split-Path $dbPath -Parent
if (-not (Test-Path $dbDir)) { New-Item -ItemType Directory -Path $dbDir -Force | Out-Null }

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  学生管理系统 - 测试数据初始化" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

# 加载 LiteDB
if (-not (Test-Path $liteDbPath)) {
    Write-Host "[错误] 未找到 LiteDB.dll，请先执行 NuGet 还原" -ForegroundColor Red
    Write-Host "       路径: $liteDbPath" -ForegroundColor Red
    pause
    exit 1
}

[System.Reflection.Assembly]::LoadFrom($liteDbPath) | Out-Null
Write-Host "[OK] LiteDB 已加载" -ForegroundColor Green

# 删除旧数据库
if (Test-Path $dbPath) {
    Remove-Item $dbPath -Force
    Write-Host "[OK] 已删除旧数据库" -ForegroundColor Yellow
}

# 密码哈希 (PBKDF2, 100000 iterations, SHA256)
function HashPassword($password) {
    $salt = New-Object byte[] 32
    $rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
    $rng.GetBytes($salt)
    $rng.Dispose()

    $pbkdf2 = New-Object System.Security.Cryptography.Rfc2898DeriveBytes(
        $password, $salt, 100000,
        [System.Security.Cryptography.HashAlgorithmName]::SHA256
    )
    $hash = $pbkdf2.GetBytes(32)
    $pbkdf2.Dispose()

    return @{
        Hash = [Convert]::ToBase64String($hash)
        Salt = [Convert]::ToBase64String($salt)
    }
}

# 中文姓名库
$surnames = @("王","李","张","刘","陈","杨","赵","黄","周","吴","徐","孙","胡","朱","高","林","何","郭","马","罗",
    "梁","宋","郑","谢","韩","唐","冯","于","董","萧","程","曹","袁","邓","许","傅","沈","曾","彭","吕",
    "苏","卢","蒋","蔡","贾","丁","魏","薛","叶","阎","余","潘","杜","戴","夏","钟","汪","田","任","姜")

$maleNames = @("伟","强","磊","洋","勇","军","杰","涛","明","超","华","浩","鑫","鹏","飞","亮","平","辉","龙","峰",
    "刚","斌","宇","轩","文","博","毅","恒","瑞","佳","子","志","国","建","海","宁","康","健","翔","然",
    "睿","晨","阳","旭","天","乐","逸","诚","达","新","力","学","德","成","安","宏","世","思","远","昌")

$femaleNames = @("芳","敏","静","丽","婷","雪","琳","玲","慧","洁","娜","秀","娟","英","华","萍","红","梅","兰","霞",
    "艳","燕","晶","瑶","佳","怡","倩","蓉","莉","颖","文","艺","欣","怡","思","婉","薇","蕾","菲","妍",
    "璐","梦","云","雨","月","诗","灵","曼","丹","琪","晓","君","昕","彤","瑶","凝","若","欢","巧","真")

$grades = @("一年级","二年级","三年级","四年级","五年级","六年级","初一","初二","初三","高一","高二","高三")
$rng = New-Object System.Random

Write-Host "[1/3] 创建管理员账号..." -ForegroundColor White

# 打开数据库
$db = New-Object LiteDB.LiteDatabase("Filename=$dbPath;Connection=direct")
$users = $db.GetCollection("users", [LiteDB.BsonAutoId]::Int32)

$pw = HashPassword "199610"
$admin = New-Object LiteDB.BsonDocument
$admin["_id"] = 1
$admin["Username"] = "lygone"
$admin["PasswordHash"] = $pw.Hash
$admin["Salt"] = $pw.Salt
$admin["Role"] = "管理员"
$admin["CreatedAt"] = [DateTime]::Now
$users.Insert($admin) | Out-Null

Write-Host "       管理员: lygone / 199610" -ForegroundColor Green

Write-Host "[2/3] 生成 1000 个学生数据..." -ForegroundColor White

$students = $db.GetCollection("students", [LiteDB.BsonAutoId]::Int32)
$batch = New-Object System.Collections.Generic.List[LiteDB.BsonDocument]

for ($i = 0; $i -lt 1000; $i++) {
    $isMale = ($rng.Next(2) -eq 0)
    $surname = $surnames[$rng.Next($surnames.Count)]
    $givenName = if ($isMale) { $maleNames[$rng.Next($maleNames.Count)] } else { $femaleNames[$rng.Next($femaleNames.Count)] }
    $name = $surname + $givenName

    $age = $rng.Next(6, 20)
    $grade = $grades[$rng.Next($grades.Count)]
    $score = [Math]::Round($rng.NextDouble() * 60 + 40, 1)  # 40~100
    $now = [DateTime]::Now.AddDays(-$rng.Next(0, 365)).AddHours(-$rng.Next(0, 24))

    $doc = New-Object LiteDB.BsonDocument
    $doc["Name"] = $name
    $doc["Age"] = $age
    $doc["Grade"] = $grade
    $doc["Score"] = $score
    $doc["CreatedAt"] = $now
    $doc["UpdatedAt"] = $now
    $batch.Add($doc)

    if ($batch.Count -ge 100) {
        $students.InsertBulk($batch) | Out-Null
        $batch.Clear()
        Write-Progress -Activity "正在生成学生数据" -Status "$($i+1)/1000" -PercentComplete (($i+1)/10)
    }
}

if ($batch.Count -gt 0) {
    $students.InsertBulk($batch) | Out-Null
}

Write-Progress -Activity "正在生成学生数据" -Completed
Write-Host "       已生成 1000 名学生" -ForegroundColor Green

Write-Host "[3/3] 验证数据..." -ForegroundColor White
$adminUser = $users.FindOne([LiteDB.Query]::EQ("Username", "lygone"))
$studentCount = $students.Count()
$gradeDist = $students.FindAll() | Group-Object { $_.Get("Grade").AsString } | ForEach-Object { "$($_.Name)($($_.Count)人)" }
$avgScore = [Math]::Round(($students.FindAll() | Measure-Object -Property Score -Average).Average, 2)

$db.Dispose()

# 重新打开验证
$db = New-Object LiteDB.LiteDatabase("Filename=$dbPath;Connection=direct;ReadOnly=true")
$users = $db.GetCollection("users", [LiteDB.BsonAutoId]::Int32)
$students = $db.GetCollection("students", [LiteDB.BsonAutoId]::Int32)

$adminOk = $users.Exists([LiteDB.Query]::EQ("Username", "lygone"))
$studentCount = $students.Count()

# 计算平均成绩
$total = 0.0; $count = 0
foreach ($s in $students.FindAll()) {
    $total += $s["Score"].AsDouble
    $count++
}
$avgScore = if ($count -gt 0) { [Math]::Round($total / $count, 2) } else { 0 }

# 班级分布
$gradeMap = @{}
foreach ($s in $students.FindAll()) {
    $g = $s["Grade"].AsString
    if (-not $gradeMap[$g]) { $gradeMap[$g] = 0 }
    $gradeMap[$g]++
}
$gradeStr = ($gradeMap.GetEnumerator() | Sort-Object Name | ForEach-Object { "$($_.Key)($($_.Value)人)" }) -join " "
$db.Dispose()

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  初始化完成！" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  管理员账号 : lygone" -ForegroundColor Yellow
Write-Host "  管理员密码 : 199610" -ForegroundColor Yellow
Write-Host "  学生总数   : $studentCount" -ForegroundColor Yellow
Write-Host "  平均成绩   : $avgScore" -ForegroundColor Yellow
Write-Host "  班级分布   : $gradeStr" -ForegroundColor Yellow
Write-Host "  数据库位置 : $dbPath" -ForegroundColor Gray
Write-Host ""
Write-Host "  现在可以运行 bin\Debug\StudentManagement.exe" -ForegroundColor White
Write-Host "========================================" -ForegroundColor Cyan

pause
