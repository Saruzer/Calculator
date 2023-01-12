using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Calculator
{
    public partial class Form1 : Form
    {
        int resultCount = 1;
        string numText = "";
        List<int> Nums = new List<int>();
        List<int> signs = new List<int>();
        int LastNum;
        public Form1()
        {
            InitializeComponent();
            addNum(0);
        }
        public void showDebbug()
        {
            Console.WriteLine("\n-----------");
            Console.Write("Nums: ");
            foreach(var i in Nums)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine("");
            Console.Write("signs: ");
            foreach (var i in signs)
            {
                Console.Write(getSign(i) + " ");
            }
            Console.WriteLine("\n-----------");
            Console.WriteLine("num text = " + numText);
            Console.WriteLine("nums count = " + Nums.Count);
            Console.WriteLine("signs count= " + signs.Count);
            Console.WriteLine("");
        }
        public char getSign(int signID)
        {
            switch (signID)
            {
                case 1:
                    return '+';
                case 2:
                    return '-';
                case 3:
                    return '*';
                case 4:
                    return '/';
            }
            return ' ';
        }
        public bool checkSign()
        {
            if(textBox1.Text[textBox1.Text.Length - 1] == '+')
            {
                return false;
            }
            if (textBox1.Text[textBox1.Text.Length - 1] == '-')
            {
                return false;
            }
            if (textBox1.Text[textBox1.Text.Length - 1] == '*')
            {
                return false;
            }
            if (textBox1.Text[textBox1.Text.Length - 1] == '/')
            {
                return false;
            }
            return true;
        }
        public void inputSign(int signID)
        {

            button14.Enabled = false;
            button16.Enabled = false;
            if (textBox1.Text.Length < 1)
            {
                numText = "";
                return;
            }
            if (numText != "")
            {
                int tempNum = Convert.ToInt32(numText);
                if (Nums.Count < signs.Count + 1)
                    Nums.Add(tempNum);
                signs.Add(signID);
                numText = "";
            }
            if(checkSign() == true)
            {
                textBox1.Text += getSign(signID);

            }
            else
            {
                char[] charsText = textBox1.Text.ToCharArray();
                charsText[textBox1.Text.Length - 1] = getSign(signID);
                string tempString = "";
                textBox1.Text = tempString;
                foreach (var i in charsText)
                {
                    textBox1.Text += Convert.ToString(i);

                }
                
                var tempList = signs.ToArray();
                tempList[tempList.Length - 1] = signID;
                signs = tempList.ToList();
            }
            showDebbug();
        }
        public void addNum(int num)
        {
            button14.Enabled = true;

            button16.Enabled = false;
            numText += num.ToString();
            textBox1.Text += num.ToString();

            showDebbug();
        }
        public void BeforeClickEquals()
        {
            
            button16.Enabled = true;
            if (Nums.Count < 1 && numText.Length > 0)
            {
                textBox2.Text = "Answer: " + Convert.ToInt32(numText).ToString();
                return;
            }
            else if(Nums.Count < 1 && numText.Length <= 0)
            {
                textBox2.Text = "Answer: " + 0;
                return;

            }
            List<int> prioritySign = new List<int>(); //*,/

            for(int i = 0; i <= signs.Count - 1; i++)
            {
                if(signs[i] == 3 || signs[i] == 4)
                {
                    prioritySign.Add(i);
                }
            }
            bool noPriority = true;

            int result = Nums[0];

            if (numText == "")
            {
                if (prioritySign.Count > 0)
                {
                    for (int i = 0; i <= prioritySign.Count - 1; i++)
                    {
                        Equal(prioritySign[i], ref result);
                    }
                }
                for (int i = 0; i <= signs.Count - 1; i++)
                {
                    noPriority = true;
                    for (int j = 0; j <= prioritySign.Count - 1; j++)
                    {
                        if (i == prioritySign[j])
                        {
                            noPriority = false;
                        }

                    }
                    if (noPriority)
                        Equal(i, ref result);

                }
            }
            else
            {
                int tempNum = Convert.ToInt32(numText);
                if (Nums.Count < signs.Count + 1)
                    Nums.Add(tempNum);

                if(prioritySign.Count > 0)
                {
                    for (int i = 0; i <= prioritySign.Count - 1; i++)
                    {
                        Equal(prioritySign[i], ref result);
                    }
                }
                for (int i = 0; i <= signs.Count - 1; i++)
                {
                    noPriority = true;
                    for (int j = 0; j <= prioritySign.Count - 1; j++)
                    {
                        if (i == prioritySign[j])
                        {
                            noPriority = false;
                        }

                    }
                    if (noPriority)
                        Equal(i, ref result);
                }
            }
            showDebbug();
            textBox2.Text = "Answer: " + result.ToString();
        }
        public void Reset()
        {
            numText = "";
            textBox1.Text = "";
            Nums.Clear();
            signs.Clear();
            addNum(0);
        }
        public void Equal(int signid, ref int result)
        {
            
            Console.WriteLine("sign = " + getSign(signs[signid]));
            switch (signs[signid])
            {
                case 1:
                    Console.WriteLine(result + " += " + Nums[signid + 1]);
                    result += Nums[signid + 1];
                    break;
                case 2:
                    Console.WriteLine(result + " -= " + Nums[signid + 1]);
                    result -= Nums[signid + 1];
                    break;
                case 3:
                    Console.WriteLine(result + " *= " + Nums[signid + 1]);
                    result *= Nums[signid + 1];
                    break;
                case 4:
                    if (Nums[signid + 1] == 0)
                    {

                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        MessageBoxIcon ico = MessageBoxIcon.Error;
                        DialogResult res;
                        res = MessageBox.Show("Попытка деления на ноль!", "Ошибка", buttons, ico);
                        result = 0;
                        Reset();
                        return;
                    }
                    Console.WriteLine(result + " /= " + Nums[signid + 1]);
                    result /= Nums[signid + 1];
                    break;
            }
            LastNum = Nums[signid + 1];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int buttonNum = 1;
            addNum(buttonNum);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int buttonNum = 2;
            addNum(buttonNum);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int buttonNum = 3;
            addNum(buttonNum);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int buttonNum = 4;
            addNum(buttonNum);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int buttonNum = 5;
            addNum(buttonNum);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int buttonNum = 6;
            addNum(buttonNum);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int buttonNum = 7;
            addNum(buttonNum);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int buttonNum = 8;
            addNum(buttonNum);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int buttonNum = 9;
            addNum(buttonNum);
        }
        private void button17_Click(object sender, EventArgs e)
        {
            int buttonNum = 0;
            addNum(buttonNum);
        }
        private void button10_Click(object sender, EventArgs e) // +
        {
            int _signID = 1;
            inputSign(_signID);
        }

        private void button11_Click(object sender, EventArgs e) // -
        {
            int _signID = 2;
            inputSign(_signID);
        }

        private void button12_Click(object sender, EventArgs e) // *
        {
            int _signID = 3;
            inputSign(_signID);
        }

        private void button13_Click(object sender, EventArgs e) // /
        {
            int _signID = 4;
            inputSign(_signID);
        }
        private void button14_Click(object sender, EventArgs e) // =
        {
            BeforeClickEquals();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Reset();
        }
        
        private void button16_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon ico = MessageBoxIcon.None;
            DialogResult result;
            result = MessageBox.Show("Specify a directory to save", "Ok", buttons, ico);
            if (result == DialogResult.OK)
            {
                string namePath = "Result" + resultCount.ToString();
                folderBrowserDialog1.ShowDialog();
                string browseDir = folderBrowserDialog1.SelectedPath;
                string dir = browseDir + @"\" + namePath + ".txt";
                string text = textBox1.Text + "\n" + textBox2.Text;
                var file = File.Create(dir, 99,FileOptions.None);
                byte[] bytes = ASCIIEncoding.ASCII.GetBytes(text);
                Console.WriteLine(bytes.Length);
                Console.WriteLine(text.Length);
                file.Write(bytes, 0, text.Length);
                resultCount++;
            }
        }
    }
}
