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
using System.Xml;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Data;

namespace UnitEdWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public struct Unit
    {
        [XmlElement("Name")]
        public string Name;
        [XmlElement("HP")]
        public int HP;
        [XmlElement("MoveSpeed")]
        public int MovSpd;
        [XmlElement("AtkSpeed")]
        public int AtkSpd;
        [XmlElement("AtkDamage")]
        public int AtkDmg;
        [XmlElement("AtkRange")]
        public int AtkRange;
        [XmlElement("FollowRange")]
        public int FlwRange;
        [XmlElement("EnergyCost")]
        public int EnCost;
        [XmlElement("ImageAsset")]
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
            nameTxtBox.Text = "";
            hpTxtBox.Text = "";
            moveSpdTxtBox.Text = "";
            atkSpdTxtBox.Text = "";
            atkDmgTxtBox.Text = "";
            atkRngTxtBox.Text = "";
            flwRngeTxtbox.Text = "";
            ecTxtBox.Text = "";
            imgAssetTxtBox.Text = "";
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".vuf";
            dlg.Filter = "Unit File (.vuf)|*.vuf";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                XmlDocument xml = new XmlDocument();
                xml.Load(filename); // suppose that myXmlString contains "<Names>...</Names>"

                XmlNodeList xnList = xml.SelectNodes("/Unit");
                foreach (XmlNode xn in xnList)
                {
                    nameTxtBox.Text = xn["Name"].InnerText;
                    hpTxtBox.Text = xn["HP"].InnerText;
                    moveSpdTxtBox.Text = xn["MoveSpeed"].InnerText;
                    atkSpdTxtBox.Text = xn["AtkSpeed"].InnerText;
                    atkDmgTxtBox.Text = xn["AtkDamage"].InnerText;
                    atkRngTxtBox.Text = xn["AtkRange"].InnerText;
                    flwRngeTxtbox.Text = xn["FollowRange"].InnerText;
                    ecTxtBox.Text = xn["EnergyCost"].InnerText;
                    imgAssetTxtBox.Text = xn["ImageAsset"].InnerText;
                }
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
                dlg.FileName = nameTxtBox.Text;
                dlg.DefaultExt = ".vuf";
                dlg.Filter = "Unit File (.vuf)|*.vuf";

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    // Save document
                    string filename = dlg.FileName;
                    XmlSerializer serializer = new XmlSerializer(typeof(Unit));
                    TextWriter textWriter = new StreamWriter(filename);
                    serializer.Serialize(textWriter, unit);
                    textWriter.Close();
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("One or more fields contains incorrect data");
            }
        }

       
    }
}
