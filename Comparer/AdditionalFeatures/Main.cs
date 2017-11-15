﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Specialized;
using Comparer.AdditionalFeatures;

namespace Comparer
{
    public partial class Main : Form
    {
        public string inputFile;
        bool IMG = false;
        
        public Main()
        {
            InitializeComponent();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            inputFile = Navigator.SelectInputFile();
            Image x;
            try
            {
                x = Image.FromFile(inputFile);
                x = resizeImage(x, new Size(740, 692));
                mainPictureBox.Image = x;
                IMG = true;
                if (IMG)
                {
                    recognizeButton.Enabled = true;
                    compareButton.Enabled = true;
                }
                imageCheck.Checked = true;
            }
            catch
            {

            }
            //something to execute from web service
            //ComparerWebService.WebServiceSoapClient client = new ComparerWebService.WebServiceSoapClient();
            //MessageBox.Show(client.HelloWorld());
        }

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        private void recognizeButton_Click(object sender, EventArgs e)
        {
            string text = ImageRecognition.ExtractText(inputFile);
            var textManager = new TextManager();
            text = textManager.PrepareText(text);
            mainLabel.Text = text;
            Program.ResultWriter(text, Directory.GetCurrentDirectory() + "\\TempResult.txt");
        }

        private void rotateLeftButton_Click(object sender, EventArgs e)
        {   
            mainPictureBox.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            mainPictureBox.Refresh();
        }

        private void rotateRightButton_Click(object sender, EventArgs e)
        {
            mainPictureBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            mainPictureBox.Refresh();
        }

        private void compareButton_Click(object sender, EventArgs e)
        {
            var shopEngine = new CompareShops();
            string infoFile = shopEngine.CompareResults();
            moneySaved.Text = File.ReadAllText(infoFile);
        }
        private static readonly HttpClient client = new HttpClient();


        private async Task btnTest_ClickAsync(object sender, EventArgs e)
        {
            string url = @"http://192.168.0.200/TestComparer/api/mytest/computemulti";

            using (var client = new HttpClient())
            {

                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("", "String to Pass")
            });
                var result = await client.PostAsync(url, content);
                string resultContent = await result.Content.ReadAsStringAsync();
                moneySaved.Text = resultContent;
            }



            //moneySaved.Text = responseString;
            
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            btnTest_ClickAsync(sender, e);
        }
    }
}