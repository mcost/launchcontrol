using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;


namespace NEAControllerFormsApplication
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            LoadDataGridView(); 
        }

        private void LoadDataGridView()
        {
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


            LoadData();
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
                DataPropertyName = name
            };
            dataGridView1.Columns.Add(comboBoxColumn);
        }


        private void LoadData()
        {
            string fetchQuery = "SELECT \r\n f.TakeoffTime, \r\n    f.LandingTime, \r\n    gt.GliderType\r\nFROM \r\n    LaunchControlSystem.dbo.FLIGHT AS f\r\nINNER JOIN \r\n    LaunchControlSystem.dbo.GLIDERS AS g ON f.GliderID = g.ID\r\nINNER JOIN \r\n    LaunchControlSystem.dbo.GLIDERTYPE AS gt ON g.GliderTypeID = gt.ID;"; // Adjust this query to match your database structure
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
                string pathToProjectBExe = @"____";
                Process.Start(pathToProjectBExe);
            }
            else
            {

            }
        }
    }
}