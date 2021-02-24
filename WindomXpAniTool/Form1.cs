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
namespace WindomXpAniTool
{
    public partial class Form1 : Form
    {
        ani2 file = new ani2();
        List<string> recentFiles = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void loadAniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Windom Animation Data (.ani)|*.ani";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < recentFiles.Count; i++)
                {
                    if (recentFiles[i] == ofd.FileName)
                        recentFiles.RemoveAt(i);
                }
                recentFiles.Insert(0, ofd.FileName);
                if (recentFiles.Count > 10)
                    recentFiles.RemoveAt(11);
                updateRecent();
                try
                {
                    saveData();
                    file.load(ofd.FileName);
                    lstAnimations.Items.Clear();
                    for (int i = 0; i < file.animations.Count; i++)
                    {
                        lstAnimations.Items.Add(i.ToString() + " - " + file.animations[i].name);
                    }

                    MsgLog.Text = "Ani File has been loaded";
                }
                catch
                {
                    MsgLog.Text = "Error - Couldn't Load File";
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lstAnimations.SelectedItems.Count > 0 && cbScriptFormat.SelectedIndex > -1 && cbHodFormat.SelectedIndex > -1)
            {
                DirectoryInfo di = new DirectoryInfo(file._filename);
                for (int i = 0; i < lstAnimations.Items.Count; i++)
                {

                    if (lstAnimations.GetSelected(i))
                    {
                        string dir = Path.Combine(di.Parent.Name, helper.replaceUnsupportedChar(lstAnimations.Items[i].ToString()));
                        switch (cbScriptFormat.SelectedIndex)
                        {
                            case 0:
                                file.animations[i].extractToFolderXML(dir, cbHodFormat.SelectedIndex);
                                break;
                            case 1:
                                file.animations[i].extractToFolderTXT(dir, cbHodFormat.SelectedIndex);
                                break;
                        }

                    }
                }
            }
            else
            {
                if (lstAnimations.SelectedItems.Count <= 0)
                    MsgLog.Text = "No animation has been selected";
                else
                {
                    if (cbScriptFormat.SelectedIndex < 0 && cbHodFormat.SelectedIndex < 0)
                        MsgLog.Text = "Script Format and Hod Format not selected";
                    else if (cbScriptFormat.SelectedIndex < 0)
                        MsgLog.Text = "Script Format not selected";
                    else if (cbHodFormat.SelectedIndex > 0)
                        MsgLog.Text = "Hod Format not selected";
                    else
                        MsgLog.Text = "Error - We have an hacker here.";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(file._filename);

            //BinaryWriter bw = new BinaryWriter(File.Open(Path.Combine(di.Parent.Name, file.structure.filename), FileMode.CreateNew));
            //bw.Write(file.structure.data);
            //bw.Close();
            if (cbHodFormat.SelectedIndex > -1)
                file.structure.saveToFile(di.Parent.Name, cbHodFormat.SelectedIndex);
            else
                MsgLog.Text = "Hod Format not selected";
        }

        private void saveAniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Windom Animation Data (.ani)|*.ani";
            DirectoryInfo di = new DirectoryInfo(file._filename);
            sfd.InitialDirectory = di.FullName;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                file.save(sfd.FileName);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(file._filename);
            for (int i = 0; i < lstAnimations.Items.Count; i++)
            {
                if (lstAnimations.GetSelected(i))
                {
                    string dir = Path.Combine(di.Parent.Name, helper.replaceUnsupportedChar(lstAnimations.Items[i].ToString()));
                    if (Directory.Exists(dir))
                    {
                        if (File.Exists(Path.Combine(dir, "script.xml"))) 
                            file.animations[i].injectFromFolderXML(dir, ref file.structure);
                        else if (File.Exists(Path.Combine(dir, "script.txt")))
                            file.animations[i].injectFromFolderTXT(dir, ref file.structure);
                    }
                }
            }

            lstAnimations.Items.Clear();
            for (int i = 0; i < file.animations.Count; i++)
            {
                lstAnimations.Items.Add(i.ToString() + " - " + file.animations[i].name);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(file._filename);
            di = di.Parent;
            file.structure.loadFromFile(Path.Combine(di.Parent.Name, file.structure.filename));
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstAnimations.Items.Count; i++)
                lstAnimations.SetSelected(i, true);
        }

