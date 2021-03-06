﻿using System;
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
    public partial class AddForm : Form
    {
        DatabaseConnection dc = new DatabaseConnection();
        string songDestPath = @"C:\Users\A455LF-I3\Documents\Visual Studio 2015\Projects\OSU!\OSU!\songs\";
        string imgDestPath = @"C:\Users\A455LF-I3\Documents\Visual Studio 2015\Projects\OSU!\OSU!\img\";

        public AddForm()
        {
            InitializeComponent();
        }

        private void browseSong_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "mp3 files|*mp3";
            openFileDialog1.Title = "Select Song";
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
            songDestPath += openFileDialog1.SafeFileName;

        }

        private void browseImg_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "images|*.jpg;*.png";
            openFileDialog1.Title = "Select Background Image";
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
            imgDestPath += openFileDialog1.SafeFileName;

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

            } else {
                DialogResult = DialogResult.OK;

                if (File.Exists(songDestPath))
                {
                    File.Delete(songDestPath);
                }
                if (File.Exists(imgDestPath))
                {
                    File.Delete(imgDestPath);
                }
                File.Copy(songPath.Text, songDestPath);
                File.Copy(imgPath.Text, imgDestPath);

                songDestPath = songDestPath.Replace('\\', '/');
                imgDestPath = imgDestPath.Replace('\\', '/');

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
                string query = "INSERT INTO `songs` (`title`, `artist`, `duration`, `BPM`, `pathimg`, `pathsong`) VALUES ('" + title.Text + "', '" + artist.Text + "', '" + dur + "', '" + BPM + "', '" + imgDestPath + "', '" + songDestPath + "')";
                dc.UpdateDB(dbName, query);

            }

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
