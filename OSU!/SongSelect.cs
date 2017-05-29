using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;
using WMPLib;

namespace OSU_
{
    public partial class Form1 : Form
    {
        DatabaseConnection dc = new DatabaseConnection();
        ListViewItem item;
        int idnum = 0, idSong;
        WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();

        //
        User userLogged;
        MainMenu mainMenu;

        public Form1(MainMenu menu)
        {
            InitializeComponent();
            updateSongView();
            listView1.Items[0].Selected = true;
            listView1.Select();
            button2.Visible = false;
            button3.Visible = false;

            //
            mainMenu = menu;
            userLogged = MainMenu.userLogged;
            userLogin(userLogged.username, userLogged.password);



        }

        private void updateSongView()
        {
            string db = "osu";
            string query = "SELECT * FROM songs";
            List<string[]> result = dc.FetchDB(db, query);

            listView1.Items.Clear();

            foreach (string[] items in result)
            {
                var listViewItem = new ListViewItem(items);
                listView1.Items.Add(listViewItem);

            }

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            item = listView1.SelectedItems[0];
            idnum = int.Parse(item.SubItems[0].Text);
            button2.Visible = true;
            button3.Visible = true;

            panel2.BackgroundImage = Image.FromFile(item.SubItems[5].Text);

            player.controls.stop();
            player.URL = item.SubItems[6].Text;
            player.controls.play();

            idSong = int.Parse(item.SubItems[0].Text);
            listScore(idSong);

        }

        private void add_Click(object sender, EventArgs e)
        {
            AddForm addform = new AddForm();
            addform.ShowDialog();
            updateSongView();

        }

        private void edit_Click(object sender, EventArgs e)
        {
            player.controls.stop();
            EditForm editform = new EditForm(idnum);
            editform.ShowDialog();
            updateSongView();

        }

        private void delete_Click(object sender, EventArgs e)
        {
            DeleteForm deleteform = new DeleteForm(idnum);
            if (deleteform.ShowDialog() == DialogResult.OK)
            {
                updateSongView();
                player.controls.stop();

            }
            
            /*string selquery = "SELECT * FROM `songs` WHERE ID = " + idnum;
            List<string[]> item = dc.FetchDB("osu", selquery);
            File.Delete(item[0][5].Replace('/', '\\'));
            File.Delete(item[0][6].Replace('/', '\\'));*/

        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            button2.Visible = false;
            button3.Visible = false;

        }


        //
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

        //Read
        public void listScore(int idSong)
        {
            scoreList.Items.Clear();
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=osu;";
            string query = "SELECT u.username, s.points FROM score s, user u WHERE u.id = s.user_id AND s.song_id = " + idSong + " ORDER BY s.points DESC";

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
                        string[] row = { reader.GetString(0), reader.GetString(1) };
                        var input = new ListViewItem(row);
                        scoreList.Items.Add(input);
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
        public void newScore()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=osu;";
            string query = String.Format(@"INSERT INTO score (user_id,song_id,points) VALUES ({0},{1},{2});", userLogged.id, idSong, Convert.ToInt32(scoreBox.Text));

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;

            try
            {
                databaseConnection.Open();
                MySqlDataReader myReader = commandDatabase.ExecuteReader();

                MessageBox.Show("Congratulation! Score is added");

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                // Show any error message.
                MessageBox.Show(ex.Message);
            }
        }

        //Update
        public void updateScore()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=osu;";
            string query = String.Format(@"UPDATE score SET points = {0} WHERE user_id = {1} AND song_id = {2}", Convert.ToInt32(scoreBox.Text), userLogged.id, idSong);

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
        public void deleteScore()
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=osu;";
            string query = String.Format(@"DELETE FROM score WHERE user_id = {0} AND song_id = {1}", userLogged.id, idSong);

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

        private void submit_Click(object sender, EventArgs e)
        {
            if (!scoreBox.Text.Equals(""))
            {
                string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=osu;";
                string query = String.Format(@"SELECT * FROM score WHERE user_id = {0} AND song_id = {1}", userLogged.id, idSong);

                MySqlConnection dbConnection = new MySqlConnection(connectionString);
                MySqlCommand commandDb = new MySqlCommand(query, dbConnection);
                commandDb.CommandTimeout = 60;
                MySqlDataReader reader;

                int userID = 0;
                int songID = 0;

                try
                {
                    dbConnection.Open();
                    reader = commandDb.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            userID = reader.GetInt32(0);
                            songID = reader.GetInt32(1);

                            if (userID.Equals(userLogged.id) && songID.Equals(idSong))
                            {
                                break;
                            }
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

                if (!userLogged.id.Equals(0))
                {
                    if (userID.Equals(userLogged.id) && songID.Equals(idSong))
                    {
                        updateScore();
                    }
                    else
                    {
                        newScore();
                    }
                }
                else
                {
                    MessageBox.Show("NOT LOGGED IN YET");
                }
            }
            else
            {
                MessageBox.Show("HEY!! The score is empty!");
            }

            listScore(idSong);
        }

        private void deteleScore_Click(object sender, EventArgs e)
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=osu;";
            string query = String.Format(@"SELECT * FROM score WHERE user_id = {0} AND song_id = {1}", userLogged.id, idSong);

            MySqlConnection dbConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDb = new MySqlCommand(query, dbConnection);
            commandDb.CommandTimeout = 60;
            MySqlDataReader reader;

            int userID = 0;
            int songID = 0;

            try
            {
                dbConnection.Open();
                reader = commandDb.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userID = reader.GetInt32(0);
                        songID = reader.GetInt32(1);

                        if (userID.Equals(userLogged.id) && songID.Equals(idSong))
                        {
                            break;
                        }
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

            if (!userLogged.id.Equals(0))
            {
                if (userID.Equals(userLogged.id) && songID.Equals(idSong))
                {
                    deleteScore();
                }
            }else
            {
                MessageBox.Show("NOT LOGGED IN YET");
            }
            
            listScore(idSong);
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            player.controls.stop();
            this.Hide();
            mainMenu.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();

        }
    }

}


