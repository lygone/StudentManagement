using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace StudentManagement
{
    public partial class Form1 : Form
    {
        private DatabaseService dbService = new DatabaseService();
        private int currentStudentId = 0;
        private bool isLoading = false;

        private string currentKeyword = null;
        private string currentGrade = null;
        private int? currentAgeMin, currentAgeMax;
        private double? currentScoreMin, currentScoreMax;
        private int currentPage = 0;
        private int pageSize = 20;
        

        public Form1()
        {
            InitializeComponent();
            nudPageSize.Value = pageSize;
            nudPageSize.ValueChanged += nudPageSize_ValueChanged;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadDataAndStatsAsync();
            await LoadGradeComboBoxAsync();
        }

        private async Task LoadDataAndStatsAsync()
        {
            await LoadStudentsPageAsync();
            await UpdateStatisticsAsync();
        }

        private async Task LoadStudentsPageAsync()
        {
            isLoading = true;
            var students = await dbService.FilterStudentsAsync(
                currentKeyword, currentGrade,
                currentAgeMin, currentAgeMax,
                currentScoreMin, currentScoreMax,
                pageSize, currentPage);
            dgvStudents.DataSource = null;
            dgvStudents.DataSource = students;
            SetColumnHeaders();
            dgvStudents.ClearSelection();
            isLoading = false;

            int total = await dbService.GetTotalCountAsync(
                currentKeyword, currentGrade,
                currentAgeMin, currentAgeMax,
                currentScoreMin, currentScoreMax);
            int totalPages = (int)Math.Ceiling(total / (double)pageSize);
            lblPageInfo.Text = $"第 {currentPage + 1}/{totalPages} 页，共 {total} 条";
            btnPrevPage.Enabled = currentPage > 0;
            btnNextPage.Enabled = currentPage < totalPages - 1;
        }

        private async Task LoadGradeComboBoxAsync()
        {
            cmbGrade.Items.Clear();
            cmbGrade.Items.Add(string.Empty);
            var grades = await dbService.GetAllGradesAsync();
            foreach (var g in grades) cmbGrade.Items.Add(g);
            if (cmbGrade.Items.Count > 0) cmbGrade.SelectedIndex = 0;
        }

        private void SetColumnHeaders()
        {
            if (dgvStudents.Columns.Count >= 5)
            {
                dgvStudents.Columns[0].HeaderText = "编号";
                dgvStudents.Columns[1].HeaderText = "姓名";
                dgvStudents.Columns[2].HeaderText = "年龄";
                dgvStudents.Columns[3].HeaderText = "班级";
                dgvStudents.Columns[4].HeaderText = "成绩";
            }
        }

        private async Task UpdateStatisticsAsync()
        {
            var (total, avgAge, avgScore, gradeCount) = await dbService.GetStatisticsAsync();
            StringBuilder sb = new StringBuilder();
            sb.Append($"总人数：{total}   |   平均年龄：{avgAge:F1}   |   平均成绩：{avgScore:F1}   |   班级分布： ");
            foreach (var kvp in gradeCount)
                sb.Append($"{kvp.Key}({kvp.Value}人) ");
            lblStats.Text = sb.ToString();
        }

        private async void btnFilter_Click(object sender, EventArgs e)
        {
            currentKeyword = txtSearchName.Text.Trim();
            currentGrade = cmbGrade.SelectedItem?.ToString();
            currentAgeMin = null; currentAgeMax = null;
            currentScoreMin = null; currentScoreMax = null;
            if (int.TryParse(txtAgeMin.Text.Trim(), out int amin)) currentAgeMin = amin;
            if (int.TryParse(txtAgeMax.Text.Trim(), out int amax)) currentAgeMax = amax;
            if (double.TryParse(txtScoreMin.Text.Trim(), out double smin)) currentScoreMin = smin;
            if (double.TryParse(txtScoreMax.Text.Trim(), out double smax)) currentScoreMax = smax;
            currentPage = 0;
            await LoadDataAndStatsAsync();
        }

        private async void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtSearchName.Clear();
            if (cmbGrade.Items.Count > 0) cmbGrade.SelectedIndex = 0;
            txtAgeMin.Clear(); txtAgeMax.Clear();
            txtScoreMin.Clear(); txtScoreMax.Clear();
            currentKeyword = null; currentGrade = null;
            currentAgeMin = currentAgeMax = null;
            currentScoreMin = currentScoreMax = null;
            currentPage = 0;
            await LoadDataAndStatsAsync();
        }

        private async void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 0) { currentPage--; await LoadStudentsPageAsync(); }
        }

        private async void btnNextPage_Click(object sender, EventArgs e)
        {
            int total = await dbService.GetTotalCountAsync(
                currentKeyword, currentGrade,
                currentAgeMin, currentAgeMax,
                currentScoreMin, currentScoreMax);
            int totalPages = (int)Math.Ceiling(total / (double)pageSize);
            if (currentPage < totalPages - 1) { currentPage++; await LoadStudentsPageAsync(); }
        }

        private async void nudPageSize_ValueChanged(object sender, EventArgs e)
        {
            pageSize = (int)nudPageSize.Value;
            currentPage = 0;
            await LoadDataAndStatsAsync();
        }

        private async void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            var student = new Student
            {
                Name = txtName.Text.Trim(),
                Age = int.Parse(txtAge.Text.Trim()),
                Grade = txtGrade.Text.Trim(),
                Score = double.Parse(txtScore.Text.Trim()),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            if (currentStudentId == 0)
            {
                await Task.Run(() => dbService.AddStudent(student));
            }
            else
            {
                var all = await dbService.GetAllStudentsAsync();
                var existing = all.FirstOrDefault(s => s.Id == currentStudentId);
                if (existing != null)
                {
                    existing.Name = student.Name;
                    existing.Age = student.Age;
                    existing.Grade = student.Grade;
                    existing.Score = student.Score;
                    existing.UpdatedAt = DateTime.Now;
                    await Task.Run(() => dbService.UpdateStudent(existing));
                }
                currentStudentId = 0;
                btnAddOrUpdate.Text = "添加";
            }
            await LoadDataAndStatsAsync();
            ClearInputs();
            await LoadGradeComboBoxAsync();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null) return;
            if (MessageBox.Show("删除该学生？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = (int)dgvStudents.CurrentRow.Cells["Id"].Value;
                await Task.Run(() => dbService.DeleteStudent(id));
                await LoadDataAndStatsAsync();
                ClearInputs();
                await LoadGradeComboBoxAsync();
            }
        }

        private void btnClear_Click(object sender, EventArgs e) { ClearInputs(); }

        private async void btnBatchDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0) { MessageBox.Show("请先选择要删除的学生！"); return; }
            if (MessageBox.Show($"确认删除选中的 {dgvStudents.SelectedRows.Count} 个学生吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var ids = new List<int>();
                foreach (DataGridViewRow row in dgvStudents.SelectedRows) ids.Add((int)row.Cells["Id"].Value);
                await Task.Run(() => dbService.DeleteStudents(ids));
                await LoadDataAndStatsAsync();
                ClearInputs();
                await LoadGradeComboBoxAsync();
            }
        }

        private async void btnBatchChangeGrade_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count == 0) { MessageBox.Show("请先选择要修改的学生！"); return; }
            string newGrade = ShowInputDialog("批量修改班级", "请输入新的班级名称：", "一年级");
            if (!string.IsNullOrWhiteSpace(newGrade))
            {
                var ids = new List<int>();
                foreach (DataGridViewRow row in dgvStudents.SelectedRows) ids.Add((int)row.Cells["Id"].Value);
                await Task.Run(() => dbService.UpdateGradeForStudents(ids, newGrade.Trim()));
                await LoadDataAndStatsAsync();
                await LoadGradeComboBoxAsync();
            }
        }

        // ---------- 导出 ----------
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog { Filter = "Excel files (*.xlsx)|*.xlsx", FileName = "学生信息导出.xlsx" };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var students = dbService.GetAllStudentsAsync().Result;
                    DataTable dt = new DataTable();
                    dt.Columns.Add("编号");
                    dt.Columns.Add("姓名");
                    dt.Columns.Add("年龄");
                    dt.Columns.Add("班级");
                    dt.Columns.Add("成绩");
                    dt.Columns.Add("创建时间");
                    dt.Columns.Add("最后修改");
                    foreach (var s in students)
                        dt.Rows.Add(s.Id, s.Name, s.Age, s.Grade, s.Score, s.CreatedAt, s.UpdatedAt);

                    using (XLWorkbook wb = new XLWorkbook()) { wb.Worksheets.Add(dt, "学生"); wb.SaveAs(sfd.FileName); }
                    MessageBox.Show("导出成功！");
                }
            }
            catch (Exception ex) { MessageBox.Show("导出失败：" + ex.Message); }
        }

        private async void btnExportCsv_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv", FileName = "学生信息导出.csv" };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var students = await dbService.GetAllStudentsAsync();
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("编号,姓名,年龄,班级,成绩,创建时间,最后修改");
                    foreach (var s in students)
                        sb.AppendLine($"{s.Id},{s.Name},{s.Age},{s.Grade},{s.Score},{s.CreatedAt},{s.UpdatedAt}");
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("导出成功！");
                }
            }
            catch (Exception ex) { MessageBox.Show("导出失败：" + ex.Message); }
        }

        // ---------- 备份恢复 ----------
        private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog { Filter = "数据库文件 (*.db)|*.db", FileName = $"Students_backup_{DateTime.Now:yyyyMMddHHmm}.db" };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Students.db");
                    if (!File.Exists(source)) { MessageBox.Show("数据库文件不存在！"); return; }
                    File.Copy(source, sfd.FileName, true);
                    MessageBox.Show("备份成功！");
                }
            }
            catch (Exception ex) { MessageBox.Show("备份失败：" + ex.Message); }
        }

        private async void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog { Filter = "数据库文件 (*.db)|*.db", Title = "选择备份文件" };
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string target = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Students.db");
                    if (MessageBox.Show("恢复将覆盖当前数据，确定继续吗？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.Copy(ofd.FileName, target, true);
                        MessageBox.Show("恢复成功，程序将重新加载数据。");
                        await LoadDataAndStatsAsync();
                        await LoadGradeComboBoxAsync();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("恢复失败：" + ex.Message); }
        }

        // ---------- 校验、清空、排序、选中等 ----------
        private bool ValidateInput()
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name)) { MessageBox.Show("姓名不能为空！"); txtName.Focus(); return false; }
            if (!Regex.IsMatch(name, @"^[\u4e00-\u9fa5]+$")) { MessageBox.Show("姓名只能包含中文汉字！"); txtName.Focus(); return false; }
            if (!int.TryParse(txtAge.Text.Trim(), out int age) || age < 1 || age > 150) { MessageBox.Show("请输入合理的年龄（1-150 的数字）！"); txtAge.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(txtGrade.Text)) { MessageBox.Show("班级不能为空！"); txtGrade.Focus(); return false; }
            if (!double.TryParse(txtScore.Text.Trim(), out double score) || score < 0 || score > 100) { MessageBox.Show("成绩必须是 0 到 100 之间的数字！"); txtScore.Focus(); return false; }
            return true;
        }

        private void ClearInputs()
        {
            txtName.Clear(); txtAge.Clear(); txtGrade.Clear(); txtScore.Clear();
            currentStudentId = 0; btnAddOrUpdate.Text = "添加";
            dgvStudents.SelectionChanged -= dgvStudents_SelectionChanged;
            dgvStudents.ClearSelection();
            dgvStudents.SelectionChanged += dgvStudents_SelectionChanged;
        }

        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            if (dgvStudents.CurrentRow == null || dgvStudents.SelectedRows.Count != 1) return;
            var row = dgvStudents.CurrentRow;
            txtName.Text = row.Cells["Name"].Value.ToString();
            txtAge.Text = row.Cells["Age"].Value.ToString();
            txtGrade.Text = row.Cells["Grade"].Value?.ToString();
            txtScore.Text = row.Cells["Score"].Value.ToString();
            currentStudentId = (int)row.Cells["Id"].Value;
            btnAddOrUpdate.Text = "更新";
        }

        private void dgvStudents_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!(dgvStudents.DataSource is List<Student> list)) return;
            var col = dgvStudents.Columns[e.ColumnIndex];
            List<Student> sorted = null;
            switch (col.DataPropertyName)
            {
                case "Name": sorted = ToggleSort(list, s => s.Name, col.Tag); break;
                case "Age": sorted = ToggleSort(list, s => s.Age, col.Tag); break;
                case "Grade": sorted = ToggleSort(list, s => s.Grade, col.Tag); break;
                case "Score": sorted = ToggleSort(list, s => s.Score, col.Tag); break;
                default: return;
            }
            dgvStudents.DataSource = null;
            dgvStudents.DataSource = sorted;
            SetColumnHeaders();
            col.Tag = (col.Tag == null || col.Tag.ToString() == "DESC") ? "ASC" : "DESC";
        }

        private List<Student> ToggleSort<TKey>(List<Student> list, Func<Student, TKey> keySelector, object tag)
        {
            bool descending = tag != null && tag.ToString() == "ASC";
            return descending ? list.OrderByDescending(keySelector).ToList() : list.OrderBy(keySelector).ToList();
        }
        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            using (UserManagementForm userForm = new UserManagementForm())
            {
                userForm.ShowDialog(this);
            }
        }
        private string ShowInputDialog(string title, string promptText, string defaultValue = "")
        {
            Form dialog = new Form();
            dialog.Width = 400; dialog.Height = 180; dialog.Text = title;
            dialog.StartPosition = FormStartPosition.CenterScreen;
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.MaximizeBox = false; dialog.MinimizeBox = false;
            Label lbl = new Label() { Left = 15, Top = 15, Width = 350, Text = promptText };
            TextBox txt = new TextBox() { Left = 15, Top = 45, Width = 350, Text = defaultValue };
            Button btnOk = new Button() { Text = "确定", Left = 200, Width = 75, Top = 80, DialogResult = DialogResult.OK };
            Button btnCancel = new Button() { Text = "取消", Left = 285, Width = 75, Top = 80, DialogResult = DialogResult.Cancel };
            dialog.Controls.Add(lbl); dialog.Controls.Add(txt); dialog.Controls.Add(btnOk); dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOk; dialog.CancelButton = btnCancel;
            return dialog.ShowDialog() == DialogResult.OK ? txt.Text : string.Empty;
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using ClosedXML.Excel;

//namespace StudentManagement
//{
//    public partial class Form1 : Form
//    {
//        private DatabaseService dbService = new DatabaseService();
//        private int currentStudentId = 0;
//        private bool isLoading = false;

//        public Form1()
//        {
//            InitializeComponent();
//            this.Load += Form1_Load;   // 添加这行
//        }

//        private async void Form1_Load(object sender, EventArgs e)
//        {
//            await LoadAllStudentsAsync();
//            await LoadGradeComboBoxAsync();
//        }

//        // ---------- 数据加载 ----------
//        private async Task LoadAllStudentsAsync()
//        {
//            isLoading = true;
//            var students = await dbService.GetAllStudentsAsync();
//            dgvStudents.DataSource = null;
//            dgvStudents.DataSource = students;
//            SetColumnHeaders();
//            dgvStudents.ClearSelection();
//            isLoading = false;
//            await UpdateStatisticsAsync();
//        }

//        private async Task LoadGradeComboBoxAsync()
//        {
//            cmbGrade.Items.Clear();
//            cmbGrade.Items.Add(string.Empty); // 不限
//            var grades = await dbService.GetAllGradesAsync();
//            foreach (var g in grades)
//                cmbGrade.Items.Add(g);
//            cmbGrade.SelectedIndex = 0;
//        }

//        private void SetColumnHeaders()
//        {
//            if (dgvStudents.Columns.Count >= 5)
//            {
//                dgvStudents.Columns[0].HeaderText = "编号";
//                dgvStudents.Columns[1].HeaderText = "姓名";
//                dgvStudents.Columns[2].HeaderText = "年龄";
//                dgvStudents.Columns[3].HeaderText = "班级";
//                dgvStudents.Columns[4].HeaderText = "成绩";
//            }
//        }

//        private async Task UpdateStatisticsAsync()
//        {
//            var (total, avgAge, avgScore, gradeCount) = await dbService.GetStatisticsAsync();
//            StringBuilder sb = new StringBuilder();
//            sb.Append($"总人数：{total}   |   平均年龄：{avgAge:F1}   |   平均成绩：{avgScore:F1}   |   班级分布： ");
//            foreach (var kvp in gradeCount)
//                sb.Append($"{kvp.Key}({kvp.Value}人) ");
//            lblStats.Text = sb.ToString();
//        }

//        // ---------- 输入校验 ----------
//        private bool ValidateInput()
//        {
//            string name = txtName.Text.Trim();
//            if (string.IsNullOrEmpty(name))
//            {
//                MessageBox.Show("姓名不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                txtName.Focus();
//                return false;
//            }
//            if (!Regex.IsMatch(name, @"^[\u4e00-\u9fa5]+$"))
//            {
//                MessageBox.Show("姓名只能包含中文汉字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                txtName.Focus();
//                return false;
//            }

//            if (!int.TryParse(txtAge.Text.Trim(), out int age) || age < 1 || age > 150)
//            {
//                MessageBox.Show("请输入合理的年龄（1-150 的数字）！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                txtAge.Focus();
//                return false;
//            }

//            if (string.IsNullOrWhiteSpace(txtGrade.Text))
//            {
//                MessageBox.Show("班级不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                txtGrade.Focus();
//                return false;
//            }

//            if (!double.TryParse(txtScore.Text.Trim(), out double score) || score < 0 || score > 100)
//            {
//                MessageBox.Show("成绩必须是 0 到 100 之间的数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                txtScore.Focus();
//                return false;
//            }

//            return true;
//        }

//        // ---------- 清空表单 ----------
//        private void ClearInputs()
//        {
//            txtName.Clear();
//            txtAge.Clear();
//            txtGrade.Clear();
//            txtScore.Clear();
//            currentStudentId = 0;
//            btnAddOrUpdate.Text = "添加";
//            dgvStudents.SelectionChanged -= dgvStudents_SelectionChanged;
//            dgvStudents.ClearSelection();
//            dgvStudents.SelectionChanged += dgvStudents_SelectionChanged;
//        }

//        // ---------- 表格选中变化（仅单选回填）----------
//        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
//        {
//            if (isLoading) return;
//            if (dgvStudents.CurrentRow == null || dgvStudents.SelectedRows.Count != 1) return;

//            var row = dgvStudents.CurrentRow;
//            txtName.Text = row.Cells["Name"].Value.ToString();
//            txtAge.Text = row.Cells["Age"].Value.ToString();
//            txtGrade.Text = row.Cells["Grade"].Value?.ToString();
//            txtScore.Text = row.Cells["Score"].Value.ToString();
//            currentStudentId = (int)row.Cells["Id"].Value;
//            btnAddOrUpdate.Text = "更新";
//        }

//        // ---------- 列头点击排序 ----------
//        private void dgvStudents_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
//        {
//            if (!(dgvStudents.DataSource is List<Student> list)) return;
//            var col = dgvStudents.Columns[e.ColumnIndex];
//            List<Student> sorted = null;

//            switch (col.DataPropertyName)
//            {
//                case "Name":
//                    sorted = ToggleSort(list, s => s.Name, col.Tag);
//                    break;
//                case "Age":
//                    sorted = ToggleSort(list, s => s.Age, col.Tag);
//                    break;
//                case "Grade":
//                    sorted = ToggleSort(list, s => s.Grade, col.Tag);
//                    break;
//                case "Score":
//                    sorted = ToggleSort(list, s => s.Score, col.Tag);
//                    break;
//                default:
//                    return;
//            }
//            dgvStudents.DataSource = null;
//            dgvStudents.DataSource = sorted;
//            SetColumnHeaders();
//            col.Tag = (col.Tag == null || col.Tag.ToString() == "DESC") ? "ASC" : "DESC";
//        }

//        private List<Student> ToggleSort<TKey>(List<Student> list, Func<Student, TKey> keySelector, object tag)
//        {
//            bool descending = tag != null && tag.ToString() == "ASC";
//            return descending ? list.OrderByDescending(keySelector).ToList() : list.OrderBy(keySelector).ToList();
//        }

//        // ---------- 添加/更新 ----------
//        private async void btnAddOrUpdate_Click(object sender, EventArgs e)
//        {
//            if (!ValidateInput()) return;

//            var student = new Student
//            {
//                Name = txtName.Text.Trim(),
//                Age = int.Parse(txtAge.Text.Trim()),
//                Grade = txtGrade.Text.Trim(),
//                Score = double.Parse(txtScore.Text.Trim()),
//                CreatedAt = DateTime.Now,
//                UpdatedAt = DateTime.Now
//            };

//            if (currentStudentId == 0)
//            {
//                await Task.Run(() => dbService.AddStudent(student));
//            }
//            else
//            {
//                var existing = (await dbService.GetAllStudentsAsync()).FirstOrDefault(s => s.Id == currentStudentId);
//                if (existing != null)
//                {
//                    existing.Name = student.Name;
//                    existing.Age = student.Age;
//                    existing.Grade = student.Grade;
//                    existing.Score = student.Score;
//                    existing.UpdatedAt = DateTime.Now;
//                    await Task.Run(() => dbService.UpdateStudent(existing));
//                }
//                currentStudentId = 0;
//                btnAddOrUpdate.Text = "添加";
//            }

//            await LoadAllStudentsAsync();
//            ClearInputs();
//            await LoadGradeComboBoxAsync();
//        }

//        // ---------- 单个删除 ----------
//        private async void btnDelete_Click(object sender, EventArgs e)
//        {
//            if (dgvStudents.CurrentRow == null) return;
//            if (MessageBox.Show("删除该学生？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
//            {
//                int id = (int)dgvStudents.CurrentRow.Cells["Id"].Value;
//                await Task.Run(() => dbService.DeleteStudent(id));
//                await LoadAllStudentsAsync();
//                ClearInputs();
//                await LoadGradeComboBoxAsync();
//            }
//        }

//        // ---------- 清空 ----------
//        private void btnClear_Click(object sender, EventArgs e)
//        {
//            ClearInputs();
//        }

//        // ---------- 筛选 ----------
//        private async void btnFilter_Click(object sender, EventArgs e)
//        {
//            string keyword = txtSearchName.Text.Trim();
//            string grade = cmbGrade.SelectedItem?.ToString();
//            int? ageMin = null, ageMax = null;
//            double? scoreMin = null, scoreMax = null;

//            if (int.TryParse(txtAgeMin.Text.Trim(), out int amin)) ageMin = amin;
//            if (int.TryParse(txtAgeMax.Text.Trim(), out int amax)) ageMax = amax;
//            if (double.TryParse(txtScoreMin.Text.Trim(), out double smin)) scoreMin = smin;
//            if (double.TryParse(txtScoreMax.Text.Trim(), out double smax)) scoreMax = smax;

//            var students = await dbService.FilterStudentsAsync(keyword, grade, ageMin, ageMax, scoreMin, scoreMax);
//            dgvStudents.DataSource = null;
//            dgvStudents.DataSource = students;
//            SetColumnHeaders();
//            await UpdateStatisticsAsync();
//        }

//        private async void btnClearFilter_Click(object sender, EventArgs e)
//        {
//            txtSearchName.Clear();
//            if (cmbGrade.Items.Count > 0)
//                cmbGrade.SelectedIndex = 0;          // 有项目才设置
//            else
//                cmbGrade.Items.Add(string.Empty);    // 否则添加空选项
//            txtAgeMin.Clear();
//            txtAgeMax.Clear();
//            txtScoreMin.Clear();
//            txtScoreMax.Clear();
//            await LoadAllStudentsAsync();
//        }

//        // ---------- 批量操作 ----------
//        private async void btnBatchDelete_Click(object sender, EventArgs e)
//        {
//            if (dgvStudents.SelectedRows.Count == 0)
//            {
//                MessageBox.Show("请先选择要删除的学生！");
//                return;
//            }
//            if (MessageBox.Show($"确认删除选中的 {dgvStudents.SelectedRows.Count} 个学生吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
//            {
//                var ids = new List<int>();
//                foreach (DataGridViewRow row in dgvStudents.SelectedRows)
//                    ids.Add((int)row.Cells["Id"].Value);
//                await Task.Run(() => dbService.DeleteStudents(ids));
//                await LoadAllStudentsAsync();
//                ClearInputs();
//                await LoadGradeComboBoxAsync();
//            }
//        }

//        private async void btnBatchChangeGrade_Click(object sender, EventArgs e)
//        {
//            if (dgvStudents.SelectedRows.Count == 0)
//            {
//                MessageBox.Show("请先选择要修改的学生！");
//                return;
//            }
//            string newGrade = ShowInputDialog("批量修改班级", "请输入新的班级名称：", "一年级");
//            if (!string.IsNullOrWhiteSpace(newGrade))
//            {
//                var ids = new List<int>();
//                foreach (DataGridViewRow row in dgvStudents.SelectedRows)
//                    ids.Add((int)row.Cells["Id"].Value);
//                await Task.Run(() => dbService.UpdateGradeForStudents(ids, newGrade.Trim()));
//                await LoadAllStudentsAsync();
//                await LoadGradeComboBoxAsync();
//                await UpdateStatisticsAsync();
//            }
//        }

//        // ---------- 导出 ----------
//        private void btnExportExcel_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                SaveFileDialog sfd = new SaveFileDialog
//                {
//                    Filter = "Excel files (*.xlsx)|*.xlsx",
//                    FileName = "学生信息导出.xlsx"
//                };
//                if (sfd.ShowDialog() == DialogResult.OK)
//                {
//                    var students = dbService.GetAllStudentsAsync().Result; // 简单取回同步等待
//                    DataTable dt = new DataTable();
//                    dt.Columns.Add("编号");
//                    dt.Columns.Add("姓名");
//                    dt.Columns.Add("年龄");
//                    dt.Columns.Add("班级");
//                    dt.Columns.Add("成绩");
//                    dt.Columns.Add("创建时间");
//                    dt.Columns.Add("最后修改");
//                    foreach (var s in students)
//                        dt.Rows.Add(s.Id, s.Name, s.Age, s.Grade, s.Score, s.CreatedAt, s.UpdatedAt);

//                    using (XLWorkbook wb = new XLWorkbook())
//                    {
//                        wb.Worksheets.Add(dt, "学生");
//                        wb.SaveAs(sfd.FileName);
//                    }
//                    MessageBox.Show("导出成功！");
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("导出失败：" + ex.Message);
//            }
//        }

//        private async void btnExportCsv_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                SaveFileDialog sfd = new SaveFileDialog
//                {
//                    Filter = "CSV files (*.csv)|*.csv",
//                    FileName = "学生信息导出.csv"
//                };
//                if (sfd.ShowDialog() == DialogResult.OK)
//                {
//                    var students = await dbService.GetAllStudentsAsync();
//                    StringBuilder sb = new StringBuilder();
//                    sb.AppendLine("编号,姓名,年龄,班级,成绩,创建时间,最后修改");
//                    foreach (var s in students)
//                        sb.AppendLine($"{s.Id},{s.Name},{s.Age},{s.Grade},{s.Score},{s.CreatedAt},{s.UpdatedAt}");
//                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
//                    MessageBox.Show("导出成功！");
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("导出失败：" + ex.Message);
//            }
//        }

//        // ---------- 备份恢复 ----------
//        private void btnBackup_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                SaveFileDialog sfd = new SaveFileDialog
//                {
//                    Filter = "数据库文件 (*.db)|*.db",
//                    FileName = $"Students_backup_{DateTime.Now:yyyyMMddHHmm}.db"
//                };
//                if (sfd.ShowDialog() == DialogResult.OK)
//                {
//                    string source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Students.db");
//                    if (!File.Exists(source))
//                    {
//                        MessageBox.Show("数据库文件不存在！");
//                        return;
//                    }
//                    File.Copy(source, sfd.FileName, true);
//                    MessageBox.Show("备份成功！");
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("备份失败：" + ex.Message);
//            }
//        }

//        private async void btnRestore_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                OpenFileDialog ofd = new OpenFileDialog
//                {
//                    Filter = "数据库文件 (*.db)|*.db",
//                    Title = "选择备份文件"
//                };
//                if (ofd.ShowDialog() == DialogResult.OK)
//                {
//                    string target = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Students.db");
//                    if (MessageBox.Show("恢复将覆盖当前数据，确定继续吗？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
//                    {
//                        File.Copy(ofd.FileName, target, true);
//                        MessageBox.Show("恢复成功，程序将重新加载数据。");
//                        await LoadAllStudentsAsync();
//                        await LoadGradeComboBoxAsync();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("恢复失败：" + ex.Message);
//            }
//        }

//        // ---------- 自定义输入框 ----------
//        private string ShowInputDialog(string title, string promptText, string defaultValue = "")
//        {
//            Form dialog = new Form();
//            dialog.Width = 400;
//            dialog.Height = 180;
//            dialog.Text = title;
//            dialog.StartPosition = FormStartPosition.CenterScreen;
//            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
//            dialog.MaximizeBox = false;
//            dialog.MinimizeBox = false;

//            Label lblPrompt = new Label() { Left = 15, Top = 15, Width = 350, Text = promptText };
//            TextBox txtInput = new TextBox() { Left = 15, Top = 45, Width = 350, Text = defaultValue };
//            Button btnOk = new Button() { Text = "确定", Left = 200, Width = 75, Top = 80, DialogResult = DialogResult.OK };
//            Button btnCancel = new Button() { Text = "取消", Left = 285, Width = 75, Top = 80, DialogResult = DialogResult.Cancel };

//            dialog.Controls.Add(lblPrompt);
//            dialog.Controls.Add(txtInput);
//            dialog.Controls.Add(btnOk);
//            dialog.Controls.Add(btnCancel);
//            dialog.AcceptButton = btnOk;
//            dialog.CancelButton = btnCancel;

//            return dialog.ShowDialog() == DialogResult.OK ? txtInput.Text : string.Empty;
//        }
//    }
//}