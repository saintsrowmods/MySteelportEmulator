using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SaintsRowAPI.Hydra;
using SaintsRowAPI.Hydra.DataTypes;

namespace CharacterRenamer
{
    public partial class MainForm : Form
    {
        private string CurrentFile;
        private Stream CurrentStream;
        private HydraHashMap SaveMap;
        private HydraHashMap MetadataMap;

        public MainForm()
        {
            InitializeComponent();
        }

        private void OpenFile(string path)
        {
            if (CurrentStream != null)
            {
                CurrentStream.Close();
            }

            CurrentFile = path;
            CurrentStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            SaveMap = new HydraHashMap();
            SaveMap.Deserialize(CurrentStream);

            MetadataMap = (HydraHashMap)SaveMap.Items["meta_data"];

            SaveButton.Enabled = true;
            CurrentCharacterLabel.Text = String.Format("Current character: {0}", Path.GetFileName(CurrentFile));
            CharacterNameField.Enabled = true;
            CharacterNameField.Text = ((HydraUtf8String)MetadataMap.Items["Name"]).Value;
        }

        private void CloseFile()
        {
            if (CurrentStream != null)
            {
                CurrentStream.Close();
            }
            CurrentFile = null;
            SaveMap = null;
            MetadataMap = null;

            SaveButton.Enabled = false;
            CurrentCharacterLabel.Text = String.Format("Current character:");
            CharacterNameField.Enabled = false;
            CharacterNameField.Text = "";
        }

        private void SaveFile()
        {

        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            CloseFile();
            DialogResult dr = TargetFile.ShowDialog();

            if (dr == DialogResult.OK)
            {
                OpenFile(TargetFile.FileName);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            ((HydraUtf8String)MetadataMap.Items["Name"]).Value = CharacterNameField.Text;

            CurrentStream.Seek(0, SeekOrigin.Begin);
            CurrentStream.SetLength(0); // truncate to zero length, so we don't have any crap left at the end when we save

            SaveMap.Serialize(CurrentStream);

            MessageBox.Show(this, "File saved.", "Character Renamer", MessageBoxButtons.OK);

            CloseFile();
        }
    }
}
