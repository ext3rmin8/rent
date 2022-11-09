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
    public partial class baseForm : Form
    {        
        public baseForm()
        {
            InitializeComponent();
            subForm = new subForm();
        }
                
        subForm subForm;
        FileStream fileStream;
        StreamReader streamReader;

        bool isFormLoaded = false;
        int defaultColumnCount;
        Month nextMonth;

        enum Month
        {
            January = 1,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }

        private void button1_Click(object sender, EventArgs e)
        {
            subForm.Show();
            subForm.Left = Cursor.Position.X - subForm.Size.Width / 2;
            subForm.Top = Cursor.Position.Y - subForm.Size.Height / 2;
        }        
        /// <summary>
        /// Расчет количества столбцов изходя из данных в файле
        /// </summary>
        /// <returns></returns>
        public int NumberOfColumns()
        {
            fileStream = new FileStream(@"base.txt", FileMode.Open);
            streamReader = new StreamReader(fileStream);

            string[] fileToRows = streamReader.ReadToEnd().Split('$');
            string[] rowsToStrings = fileToRows[1].Split('#');

            streamReader.Close();
            fileStream.Close();

            return rowsToStrings.Count();
        }
        /// <summary>
        /// Формирование таблиц
        /// </summary>
        /// <param name="dgv"></param>
        public void CreateDataGrid(DataGridView dgv)
        {
            if (dgv == dataGridView1 )
                dataGridView1.Columns.Add("Column1", "При въезде");
            
            Month month;
            bool isYearChanged = false;
                       
            int firstMonth = (int)Month.October;
            int realMonth = 0;

            for (int i = firstMonth; i < firstMonth + NumberOfColumns() - 1; i++)
            {
                if (i > 12)
                {
                    i = 1;
                    isYearChanged = true;
                }                
                
                month = (Month)i;
                dgv.Columns.Add("Column1", month.ToString());
                realMonth++;

                if (isYearChanged && realMonth == (NumberOfColumns() - 1))
                    break;
            }

            dgv.Rows.Add(8);
            dgv.TopLeftHeaderCell.Value = "Счетчики";
            dgv.Rows[0].HeaderCell.Value = "Х. вода туалет";
            dgv.Rows[1].HeaderCell.Value = "Г. вода туалет";
            dgv.Rows[2].HeaderCell.Value = "Х. вода ванная";
            dgv.Rows[3].HeaderCell.Value = "Г. вода ванная";
            dgv.Rows[4].HeaderCell.Value = "Эл-во день";
            dgv.Rows[5].HeaderCell.Value = "Эл-во ночь";

            if (dgv == dataGridView1)
            {                
                dgv.Rows[6].HeaderCell.Value = "Отопление КПУ";
                dgv.Rows[7].HeaderCell.Value = "Г. вод. снаб";
            }
            else if (dgv == dataGridView2)
            {                
                dgv.Rows[6].HeaderCell.Value = "Водоотведение";
                dgv.Rows[7].HeaderCell.Value = "Итого";
            }

            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            int defaultRowHeaderWidth = dgv.TopLeftHeaderCell.Size.Width + 2;
            dgv.Width = defaultRowHeaderWidth;

            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                dgv.Width += dgv.Columns[i].Width;
            }

            dgv.Width = dgv.Width + 35;///asdadasd///

            dgv.Height = 24;

            for (int i = 0; i < dgv.RowCount; i++)
            {
                dgv.Height += dgv.Rows[i].Height;
            }

            if (dgv == dataGridView1)
            {
                defaultColumnCount = dgv.ColumnCount;
            }

            dgv.Height = dgv.Height + 15;////

            isFormLoaded = true;
        }
        /// <summary>
        /// Заполнение исходной таблицы данными из файла
        /// </summary>
        public void FillDataGrid()
        {
            fileStream = new FileStream(@"base.txt", FileMode.Open);
            streamReader = new StreamReader(fileStream);

            string[] rowsToStrings;
            int numberOfRows;
            try
            {
                string[] fileToRows = streamReader.ReadToEnd().Split('$');
                numberOfRows = fileToRows.Count();

                dataGridView1.RowCount = numberOfRows - 1;
                for (int i = 0; i < numberOfRows - 1; i++)
                {
                    rowsToStrings = fileToRows[i].Split('#');
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        dataGridView1.Rows[i].Cells[j].Value = rowsToStrings[j];
                    }
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при открытии файла!");
            }

            streamReader.Close();
            fileStream.Close();
        }
        /// <summary>
        /// Расчет результатов
        /// </summary>
        public void CalculateTheResult()
        {
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo()
            {
                NumberDecimalSeparator = ".",
            };

            double waterRate = subForm.waterRate;
            double elDay = subForm.elDay;
            double elNight = subForm.elNight;
            double waterDrainage = subForm.waterDrainage;
            
            for (int i = 0; i <= 5; i++)
            {
                for (int j = 0; j < dataGridView2.ColumnCount; j++)
                {
                    double rate;
                    if (i <= 3)
                        rate = waterRate;
                    else if (i == 4)
                        rate = elDay;
                    else
                        rate = elNight;

                    var value = (Double.Parse(dataGridView1.Rows[i].Cells[j + 1].Value.ToString(), numberFormatInfo)
                                - Double.Parse(dataGridView1.Rows[i].Cells[j].Value.ToString(), numberFormatInfo))
                                * rate;
                    dataGridView2.Rows[i].Cells[j].Value = Math.Round(value, 1);
                }
            }

            double [] waterDrainageValues = new double[dataGridView2.ColumnCount];
            
            for (int j = 0; j < dataGridView1.ColumnCount - 1  ; j++)
            {
                for (int i = 0; i <=3; i++)
                {
                    waterDrainageValues[j] += (Double.Parse(dataGridView1.Rows[i].Cells[j + 1].Value.ToString(), numberFormatInfo)
                                - Double.Parse(dataGridView1.Rows[i].Cells[j].Value.ToString(), numberFormatInfo));

                }
            }

            for (int i = 0; i < waterDrainageValues.Length; i++)
            {
                waterDrainageValues[i] *= waterDrainage;
                dataGridView2.Rows[6].Cells[i].Value = Math.Round(waterDrainageValues[i],1);
            }
            
            double [] monthResults = new double[dataGridView2.ColumnCount];

            for (int i = 0; i < dataGridView2.ColumnCount; i++)
            {
                dataGridView2.Rows[7].Cells[i].Value = 0;

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    double.TryParse((row.Cells[i].Value ?? "0").ToString().Replace(".", ","), out double result);
                    monthResults[i] += result;                
                }

                var heating = Double.Parse(dataGridView1.Rows[6].Cells[i + 1].Value.ToString(), numberFormatInfo) + Double.Parse(dataGridView1.Rows[7].Cells[i + 1].Value.ToString(), numberFormatInfo);
                if (heating > 1500)
                    heating = 1500;

                monthResults[i] += heating;
                dataGridView2.Rows[7].Cells[i].Value = Math.Round(monthResults[i], 1);
            }

        }

        private void baseForm_Load(object sender, EventArgs e)
        {
            CreateDataGrid(dataGridView1);
            CreateDataGrid(dataGridView2);
            FillDataGrid();
            CalculateTheResult();
        }

        /// <summary>
        /// Запись в файл и расчет результатов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {

            //if (File.Exists(@"base.txt"))            
            //    fileStream = new FileStream(@"base.txt", FileMode.Open);            
            //else            
                fileStream = new FileStream(@"base.txt", FileMode.Create);                            
                
            StreamWriter streamWriter = new StreamWriter(fileStream);

            try
            {
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    for (int i = 0; i < dataGridView1.Rows[j].Cells.Count; i++)
                    {
                        if (i == dataGridView1.Rows[j].Cells.Count - 1)                        
                            streamWriter.Write(dataGridView1.Rows[j].Cells[i].Value + "$");                        
                        else                         
                            streamWriter.Write(dataGridView1.Rows[j].Cells[i].Value + "#");                        
                    }
                }

                streamWriter.Close();
                fileStream.Close();

                CalculateTheResult();

                MessageBox.Show("Файл успешно сохранен");
            }
            catch
            {
                MessageBox.Show("Ошибка при сохранении файла!");
            }
        }
        /// <summary>
        /// Новый месяц в исходной таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            nextMonth = (Month)Enum.Parse(typeof(Month), dataGridView1.Columns[dataGridView1.ColumnCount - 1].HeaderCell.Value.ToString()) + 1;
            if ((int)nextMonth > 12)
                nextMonth = Month.January;

            dataGridView1.Columns.Add("Column1", nextMonth.ToString());

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Cells[dataGridView1.ColumnCount - 1].Value = 0;
            }

            dataGridView1.Width += dataGridView1.Columns[dataGridView1.ColumnCount-1].Width;
            
        }
        /// <summary>
        /// Новый месяц в результатах
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            if (isFormLoaded)
            {
                dataGridView2.Columns.Add("Column1", nextMonth.ToString());
                dataGridView2.Width += dataGridView2.Columns[dataGridView2.ColumnCount - 1].Width;
            }
        }

        /// <summary>
        /// Удаление столбца в исходной таблице
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.ColumnCount > defaultColumnCount)
            {
                var width = dataGridView1.Columns[dataGridView1.ColumnCount - 1].Width;
                dataGridView1.Columns.Remove(dataGridView1.Columns[dataGridView1.ColumnCount - 1]);
                dataGridView1.Width -= width;
            }

            if (dataGridView1.ColumnCount <= defaultColumnCount && checkBox1.Checked)
            {
                var width = dataGridView1.Columns[dataGridView1.ColumnCount - 1].Width;
                dataGridView1.Columns.Remove(dataGridView1.Columns[dataGridView1.ColumnCount - 1]);
                dataGridView1.Width -= width;
            }
            checkBox1.Checked = false;
        }

        /// <summary>
        /// Удаление столбца в таблице с результатами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_ColumnRemoved(object sender, DataGridViewColumnEventArgs e)
        {
            var width = dataGridView2.Columns[dataGridView2.ColumnCount - 1].Width;
            dataGridView2.Columns.Remove(dataGridView2.Columns[dataGridView2.ColumnCount - 1]);
            dataGridView2.Width -= width;
        }
    }
}
