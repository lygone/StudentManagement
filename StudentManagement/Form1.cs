using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
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

        private Dictionary<string, int> gradeChartData = new Dictionary<string, int>();
        private Dictionary<string, double> scoreChartData = new Dictionary<string, double>();
        private System.Windows.Forms.Timer tmrClock;
        private DateTime lastWeatherFetch = DateTime.MinValue;

        public Form1()
        {
            InitializeComponent();
            nudPageSize.Value = pageSize;
            nudPageSize.ValueChanged += nudPageSize_ValueChanged;
            tmrClock = new System.Windows.Forms.Timer { Interval = 1000 };
            tmrClock.Tick += tmrClock_Tick;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadDataAndStatsAsync();
            await LoadGradeComboBoxAsync();
            await UpdateInfoPanelAsync();
            tmrClock.Start();
            _ = FetchWeatherAsync();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DatabaseService.Shutdown();
        }

        // ==================== 数据加载 ====================
        private async Task LoadDataAndStatsAsync()
        {
            await LoadStudentsPageAsync();
            await UpdateStatisticsAsync();
            await UpdateChartsAsync();
            await UpdateQuickStatsAsync();
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
            if (totalPages == 0) totalPages = 1;
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

        // ==================== 统计与图表 ====================
        private async Task UpdateStatisticsAsync()
        {
            var (total, avgAge, avgScore, gradeCount) = await dbService.GetStatisticsAsync(
                currentKeyword, currentGrade,
                currentAgeMin, currentAgeMax,
                currentScoreMin, currentScoreMax);
            gradeChartData = gradeCount;
            StringBuilder sb = new StringBuilder();
            sb.Append($"总人数：{total}   |   平均年龄：{avgAge:F1}   |   平均成绩：{avgScore:F1}   |   班级分布： ");
            foreach (var kvp in gradeCount)
                sb.Append($"{kvp.Key}({kvp.Value}人) ");
            lblStats.Text = sb.ToString();
        }

        private async Task UpdateChartsAsync()
        {
            scoreChartData = await dbService.GetScoreDistributionAsync(
                currentKeyword, currentGrade,
                currentAgeMin, currentAgeMax,
                currentScoreMin, currentScoreMax);
            picGradeChart.Invalidate();
            picScoreChart.Invalidate();
        }

        private void picGradeChart_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var rect = picGradeChart.ClientRectangle;
            int size = Math.Min(rect.Width - 20, rect.Height - 20);
            int cx = rect.Width / 2, cy = rect.Height / 2;
            var pieRect = new Rectangle(cx - size / 2, cy - size / 2, size, size);

            if (gradeChartData == null || gradeChartData.Count == 0 || gradeChartData.Values.Sum() == 0)
            {
                using (var f = new Font("微软雅黑", 11))
                    g.DrawString("暂无数据", f, Brushes.Gray, cx - 40, cy - 10);
                return;
            }

            var colors = new Color[] {
                Color.FromArgb(41, 128, 185), Color.FromArgb(39, 174, 96),
                Color.FromArgb(241, 196, 15), Color.FromArgb(231, 76, 60),
                Color.FromArgb(155, 89, 182), Color.FromArgb(52, 152, 219),
                Color.FromArgb(230, 126, 34), Color.FromArgb(149, 165, 166)
            };

            float total = gradeChartData.Values.Sum();
            float startAngle = 0;
            int ci = 0;
            var legendY = cy + size / 2 + 10;
            foreach (var kvp in gradeChartData)
            {
                float sweep = (kvp.Value / total) * 360f;
                using (var brush = new SolidBrush(colors[ci % colors.Length]))
                {
                    g.FillPie(brush, pieRect, startAngle, sweep);
                }
                startAngle += sweep;

                int pct = (int)Math.Round(kvp.Value / total * 100);
                using (var brush = new SolidBrush(colors[ci % colors.Length]))
                using (var font = new Font("微软雅黑", 8))
                {
                    int lx = ci % 4 * 75 + 10;
                    int ly = legendY + (ci / 4) * 20;
                    g.FillRectangle(brush, lx, ly + 3, 12, 12);
                    g.DrawString($"{kvp.Key} ({pct}%)", font, Brushes.Black, lx + 16, ly + 1);
                }
                ci++;
            }
        }

        private void picScoreChart_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var rect = picScoreChart.ClientRectangle;
            int margin = 40;
            int barAreaWidth = rect.Width - margin * 2;
            int barAreaHeight = rect.Height - margin * 2;

            if (scoreChartData == null || scoreChartData.Count == 0 || scoreChartData.Values.Sum() == 0)
            {
                using (var f = new Font("微软雅黑", 11))
                    g.DrawString("暂无数据", f, Brushes.Gray, rect.Width / 2 - 40, rect.Height / 2 - 10);
                return;
            }

            var colors = new Color[] {
                Color.FromArgb(231, 76, 60), Color.FromArgb(230, 126, 34),
                Color.FromArgb(241, 196, 15), Color.FromArgb(39, 174, 96),
                Color.FromArgb(41, 128, 185)
            };

            double maxVal = scoreChartData.Values.Max();
            if (maxVal == 0) maxVal = 1;
            int barCount = scoreChartData.Count;
            int barWidth = Math.Min(60, barAreaWidth / barCount - 10);
            int gap = (barAreaWidth - barWidth * barCount) / (barCount + 1);

            using (var pen = new Pen(Color.FromArgb(200, 200, 200)))
            using (var font = new Font("微软雅黑", 8))
            {
                g.DrawLine(pen, margin, rect.Height - margin, rect.Width - margin, rect.Height - margin);
                int ci = 0;
                foreach (var kvp in scoreChartData)
                {
                    int barHeight = (int)(kvp.Value / maxVal * barAreaHeight);
                    int x = margin + gap + ci * (barWidth + gap);
                    int y = rect.Height - margin - barHeight;

                    using (var brush = new SolidBrush(colors[ci % colors.Length]))
                    {
                        g.FillRectangle(brush, x, y, barWidth, barHeight);
                        g.DrawRectangle(Pens.White, x, y, barWidth, barHeight);
                    }

                    string label = kvp.Key;
                    g.DrawString(label, font, Brushes.Black, x + barWidth / 2 - 15, rect.Height - margin + 5);

                    string valStr = kvp.Value == Math.Floor(kvp.Value) ? ((int)kvp.Value).ToString() : kvp.Value.ToString("F1");
                    g.DrawString(valStr, font, Brushes.Black, x + barWidth / 2 - 10, y - 15);

                    ci++;
                }
            }
        }

        // ==================== 筛选 ====================
        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            tmrSearchDebounce.Stop();
            tmrSearchDebounce.Start();
        }

        private void cmbGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            tmrSearchDebounce.Stop();
            tmrSearchDebounce.Start();
        }

        private void filterField_TextChanged(object sender, EventArgs e)
        {
            tmrSearchDebounce.Stop();
            tmrSearchDebounce.Start();
        }

        private async void tmrSearchDebounce_Tick(object sender, EventArgs e)
        {
            tmrSearchDebounce.Stop();
            await ApplyFilterAsync();
        }

        private async void btnFilter_Click(object sender, EventArgs e)
        {
            tmrSearchDebounce.Stop();
            await ApplyFilterAsync();
        }

        private async void btnClearFilter_Click(object sender, EventArgs e)
        {
            tmrSearchDebounce.Stop();
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

        private async Task ApplyFilterAsync()
        {
            currentKeyword = string.IsNullOrWhiteSpace(txtSearchName.Text) ? null : txtSearchName.Text.Trim();
            currentGrade = string.IsNullOrEmpty(cmbGrade.SelectedItem?.ToString()) ? null : cmbGrade.SelectedItem.ToString();
            currentAgeMin = null; currentAgeMax = null;
            currentScoreMin = null; currentScoreMax = null;
            if (int.TryParse(txtAgeMin.Text.Trim(), out int amin)) currentAgeMin = amin;
            if (int.TryParse(txtAgeMax.Text.Trim(), out int amax)) currentAgeMax = amax;
            if (double.TryParse(txtScoreMin.Text.Trim(), out double smin)) currentScoreMin = smin;
            if (double.TryParse(txtScoreMax.Text.Trim(), out double smax)) currentScoreMax = smax;
            currentPage = 0;
            await LoadDataAndStatsAsync();
        }

        // ==================== 分页 ====================
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
            if (totalPages == 0) totalPages = 1;
            if (currentPage < totalPages - 1) { currentPage++; await LoadStudentsPageAsync(); }
        }

        private async void nudPageSize_ValueChanged(object sender, EventArgs e)
        {
            pageSize = (int)nudPageSize.Value;
            currentPage = 0;
            await LoadDataAndStatsAsync();
        }

        // ==================== CRUD 操作 ====================
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
            bool isAdd = (currentStudentId == 0);
            if (isAdd)
            {
                await Task.Run(() => dbService.AddStudent(student));
            }
            else
            {
                var existing = dbService.GetStudentById(currentStudentId);
                if (existing != null)
                {
                    existing.Name = student.Name;
                    existing.Age = student.Age;
                    existing.Grade = student.Grade;
                    existing.Score = student.Score;
                    await Task.Run(() => dbService.UpdateStudent(existing));
                }
                currentStudentId = 0;
                btnAddOrUpdate.Text = "添加";
            }
            await LoadDataAndStatsAsync();
            if (isAdd) ClearInputs();
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

        // ==================== 导入 ====================
        private async void btnImportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog { Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls", Title = "选择Excel文件导入" };
                if (ofd.ShowDialog() != DialogResult.OK) return;

                var students = await Task.Run(() =>
                {
                    var list = new List<Student>();
                    using (var wb = new XLWorkbook(ofd.FileName))
                    {
                        var ws = wb.Worksheet(1);
                        var rows = ws.RangeUsed().RowsUsed().Skip(1);
                        foreach (var row in rows)
                        {
                            if (row.Cell(1).IsEmpty()) continue;
                            var s = new Student
                            {
                                Name = row.Cell(1).GetString().Trim(),
                                Age = row.Cell(2).TryGetValue<int>(out int age) ? age : 0,
                                Grade = row.Cell(3).GetString().Trim(),
                                Score = row.Cell(4).TryGetValue<double>(out double score) ? score : 0,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                            };
                            if (!string.IsNullOrEmpty(s.Name) && s.Age > 0)
                                list.Add(s);
                        }
                    }
                    return list;
                });

                if (students.Count == 0) { MessageBox.Show("未找到有效数据！请确保Excel第一行为标题，数据从第二行开始。"); return; }
                if (MessageBox.Show($"找到 {students.Count} 条记录，确认导入？", "导入确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    await Task.Run(() => dbService.AddStudents(students));
                    MessageBox.Show($"成功导入 {students.Count} 条记录！");
                    await LoadDataAndStatsAsync();
                    await LoadGradeComboBoxAsync();
                }
            }
            catch (Exception ex) { MessageBox.Show("导入失败：" + ex.Message); }
        }

        private async void btnImportCsv_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog { Filter = "CSV files (*.csv)|*.csv", Title = "选择CSV文件导入" };
                if (ofd.ShowDialog() != DialogResult.OK) return;

                var students = await Task.Run(() =>
                {
                    var list = new List<Student>();
                    var lines = File.ReadAllLines(ofd.FileName, Encoding.UTF8);
                    for (int i = 1; i < lines.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(lines[i])) continue;
                        var parts = ParseCsvLine(lines[i]);
                        if (parts.Length < 4) continue;
                        if (!int.TryParse(parts[1].Trim(), out int age) || age <= 0) continue;
                        if (!double.TryParse(parts[3].Trim(), out double score)) score = 0;
                        list.Add(new Student
                        {
                            Name = parts[0].Trim(),
                            Age = age,
                            Grade = parts[2].Trim(),
                            Score = score,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        });
                    }
                    return list;
                });

                if (students.Count == 0) { MessageBox.Show("未找到有效数据！请确保CSV第一行为标题，格式：姓名,年龄,班级,成绩"); return; }
                if (MessageBox.Show($"找到 {students.Count} 条记录，确认导入？", "导入确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    await Task.Run(() => dbService.AddStudents(students));
                    MessageBox.Show($"成功导入 {students.Count} 条记录！");
                    await LoadDataAndStatsAsync();
                    await LoadGradeComboBoxAsync();
                }
            }
            catch (Exception ex) { MessageBox.Show("导入失败：" + ex.Message); }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            var item1 = menu.Items.Add("从Excel导入");
            var item2 = menu.Items.Add("从CSV导入");
            item1.Click += (s, ea) => btnImportExcel_Click(s, ea);
            item2.Click += (s, ea) => btnImportCsv_Click(s, ea);
            menu.Show(btnImport, new System.Drawing.Point(0, btnImport.Height));
        }

        // ==================== 导出 ====================
        private async void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog { Filter = "Excel files (*.xlsx)|*.xlsx", FileName = "学生信息导出.xlsx" };
                if (sfd.ShowDialog() != DialogResult.OK) return;

                var students = await dbService.GetAllStudentsAsync();
                await Task.Run(() =>
                {
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
                });
                MessageBox.Show("导出成功！");
            }
            catch (Exception ex) { MessageBox.Show("导出失败：" + ex.Message); }
        }

        private async void btnExportCsv_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv", FileName = "学生信息导出.csv" };
                if (sfd.ShowDialog() != DialogResult.OK) return;

                var students = await dbService.GetAllStudentsAsync();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("编号,姓名,年龄,班级,成绩,创建时间,最后修改");
                foreach (var s in students)
                    sb.AppendLine($"{s.Id},{EscapeCsvField(s.Name)},{s.Age},{EscapeCsvField(s.Grade)},{s.Score},{s.CreatedAt},{s.UpdatedAt}");
                File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show("导出成功！");
            }
            catch (Exception ex) { MessageBox.Show("导出失败：" + ex.Message); }
        }

        // ==================== 备份恢复 ====================
        private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog { Filter = "数据库文件 (*.db)|*.db", FileName = $"Students_backup_{DateTime.Now:yyyyMMddHHmm}.db" };
                if (sfd.ShowDialog() != DialogResult.OK) return;
                string source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Students.db");
                if (!File.Exists(source)) { MessageBox.Show("数据库文件不存在！"); return; }
                File.Copy(source, sfd.FileName, true);
                MessageBox.Show("备份成功！");
            }
            catch (Exception ex) { MessageBox.Show("备份失败：" + ex.Message); }
        }

        private async void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog { Filter = "数据库文件 (*.db)|*.db", Title = "选择备份文件" };
                if (ofd.ShowDialog() != DialogResult.OK) return;
                string target = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Students.db");
                if (MessageBox.Show("恢复将覆盖当前数据，确定继续吗？", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DatabaseService.Shutdown();
                    File.Copy(ofd.FileName, target, true);
                    MessageBox.Show("恢复成功，程序将重新加载数据。");
                    dbService = new DatabaseService();
                    await LoadDataAndStatsAsync();
                    await LoadGradeComboBoxAsync();
                }
            }
            catch (Exception ex) { MessageBox.Show("恢复失败：" + ex.Message); }
        }

        // ==================== 校验与排序 ====================
        private bool ValidateInput()
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name)) { MessageBox.Show("姓名不能为空！"); txtName.Focus(); return false; }
            if (!Regex.IsMatch(name, @"^[\u4e00-\u9fa5·]+$")) { MessageBox.Show("姓名只能包含中文汉字！"); txtName.Focus(); return false; }
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

        // ==================== 工具方法 ====================
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

        private static string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field)) return "";
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
                return "\"" + field.Replace("\"", "\"\"") + "\"";
            return field;
        }

        private static string[] ParseCsvLine(string line)
        {
            var result = new List<string>();
            bool inQuotes = false;
            var current = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < line.Length && line[i + 1] == '"') { current.Append('"'); i++; }
                        else inQuotes = false;
                    }
                    else current.Append(c);
                }
                else
                {
                    if (c == '"') inQuotes = true;
                    else if (c == ',') { result.Add(current.ToString()); current.Clear(); }
                    else current.Append(c);
                }
            }
            result.Add(current.ToString());
            return result.ToArray();
        }

        // ==================== 时钟/天气/系统信息 ====================
        private void tmrClock_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            lblTime.Text = now.ToString("HH:mm:ss");
            lblDate.Text = now.ToString("yyyy年MM月dd日  dddd");
            int hour = now.Hour;
            lblGreeting.Text = hour < 6 ? "🌙 夜深了" : hour < 9 ? "🌅 早上好" : hour < 12 ? "☀️ 上午好" : hour < 14 ? "🌤 中午好" : hour < 18 ? "🌻 下午好" : "🌆 晚上好";
            if ((now - lastWeatherFetch).TotalMinutes > 30)
                _ = FetchWeatherAsync();
        }

        private async Task UpdateInfoPanelAsync()
        {
            UpdateSystemInfo();
            await UpdateQuickStatsAsync();
        }

        private async Task UpdateQuickStatsAsync()
        {
            try
            {
                var (today, thisMonth, maxScore, maxName, maxGrade, minScore, minName, minGrade) = await dbService.GetQuickStatsAsync();
                string line1 = $"📊 今日新增 {today} 人    本月新增 {thisMonth} 人";
                string line2 = !string.IsNullOrEmpty(maxName) ? $"🏆 最高分 {maxScore:F0} — {maxName} · {maxGrade}" : "🏆 暂无数据";
                string line3 = !string.IsNullOrEmpty(minName) ? $"📉 最低分 {minScore:F0} — {minName} · {minGrade}" : "";
                lblQuickStats.Text = $"{line1}\r\n{line2}\r\n{line3}";
            }
            catch
            {
                lblQuickStats.Text = "📊 加载统计数据...";
            }
        }

        private void UpdateSystemInfo()
        {
            try
            {
                string hostName = Dns.GetHostName();
                var ips = Dns.GetHostEntry(hostName).AddressList
                    .Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .Select(a => a.ToString())
                    .ToList();

                string bestIp = null;
                var preferRanges = new[] { "192.168.", "10.", "172.16.", "172.17.", "172.18.", "172.19.",
                    "172.20.", "172.21.", "172.22.", "172.23.", "172.24.", "172.25.", "172.26.",
                    "172.27.", "172.28.", "172.29.", "172.30.", "172.31." };
                foreach (var prefix in preferRanges)
                {
                    bestIp = ips.FirstOrDefault(ip => ip.StartsWith(prefix));
                    if (bestIp != null) break;
                }
                if (bestIp == null) bestIp = ips.FirstOrDefault(ip => ip != "127.0.0.1");
                if (bestIp == null && ips.Count > 0) bestIp = ips[0];

                lblComputer.Text = (bestIp != null)
                    ? $"💻 {hostName}    🌐 {bestIp}"
                    : $"💻 {hostName}";
            }
            catch
            {
                lblComputer.Text = "💻 本机";
            }
        }

        private async Task FetchWeatherAsync()
        {
            try
            {
                lblWeather.Text = "⏳ 获取天气中...";
                using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) })
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("StudentManagement/1.0");
                    var response = await client.GetStringAsync("http://wttr.in/Beijing?format=%c+%t");
                    var text = response.Trim();
                    lblWeather.Text = text;
                }
                lastWeatherFetch = DateTime.Now;
            }
            catch
            {
                lblWeather.Text = "🌍 暂无天气数据";
            }
        }
    }
}
