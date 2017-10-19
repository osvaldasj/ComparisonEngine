﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Comparer.CompareEngine;

namespace Comparer
{
    public class CompareShops : IComparer
    {

        private string infoFile;
        public string infofile
        {
            get { return infofile; }
            set { infofile = value; }
        }

        //get path to database directory
        private static string currentDirectory = (Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()))) + @"\Comparer\bin\Debug"); //Directory.GetCurrentDirectory();
        //class which compares current check product list with one of the databases lists
        public string CompareResults()
        {
            List<FromFileToStruct.Product> currentCheck = new List<FromFileToStruct.Product>();
            currentCheck = FromFileToStruct.MakeProductList(currentDirectory + "\\TempResult.txt");

            List<FromFileToStruct.Product> fullDatabase = new List<FromFileToStruct.Product>();
            fullDatabase = FromFileToStruct.MakeProductList2(currentDirectory + "\\FullDatabase.txt");

            var maxima = from x in fullDatabase where x.shop == "maxima" select x;
            var rimi = from x in fullDatabase where x.shop == "rimi" select x;            

            WriteToFile write = new WriteToFile();
            float moneyDifference = 0;


            if (currentCheck[0].shop == "maxima")
            {
                infoFile = write.Write(currentDirectory, 1);
                moneyDifference = EvaluateTwoChecks(currentCheck, rimi, write);
            }
            else if (currentCheck[0].shop == "rimi")
            {
                infoFile = write.Write(currentDirectory, 2);
                moneyDifference = EvaluateTwoChecks(currentCheck, maxima, write);
            }
            else
            {
                infoFile = "";
                MessageBox.Show("unrecognized shop");
            }

            write.Write(infoFile, moneyDifference);

            return infoFile;
        }

        public float EvaluateTwoChecks(List<FromFileToStruct.Product> currentCheck, IEnumerable otherShopList, WriteToFile write)
        {
            int neededValue = 85;
            int currentValue;
            float moneyDifference = 0;
            int counter = 0;
       
            for (int i = 0; i <= currentCheck.Count - 1; i++)
            {
                foreach(FromFileToStruct.Product otherShop in otherShopList)
                {
                    currentValue = Compare(currentCheck[i].name, otherShop.name);
                    if (currentValue >= neededValue)
                    {
                        moneyDifference += (currentCheck[i].price - otherShop.price);
                        write.Write(currentCheck[i].name, infoFile, (currentCheck[i].price - otherShop.price));
                    }
                    else
                    {
                        counter++;
                    }
                }
                if (counter == currentCheck.Count)
                {
                    //RequestForDatabase(currentCheck[i]);  //to be made in the future
                    MessageBox.Show("unrecognized product");
                }
                counter = 0;
            }

            return moneyDifference;
        }

        //compares two strings how close they are the same and returns the value between 0 and 100 meaning %
        public int Compare(object x, object y)
        {
            int counter = 0;
            string A = (string)x;
            string B = (string)y;
            int lenA = A.Length, lenB = B.Length; ;
            int lenMin, lenMax;
            if (lenA > lenB)
            {
                lenMin = lenB;
                lenMax = lenA;
            }
            else
            {
                lenMin = lenA;
                lenMax = lenB;
            }
            for (int i = 0; i < lenMin; i++)
            {
                if (A[i] == B[i])
                    counter++;
            }
            if (lenMax != 0)
                return counter * 100 / lenMax;
            else
                return 0;
        }
    }
}