        private void deselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstAnimations.Items.Count; i++)
                lstAnimations.SetSelected(i, false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("Settings.txt"))
            {
                StreamReader sr = new StreamReader("Settings.txt");
                int pInt = 0;
                if (int.TryParse(sr.ReadLine(), out pInt))
                    cbScriptFormat.SelectedIndex = pInt;
                if (int.TryParse(sr.ReadLine(), out pInt))
                    cbHodFormat.SelectedIndex = pInt;
                while (sr.EndOfStream != true)
                {
                    recentFiles.Add(sr.ReadLine());
                }
                updateRecent();
                sr.Close();
            }
        }

        private void saveData()
        {
            try
            {
                StreamWriter sw = new StreamWriter("Settings.txt");
                sw.WriteLine(cbScriptFormat.SelectedIndex);
                sw.WriteLine(cbHodFormat.SelectedIndex);
                for (int i = 0; i < recentFiles.Count; i++)
                    sw.WriteLine(recentFiles[i]);
                sw.Close();
            }
            catch { };
        }

        private void updateRecent()
        {
            if (recentFiles.Count > 0)
            {
                toolStripMenuItem2.Text = "1. " + recentFiles[0];
                toolStripMenuItem2.Visible = true;
            }
            else
                toolStripMenuItem2.Visible = false;

            if (recentFiles.Count > 1)
            {
                toolStripMenuItem3.Text = "2. " + recentFiles[1];
                toolStripMenuItem3.Visible = true;
            }
            else
                toolStripMenuItem3.Visible = false;

            if (recentFiles.Count > 2)
            {
                toolStripMenuItem4.Text = "3. " + recentFiles[2];
                toolStripMenuItem4.Visible = true;
            }
            else
                toolStripMenuItem4.Visible = false;

            if (recentFiles.Count > 3)
            {
                toolStripMenuItem5.Text = "4. " + recentFiles[3];
                toolStripMenuItem5.Visible = true;
            }
            else
                toolStripMenuItem5.Visible = false;

            if (recentFiles.Count > 4)
            {
                toolStripMenuItem6.Text = "5. " + recentFiles[4];
                toolStripMenuItem6.Visible = true;
            }
            else
                toolStripMenuItem6.Visible = false;

            if (recentFiles.Count > 5)
            {
                toolStripMenuItem7.Text = "6. " + recentFiles[5];
                toolStripMenuItem7.Visible = true;
            }
            else
                toolStripMenuItem7.Visible = false;

            if (recentFiles.Count > 6)
            {
                toolStripMenuItem8.Text = "7. " + recentFiles[6];
                toolStripMenuItem8.Visible = true;
            }
            else
                toolStripMenuItem8.Visible = false;

            if (recentFiles.Count > 7)
            {
                toolStripMenuItem9.Text = "8. " + recentFiles[7];
                toolStripMenuItem9.Visible = true;
            }
            else
                toolStripMenuItem9.Visible = false;

            if (recentFiles.Count > 8)
            {
                toolStripMenuItem10.Text = "9. " + recentFiles[8];
                toolStripMenuItem10.Visible = true;
            }
            else
                toolStripMenuItem10.Visible = false;

            if (recentFiles.Count > 9)
            {
                toolStripMenuItem11.Text = "10. " + recentFiles[9];
                toolStripMenuItem11.Visible = true;
            }
            else
                toolStripMenuItem11.Visible = false;

        }

        private void loadFileIntoRecent(string filename)
        {
            for (int i = 0; i < recentFiles.Count; i++)
            {
                if (recentFiles[i] == filename)
                    recentFiles.RemoveAt(i);
            }
            recentFiles.Insert(0, filename);
            if (recentFiles.Count > 10)
                recentFiles.RemoveAt(11);
            updateRecent();
            saveData();
        }

        //recent files
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            file.load(recentFiles[0]);
            loadFileIntoRecent(recentFiles[0]);
            lstAnimations.Items.Clear();
            for (int i = 0; i < file.animations.Count; i++)
            {
                lstAnimations.Items.Add(i.ToString() + " - " + file.animations[i].name);
            }

        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            file.load(recentFiles[1]);
            loadFileIntoRecent(recentFiles[1]);
            lstAnimations.Items.Clear();
            for (int i = 0; i < file.animations.Count; i++)
            {
                lstAnimations.Items.Add(i.ToString() + " - " + file.animations[i].name);
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            file.load(recentFiles[2]);
            loadFileIntoRecent(recentFiles[2]);
            lstAnimations.Items.Clear();
            for (int i = 0; i < file.animations.Count; i++)
            {
                lstAnimations.Items.Add(i.ToString() + " - " + file.animations[i].name);
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            file.load(recentFiles[3]);
            loadFileIntoRecent(recentFiles[3]);
            lstAnimations.Items.Clear();
            for (int i = 0; i < file.animations.Count; i++)
            {
                lstAnimations.Items.Add(i.ToString() + " - " + file.animations[i].name);
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            file.load(recentFiles[4]);
            loadFileIntoRecent(recentFiles[4]);
            lstAnimations.Items.Clear();
            for (int i = 0; i < file.animations.Count; i++)
            {
                lstAnimations.Items.Add(i.ToString() + " - " + file.animations[i].name);
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            file.load(recentFiles[5]);
            loadFileIntoRecent(recentFiles[5]);
            lstAnimations.Items.Clear();
            for (int i = 0; i < file.animations.Count; i++)
            {
                lstAnimations.Items.Add(i.ToString() + " - " + file.animations[i].name);
            }
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            file.load(recentFiles[6]);
            loadFileIntoRecent(recentFiles[6]);
            lstAnimations.Items.Clear();
            for (int i = 0; i < file.animations.Count; i++)
            {
                lstAnimations.Items.Add(i.ToString() + " - " + file.animations[i].name);
            }
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            file.load(recentFiles[7]);
            loadFileIntoRecent(recentFiles[7]);
            lstAnimations.Items.Clear();
            for (int i = 0; i < file.animations.Count; i++)
            {
                lstAnimations.Items.Add(i.ToString() + " - " + file.animations[i].name);
            }
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            file.load(recentFiles[8]);
            loadFileIntoRecent(recentFiles[8]);
            lstAnimations.Items.Clear();
            for (int i = 0; i < file.animations.Count; i++)
            {
                lstAnimations.Items.Add(i.ToString() + " - " + file.animations[i].name);
            }
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            file.load(recentFiles[9]);
            loadFileIntoRecent(recentFiles[9]);
            lstAnimations.Items.Clear();
            for (int i = 0; i < file.animations.Count; i++)
            {
                lstAnimations.Items.Add(i.ToString() + " - " + file.animations[i].name);
            }
        }

        private void cbScriptFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveData();
        }

        private void cbHodFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveData();
        }

        private void renameSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstAnimations.Items.Count; i++)
            {
                if (lstAnimations.GetSelected(i))
                {
                    RenameAnimation ra = new RenameAnimation();
                    ra.setTxtName(file.animations[i].name);
                    ra.ShowDialog();
                    file.animations[i].name = ra.getTxtName();
                }
            }

            lstAnimations.Items.Clear();
            for (int i = 0; i < file.animations.Count; i++)
            {
                lstAnimations.Items.Add(i.ToString() + " - " + file.animations[i].name);
            }
        }
    }
}
