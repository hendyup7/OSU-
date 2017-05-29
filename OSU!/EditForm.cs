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

namespace OSU_
{
    public partial class EditForm : Form
    {
        DatabaseConnection dc = new DatabaseConnection();
        int idNum;
        string songDestPath = @"C:\Users\A455LF-I3\Documents\Visual Studio 2015\Projects\OSU!\OSU!\songs\";
        string imgDestPath = @"C:\Users\A455LF-I3\Documents\Visual Studio 2015\Projects\OSU!\OSU!\img\";
        string songDest, imgDest;

        public EditForm(int idnum)
        {
            InitializeComponent();
            idNum = idnum;

            string db = "osu";
            string query = "SELECT * FROM songs WHERE ID = " + idnum;
            List<string[]> item = dc.FetchDB(db, query);
            
            songPath.Text = item[0][6].Replace('/','\\');
            imgPath.Text = item[0][5].Replace('/', '\\');
            title.Text = item[0][1];
            artist.Text = item[0][2];
            duration.Text = item[0][3];
            bpm.Text = item[0][4];

            songDest = songPath.Text;
            imgDest = imgPath.Text;

            Console.WriteLine(songDest);
            Console.WriteLine(imgDest);
            
        }

        private void browseSong_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = songPath.Text.Substring(songPath.Text.LastIndexOf('\\') + 1);
            openFileDialog1.Filter = "mp3 files|*mp3";
            openFileDialog1.Title = "Select Song";
            //openFileDialog1.InitialDirectory = @"C:\Users\A455LF-I3\Documents\Visual Studio 2015\Projects\OSU!\OSU!\songs\";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                songPath.Text = filename;

            }
            else if (dr == DialogResult.Cancel)
            {
                return;
            }
            songDest = songDestPath + openFileDialog1.SafeFileName;

        }

        private void browseImg_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = imgPath.Text.Substring(imgPath.Text.LastIndexOf('\\') + 1);
            openFileDialog1.Filter = "images|*.jpg;*.png";
            openFileDialog1.Title = "Select Background Image";
            //openFileDialog1.InitialDirectory = @"C:\Users\A455LF-I3\Documents\Visual Studio 2015\Projects\OSU!\OSU!\img\";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                imgPath.Text = filename;

            }
            else if (dr == DialogResult.Cancel)
            {
                return;
            }
            imgDest = imgDestPath + openFileDialog1.SafeFileName;

        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (songPath.Text.Equals("") || imgPath.Text.Equals("") || title.Text.Equals("") ||
                artist.Text.Equals("") || duration.Text.Equals("") || bpm.Text.Equals(""))
            {
                MessageBox.Show("Tolong isi semua field");

            }
            else
            {
                DialogResult = DialogResult.OK;

                if (!songPath.Text.Equals(songDest))
                {
                    if (File.Exists(songDest))
                    {
                        File.Delete(songDest);
                    }
                    File.Copy(songPath.Text, songDest);
                }
                    

                if (!imgPath.Text.Equals(imgDest))
                {
                    if (File.Exists(imgDest))
                    {
                        File.Delete(imgDest);
                    }
                    File.Copy(imgPath.Text, imgDest);
                }
                    

                songDest = songDest.Replace('\\', '/');
                imgDest = imgDest.Replace('\\', '/');

                while (duration.Text.StartsWith("0"))
                {
                    duration.Text = duration.Text.Substring(1);
                }
                while (bpm.Text.StartsWith("0"))
                {
                    bpm.Text = bpm.Text.Substring(1);
                }
                int dur = int.Parse(duration.Text);
                int BPM = int.Parse(bpm.Text);

                string dbName = "osu";
                string query = "UPDATE songs SET `title` = '" + title.Text + "', `artist` = '" + artist.Text + "', `duration` = '" + dur + "', `BPM` = '" + BPM + "', `pathimg` = '" + imgDest + "', `pathsong` = '" + songDest + "' WHERE ID = " + idNum;
                dc.UpdateDB(dbName, query);

            }

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
