# 学生管理系统

一个基于 **C# WinForms (.NET Framework 4.8) + LiteDB** 的桌面端学生信息管理系统，练手项目。

## 功能

| 模块 | 说明 |
|------|------|
| **登录/用户管理** | 管理员账号登录，支持多用户、修改密码、删除用户 |
| **学生 CRUD** | 添加、编辑、删除学生信息（姓名/年龄/班级/成绩） |
| **筛选搜索** | 按姓名、班级、年龄区间、成绩区间筛选，输入实时防抖 |
| **分页排序** | 每页可调条数，点击列头正反排序 |
| **批量操作** | 批量删除、批量修改班级 |
| **统计图表** | 班级分布饼图 + 成绩分段柱状图（GDI+ 绘制） |
| **数据导入/导出** | 支持 Excel(.xlsx) 和 CSV 格式的导入与导出 |
| **备份恢复** | 数据库一键备份与恢复 |
| **信息面板** | 实时时钟、日期星期、天气（wttr.in API）、本机 IP、时段问候、今日/本月新增统计、最高/最低分详情 |

## 技术栈

- **语言**: C# (.NET Framework 4.8)
- **UI**: Windows Forms
- **数据库**: [LiteDB](https://www.litedb.org/)（嵌入式 NoSQL）
- **Excel**: [ClosedXML](https://closedxml.github.io/ClosedXML/)
- **密码**: PBKDF2（SHA256，100,000 次迭代）

## 快速开始

```bash
# 1. 克隆仓库
git clone <repo-url>

# 2. 用 Visual Studio 打开 StudentManagement.slnx，生成解决方案

# 3. 双击运行 seed.bat 生成测试数据

# 4. 运行 bin\Debug\StudentManagement.exe
#    管理员登录: lygone / 199610
```

## 项目结构

```
StudentManagement/
├── StudentManagement/       # 主项目
│   ├── Form1.cs             # 主界面（三栏布局）
│   ├── DatabaseService.cs   # 数据库服务（LiteDB 单例）
│   ├── PasswordHelper.cs    # 密码哈希（PBKDF2）
│   ├── Student.cs / User.cs # 数据模型
│   ├── LoginForm.cs         # 登录窗口
│   ├── AdminInitForm.cs     # 首次初始化管理员
│   └── UserManagementForm.cs# 用户管理
├── seed.bat + SeedData.cs    # 测试数据生成脚本
└── packages/                # NuGet 包
```

## seed.bat 脚本

项目克隆后**没有数据库文件**，双击 `seed.bat` 自动生成测试数据：

- 编译 `SeedData.cs`（使用系统自带的 csc.exe）
- 创建管理员账号：`lygone` / `199610`
- 随机生成 **1000 名学生**（中文姓名、6~19岁、12个班级、40~100分）
- 数据库自动复制到 `bin\Debug\Students.db`

## 注意事项

- 此为**个人练手项目**，非生产环境代码
- 首次运行无数据库时会弹出管理员初始化界面
- 密码使用 PBKDF2 哈希存储，旧版本 SHA256 密码首次登录时会自动升级
