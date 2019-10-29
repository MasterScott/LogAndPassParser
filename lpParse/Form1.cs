using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MaterialSkin.Controls;
using System.IO;
using System.IO.Compression;

namespace lpParse
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Directory.CreateDirectory("Archives\\");
        }

        private void materialFlatButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog arch = new OpenFileDialog();
            arch.Title = "Выберите архив для импорта текста из него";
            arch.Filter = "Archive files(*.rar, *.zip)|*.zip|Text files (*.txt)|*.txt|All files(*.*)|*.*";
            if (arch.ShowDialog() == DialogResult.OK)
            {
                string[] fileType = arch.FileName.Select(s => arch.FileName.Remove(0, arch.FileName.LastIndexOf('.') + 1)).ToArray();
                string[] fName = arch.FileName.Split('\\');
                
                if (fileType.First().ToLower() == "zip" || fileType.First().ToLower() == "txt")
                {
                    if (fileType.First().ToLower() == "zip")
                    {
                        int dots = 0;
                        for (int i = 0; i < fName.Length; i++)
                        {
                            if (fName.Last()[i] == '.')
                            {
                                dots++;
                            }
                            if (dots > 1)
                            {
                                MessageBox.Show("Ошибка. В имени архива больше одной точки. Пожалуйста, переименуйте архив.");
                                return;
                            }
                        }

                        string[] fileSOnDot = fName.Last().Split('.');
                        try
                        {
                            Directory.CreateDirectory("Archives\\" + fileSOnDot.First());
                            try
                            {
                                ZipFile.ExtractToDirectory(arch.FileName, @"Archives\\" + fileSOnDot.First() + "\\");
                            }
                            catch(Exception)
                            {
                                MessageBox.Show("Не удалось открыть архив.");
                            }
                        }
                        catch (IOException)
                        {
                            MessageBox.Show("Ошибка. Такой архив уже импортировался.");
                            return;
                        }

                        string[] files = Directory.GetFiles(@"Archives\", "*.txt", SearchOption.AllDirectories);

                        foreach (string file in files)
                        {
                            StreamReader readF = new StreamReader(file);
                            textBox1.Text += "\r\n" + readF.ReadToEnd() + "\r\n";
                            readF.Close();
                        }

                        Directory.Delete("Archives\\" + fileSOnDot.First(), true);
                        MessageBox.Show("Succcess", "FormatCParser");
                    }
                    else if (fileType.First().ToLower() == "txt")
                    {
                        StreamReader readF = new StreamReader(arch.FileName);
                        textBox1.Text += "\r\n" + readF.ReadToEnd() + "\r\n";
                        readF.Close();
                        MessageBox.Show("Success", "FormatCParser");
                    }
                }
                else
                {
                    MessageBox.Show("Расширение файла не поддерживается.");
                }
            }
        }

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void materialFlatButton3_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true) { textBox2.Text += "\r\n\r\n[" + DateTime.Now + "]\r\n"; }

            if (textBox1.Text.Trim() != "" && materialSingleLineTextField1.Text.Trim() != "")
            {
                foreach (var lines in textBox1.Text.Split(new[] 
                {
                    Environment.NewLine
                }, StringSplitOptions.RemoveEmptyEntries))
                {
                    try
                    {
                        if (lines.ToLower().Contains(materialSingleLineTextField1.Text.ToLower()) == true)
                        {
                            string[] text = lines.Split('|');
                            if (checkBox1.Checked == true)
                            {
                                if (text.First().ToLower().Contains("https") == true && text.First().ToLower().Contains("http") == true)
                                {
                                    textBox2.Text += "\r\n" + text[1].Replace(" ", "") + ":" + text.Last().Replace(" ", "");
                                }
                            }
                            else
                            {
                                if (text.First().ToLower().Contains("https") == true || text.First().ToLower().Contains("http") == true)
                                {
                                    textBox2.Text += "\r\n" + text[1].Replace(" ", "") + ":" + text.Last().Replace(" ", "") + "\r\n";
                                }
                                else
                                {
                                    textBox2.Text += "\r\n" + lines.Replace(" ", "") + "\r\n";
                                }
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
            else
            {
                MessageBox.Show("Text boxes is empty.");
            }
        }

        private void materialFlatButton4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                MessageBox.Show("Включены только те результаты, у которых есть \"https\" или \"http\"");
            }
            else
            {
                MessageBox.Show("Включены все результаты");
            }
        }

        private void materialFlatButton1_Click_1(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() != "")
            {
                SaveFileDialog saveF = new SaveFileDialog();
                saveF.Title = "Сохранение вывода";
                saveF.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
                if (saveF.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter saveFile = new StreamWriter(saveF.FileName);
                    saveFile.WriteLine(textBox2.Text);
                    saveFile.Close();
                }
            }
            else
            {
                MessageBox.Show("Text box is empty.");
            }
        }

        private void materialFlatButton5_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "FormatC:  Привет! Ты заинтересовался кто создал эту программу?\n" +
                "Не сомневайся, её создал - FormatC или Armadillo-CLD, называй как хочешь :D\n" +
                "Мои контакты: \n" +
                "Github: https://github.com/armadillo-cld\n" +
                "VK: https://vk.com/sha1.encryption\n" +
                "Telegram: @cyber_andrey\n" +
                "Приятного использования :)");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void materialFlatButton6_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                MessageBox.Show("Текущая дата включена");
            }
            else
            {
                MessageBox.Show("Текущая дата выключена");
            }
        }
    }
}
