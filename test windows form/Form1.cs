using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test_windows_form
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            init();
        }

        private int high_1_matrix, width_1_matrix, high_2_matrix, width_2_matrix = 0;

        public void init()//створення та підготовка елементів
        {
            comboBox_high1.Items.AddRange(new string[] { "1", "2", "3", "4", "5" });
            comboBox_width1.Items.AddRange(new string[] { "1", "2", "3", "4", "5" });
            comboBox_high1.SelectedIndex = 0;
            comboBox_width1.SelectedIndex = 0;
            comboBox_width2.Items.AddRange(new string[] { "1", "2", "3", "4", "5" });
            comboBox_width2.SelectedIndex = 0;
            comboBox_operation.Items.AddRange(new string[] { "Додавання","Віднімання","Множення на число", "Ділення на число", "Множення на Матрицю", "Піднесення до степеня","Транспонування" });
            comboBox_operation.SelectedIndex = 0;
            for (int i = 0; i < 5; i++)//початковий вид: доступні та видимі тільки по 1 комірці
                for (int j = 0; j < 5; j++)
                {
                    Controls["textBox1_" + i + "_" + j].Enabled = false;
                    Controls["textBox1_" + i + "_" + j].Visible = false;
                    Controls["textBox2_" + i + "_" + j].Enabled = false;
                    Controls["textBox2_" + i + "_" + j].Visible = false;
                    output.Controls["textBox3_" + i + "_" + j].Visible = false;
                }
            textBox1_0_0.Enabled = true;
            textBox1_0_0.Visible = true;
            textBox2_0_0.Enabled = true;
            textBox2_0_0.Visible = true;
            TextBox[,,] matrix_in = new TextBox[2, 5, 5];//створення комірок для тексту
            for (int m = 0; m < 2; m++)
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 5; j++)
                    {
                        matrix_in[m, i, j] = Controls["textBox" + (m + 1) + "_" + i + "_" + j] as TextBox;


                        matrix_in[m, i, j].KeyPress += new KeyPressEventHandler(beforechek);//фільтр введення
                        void beforechek(object sender, KeyPressEventArgs e)
                        {
                            TextBox aim_textbox = sender as TextBox;
                            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 44 && e.KeyChar != 46 && e.KeyChar != 45) //8 - backspace; 44 - ','; 46 - '.'; 45 - '-';
                                e.Handled = true;
                            if (e.KeyChar == '.')
                                e.KeyChar = ',';
                            if (e.KeyChar == ',')//перевірка позиції знаків
                            {
                                if (aim_textbox.Text.IndexOf(',') != -1)
                                    e.Handled = true;
                                return;
                            }
                            if (e.KeyChar == '-')
                            {
                                if (aim_textbox.Text.IndexOf('-') != -1)
                                    e.Handled = true;
                                return;
                            }
                            if (Char.IsControl(e.KeyChar))//перенесення курсора на наступну комірку
                            {
                                int textbox_m = 0;
                                int textbox_i = 0;
                                int textbox_j = 0;
                                findIndex(matrix_in, aim_textbox, ref textbox_m, ref textbox_i, ref textbox_j);
                                if (e.KeyChar == (char)Keys.Enter && comboBox_width1.SelectedIndex > textbox_j)
                                    matrix_in[textbox_m,textbox_i,textbox_j+1].Focus();
                                else if (e.KeyChar == (char)Keys.Enter && comboBox_high1.SelectedIndex > textbox_i)
                                    matrix_in[textbox_m,textbox_i+1,0].Focus();
                                else if (e.KeyChar == (char)Keys.Enter && textbox_m == 0) textBox2_0_0.Focus();
                                else if (e.KeyChar == (char)Keys.Enter && textbox_m == 1) button_calc.Focus();
                                return;
                            }
                        }
                    }
            foreach (TextBox item in this.Controls.OfType<TextBox>())//перевірка позиції '-' та ',' та доповнення рядку
            {
                item.TextChanged += new EventHandler(afterchek);

                void afterchek(object sender, EventArgs e)
                {
                    if (item.Text.IndexOf('-') != -1 && item.Text.IndexOf('-') != 0)
                    {
                        string temp = item.Text.Substring(0, item.Text.Length - 1);
                        item.Text = temp;
                    }
                    if (item.Text.IndexOf(',') == 0)
                    {
                        string temp = "0" + item.Text.Substring(0, item.Text.Length);
                        item.Text = temp;
                    }
                }
            }
            foreach (TextBox item in output.Controls.OfType<TextBox>()) 

            {
                item.KeyPress += new KeyPressEventHandler(unvisible);

                void unvisible(object sender, KeyPressEventArgs e)
                {
                    e.Handled = true;
                }
            }
        }
        private void findIndex(TextBox[,,] array, TextBox item, ref int textbox_m, ref int textbox_i, ref int textbox_j)//пошук положення комірки
        {
            for (int m = 0; m < 2; m++)
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 5; j++)
                    {
                        if (item.Location == array[m, i, j].Location)
                        {
                            textbox_m = m;
                            textbox_i = i;
                            textbox_j = j;
                        }
                    }
        }
        private void comboBox_operation_SelectedIndexChanged(object sender, EventArgs e)
        {
            check_operation();
        }
        private void comboBox_widht1_SelectedIndexChanged(object sender, EventArgs e)//встановлення ширини 1 матриці
        {
            check_operation();
        }

        private void comboBox_high1_SelectedIndexChanged(object sender, EventArgs e)//встановлення висоти 1 матриці
        {
            check_operation();
        }
        private void comboBox_width2_SelectedIndexChanged(object sender, EventArgs e)//встановлення ширини 2 матриці
        {
            check_operation();
        }


        private void button_calc_Click(object sender, EventArgs e) //при натисканні обчислити
        {
            matrix_calc matrix_calc = new matrix_calc();
            int width, high;
            for(int i=0; i<5;i++)
                for(int j=0;j<5;j++)
                {
                    output.Controls["textBox3_" + i + "_" + j].Text = "";//очищення матриці результатів
                    output.Controls["textBox3_" + i + "_" + j].Visible = false;
                    if (Controls["textBox1_" + i + "_" + j].Text == "") Controls["textBox1_" + i + "_" + j].Text = "0";//доповнення некорректних значень комірок
                    else if(Controls["textBox1_" + i + "_" + j].Text == "-") Controls["textBox1_" + i + "_" + j].Text += "1";
                    if (Controls["textBox2_" + i + "_" + j].Text == "") Controls["textBox2_" + i + "_" + j].Text = "0";
                    else if (Controls["textBox2_" + i + "_" + j].Text == "-") Controls["textBox2_" + i + "_" + j].Text += "1";
                }
            //переведення массиву комірок в массив чисел
            matrix_calc.input_start_data(comboBox_operation.SelectedIndex, width_1_matrix, high_1_matrix, width_2_matrix, high_2_matrix);
            for (int i = 0; i <= comboBox_high1.SelectedIndex; i++)
                for (int j = 0; j <= comboBox_width1.SelectedIndex; j++)
                {
                    matrix_calc.input_matrix1(Convert.ToDouble(Controls["textBox1_" + i + "_" + j].Text), i, j);
                }
            for (int i = 0; i <= high_2_matrix; i++)
                for (int j = 0; j <= comboBox_width2.SelectedIndex; j++)
                {
                    matrix_calc.input_matrix2(Convert.ToDouble(Controls["textBox2_" + i + "_" + j].Text), i, j);
                }
            width = matrix_calc.final_width();
            high = matrix_calc.final_high();
            switch (comboBox_operation.SelectedIndex)//виведення результату
            {
                case 0:
                    for (int i = 0; i <= high; i++)
                        for (int j = 0; j <= width; j++)
                        {
                            output.Controls["textBox3_" + i + '_' + j].Visible = true;
                            output.Controls["textBox3_" + i + '_' + j].Text = Convert.ToString(matrix_calc.op0(i, j));
                        }
                    break;
                case 1:
                    for (int i = 0; i <= high; i++)
                        for (int j = 0; j <= width; j++)
                        {
                            output.Controls["textBox3_" + i + '_' + j].Visible = true;
                            output.Controls["textBox3_" + i + '_' + j].Text = Convert.ToString(matrix_calc.op1(i, j));
                        }
                    break;
                case 2:
                    for (int i = 0; i <= high; i++)
                        for (int j = 0; j <= width; j++)
                        {
                            output.Controls["textBox3_" + i + '_' + j].Visible = true;
                            output.Controls["textBox3_" + i + '_' + j].Text = Convert.ToString(matrix_calc.op2(i, j));
                        }
                    break;
                case 3:
                    for (int i = 0; i <= high; i++)
                        for (int j = 0; j <= width; j++)
                        {
                            output.Controls["textBox3_" + i + '_' + j].Visible = true;
                            output.Controls["textBox3_" + i + '_' + j].Text = Convert.ToString(matrix_calc.op3(i, j));
                        }
                    break;
                case 4:
                    for (int i = 0; i <= high; i++)
                        for (int j = 0; j <= width; j++)
                        {
                            output.Controls["textBox3_" + i + '_' + j].Visible = true;
                            output.Controls["textBox3_" + i + '_' + j].Text = Convert.ToString(matrix_calc.op4(i, j));
                        }
                    break;
                case 5:
                    for (int i = 0; i <= high; i++)
                        for (int j = 0; j <= width; j++)
                        {
                            output.Controls["textBox3_" + i + '_' + j].Visible = true;
                            output.Controls["textBox3_" + i + '_' + j].Text = Convert.ToString(matrix_calc.op5(i, j));
                        }
                    break; 
                case 6:
                    for (int i = 0; i <= high; i++)
                        for (int j = 0; j <= width; j++)
                        {
                            output.Controls["textBox3_" + i + '_' + j].Visible = true;
                            output.Controls["textBox3_" + i + '_' + j].Text = Convert.ToString(matrix_calc.op6(i, j));
                        }
                    break;
            }
        }
        private void change_matrix_size(int matrix, int high, int width)//коррекція кількості комірок
        {
            for(int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    Controls["textBox" + matrix + "_" + i + "_" + j].Enabled = false;
                    Controls["textBox" + matrix + "_" + i + "_" + j].Visible = false;
                }
            int i1 = 0;
            int j1 = 0;
            do
            {
                Controls["textBox" + matrix + "_" + i1 + "_" + j1].Enabled = true;
                Controls["textBox" + matrix + "_" + i1 + "_" + j1].Visible = true;
                j1++;
                if ((high == -1) || (width == -1)) break;
                if ((j1 == width + 1) && (i1 == high))
                    break;
                else if (j1 == width + 1)
                {
                    j1 = 0;
                    i1++;
                }
            }
            while (true);
        }

        private void check_operation()//перевірка вибраної операції та відповідна зміна розмірів матриць і доступу до вибору ширини 2 матриці
        {
            switch (comboBox_operation.SelectedIndex)
            {
                case 0://додавання
                    high_1_matrix = comboBox_high1.SelectedIndex;
                    width_1_matrix = comboBox_width1.SelectedIndex;
                    high_2_matrix = comboBox_high1.SelectedIndex;
                    width_2_matrix = comboBox_width1.SelectedIndex;
                    change_matrix_size(1, high_1_matrix, width_1_matrix);
                    change_matrix_size(2, high_2_matrix, width_2_matrix);
                    comboBox_width2.Text = comboBox_width1.Text;
                    comboBox_width2.Enabled = false;
                    break;
                case 1://віднімання
                    high_1_matrix = comboBox_high1.SelectedIndex;
                    width_1_matrix = comboBox_width1.SelectedIndex;
                    high_2_matrix = comboBox_width1.SelectedIndex;
                    width_2_matrix = comboBox_high1.SelectedIndex;
                    change_matrix_size(1, high_1_matrix, width_1_matrix);
                    change_matrix_size(2, high_2_matrix, width_2_matrix);
                    comboBox_width2.Text = comboBox_high1.Text;
                    comboBox_width2.Enabled = false;
                    break;
                case 2://множення на число
                    high_1_matrix = comboBox_high1.SelectedIndex;
                    width_1_matrix = comboBox_width1.SelectedIndex;
                    high_2_matrix = 0;
                    width_2_matrix = 0;
                    change_matrix_size(1, high_1_matrix, width_1_matrix);
                    change_matrix_size(2, high_2_matrix, width_2_matrix);
                    comboBox_width2.Text = "1";
                    comboBox_width2.Enabled = false;
                    break;
                case 3://ділення на число
                    high_1_matrix = comboBox_high1.SelectedIndex;
                    width_1_matrix = comboBox_width1.SelectedIndex;
                    high_2_matrix = 0;
                    width_2_matrix = 0;
                    change_matrix_size(1, high_1_matrix, width_1_matrix);
                    change_matrix_size(2, high_2_matrix, width_2_matrix);
                    comboBox_width2.Text = "1";
                    comboBox_width2.Enabled = false;
                    break;
                case 4://множення на матрицю
                    high_1_matrix = comboBox_high1.SelectedIndex;
                    width_1_matrix = comboBox_width1.SelectedIndex;
                    high_2_matrix = comboBox_width1.SelectedIndex;
                    width_2_matrix = comboBox_width2.SelectedIndex; 
                    change_matrix_size(1, high_1_matrix, width_1_matrix);
                    change_matrix_size(2, high_2_matrix, width_2_matrix);
                    comboBox_width2.Enabled = true;
                    break;
                case 5:// піднесення до степеню
                    high_1_matrix = comboBox_high1.SelectedIndex;
                    width_1_matrix = comboBox_width1.SelectedIndex;
                    high_2_matrix = 0;
                    width_2_matrix = 0;
                    change_matrix_size(1, high_1_matrix, width_1_matrix);
                    change_matrix_size(2, high_2_matrix, width_2_matrix);
                    comboBox_width2.Text = "1";
                    comboBox_width2.Enabled = false;
                    break;
                case 6:
                    high_1_matrix = comboBox_high1.SelectedIndex;
                    width_1_matrix = comboBox_width1.SelectedIndex;
                    high_2_matrix = 0;
                    width_2_matrix = 0;
                    change_matrix_size(1, high_1_matrix, width_1_matrix);
                    change_matrix_size(2, high_2_matrix, width_2_matrix);
                    textBox2_0_0.Enabled = false;
                    textBox2_0_0.Visible = false;
                    comboBox_width2.Enabled = false;
                    comboBox_width2.Visible = false;
                    label_width2.Enabled = false;
                    label_width2.Visible = false;
                    break;
            }
        }
    }
}