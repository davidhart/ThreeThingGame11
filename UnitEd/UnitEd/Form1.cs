using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace UnitEd
{
    public struct Unit
    {
        public string Name;
        public int HP;
        public int MovSpd;
        public int AtkSpd;
        public int AtkDmg;
        public int AtkRange;
        public int FlwRange;
        public int EnCost;
        public string imgAsset;
    }
    public partial class Form1 : Form
    {
        Unit currentUnit = new Unit();
        public Form1()
        {
            InitializeComponent();

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void nameTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                currentUnit.Name = nameTxtBox.Text;
                currentUnit.HP = int.Parse(hpTxtBox.Text);
                currentUnit.MovSpd = int.Parse(movSpdTxtBox.Text);
                currentUnit.AtkSpd = int.Parse(atkSpdTxtBox.Text);
                currentUnit.AtkDmg = int.Parse(atkDmgTxtBox.Text);
                currentUnit.AtkRange = int.Parse(atkRangeTxtBox.Text);
                currentUnit.FlwRange = int.Parse(followRngTxtBox.Text);
                currentUnit.EnCost = int.Parse(energyCostBox.Text);
                currentUnit.imgAsset = imgAssetTxtBox.Text;
                Stream myStream;
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                sfd.FilterIndex = 1;
                sfd.RestoreDirectory = true;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = sfd.OpenFile()) != null)
                    {
                        // Code to write the stream goes here.
                        myStream.Close();
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("One or more values is in an incorrect format" + "\n" + "Name and Image Asset - Text" + "\n" +
                    "Everything else - integers");
            }

           
            
        }

        private void newBtn_Click(object sender, EventArgs e)
        {
            nameTxtBox.Text = "";
            hpTxtBox.Text = "";
            movSpdTxtBox.Text = "";
            atkSpdTxtBox.Text = "";
            atkDmgTxtBox.Text = "";
            atkRangeTxtBox.Text = "";
            followRngTxtBox.Text = "";
            energyCostBox.Text = "";
            imgAssetTxtBox.Text = "";
        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
        }
    }
}
