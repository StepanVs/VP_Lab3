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
using System.Linq;
using System.Xml.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lab_3
{
    public partial class Form1 : Form
    {
        string Path { get; set; } = "";
        OpenFileDialog openFileDialog = new OpenFileDialog();
        private DataView dataView=new DataView();

        string path;
        public Form1()
        {
            InitializeComponent();
            dataView.Table = Airport;
            bindingSource.DataSource = dataView;
            this.Controls.SetChildIndex(statusStrip1, 1);
            openFileDialog.Filter = "dat|*.dat"; 
            openFileDialog.Multiselect = false;
            openFileDialog.FileOk += OpenFileDialog_FileOk;
            MessageBox.Show(DateTime.Now.ToString());
        }
        
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }
        private void OpenFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            path = openFileDialog.FileName;
            FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            DataTable dataTable=  binaryFormatter.Deserialize(fileStream) as DataTable;
            foreach (DataRow item in dataTable.Rows)
            {
                Airport.ImportRow(item);
                toolStripStatusLabel1.Text= item["DepartureTime"].ToString();
                
            }
            fileStream.Close();
            Path = openFileDialog.FileName;
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileStream fileStream = new FileStream(Path, FileMode.Create, FileAccess.Write);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, Airport);
            fileStream.Close();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }

       

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, Airport);
            fileStream.Close();
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(Path))
               saveToolStripMenuItem.Enabled = false;
            else
                saveToolStripMenuItem.Enabled = true;
        }

    
        private void Form1_Load(object sender, EventArgs e) { }

        private void toolStripTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //Destination
                DataView dataView1 = new DataView();
                DataTable filteredData = null;
                EnumerableRowCollection<DataRow> b = null;
                string str1 = toolStripTextBox1.Text;
                string str2 = toolStripTextBox2.Text;
                try
                {
                    int lw = Convert.ToInt32(str1);
                    b = Airport.AsEnumerable().Where(row => row.Field<string>("Destination").Contains(str2) && row.Field<int>("LuggageWeight") > lw);
                }
                catch (Exception)
                {

                    b = Airport.AsEnumerable().Where(row => row.Field<string>("Destination").Contains(str2));
                }
                filteredData = Airport.Clone();
                filteredData.TableName = "filteredData";
                filteredData.Clear();
                foreach (DataRow row in b)
                {
                    filteredData.ImportRow(row);
                }
                dataView.Table = filteredData;
            }




        }

        private void toolStripTextBox3_Click(object sender, EventArgs e)
        {

            bindingSource.RemoveFilter();
            dataView.Table = Airport;
            toolStripTextBox1.Text = "";
            toolStripTextBox2.Text = "";
            toolStripTextBox7.Text = "";
        }

        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                DataView dataView1 = new DataView();
                DataTable filteredData = null;
                EnumerableRowCollection<DataRow> b = null;
                EnumerableRowCollection<DataColumn> c = null;
                DateTime date = DateTime.Now;
                int lw = Convert.ToInt32(toolStripTextBox1.Text);
                b = Airport.AsEnumerable().Where(row => row.Field<int>("LuggageWeight") > lw);

                filteredData = Airport.Clone();
                filteredData.TableName = "filteredData";
                filteredData.Clear();
                foreach (DataRow row in b)
                {
                    
                    
                    
                    filteredData.ImportRow(row);
                }
                dataView.Table = filteredData;
                
            }
        }

        private void toolStripTextBox7_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                
                DataView dataView1 = new DataView();
                DataTable filteredData = null;
                EnumerableRowCollection<DataRow> b = null;
                string str1 = toolStripTextBox1.Text;
                string str2 = toolStripTextBox2.Text;
                string str3 = toolStripTextBox7.Text;
                DateTime date = Convert.ToDateTime(str3);
              
                try
                {
                    int lw = Convert.ToInt32(str1);
                    b = Airport.AsEnumerable().Where(row => row.Field<string>("Destination").Contains(str2) && row.Field<int>("LuggageWeight") > lw);
                }
                catch (Exception)
                {
                    b = Airport.AsEnumerable().Where(row => row.Field<DateTime>("ArrivalTime").Date==date.Date);
                }
                filteredData = Airport.Clone();
                filteredData.TableName = "filteredData";
                filteredData.Clear();
                foreach (DataRow row in b)
                {
                    filteredData.ImportRow(row);
                }
                dataView.Table = filteredData;
            }
        }
    }
}
