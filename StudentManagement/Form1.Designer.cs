namespace StudentManagement
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnExportCsv = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnUserManagement = new System.Windows.Forms.Button();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.txtSearchName = new System.Windows.Forms.TextBox();
            this.cmbGrade = new System.Windows.Forms.ComboBox();
            this.txtAgeMin = new System.Windows.Forms.TextBox();
            this.txtAgeMax = new System.Windows.Forms.TextBox();
            this.txtScoreMin = new System.Windows.Forms.TextBox();
            this.txtScoreMax = new System.Windows.Forms.TextBox();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnClearFilter = new System.Windows.Forms.Button();
            this.lblGradeFilter = new System.Windows.Forms.Label();
            this.lblAgeRange = new System.Windows.Forms.Label();
            this.lblScoreRange = new System.Windows.Forms.Label();
            this.lblNameFilter = new System.Windows.Forms.Label();
            this.tmrSearchDebounce = new System.Windows.Forms.Timer(this.components);
            this.pnlStats = new System.Windows.Forms.Panel();
            this.lblStats = new System.Windows.Forms.Label();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.tableLayoutInput = new System.Windows.Forms.TableLayoutPanel();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblAge = new System.Windows.Forms.Label();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.lblGrade = new System.Windows.Forms.Label();
            this.txtGrade = new System.Windows.Forms.TextBox();
            this.lblScore = new System.Windows.Forms.Label();
            this.txtScore = new System.Windows.Forms.TextBox();
            this.grpActions = new System.Windows.Forms.GroupBox();
            this.btnAddOrUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblWeather = new System.Windows.Forms.Label();
            this.lblComputer = new System.Windows.Forms.Label();
            this.lblGreeting = new System.Windows.Forms.Label();
            this.lblQuickStats = new System.Windows.Forms.Label();
            this.grpBatch = new System.Windows.Forms.GroupBox();
            this.btnBatchDelete = new System.Windows.Forms.Button();
            this.btnBatchChangeGrade = new System.Windows.Forms.Button();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.dgvStudents = new System.Windows.Forms.DataGridView();
            this.pnlPagination = new System.Windows.Forms.Panel();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.lblPageSize = new System.Windows.Forms.Label();
            this.nudPageSize = new System.Windows.Forms.NumericUpDown();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.grpCharts = new System.Windows.Forms.GroupBox();
            this.picScoreChart = new System.Windows.Forms.PictureBox();
            this.picGradeChart = new System.Windows.Forms.PictureBox();
            this.grpImport = new System.Windows.Forms.GroupBox();
            this.btnImportCsv = new System.Windows.Forms.Button();
            this.btnImportExcel = new System.Windows.Forms.Button();
            this.splitChart = new System.Windows.Forms.SplitContainer();
            this.pnlTop.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.pnlStats.SuspendLayout();
            this.mainLayout.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.grpInput.SuspendLayout();
            this.tableLayoutInput.SuspendLayout();
            this.grpActions.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.grpBatch.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            this.pnlPagination.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPageSize)).BeginInit();
            this.pnlRight.SuspendLayout();
            this.grpCharts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picScoreChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGradeChart)).BeginInit();
            this.grpImport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitChart)).BeginInit();
            this.splitChart.Panel1.SuspendLayout();
            this.splitChart.Panel2.SuspendLayout();
            this.splitChart.SuspendLayout();
            this.SuspendLayout();

            // pnlTop - 顶部工具栏
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.btnImport);
            this.pnlTop.Controls.Add(this.btnExportExcel);
            this.pnlTop.Controls.Add(this.btnExportCsv);
            this.pnlTop.Controls.Add(this.btnBackup);
            this.pnlTop.Controls.Add(this.btnRestore);
            this.pnlTop.Controls.Add(this.btnUserManagement);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Height = 50;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(22, 12);
            this.lblTitle.Text = "学生管理系统";

            // top buttons (right-aligned, right-to-left)
            this.btnRestore.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.btnRestore.FlatAppearance.BorderSize = 0;
            this.btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestore.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnRestore.ForeColor = System.Drawing.Color.White;
            this.btnRestore.Location = new System.Drawing.Point(1310, 11);
            this.btnRestore.Size = new System.Drawing.Size(65, 28);
            this.btnRestore.Text = "恢复";
            this.btnRestore.UseVisualStyleBackColor = false;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);

            this.btnBackup.BackColor = System.Drawing.Color.FromArgb(155, 89, 182);
            this.btnBackup.FlatAppearance.BorderSize = 0;
            this.btnBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackup.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnBackup.ForeColor = System.Drawing.Color.White;
            this.btnBackup.Location = new System.Drawing.Point(1235, 11);
            this.btnBackup.Size = new System.Drawing.Size(65, 28);
            this.btnBackup.Text = "备份";
            this.btnBackup.UseVisualStyleBackColor = false;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);

            this.btnExportCsv.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnExportCsv.FlatAppearance.BorderSize = 0;
            this.btnExportCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportCsv.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnExportCsv.ForeColor = System.Drawing.Color.White;
            this.btnExportCsv.Location = new System.Drawing.Point(1145, 11);
            this.btnExportCsv.Size = new System.Drawing.Size(80, 28);
            this.btnExportCsv.Text = "导出CSV";
            this.btnExportCsv.UseVisualStyleBackColor = false;
            this.btnExportCsv.Click += new System.EventHandler(this.btnExportCsv_Click);

            this.btnExportExcel.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnExportExcel.FlatAppearance.BorderSize = 0;
            this.btnExportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportExcel.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnExportExcel.ForeColor = System.Drawing.Color.White;
            this.btnExportExcel.Location = new System.Drawing.Point(1050, 11);
            this.btnExportExcel.Size = new System.Drawing.Size(85, 28);
            this.btnExportExcel.Text = "导出Excel";
            this.btnExportExcel.UseVisualStyleBackColor = false;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);

            this.btnImport.BackColor = System.Drawing.Color.FromArgb(241, 196, 15);
            this.btnImport.FlatAppearance.BorderSize = 0;
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnImport.ForeColor = System.Drawing.Color.White;
            this.btnImport.Location = new System.Drawing.Point(965, 11);
            this.btnImport.Size = new System.Drawing.Size(75, 28);
            this.btnImport.Text = "导入";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);

            this.btnUserManagement.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.btnUserManagement.FlatAppearance.BorderSize = 0;
            this.btnUserManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserManagement.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnUserManagement.ForeColor = System.Drawing.Color.White;
            this.btnUserManagement.Location = new System.Drawing.Point(875, 11);
            this.btnUserManagement.Size = new System.Drawing.Size(80, 28);
            this.btnUserManagement.Text = "用户管理";
            this.btnUserManagement.UseVisualStyleBackColor = false;
            this.btnUserManagement.Click += new System.EventHandler(this.btnUserManagement_Click);

            // pnlFilter - 筛选栏
            this.pnlFilter.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.pnlFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilter.Controls.Add(this.lblNameFilter);
            this.pnlFilter.Controls.Add(this.txtSearchName);
            this.pnlFilter.Controls.Add(this.cmbGrade);
            this.pnlFilter.Controls.Add(this.lblGradeFilter);
            this.pnlFilter.Controls.Add(this.txtAgeMin);
            this.pnlFilter.Controls.Add(this.txtAgeMax);
            this.pnlFilter.Controls.Add(this.lblAgeRange);
            this.pnlFilter.Controls.Add(this.txtScoreMin);
            this.pnlFilter.Controls.Add(this.txtScoreMax);
            this.pnlFilter.Controls.Add(this.lblScoreRange);
            this.pnlFilter.Controls.Add(this.btnFilter);
            this.pnlFilter.Controls.Add(this.btnClearFilter);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Height = 46;

            this.lblNameFilter.AutoSize = true;
            this.lblNameFilter.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.lblNameFilter.Location = new System.Drawing.Point(15, 14);
            this.lblNameFilter.Text = "搜索：";

            this.txtSearchName.Location = new System.Drawing.Point(62, 11);
            this.txtSearchName.Size = new System.Drawing.Size(130, 23);
            this.txtSearchName.TextChanged += new System.EventHandler(this.txtSearchName_TextChanged);

            this.lblGradeFilter.AutoSize = true;
            this.lblGradeFilter.Location = new System.Drawing.Point(205, 14);
            this.lblGradeFilter.Text = "班级：";

            this.cmbGrade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGrade.Location = new System.Drawing.Point(248, 11);
            this.cmbGrade.Size = new System.Drawing.Size(90, 25);
            this.cmbGrade.SelectedIndexChanged += new System.EventHandler(this.cmbGrade_SelectedIndexChanged);

            this.lblAgeRange.AutoSize = true;
            this.lblAgeRange.Location = new System.Drawing.Point(350, 14);
            this.lblAgeRange.Text = "年龄：";

            this.txtAgeMin.Location = new System.Drawing.Point(395, 11);
            this.txtAgeMin.Size = new System.Drawing.Size(45, 23);
            this.txtAgeMin.TextChanged += new System.EventHandler(this.filterField_TextChanged);

            this.txtAgeMax.Location = new System.Drawing.Point(445, 11);
            this.txtAgeMax.Size = new System.Drawing.Size(45, 23);
            this.txtAgeMax.TextChanged += new System.EventHandler(this.filterField_TextChanged);

            this.lblScoreRange.AutoSize = true;
            this.lblScoreRange.Location = new System.Drawing.Point(500, 14);
            this.lblScoreRange.Text = "成绩：";

            this.txtScoreMin.Location = new System.Drawing.Point(545, 11);
            this.txtScoreMin.Size = new System.Drawing.Size(45, 23);
            this.txtScoreMin.TextChanged += new System.EventHandler(this.filterField_TextChanged);

            this.txtScoreMax.Location = new System.Drawing.Point(595, 11);
            this.txtScoreMax.Size = new System.Drawing.Size(45, 23);
            this.txtScoreMax.TextChanged += new System.EventHandler(this.filterField_TextChanged);

            this.btnFilter.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.btnFilter.FlatAppearance.BorderSize = 0;
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.ForeColor = System.Drawing.Color.White;
            this.btnFilter.Location = new System.Drawing.Point(655, 9);
            this.btnFilter.Size = new System.Drawing.Size(70, 28);
            this.btnFilter.Text = "筛选";
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);

            this.btnClearFilter.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnClearFilter.FlatAppearance.BorderSize = 0;
            this.btnClearFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearFilter.ForeColor = System.Drawing.Color.White;
            this.btnClearFilter.Location = new System.Drawing.Point(735, 9);
            this.btnClearFilter.Size = new System.Drawing.Size(70, 28);
            this.btnClearFilter.Text = "清除";
            this.btnClearFilter.UseVisualStyleBackColor = false;
            this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);

            // tmrSearchDebounce
            this.tmrSearchDebounce.Interval = 350;
            this.tmrSearchDebounce.Tick += new System.EventHandler(this.tmrSearchDebounce_Tick);

            // pnlStats - 底部统计栏
            this.pnlStats.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.pnlStats.Controls.Add(this.lblStats);
            this.pnlStats.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStats.Height = 42;

            this.lblStats.AutoSize = true;
            this.lblStats.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblStats.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblStats.Location = new System.Drawing.Point(15, 12);

            // mainLayout - 三栏主布局
            this.mainLayout.ColumnCount = 3;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 340F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 390F));
            this.mainLayout.Controls.Add(this.pnlLeft, 0, 0);
            this.mainLayout.Controls.Add(this.pnlCenter, 1, 0);
            this.mainLayout.Controls.Add(this.pnlRight, 2, 0);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.RowCount = 1;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            // ====== pnlLeft - 左侧: 输入 + 操作 ======
            this.pnlLeft.Controls.Add(this.grpBatch);
            this.pnlLeft.Controls.Add(this.pnlInfo);
            this.pnlLeft.Controls.Add(this.grpActions);
            this.pnlLeft.Controls.Add(this.grpInput);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(6);

            // grpInput
            this.grpInput.Controls.Add(this.tableLayoutInput);
            this.grpInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpInput.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.grpInput.Height = 210;
            this.grpInput.Text = "学生信息";

            this.tableLayoutInput.ColumnCount = 2;
            this.tableLayoutInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutInput.Controls.Add(this.lblName, 0, 0);
            this.tableLayoutInput.Controls.Add(this.txtName, 1, 0);
            this.tableLayoutInput.Controls.Add(this.lblAge, 0, 1);
            this.tableLayoutInput.Controls.Add(this.txtAge, 1, 1);
            this.tableLayoutInput.Controls.Add(this.lblGrade, 0, 2);
            this.tableLayoutInput.Controls.Add(this.txtGrade, 1, 2);
            this.tableLayoutInput.Controls.Add(this.lblScore, 0, 3);
            this.tableLayoutInput.Controls.Add(this.txtScore, 1, 3);
            this.tableLayoutInput.Location = new System.Drawing.Point(12, 28);
            this.tableLayoutInput.RowCount = 4;
            this.tableLayoutInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutInput.Size = new System.Drawing.Size(300, 168);

            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(3, 10);
            this.lblName.Text = "姓名：";
            this.txtName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtName.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtName.Location = new System.Drawing.Point(58, 3);

            this.lblAge.AutoSize = true;
            this.lblAge.Location = new System.Drawing.Point(3, 50);
            this.lblAge.Text = "年龄：";
            this.txtAge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAge.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtAge.Location = new System.Drawing.Point(58, 43);

            this.lblGrade.AutoSize = true;
            this.lblGrade.Location = new System.Drawing.Point(3, 90);
            this.lblGrade.Text = "班级：";
            this.txtGrade.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGrade.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtGrade.Location = new System.Drawing.Point(58, 83);

            this.lblScore.AutoSize = true;
            this.lblScore.Location = new System.Drawing.Point(3, 130);
            this.lblScore.Text = "成绩：";
            this.txtScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtScore.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.txtScore.Location = new System.Drawing.Point(58, 123);

            // grpActions
            this.grpActions.Controls.Add(this.btnAddOrUpdate);
            this.grpActions.Controls.Add(this.btnDelete);
            this.grpActions.Controls.Add(this.btnClear);
            this.grpActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpActions.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.grpActions.Height = 125;
            this.grpActions.Text = "操作";

            this.btnAddOrUpdate.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.btnAddOrUpdate.FlatAppearance.BorderSize = 0;
            this.btnAddOrUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddOrUpdate.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddOrUpdate.ForeColor = System.Drawing.Color.White;
            this.btnAddOrUpdate.Location = new System.Drawing.Point(15, 28);
            this.btnAddOrUpdate.Size = new System.Drawing.Size(300, 36);
            this.btnAddOrUpdate.Text = "添加";
            this.btnAddOrUpdate.UseVisualStyleBackColor = false;
            this.btnAddOrUpdate.Click += new System.EventHandler(this.btnAddOrUpdate_Click);

            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(15, 72);
            this.btnDelete.Size = new System.Drawing.Size(145, 36);
            this.btnDelete.Text = "删除选中";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            this.btnClear.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(170, 72);
            this.btnClear.Size = new System.Drawing.Size(145, 36);
            this.btnClear.Text = "清空表单";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // pnlInfo - 信息面板（时钟/天气/系统信息）
            this.pnlInfo.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);
            this.pnlInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInfo.Controls.Add(this.lblTime);
            this.pnlInfo.Controls.Add(this.lblDate);
            this.pnlInfo.Controls.Add(this.lblWeather);
            this.pnlInfo.Controls.Add(this.lblComputer);
            this.pnlInfo.Controls.Add(this.lblGreeting);
            this.pnlInfo.Controls.Add(this.lblQuickStats);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInfo.Padding = new System.Windows.Forms.Padding(12);

            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold);
            this.lblTime.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblTime.Location = new System.Drawing.Point(15, 15);

            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(127, 140, 141);
            this.lblDate.Location = new System.Drawing.Point(18, 48);

            this.lblWeather.AutoSize = true;
            this.lblWeather.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lblWeather.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblWeather.Location = new System.Drawing.Point(18, 75);

            this.lblComputer.AutoSize = true;
            this.lblComputer.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblComputer.ForeColor = System.Drawing.Color.FromArgb(127, 140, 141);
            this.lblComputer.Location = new System.Drawing.Point(18, 102);

            this.lblGreeting.AutoSize = true;
            this.lblGreeting.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.lblGreeting.ForeColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.lblGreeting.Location = new System.Drawing.Point(18, 130);

            this.lblQuickStats.AutoSize = false;
            this.lblQuickStats.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblQuickStats.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblQuickStats.Location = new System.Drawing.Point(18, 155);
            this.lblQuickStats.Size = new System.Drawing.Size(310, 60);

            // grpBatch
            this.grpBatch.Controls.Add(this.btnBatchDelete);
            this.grpBatch.Controls.Add(this.btnBatchChangeGrade);
            this.grpBatch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpBatch.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.grpBatch.Height = 85;
            this.grpBatch.Text = "批量操作";

            this.btnBatchDelete.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
            this.btnBatchDelete.FlatAppearance.BorderSize = 0;
            this.btnBatchDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchDelete.ForeColor = System.Drawing.Color.White;
            this.btnBatchDelete.Location = new System.Drawing.Point(15, 28);
            this.btnBatchDelete.Size = new System.Drawing.Size(145, 36);
            this.btnBatchDelete.Text = "批量删除";
            this.btnBatchDelete.UseVisualStyleBackColor = false;
            this.btnBatchDelete.Click += new System.EventHandler(this.btnBatchDelete_Click);

            this.btnBatchChangeGrade.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnBatchChangeGrade.FlatAppearance.BorderSize = 0;
            this.btnBatchChangeGrade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchChangeGrade.ForeColor = System.Drawing.Color.White;
            this.btnBatchChangeGrade.Location = new System.Drawing.Point(170, 28);
            this.btnBatchChangeGrade.Size = new System.Drawing.Size(145, 36);
            this.btnBatchChangeGrade.Text = "批量改班级";
            this.btnBatchChangeGrade.UseVisualStyleBackColor = false;
            this.btnBatchChangeGrade.Click += new System.EventHandler(this.btnBatchChangeGrade_Click);

            // ====== pnlCenter - 中间: 表格 + 分页 ======
            this.pnlCenter.Controls.Add(this.dgvStudents);
            this.pnlCenter.Controls.Add(this.pnlPagination);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;

            this.dgvStudents.AllowUserToAddRows = false;
            this.dgvStudents.AllowUserToDeleteRows = false;
            this.dgvStudents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStudents.BackgroundColor = System.Drawing.Color.White;
            this.dgvStudents.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvStudents.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvStudents.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStudents.GridColor = System.Drawing.Color.FromArgb(230, 230, 230);
            this.dgvStudents.ReadOnly = true;
            this.dgvStudents.RowHeadersVisible = false;
            this.dgvStudents.RowTemplate.Height = 28;
            this.dgvStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStudents.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvStudents_ColumnHeaderMouseClick);
            this.dgvStudents.SelectionChanged += new System.EventHandler(this.dgvStudents_SelectionChanged);

            // pnlPagination
            this.pnlPagination.Controls.Add(this.btnPrevPage);
            this.pnlPagination.Controls.Add(this.lblPageInfo);
            this.pnlPagination.Controls.Add(this.btnNextPage);
            this.pnlPagination.Controls.Add(this.lblPageSize);
            this.pnlPagination.Controls.Add(this.nudPageSize);
            this.pnlPagination.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPagination.Height = 45;

            this.btnPrevPage.Location = new System.Drawing.Point(10, 10);
            this.btnPrevPage.Size = new System.Drawing.Size(75, 25);
            this.btnPrevPage.Text = "上一页";
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);

            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.Location = new System.Drawing.Point(100, 13);

            this.btnNextPage.Location = new System.Drawing.Point(280, 10);
            this.btnNextPage.Size = new System.Drawing.Size(75, 25);
            this.btnNextPage.Text = "下一页";
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);

            this.lblPageSize.AutoSize = true;
            this.lblPageSize.Location = new System.Drawing.Point(390, 13);
            this.lblPageSize.Text = "每页条数";

            this.nudPageSize.Location = new System.Drawing.Point(455, 10);
            this.nudPageSize.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            this.nudPageSize.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            this.nudPageSize.Size = new System.Drawing.Size(55, 23);
            this.nudPageSize.Value = new decimal(new int[] { 20, 0, 0, 0 });

            // ====== pnlRight - 右侧: 图表 + 导入 ======
            this.pnlRight.Controls.Add(this.grpCharts);
            this.pnlRight.Controls.Add(this.grpImport);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Padding = new System.Windows.Forms.Padding(6);

            // grpCharts
            this.grpCharts.Controls.Add(this.splitChart);
            this.grpCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCharts.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.grpCharts.Text = "统计图表";

            this.splitChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitChart.Location = new System.Drawing.Point(3, 22);
            this.splitChart.Name = "splitChart";
            this.splitChart.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitChart.SplitterDistance = 250;

            this.picGradeChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picGradeChart.BackColor = System.Drawing.Color.White;
            this.picGradeChart.Paint += new System.Windows.Forms.PaintEventHandler(this.picGradeChart_Paint);

            this.picScoreChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picScoreChart.BackColor = System.Drawing.Color.White;
            this.picScoreChart.Paint += new System.Windows.Forms.PaintEventHandler(this.picScoreChart_Paint);

            this.splitChart.Panel1.Controls.Add(this.picGradeChart);
            this.splitChart.Panel2.Controls.Add(this.picScoreChart);

            // grpImport
            this.grpImport.Controls.Add(this.btnImportCsv);
            this.grpImport.Controls.Add(this.btnImportExcel);
            this.grpImport.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpImport.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold);
            this.grpImport.Height = 65;
            this.grpImport.Text = "数据导入";

            this.btnImportExcel.BackColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.btnImportExcel.FlatAppearance.BorderSize = 0;
            this.btnImportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportExcel.ForeColor = System.Drawing.Color.White;
            this.btnImportExcel.Location = new System.Drawing.Point(15, 25);
            this.btnImportExcel.Size = new System.Drawing.Size(150, 30);
            this.btnImportExcel.Text = "从Excel导入";
            this.btnImportExcel.UseVisualStyleBackColor = false;
            this.btnImportExcel.Click += new System.EventHandler(this.btnImportExcel_Click);

            this.btnImportCsv.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnImportCsv.FlatAppearance.BorderSize = 0;
            this.btnImportCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportCsv.ForeColor = System.Drawing.Color.White;
            this.btnImportCsv.Location = new System.Drawing.Point(175, 25);
            this.btnImportCsv.Size = new System.Drawing.Size(150, 30);
            this.btnImportCsv.Text = "从CSV导入";
            this.btnImportCsv.UseVisualStyleBackColor = false;
            this.btnImportCsv.Click += new System.EventHandler(this.btnImportCsv_Click);

            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1400, 860);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.pnlStats);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "学生管理系统";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);

            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.pnlStats.ResumeLayout(false);
            this.pnlStats.PerformLayout();
            this.mainLayout.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.grpInput.ResumeLayout(false);
            this.tableLayoutInput.ResumeLayout(false);
            this.tableLayoutInput.PerformLayout();
            this.grpActions.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.grpBatch.ResumeLayout(false);
            this.pnlCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            this.pnlPagination.ResumeLayout(false);
            this.pnlPagination.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPageSize)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.grpCharts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picScoreChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGradeChart)).EndInit();
            this.grpImport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitChart)).EndInit();
            this.splitChart.Panel1.ResumeLayout(false);
            this.splitChart.Panel2.ResumeLayout(false);
            this.splitChart.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnExportCsv;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnUserManagement;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.TextBox txtSearchName;
        private System.Windows.Forms.ComboBox cmbGrade;
        private System.Windows.Forms.TextBox txtAgeMin;
        private System.Windows.Forms.TextBox txtAgeMax;
        private System.Windows.Forms.TextBox txtScoreMin;
        private System.Windows.Forms.TextBox txtScoreMax;
        private System.Windows.Forms.Label lblNameFilter;
        private System.Windows.Forms.Label lblGradeFilter;
        private System.Windows.Forms.Label lblAgeRange;
        private System.Windows.Forms.Label lblScoreRange;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnClearFilter;
        private System.Windows.Forms.Timer tmrSearchDebounce;
        private System.Windows.Forms.Panel pnlStats;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.GroupBox grpInput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutInput;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblAge;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.Label lblGrade;
        private System.Windows.Forms.TextBox txtGrade;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.TextBox txtScore;
        private System.Windows.Forms.GroupBox grpActions;
        private System.Windows.Forms.Button btnAddOrUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox grpBatch;
        private System.Windows.Forms.Button btnBatchDelete;
        private System.Windows.Forms.Button btnBatchChangeGrade;
        private System.Windows.Forms.DataGridView dgvStudents;
        private System.Windows.Forms.Panel pnlPagination;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Label lblPageInfo;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Label lblPageSize;
        private System.Windows.Forms.NumericUpDown nudPageSize;
        private System.Windows.Forms.GroupBox grpCharts;
        private System.Windows.Forms.SplitContainer splitChart;
        private System.Windows.Forms.PictureBox picGradeChart;
        private System.Windows.Forms.PictureBox picScoreChart;
        private System.Windows.Forms.GroupBox grpImport;
        private System.Windows.Forms.Button btnImportExcel;
        private System.Windows.Forms.Button btnImportCsv;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblWeather;
        private System.Windows.Forms.Label lblComputer;
        private System.Windows.Forms.Label lblGreeting;
        private System.Windows.Forms.Label lblQuickStats;
    }
}
