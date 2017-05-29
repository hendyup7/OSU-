using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace OSU_
{
    public partial class MainMenu : Form
    {
        public static User userLogged;

        public MainMenu()
        {
            InitializeComponent();
            userLogged = new User();
        }

        private void play_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 songList = new Form1(this);
            songList.ShowDialog();
        }

        private void login_Click(object sender, EventArgs e)
        {
            loginPanel.Visible = true;
            registerPanel.Visible = false;
            updatePanel.Visible = false;
        }

        public void userLogin(string username, string password)
        {
            userView.Items.Clear();

            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=osu;";
            string query = String.Format(@"SELECT * FROM user WHERE username = '{0}' AND password = '{1}'; ", username, password);

            MySqlConnection dbConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDb = new MySqlCommand(query, dbConnection);
            commandDb.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                dbConnection.Open();
                reader = commandDb.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userLogged.id = reader.GetInt32(0);
                        userLogged.username = reader.GetString(1);
                        userLogged.password = reader.GetString(2);
                        userLogged.email = reader.GetString(3);

                        string[] row = { userLogged.username, userLogged.email };
                        var input = new ListViewItem(row);
                        userView.Items.Add(input);
                    }
                }
                else
                {
                    Console.WriteLine("NO ROWS FOUND");
                }

                dbConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //Create
        public void newUser(string username, string password, string email)
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=osu;";
            string query = String.Format(@"INSERT INTO user (id,username,password,email,picture) VALUES (NULL,'{0}','{1}','{2}',NULL);", username, password, email);

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;

            try
            {
                databaseConnection.Open();
                MySqlDataReader myReader = commandDatabase.ExecuteReader();

                MessageBox.Show("Register DONE!");

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                // Show any error message.
                MessageBox.Show(ex.Message);
            }
        }

        //Update
        public void updateUser()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=osu;";
            string query = String.Format(@"UPDATE user SET username = '{0}', password = '{1}', email = '{2}' WHERE id = {3}", usernameEdit.Text, passwordEdit.Text, emailEdit.Text, userLogged.id);

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                // Succesfully updated

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                // Ops, maybe the id doesn't exists ?
                MessageBox.Show(ex.Message);
            }
        }

        //Delete
        public void deleteUser()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=osu;";
            string query = String.Format(@"DELETE FROM user WHERE id = {0}", userLogged.id);

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                // Succesfully deleted

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                // Ops, maybe the id doesn't exists ?
                MessageBox.Show(ex.Message);
            }
        }

        private void loginSubmit_Click(object sender, EventArgs e)
        {
            if (!usernameBox.Text.Equals("") && !passwordBox.Text.Equals(""))
            {
                userLogin(usernameBox.Text, passwordBox.Text);

                if (!userLogged.id.Equals(0))
                {
                    loginPanel.Hide();
                    loginBtn.Hide();
                    logoutBtn.Show();
                    updateBtn.Show();
                    deleteBtn.Show();
                    registerBtn.Hide();
                }
                else
                {
                    MessageBox.Show("HEY! Username not found");
                }
            }
            else
            {
                MessageBox.Show("FIELD IS EMPTY");
            }
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            userLogged = new User();
            loginBtn.Show();
            logoutBtn.Hide();
            updateBtn.Hide();
            registerBtn.Show();
        }

        private void registerBtn_Click(object sender, EventArgs e)
        {
            registerPanel.Show();
            loginPanel.Visible = false;
            updatePanel.Visible = false;
        }

        private void registerSubmit_Click(object sender, EventArgs e)
        {
            if (!usernameReg.Text.Equals("") && !passwordReg.Text.Equals("") && !emailReg.Text.Equals(""))
            {
                newUser(usernameReg.Text, passwordReg.Text, emailReg.Text);
                registerPanel.Hide();
            }
            else
            {
                MessageBox.Show("FIELD IS EMPTY");
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            updatePanel.Show();
            loginPanel.Visible = false;
            registerPanel.Visible = false;

            usernameEdit.Text = userLogged.username;
            passwordEdit.Text = userLogged.password;
            emailEdit.Text = userLogged.email;
        }

        private void updateSubmit_Click(object sender, EventArgs e)
        {
            if (!usernameEdit.Text.Equals("") && !passwordEdit.Text.Equals("") && !emailEdit.Text.Equals(""))
            {
                userLogged.username = usernameEdit.Text;
                userLogged.password = passwordEdit.Text;
                userLogged.email = emailEdit.Text;

                updateUser();
                userLogin(userLogged.username, userLogged.password);
                updatePanel.Hide();
            }
            else
            {
                MessageBox.Show("FIELD IS EMPTY!");
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            deleteUser();
            deleteBtn.Hide();
            userLogged = new User();
            loginBtn.Show();
            logoutBtn.Hide();
            updateBtn.Hide();
            registerBtn.Show();
        }
    }
}
