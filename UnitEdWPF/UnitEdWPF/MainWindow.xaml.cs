using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace UnitEdWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

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

    public partial class MainWindow : Window
    {
        Unit unit = new Unit();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".vuf"; // Default file extension
            dlg.Filter = "Unit File (.vuf)|*.vuf"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                unit.Name = nameTxtBox.Text;
                unit.HP = int.Parse(hpTxtBox.Text);
                unit.MovSpd = int.Parse(moveSpdTxtBox.Text);
                unit.AtkSpd = int.Parse(atkSpdTxtBox.Text);
                unit.AtkDmg = int.Parse(atkDmgTxtBox.Text);
                unit.AtkRange = int.Parse(atkRngTxtBox.Text);
                unit.FlwRange = int.Parse(flwRngeTxtbox.Text);
                unit.EnCost = int.Parse(ecTxtBox.Text);
                unit.imgAsset = imgAssetTxtBox.Text;

                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = nameTxtBox.Text; // Default file name
                dlg.DefaultExt = ".vuf"; // Default file extension
                dlg.Filter = "Unit File (.vuf)|*.vuf"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    string filename = dlg.FileName;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("One or more fields contains incorrect data");
            }
        }

       
    }
}
