using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NEAFormsApplication
{
    public partial class Form6 : Form
    {
        private string connectionString = "Server=DESKTOP-CMMVASL\\SQLEXPRESS;Database=LaunchControlSystem;Integrated Security=True;";

        private void LoadGliders()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT GliderType FROM GLIDERTYPE", connection);
                SqlDataReader reader = command.ExecuteReader();

                comboBox1.Items.Clear();
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["GliderType"].ToString());
                }

                reader.Close();
            }
        }
        private void LoadGliderREG()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT GT.GliderType, G.GliderREG 
            FROM GLIDERTYPE GT
            INNER JOIN GLIDERS G ON GT.ID = G.GliderTypeID";

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                comboBox2.Items.Clear();
                while (reader.Read())
                {
                    string displayText = $"{reader["GliderType"]} - {reader["GliderREG"]}";
                    comboBox2.Items.Add(displayText);
                }

                reader.Close();
            }
        }
        private void LoadLaunchTypes()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT Type FROM LAUNCHTYPE", connection);
                SqlDataReader reader = command.ExecuteReader();

                comboBox3.Items.Clear();
                while (reader.Read())
                {
                    comboBox3.Items.Add(reader["Type"].ToString());
                }

                reader.Close();
            }
        }
        private List<string> GetGliderREGbyGliderType(string gliderType)
        {
            List<string> gliderREGList = new List<string>();
            int? gliderTypeID = null;
            string getTypeIDQuery = "SELECT ID FROM GLIDERTYPE WHERE GliderType = @gliderType";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(getTypeIDQuery, connection);
                command.Parameters.AddWithValue("@gliderType", gliderType);
                connection.Open();

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    gliderTypeID = (int)result;
                }
            }
            if (gliderTypeID.HasValue)
            {
                string getRegQuery = "SELECT GliderREG FROM GLIDERS WHERE GliderTypeID = @gliderTypeID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(getRegQuery, connection);
                    command.Parameters.AddWithValue("@gliderTypeID", gliderTypeID.Value);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            gliderREGList.Add(reader["GliderREG"].ToString());
                        }
                    }
                }
            }

            return gliderREGList;
        }
        public Form6()
        {
            InitializeComponent();
            LoadGliders();
            LoadGliderREG();
            LoadLaunchTypes();
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }
        private void SubmitForm6Data()
        {
            string gliderType = comboBox1.Text;
            string gliderREG = comboBox2.Text;
            string launchType = comboBox3.Text;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlCommand flightCommand = new SqlCommand(
                            "INSERT INTO FLIGHT (GliderType, GliderREG, LaunchType) VALUES (@GliderType, @GliderREG, @LaunchType)",
                            connection, transaction);

                        flightCommand.Parameters.AddWithValue("@GliderType", gliderType);
                        flightCommand.Parameters.AddWithValue("@GliderREG", gliderREG);
                        flightCommand.Parameters.AddWithValue("@LaunchType", launchType);

                        flightCommand.ExecuteNonQuery();

                        transaction.Commit();
                        MessageBox.Show("Flight data submitted successfully.");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error submitting flight data: " + ex.Message);
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            string selectedName = comboBox1.Text;
            List<string> GliderREG = GetGliderREGbyGliderType(selectedName);
            foreach (string REG in GliderREG)
            {
                comboBox2.Items.Add(REG);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
            var form2 = new Form6();
            form2.Closed += (s, args) => this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
            this.Hide();
            var form3 = new Form6();
            form3.Closed += (s, args) => this.Close();

            SubmitForm6Data();

            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }
    }
}
