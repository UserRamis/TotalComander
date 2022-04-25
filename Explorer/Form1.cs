using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace Explorer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            listView1.ColumnClick += new ColumnClickEventHandler(ClickOnColumn);
            ColumnHeader c = new ColumnHeader();
            c.Text = "Имя";
            c.Width = c.Width + 80;
            ColumnHeader c2 = new ColumnHeader();
            c2.Text = "Размер";
            c2.Width = c2.Width + 60;
            ColumnHeader c3 = new ColumnHeader();
            c3.Text = "Тип";
            c3.Width = c3.Width + 60;
            ColumnHeader c4 = new ColumnHeader();
            c4.Text = "Изменен";
            c4.Width = c4.Width + 60;
            listView1.Columns.Add(c);
            listView1.Columns.Add(c2);
            listView1.Columns.Add(c3);
            listView1.Columns.Add(c4);
            
            int n = 1;
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo drInfo in allDrives)
            {
                try
                {
                    TreeNode tn = new TreeNode();
                    tn.Name = drInfo.Name;
                    tn.Text = "Локальный диск " + drInfo.Name;
                    treeView1.Nodes.Add(tn.Name, tn.Text, 6);
                    FileInfo f = new FileInfo(@drInfo.Name);
                    string t = "";
                    string[] str2 = Directory.GetDirectories(@drInfo.Name);
                    foreach (string s2 in str2)
                    {
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        ((TreeNode)treeView1.Nodes[1]).Nodes.Add(s2, t, 7);
                    }
                }
                catch { }
               
            }
            foreach (TreeNode tn in treeView1.Nodes)
            {
                for (int i = 65; i < 91; i++)
                {
                    char sym = Convert.ToChar(i);
                    if (tn.Name == sym + ":\\")
                        tn.SelectedImageIndex = 6;
                }
            }
        }

        

        ArrayList Adresses = new ArrayList();
        string currListViewAdress = "";
        int currIndex = -1;
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
            if (listView1.SelectedItems[0].ImageIndex == 7 || listView1.SelectedItems[0].ImageIndex == 0)
            {
                
                    currListViewAdress = ((string)Adresses[currIndex]);
                    currListViewAdress = toolStripTextBox1.Text;
                    
                    Adresses.Add(listView1.SelectedItems[0].Name);
                    currIndex++;
                    currListViewAdress = ((string)Adresses[currIndex]);
                    FileInfo f = new FileInfo(@listView1.SelectedItems[0].Name);
                    string t = "";
                    string[] str2 = Directory.GetDirectories(@listView1.SelectedItems[0].Name);
                    string[] str3 = Directory.GetFiles(@listView1.SelectedItems[0].Name);
                    listView1.Items.Clear();
                    ListViewItem lw = new ListViewItem();
                    if (listView1.View == View.Details)
                    {
                        foreach (string s2 in str2)
                        {
                            f = new FileInfo(@s2);
                            string type = "Папка";
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 7);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                        foreach (string s2 in str3)
                        {
                            f = new FileInfo(@s2);
                            string type = "Файл";
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                    }
                    else
                    {
                        foreach (string s2 in str2)
                        {
                            f = new FileInfo(@s2);
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t }, 7);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                        foreach (string s2 in str3)
                        {
                            f = new FileInfo(@s2);
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t }, 1);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                        toolStripTextBox1.Text = currListViewAdress;
                    }
                    toolStripButton1.Enabled = true;
                
            }
            else
            {
                
                System.Diagnostics.Process MyProc = new System.Diagnostics.Process();
                MyProc.StartInfo.FileName = @listView1.SelectedItems[0].Name;
                MyProc.Start();
                currListViewAdress = toolStripTextBox1.Text;
            }
            toolStripTextBox1.Text = currListViewAdress;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string strtmp = "";
            if (Adresses.Count != 0)
            {
                strtmp = ((string)Adresses[Adresses.Count - 1]);
                listView1.Items.Clear();
                Adresses.Clear();
                Adresses.Add(strtmp);
                currIndex = 0;
            }
            Adresses.Add(e.Node.Name);
            currIndex++;
            toolStripTextBox1.Text = ((string)Adresses[Adresses.Count - 1]);
            
            try
            {
                if (listView1.View != View.Tile)
                {
                    FileInfo f = new FileInfo(@e.Node.Name);
                    string t = "";
                    string[] str2 = Directory.GetDirectories(@e.Node.Name);
                    ListViewItem lw = new ListViewItem();
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        string type = "Папка";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 7);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    str2 = Directory.GetFiles(@e.Node.Name);
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        string type = "Файл";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                }
                else
                {
                    FileInfo f = new FileInfo(@e.Node.Name);
                    string t = "";
                    string[] str2 = Directory.GetDirectories(@e.Node.Name);
                    ListViewItem lw = new ListViewItem();
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 7);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    str2 = Directory.GetFiles(@e.Node.Name);
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 1);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                }
                currListViewAdress = toolStripTextBox1.Text;
            }
            catch { }
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            int i = 0;
            
            try
            {
                foreach (TreeNode tn in e.Node.Nodes)
                {
                    string[] str2 = Directory.GetDirectories(@tn.Name);
                    foreach (string str in str2)
                    {
                        TreeNode temp = new TreeNode();
                        temp.Name = str;
                        temp.Text = str.Substring(str.LastIndexOf('\\') + 1);
                        e.Node.Nodes[i].Nodes.Add(temp);
                    }
                    i++;
                }
            }
            catch { }
            toolStripTextBox1.Text = currListViewAdress;
        }

       

        private void ClickOnColumn(object sender, ColumnClickEventArgs e)
        {
            
            if (e.Column == 0)
            {
                if (listView1.Sorting == SortOrder.Descending)
                    listView1.Sorting = SortOrder.Ascending;
                else
                    listView1.Sorting = SortOrder.Descending;
            }
            toolStripTextBox1.Text = currListViewAdress;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           
            if (currIndex - 1 != -1)
            {
                try
                {
                    currIndex--;
                    currListViewAdress = ((string)Adresses[currIndex]);
                    
                    if (currIndex - 1 == -1)
                        toolStripButton1.Enabled = false;
                    else
                        toolStripButton1.Enabled = true;
                    FileInfo f = new FileInfo(@currListViewAdress);
                    string t = "";
                    string[] str2 = Directory.GetDirectories(@currListViewAdress);
                    string[] str3 = Directory.GetFiles(@currListViewAdress);
                    listView1.Items.Clear();
                    ListViewItem lw = new ListViewItem();
                    if (listView1.View == View.Details)
                    {
                        foreach (string s2 in str2)
                        {
                            f = new FileInfo(@s2);
                            string type = "Папка";
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 7);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                        foreach (string s2 in str3)
                        {
                            f = new FileInfo(@s2);
                            string type = "Файл";
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                    }
                    else
                    {
                        foreach (string s2 in str2)
                        {
                            f = new FileInfo(@s2);
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t }, 7);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                        foreach (string s2 in str3)
                        {
                            f = new FileInfo(@s2);
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t }, 1);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                    }
                    toolStripTextBox1.Text = currListViewAdress;
                }
                catch { }
            }

        }
       private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition, ToolStripDropDownDirection.Right);
            }            
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (listView1.SelectedItems[0].ImageIndex == 7 || listView1.SelectedItems[0].ImageIndex == 0)
            {
                currListViewAdress = ((string)Adresses[currIndex]);
                currListViewAdress = toolStripTextBox1.Text;
                
                Adresses.Add(listView1.SelectedItems[0].Name);
                currIndex++;
                currListViewAdress = ((string)Adresses[currIndex]);
                FileInfo f = new FileInfo(@listView1.SelectedItems[0].Name);
                string t = "";
                string[] str2 = Directory.GetDirectories(@listView1.SelectedItems[0].Name);
                string[] str3 = Directory.GetFiles(@listView1.SelectedItems[0].Name);
                listView1.Items.Clear();
                ListViewItem lw = new ListViewItem();
                if (listView1.View == View.Details)
                {
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        string type = "Папка";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 7);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    foreach (string s2 in str3)
                    {
                        f = new FileInfo(@s2);
                        string type = "Файл";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                }
                else
                {
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 7);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    foreach (string s2 in str3)
                    {
                        f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 1);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    toolStripTextBox1.Text = currListViewAdress;
                }
            }
            else
            {
                
                System.Diagnostics.Process MyProc = new System.Diagnostics.Process();
                MyProc.StartInfo.FileName = @listView1.SelectedItems[0].Name;
                MyProc.Start();
                currListViewAdress = toolStripTextBox1.Text;
            }
            toolStripTextBox1.Text = currListViewAdress;
            toolStripButton1.Enabled = true;
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Удалить этот файл безвозвратно?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Yes)
            {
                if (listView1.SelectedItems[0].ImageIndex == 7 || listView1.SelectedItems[0].ImageIndex == 0)
                {
                    Directory.Delete(listView1.SelectedItems[0].Name, true);
                }
                else
                {
                    File.Delete(listView1.SelectedItems[0].Name);
                }
                try
                {
                    string[] str2 = Directory.GetDirectories((string)Adresses[currIndex]);
                    string[] str3 = Directory.GetFiles((string)Adresses[currIndex]);
                    currIndex++;
                    currListViewAdress = toolStripTextBox1.Text;
                    Adresses.Add(toolStripTextBox1.Text);
                    
                    if (currIndex - 1 == -1)
                        toolStripButton1.Enabled = false;
                    else
                        toolStripButton1.Enabled = true;
                    FileInfo f = new FileInfo(@toolStripTextBox1.Text);
                    string t = "";
                    listView1.Items.Clear();
                    ListViewItem lw = new ListViewItem();
                    if (listView1.View == View.Details)
                    {
                        foreach (string s2 in str2)
                        {
                            f = new FileInfo(@s2);
                            string type = "Папка";
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 0);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                        foreach (string s2 in str3)
                        {
                            f = new FileInfo(@s2);
                            string type = "Файл";
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                    }
                    else
                    {
                        foreach (string s2 in str2)
                        {
                            f = new FileInfo(@s2);
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t }, 0);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                        foreach (string s2 in str3)
                        {
                            f = new FileInfo(@s2);
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            lw = new ListViewItem(new string[] { t }, 1);
                            lw.Name = s2;
                            listView1.Items.Add(lw);
                        }
                    }
                }
                catch { }

                try
                {
                    treeView1.Nodes.Clear();
                    
                    listView1.ColumnClick += new ColumnClickEventHandler(ClickOnColumn);
                    ColumnHeader c = new ColumnHeader();
                    c.Text = "Имя";
                    c.Width = c.Width + 80;
                    ColumnHeader c2 = new ColumnHeader();
                    c2.Text = "Размер";
                    c2.Width = c2.Width + 60;
                    ColumnHeader c3 = new ColumnHeader();
                    c3.Text = "Тип";
                    c3.Width = c3.Width + 60;
                    ColumnHeader c4 = new ColumnHeader();
                    c4.Text = "Изменен";
                    c4.Width = c4.Width + 60;
                    listView1.Columns.Add(c);
                    listView1.Columns.Add(c2);
                    listView1.Columns.Add(c3);
                    listView1.Columns.Add(c4);
                    
                    string[] str = Environment.GetLogicalDrives();
                    int n = 1;
                    foreach (string s in str)
                    {
                        try
                        {
                            TreeNode tn = new TreeNode();
                            tn.Name = s;
                            tn.Text = "Локальный диск " + s;
                            treeView1.Nodes.Add(tn.Name, tn.Text, 6);
                            FileInfo f = new FileInfo(@s);
                            string t = "";
                            string[] str2 = Directory.GetDirectories(@s);
                            foreach (string s2 in str2)
                            {
                                t = s2.Substring(s2.LastIndexOf('\\') + 1);
                                ((TreeNode)treeView1.Nodes[1]).Nodes.Add(s2, t, 7);
                            }
                        }
                        catch { }
                        n++;
                    }
                    foreach (TreeNode tn in treeView1.Nodes)
                    {
                        for (int i = 65; i < 91; i++)
                        {
                            char sym = Convert.ToChar(i);
                            if (tn.Name == sym + ":\\")
                                tn.SelectedImageIndex = 6;
                        }
                    }
                }
                catch { }
            }
        }

        private void свойстваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FileAttributes attributes = File.GetAttributes(listView1.SelectedItems[0].Name);
                File.SetAttributes(listView1.SelectedItems[0].Name, attributes);
               
            }
            catch { }
        }

        private void toolStripTextBox1_Enter(object sender, EventArgs e)
        {
            toolStripTextBox1.ForeColor = Color.Black;
        }

        private void toolStripTextBox1_Leave(object sender, EventArgs e)
        {
            toolStripTextBox1.ForeColor = Color.DimGray;
        }

        StringCollection paths = new StringCollection();
        string copypast;
        string copypastName;
        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paths.Add(listView1.SelectedItems[0].Name);
            copypast = listView1.SelectedItems[0].Name;
            copypastName = listView1.SelectedItems[0].Text;
            Clipboard.SetFileDropList(paths);
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FileStream fs = File.Create(toolStripTextBox1.Text + @"\" + copypastName))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
                fs.Write(info, 0, info.Length);
            }
            File.Copy(copypast, toolStripTextBox1.Text + @"\" + copypastName, true);
            try
            {
                string[] str2 = Directory.GetDirectories((string)Adresses[currIndex]);
                string[] str3 = Directory.GetFiles((string)Adresses[currIndex]);
                currIndex++;
                currListViewAdress = toolStripTextBox1.Text;
                Adresses.Add(toolStripTextBox1.Text);
                
                if (currIndex - 1 == -1)
                    toolStripButton1.Enabled = false;
                else
                    toolStripButton1.Enabled = true;
                FileInfo f = new FileInfo(@toolStripTextBox1.Text);
                string t = "";
                listView1.Items.Clear();
                ListViewItem lw = new ListViewItem();
                if (listView1.View == View.Details)
                {
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        string type = "Папка";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, "", type, f.LastWriteTime.ToString() }, 0);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    foreach (string s2 in str3)
                    {
                        f = new FileInfo(@s2);
                        string type = "Файл";
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, f.Length.ToString() + " байт", type, f.LastWriteTime.ToString() }, 1);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                }
                else
                {
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 0);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    foreach (string s2 in str3)
                    {
                        f = new FileInfo(@s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t }, 1);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                }
            }
            catch { }

            try
            {
                treeView1.Nodes.Clear();
                
                listView1.ColumnClick += new ColumnClickEventHandler(ClickOnColumn);
                ColumnHeader c = new ColumnHeader();
                c.Text = "Имя";
                c.Width = c.Width + 80;
                ColumnHeader c2 = new ColumnHeader();
                c2.Text = "Размер";
                c2.Width = c2.Width + 60;
                ColumnHeader c3 = new ColumnHeader();
                c3.Text = "Тип";
                c3.Width = c3.Width + 60;
                ColumnHeader c4 = new ColumnHeader();
                c4.Text = "Изменен";
                c4.Width = c4.Width + 60;
                listView1.Columns.Add(c);
                listView1.Columns.Add(c2);
                listView1.Columns.Add(c3);
                listView1.Columns.Add(c4);
                
                string[] str = Environment.GetLogicalDrives();
                int n = 1;
                foreach (string s in str)
                {
                    try
                    {
                        TreeNode tn = new TreeNode();
                        tn.Name = s;
                        tn.Text = "Локальный диск " + s;
                        treeView1.Nodes.Add(tn.Name, tn.Text, 6);
                        FileInfo f = new FileInfo(@s);
                        string t = "";
                        string[] str2 = Directory.GetDirectories(@s);
                        foreach (string s2 in str2)
                        {
                            t = s2.Substring(s2.LastIndexOf('\\') + 1);
                            ((TreeNode)treeView1.Nodes[1]).Nodes.Add(s2, t, 7);
                        }
                    }
                    catch { }
                    n++;
                }
                foreach (TreeNode tn in treeView1.Nodes)
                {
                    for (int i = 65; i < 91; i++)
                    {
                        char sym = Convert.ToChar(i);
                        if (tn.Name == sym + ":\\")
                            tn.SelectedImageIndex = 6;
                    }
                }
            }
            catch { }
        }

        private void переименоватьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        

        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.BackColor = colorDialog1.Color;
                treeView1.BackColor = colorDialog1.Color;
                listView1.BackColor = colorDialog1.Color;
                toolStrip1.BackColor = colorDialog1.Color;
                toolStrip2.BackColor = colorDialog1.Color;
                
            }
        }

        private void toolStripDropDownButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
