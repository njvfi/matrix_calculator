using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_windows_form
{
    class matrix_calc
    {
        private int op;//0-додавання 1 віднімання 2,3 множ/діл на число 4- множ на матрицю 5-підн до степеня 6 транспонування
        private int width1;
        private int width2;
        private int high1;
        private int high2;
        private double[,] num1 = new double[5, 5];
        private double[,] num2 = new double[5, 5];


        public void input_start_data(int coming_op, int coming_width1, int coming_high1, int coming_width2, int coming_high2)
        {
            op = coming_op;
            width1 = coming_width1;
            width2 = coming_width2;
            high1 = coming_high1;
            high2 = coming_high2;
        }
        public void input_matrix1(double coming_num, int i, int j)
        {
            num1[i, j] = coming_num;
        }
        public void input_matrix2(double coming_num, int i, int j)
        {
            num2[i, j] = coming_num;
        }
        public int final_width() 
        {
            switch(op)
            {
                case 0://+
                    return width1;
                case 1://-
                    return high1;
                case 2://*число
                    return width1;
                case 3:// /число
                    return width1;
                case 4://*матриця
                    return width2;
                case 5://степінь
                    return width1;
                case 6://транспонування
                    return high1;
                default: return 1;
            }
        }
        public int final_high()
        {
            if (op == 6||op == 1) return width1;
            else return high1;
        }
        public double op0(int i,int j)//+
        {
            return num1[i,j]+num2[i,j];
        }
        public double op1(int i,int j)//-
        {
            return num1[i, j] + num2[j, i];
        }
        public double op2(int i,int j)//*число
        {
            return num1[i,j]*num2[0,0];
        }
        public double op3(int i, int j)// /число
        {
            return num1[i, j] / num2[0, 0];
        }
        public double op4(int i,int j)// * матриця
        {
            double sum=0;
            for(int i1=0;i1<=high2;i1++)
            {
                sum += (num1[i, i1] * num2[i1, j]);
            }
            return sum;
        }
        public double op5(int i, int j)//степінь
        {
            double sum = 0;
            for (int i1 = 0; i1 <= high1; i1++)
            {
                sum += (num1[i, i1] * num1[i1, j]);
            }
            return sum;
        }
        public double op6(int i, int j)//транспонування
        {
            return num1[j, i];
        }
    }
}
