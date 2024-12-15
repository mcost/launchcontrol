using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComboBox = System.Windows.Forms.ComboBox;



namespace NEAControllerFormsApplication
{
    public partial class Form2 : Form
    {
        private Timer timer;
        public Form2()
        {
            InitializeComponent();
            LoadDataGridView();
            InitializeTimer();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += label15_Click;
            timer.Tick += label16_Click;
            timer.Start();
            dataGridView1.RowsAdded += dataGridView1_RowsAdded;
            dataGridView1.RowsRemoved += dataGridView1_RowsRemoved;
        }

        private void LoadDataGridView()
        {
            dataGridView1.DefaultCellStyle.Font = new Font("Product Sans", 14);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Product Sans", 13, FontStyle.Bold);
            dataGridView1.RowTemplate.Height = 30;
            AddCount();
            AddComboBoxColumn("P1 Name", "P1Name", GetColumnData("SELECT DISTINCT Name FROM MemberClub"));
            AddComboBoxColumn("P1 Surname", "P1Surname", GetColumnData("SELECT DISTINCT Surname FROM MemberClub"));
            AddComboBoxColumn("P1 Membership Number", "P1MembershipNumber", GetColumnData("SELECT DISTINCT MemberTypeID FROM MemberClub"));

            AddComboBoxColumn("P2 Name", "P2Name", GetColumnData("SELECT DISTINCT Name FROM MemberClub"));
            AddComboBoxColumn("P2 Surname", "P2Surname", GetColumnData("SELECT DISTINCT Surname FROM MemberClub"));
            AddComboBoxColumn("P2 Membership Number", "P2MembershipNumber", GetColumnData("SELECT DISTINCT MemberTypeID FROM MemberClub"));

            AddComboBoxColumn("Glider Type", "GliderType", GetColumnData("SELECT DISTINCT GliderType FROM GliderType"));
            AddComboBoxColumn("Glider Registration", "GliderRegistration", GetColumnData("SELECT DISTINCT GliderREG FROM Gliders"));

            AddComboBoxColumn("Launch Type", "LaunchType", GetColumnData("SELECT DISTINCT Type FROM LAUNCHTYPE"));

            AddComboBoxColumn("Takeoff Time", "TakeoffTime", GetColumnData("SELECT DISTINCT TakeoffTime FROM FLIGHT"));
            AddComboBoxColumn("Landing Time", "LandingTime", GetColumnData("SELECT DISTINCT LandingTime FROM FLIGHT"));
            AddComboBoxColumn("Total Flight Time", "TotalFlightTime", GetColumnData("SELECT DISTINCT TotalFlightTime FROM FLIGHT"));

            LoadData();
        }
        private void RefreshDataGridView()
        {
            (dataGridView1.Columns["P1Name"] as DataGridViewComboBoxColumn).DataSource = GetColumnData("SELECT DISTINCT Name FROM MemberClub");
            (dataGridView1.Columns["P1Surname"] as DataGridViewComboBoxColumn).DataSource = GetColumnData("SELECT DISTINCT Surname FROM MemberClub");
            (dataGridView1.Columns["P1MembershipNumber"] as DataGridViewComboBoxColumn).DataSource = GetColumnData("SELECT DISTINCT MemberTypeID FROM MemberClub");

            (dataGridView1.Columns["P2Name"] as DataGridViewComboBoxColumn).DataSource = GetColumnData("SELECT DISTINCT Name FROM MemberClub");
            (dataGridView1.Columns["P2Surname"] as DataGridViewComboBoxColumn).DataSource = GetColumnData("SELECT DISTINCT Surname FROM MemberClub");
            (dataGridView1.Columns["P2MembershipNumber"] as DataGridViewComboBoxColumn).DataSource = GetColumnData("SELECT DISTINCT MemberTypeID FROM MemberClub");

            (dataGridView1.Columns["GliderType"] as DataGridViewComboBoxColumn).DataSource = GetColumnData("SELECT DISTINCT GliderType FROM GliderType");
            (dataGridView1.Columns["GliderRegistration"] as DataGridViewComboBoxColumn).DataSource = GetColumnData("SELECT DISTINCT GliderREG FROM Gliders");

            (dataGridView1.Columns["LaunchType"] as DataGridViewComboBoxColumn).DataSource = GetColumnData("SELECT DISTINCT Type FROM LAUNCHTYPE");

            (dataGridView1.Columns["TakeoffTime"] as DataGridViewComboBoxColumn).DataSource = GetColumnData("SELECT DISTINCT TakeoffTime FROM FLIGHT");
            (dataGridView1.Columns["LandingTime"] as DataGridViewComboBoxColumn).DataSource = GetColumnData("SELECT DISTINCT LandingTime FROM FLIGHT");
            (dataGridView1.Columns["TotalFlightTime"] as DataGridViewComboBoxColumn).DataSource = GetColumnData("SELECT DISTINCT TotalFlightTime FROM FLIGHT");
        }
        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox comboBox)
            {
                var currentColumn = dataGridView1.CurrentCell.OwningColumn;
                comboBox.DropDownWidth = currentColumn.Width;
                comboBox.IntegralHeight = false;
                comboBox.DropDownHeight = 200;
            }
        }
        private void AddCount()
        {
            DataGridViewTextBoxColumn counterColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Flight No.",
                Name = "FlightCount",
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            };
            dataGridView1.Columns.Insert(0, counterColumn);  
        }
        private DataTable GetColumnData(string query)
        {
            string connectionString = "Server=DESKTOP-CMMVASL\\SQLEXPRESS;Database=LaunchControlSystem;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        private void AddComboBoxColumn(string headerText, string name, DataTable dataSource)
        {
            DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn
            {
                HeaderText = headerText,
                Name = name,
                DataSource = dataSource,
                DisplayMember = dataSource.Columns[0].ColumnName,
                ValueMember = dataSource.Columns[0].ColumnName,
                DataPropertyName = name,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            dataGridView1.Columns.Add(comboBoxColumn);
        }


        private void LoadData()
        {
            string fetchQuery = "SELECT \r\n f.TakeoffTime, \r\n    f.LandingTime, \r\n    gt.GliderType\r\nFROM \r\n    LaunchControlSystem.dbo.FLIGHT AS f\r\nINNER JOIN \r\n    LaunchControlSystem.dbo.GLIDERS AS g ON f.GliderID = g.ID\r\nINNER JOIN \r\n    LaunchControlSystem.dbo.GLIDERTYPE AS gt ON g.GliderTypeID = gt.ID;";
            DataTable data = GetData(fetchQuery);
            dataGridView1.DataSource = data;
        }

        private DataTable GetData(string query)
        {
            string connectionString = "Server=DESKTOP-CMMVASL\\SQLEXPRESS;Database=LaunchControlSystem;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var cellValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                MessageBox.Show($"You clicked on cell with value: {cellValue}");
            }
        }
        private void LoadDate()
        {

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var cellValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(
            "Are you sure that you want to delete this Log?",
            "Delete Log",
            MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                Form1 f1 = new Form1();
                f1.Show();
                this.Hide();
                var form1 = new Form2();
                form1.Closed += (s, args) => this.Close();
            }
            else
            {

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(
                "Are you sure that you want to open LCS Manager?",
                "LCS Manager",
                MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                string pathToProjectBExe = @"C:\Users\Christian Costantino\source\repos\NEAManagerFormsApplication\NEAManagerFormsApplication.sln";
                Process.Start(pathToProjectBExe);
                MessageBox.Show("LCS Manager is now opening.");
            }
            else
            {
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.Show();
            var form5 = new Form2();
            form5.Closed += (s, args) => this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
            MessageBox.Show("Refreshed page succesfully");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
            MessageBox.Show("Refreshed page succesfully");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.Show();
            var form6 = new Form2();
            form6.Closed += (s, args) => this.Close();
        }
        private void label15_Click(object sender, EventArgs e)
        {
            label15.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void label16_Click(object sender, EventArgs e)
        {
            label16.Text = DateTime.Now.ToString("MM/dd/yyyy");
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }
        private DateTime startTime;
        private System.Windows.Forms.Timer elapsedTimer = new System.Windows.Forms.Timer();
        private void InitializeTimer()
        {
            startTime = DateTime.Now;
            elapsedTimer.Interval = 1000;
            elapsedTimer.Tick += ElapsedTimer_Tick;
            elapsedTimer.Start();
            UpdateElapsedTime();
        }

        void ElapsedTimer_Tick(object sender, EventArgs e)
        {
            UpdateElapsedTime();
        }

        void UpdateElapsedTime()
        {
            var elapsed = DateTime.Now.Subtract(startTime);
            label19.Text = string.Format("{0:D2}:{1:D2}:{2:D2}",
                                         elapsed.Hours, elapsed.Minutes, elapsed.Seconds);
        }
        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.Show();
            var form7 = new Form2();
            form7.Closed += (s, args) => this.Close();
        }
        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateFlightCount();
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            UpdateFlightCount();
        }

        private void UpdateFlightCount()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["FlightCount"].Value = i + 1;
            }
        }
    }
}