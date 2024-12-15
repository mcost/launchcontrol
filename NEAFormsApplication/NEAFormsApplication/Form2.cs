using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NEAFormsApplication
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            LoadNames();
            LoadSurnames();
            LoadMembershipID();
            GetMembers();
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox5.Enabled = false;
            comboBox6.Enabled = false;
            comboBox8.Enabled = false;
            comboBox9.Enabled = false;
            comboBox1.TextChanged += comboBox1_TextChanged;
            comboBox1.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
            comboBox2.TextChanged += comboBox2_TextChanged;
            comboBox4.TextChanged += comboBox4_TextChanged;
            comboBox5.TextChanged += comboBox5_TextChanged;
            comboBox7.TextChanged += comboBox7_TextChanged;
            comboBox8.TextChanged += comboBox8_TextChanged;
        }
        private string connectionString = "Server=DESKTOP-CMMVASL\\SQLEXPRESS;Database=LaunchControlSystem;Integrated Security=True;";
        private void LoadNames()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT Name FROM MEMBERCLUB", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["Name"].ToString());
                    comboBox4.Items.Add(reader["Name"].ToString());
                    comboBox7.Items.Add(reader["Name"].ToString());
                }

                reader.Close();
            }
        }
        private void LoadSurnames()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT DISTINCT Surname FROM MEMBERCLUB", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["Surname"].ToString());
                    comboBox5.Items.Add(reader["Surname"].ToString());
                    comboBox8.Items.Add(reader["Surname"].ToString());
                }

                reader.Close();
            }
        }
        private void LoadMembershipID()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT Name, Surname, ID FROM MEMBERCLUB", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string name = reader["Name"].ToString();
                    string surname = reader["Surname"].ToString();
                    int id = Convert.ToInt32(reader["ID"]);
                    string displayText = $"{id} - {name} {surname}";

                    comboBox3.Items.Add(displayText);
                    comboBox6.Items.Add(displayText);
                    comboBox9.Items.Add(displayText);
                }

                reader.Close();
            }
        }
        public class Member
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int MembershipNumber { get; set; }

            public override string ToString()
            {
                return $"{MembershipNumber} ({Name} {Surname}) ";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.Show();
            this.Hide();
            var form6 = new Form6();
            form6.Closed += (s, args) => this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
            var form3 = new Form2();
            form3.Closed += (s, args) => this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
            this.Hide();
            var form4 = new Form2();
            form4.Closed += (s, args) => this.Close();
        }

        private void LoadMemberDetails(string name)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))

                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT Surname, MemberTypeID, MembershipStart, MembershipEnd FROM MEMBERCLUB WHERE Name = @Name", connection);
                    command.Parameters.AddWithValue("@Name", name);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string surname = reader["Surname"].ToString();
                        int memberTypeID = Convert.ToInt32(reader["MemberTypeID"]);
                        DateTime membershipStart = Convert.ToDateTime(reader["MembershipStart"]);
                        DateTime membershipEnd = Convert.ToDateTime(reader["MembershipEnd"]);
                    }

                    reader.Close();
                }
        }
        private bool IsValidName(string Name)
        {
            bool IsValid = false;
            string query = "SELECT COUNT(*) FROM MEMBERCLUB WHERE Name = @Name";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", Name);
                connection.Open();
                int count = (int)command.ExecuteScalar();
                IsValid = count > 0;
            }
            return IsValid;
        }
        private bool IsValidSurname(string Name)
        {
            bool IsValid = false;
            string query = "SELECT COUNT(*) FROM MEMBERCLUB WHERE Surname = @Surname";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Surname", Name);
                connection.Open();
                int count = (int)command.ExecuteScalar();
                IsValid = count > 0;
            }
            return IsValid;
        }
        private List<string> GetSurnamesByFirstName(string firstName)
        {
            List<string> surnames = new List<string>();
            string query = "SELECT SURNAME FROM MEMBERCLUB WHERE Name LIKE @namePattern";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@namePattern", firstName + "%");
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        surnames.Add(reader["SURNAME"].ToString());
                    }
                }
            }
            return surnames;
        }
        public List<Member> GetMembers()
        {
            List<Member> members = new List<Member>();
            string query = "SELECT Name, Surname, ID AS MembershipNumber FROM LaunchControlSystem.dbo.MEMBERCLUB ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        members.Add(new Member
                        {
                            Name = reader["Name"].ToString(),
                            Surname = reader["Surname"].ToString(),
                            MembershipNumber = (int)reader["MembershipNumber"]
                        });
                    }
                }
            }

            return members;
        }
        public void SubmitForm2Data(string pilot1FirstName, string pilot1Surname, string pilot2FirstName, string pilot2Surname, int pilot1MembershipNumber, int pilot2MembershipNumber)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(
                            "INSERT INTO MEMBERCLUB (FirstName, Surname, ID) VALUES (@FirstName, @Surname, @MembershipNumber)", 
                            connection, transaction);

                        command.Parameters.AddWithValue("@FirstName", pilot1FirstName);
                        command.Parameters.AddWithValue("@Surname", pilot1Surname);
                        command.Parameters.AddWithValue("@MembershipNumber", pilot1MembershipNumber);
                        command.Parameters.AddWithValue("@MembershipNumber", pilot2MembershipNumber);
                        command.ExecuteNonQuery();

                        transaction.Commit();
                        MessageBox.Show("Pilot details submitted successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error submitting pilot data: " + ex.Message);
                    }
                }
            }
        }
        
        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox2.Enabled = IsValidName(comboBox1.Text);
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            string selectedName = comboBox1.Text;
            List<string> surnames = GetSurnamesByFirstName(selectedName);
            foreach (string surname in surnames)
            {
                comboBox2.Items.Add(surname);
            }
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                string selectedName = comboBox2.SelectedItem.ToString();
                LoadMemberDetails(selectedName);
            }
        }
        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            comboBox3.Enabled = IsValidSurname(comboBox2.Text);
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem is Member selectedMember)
            {
                MessageBox.Show($"Selected Member: {selectedMember}");
            }
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox5.Items.Clear();
            string selectedName = comboBox4.Text;
            List<string> surnames = GetSurnamesByFirstName(selectedName);
            foreach (string surname in surnames)
            {
                comboBox5.Items.Add(surname);
            }
        }
        private void comboBox4_TextChanged(object sender, EventArgs e)
        {
            comboBox5.Enabled = IsValidName(comboBox4.Text);
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedItem != null)
            {
                string selectedName = comboBox5.SelectedItem.ToString();
                LoadMemberDetails(selectedName);
            }
        }
        private void comboBox5_TextChanged(object sender, EventArgs e)
        {
            comboBox6.Enabled = IsValidSurname(comboBox5.Text);
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.SelectedItem is Member selectedMember)
            {
                MessageBox.Show($"Selected Member: {selectedMember}");
            }
        }


        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox8.Items.Clear();
            string selectedName = comboBox7.Text;
            List<string> surnames = GetSurnamesByFirstName(selectedName);
            foreach (string surname in surnames)
            {
                comboBox8.Items.Add(surname);
            }
        }
        private void comboBox7_TextChanged(object sender, EventArgs e)
        {
            comboBox8.Enabled = IsValidName(comboBox7.Text);
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox8.SelectedItem != null)
            {
                string selectedName = comboBox8.SelectedItem.ToString();
                LoadMemberDetails(selectedName);
            }
        }
        private void comboBox8_TextChanged(object sender, EventArgs e)
        {
            comboBox9.Enabled = IsValidSurname(comboBox8.Text);
        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox9.SelectedItem is Member selectedMember)
            {
                MessageBox.Show($"Selected Member: {selectedMember}");
            }
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.Show();
            this.Hide();
            var form5 = new Form5();
            form5.Closed += (s, args) => this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
