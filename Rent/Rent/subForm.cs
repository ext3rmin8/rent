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
using System.Globalization;

namespace Rent
{
    public partial class subForm : Form
    {
        public subForm()
        {
            InitializeComponent();                       

            rates.Columns.Add("Column1", "");
            rates.Rows.Add(4);
            
            FillDataGrid();
            SetRates();
        }

        baseForm bF;

        public double waterRate { get; private set; }
        public double elDay { get; private set; }
        public double elNight { get; private set; }
        public double waterDrainage { get; private set; }

        private void SetRates()
        {
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo()
            {
                NumberDecimalSeparator = ".",
            };

            waterRate = Double.Parse(rates.Rows[0].Cells[0].Value.ToString(), numberFormatInfo);
            elDay = Double.Parse(rates.Rows[1].Cells[0].Value.ToString(), numberFormatInfo);
            elNight = Double.Parse(rates.Rows[2].Cells[0].Value.ToString(), numberFormatInfo);
            waterDrainage = Double.Parse(rates.Rows[3].Cells[0].Value.ToString(), numberFormatInfo);
        }

        FileStream fStream;
        StreamReader streamReader;

        private void subForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void FillDataGrid()
        {
            fStream = new FileStream(@"rates.txt", FileMode.Open);
            streamReader = new StreamReader(fStream);

            string[] fileToRows = streamReader.ReadToEnd().Split('#');
            for (int i = 0; i < rates.RowCount; i++)
            {
                rates.Rows[i].Cells[0].Value = fileToRows[i];
            }

            streamReader.Close();
            fStream.Close();
        }

        private void subForm_Load(object sender, EventArgs e)
        {
            rates.Rows[0].HeaderCell.Value = "Вода";
            rates.Rows[1].HeaderCell.Value = "Эл-во день";
            rates.Rows[2].HeaderCell.Value = "Эл-во ночь";
            rates.Rows[3].HeaderCell.Value = "Водоотведение";

            rates.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            rates.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            rates.Height = 3;

            for (int i = 0; i < rates.RowCount; i++)
            {
                rates.Height += rates.Rows[i].Height;
            }

            button1.Enabled = false;            
        }

        private void rates_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            fStream = new FileStream(@"rates.txt", FileMode.Open);
            StreamWriter streamWriter = new StreamWriter(fStream);

            for (int i = 0; i < rates.RowCount; i++)
            {
                streamWriter.Write(rates.Rows[i].Cells[0].Value + "#");
            }

            streamWriter.Close();
            fStream.Close();

            SetRates();
            bF = new baseForm();
            bF.CalculateTheResult();
        }
    }
}
