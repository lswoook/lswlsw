using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.Threading;
using OpenQA.Selenium.Firefox;
using System.Windows;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using OpenQA.Selenium.Remote;
using System.IO;
using System.Diagnostics;

namespace ConsoleApplication7
{
    class Program
    {
        static void Main(string[] args)
        {
            string id,pw,snkeywords, snmem, smaxf, sminf;
            string[] kw = new string[1000];
            int nmem , maxf , minf;

            
            object TypMissing = Type.Missing;
            // Excel Application 정의
            Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Excel.Workbook _workbook = null;

            // 파일이 존재 하면 열고, 없으면 새로 만듬.
            if (File.Exists(@"d:\Test.xls"))
            {
                _workbook = ExcelApp.Workbooks.Open(@"c:\URl검색\url.xls", TypMissing, TypMissing, TypMissing, TypMissing,
                    TypMissing, TypMissing,
                    TypMissing, TypMissing, TypMissing, TypMissing, TypMissing, TypMissing, TypMissing, TypMissing);
            }
            else
            {
                // Add Work Book
                _workbook = ExcelApp.Workbooks.Add(Type.Missing);
                // Save Excel File
                _workbook.SaveAs(@"c:\URL검색기\url.xls", Excel.XlFileFormat.xlWorkbookNormal, TypMissing, TypMissing,
               TypMissing, TypMissing, Excel.XlSaveAsAccessMode.xlNoChange,
               TypMissing, TypMissing, TypMissing, TypMissing, TypMissing);
            }  

            //키워드 읽기
            string[] KWList = new string[1000];
            StreamReader reader = new StreamReader(@"C:\밴드검색기\키워드.txt");
            int k = 1;
            while (reader.EndOfStream == false)
            {
                string line = reader.ReadLine();
                KWList[k] = line;
                Console.WriteLine(k + ":" + KWList[k]);
                k++;
            }

            int Cline = k - 1;
            Console.WriteLine(Cline + "개 키워드 검색 되었습니다.");
            reader.Close();


            string[] Black = new string[1000];
            StreamReader reader2 = new StreamReader(@"C:\밴드검색기\블랙단어.txt");
            int k1 = 1;

            while (reader2.EndOfStream == false)
            {
                string line = reader2.ReadLine();
                Black[k1] = line;
                Console.WriteLine(k1 + ":" + Black[k1]);
                k1++;
            }

            int CBlack = k1 - 1;
            Console.WriteLine(CBlack + "개 블랙단어 검색 되었습니다.");
            reader2.Close();

            IWebElement query;

            Console.Write("아이디:");
            id = Console.ReadLine();

            Console.Write("비밀번호:");
            pw = Console.ReadLine();

            //IWebDriver driver = new FirefoxDriver();

            ChromeOptions opts = new ChromeOptions();
            opts.AddArgument("--incognito");
            IWebDriver driver = new ChromeDriver(opts); //<-Add your path

            driver.Navigate().GoToUrl("http://band.us/#!/");
            Console.WriteLine("Trying to access " + "http://band.us/#!/");
            Actions action = new Actions(driver);
            IWait<IWebDriver> wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5.00));
            IWait<IWebDriver> wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(1.00));

            /*메인 로그인*/
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='header']/div/div/div/a[3]")));
            IWebElement ML = driver.FindElement(By.XPath("//*[@id='header']/div/div/div/a[3]"));
            ML.Click();

            /*이메일 로그인 선택*/
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='login_list']/li[2]/a")));
            IWebElement EB = driver.FindElement(By.XPath("//*[@id='login_list']/li[2]/a"));
            EB.Click();

            /* 아이디입력 */
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='input_email']")));
            IWebElement ID = driver.FindElement(By.XPath("//*[@id='input_email']"));
            ID.SendKeys(id);

            /* 비번입력 */
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='input_password']")));
            IWebElement PW = driver.FindElement(By.XPath("//*[@id='input_password']"));
            PW.SendKeys(pw);

            /*로그인 버튼 누르기*/
            IWebElement submit = driver.FindElement(By.XPath("//*[@id='email_login_form']/button"));
            submit.Click();


            for (int cox = 1; cox <= Cline; cox++) // Cline = 키워드 갯수
            {
                int resultnum = 0;
                //검색창 클릭 후 키워드 검색
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='input_search_view25']")));
                query = driver.FindElement(By.XPath("//*[@id='input_search_view25']"));
                action.Click(query).Perform();
                action.SendKeys(KWList[cox]).Perform();
                action.SendKeys(Keys.Enter).Perform();

                //검색 결과를 기다린다            
                string Tnum;
                int num;
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='content']/div/div[2]/section/div/ul/li[2]")));

                Tnum = driver.FindElement(By.XPath("//*[@id='content']/div/div[2]/section/div/h1/span")).Text;

                num = Convert.ToInt32(Tnum);

                Console.WriteLine(num + "개 검색 되었습니다.");
                int rnum = 0;

                for (int i = 1; i <= rnum; i++)
                {
                    //밴드로 이동
                    string bstr = "//*[@id='content']/div/div[2]/section/div/ul/li[{0}]";
                    string data = i.ToString();
                    string address = string.Format(bstr, data);

                    IWebElement bio = driver.FindElement(By.XPath(address));
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(address)));
                    action.MoveToElement(bio).Perform();

                    //밴드 멤버수 수집
                    int mnum;
                    string smnum;
                    bstr = "//*[@id='content']/div/div[2]/section/div/ul/li[{0}]/div/a/div[2]/p[2]/span[1]/strong";
                    address = string.Format(bstr, data);
                    smnum = driver.FindElement(By.XPath(address)).Text.Replace(",", "");
                    mnum = Convert.ToInt32(smnum);
                    int blackflag = 0;

                    //밴드명 수집
                    string name;
                    bstr = "//*[@id='content']/div/div[2]/section/div/ul/li[{0}]/div/a/div[2]/strong";
                    address = string.Format(bstr, data);
                    name = driver.FindElement(By.XPath(address)).Text;

                    for (int kq = 1; kq <= CBlack; kq++)
                    {
                        if (name.Contains(Black[kq]))
                        {
                            blackflag = 1;
                            break;
                        }
                    }

                    if (blackflag == 0)
                    {
                        if (maxf == 0)
                        {
                            if (mnum >= minf)
                            {
                                //밴드주소, 밴드명 수집
                                string link;
                                bstr = "//*[@id='content']/div/div[2]/section/div/ul/li[{0}]/div/a";
                                address = string.Format(bstr, data);

                                link = driver.FindElement(By.XPath(address)).GetAttribute("href");
                                using (StreamWriter sw = new StreamWriter(@"C:\밴드검색기\result.txt", true))
                                {
                                    sw.Write(KWList[cox] + ";");
                                    sw.Write(link + ";");
                                    sw.Write(mnum + ";");
                                    sw.WriteLine(name + ";");
                                    resultnum++;
                                }
                                /*
                                Sheet.Cells[i, 1] = link;
                                Sheet.Cells[i, 2] = mnum;
                                Sheet.Cells[i, 3] = name;
                                //Sheet.Rows.AutoFit();
                                Sheet.Columns.AutoFit(); // 글자 크기에 맞게 셀 크기를 자동으로 조절.
                                                         //Range_.Rows.AutoFit();
                                                         //Range_.Columns.AutoFit();   
                                _workbook.Save();
                                */
                            }
                        }
                        else if (minf == 0)
                        {
                            if (mnum <= maxf)
                            {
                                //밴드주소, 밴드명 수집
                                string link;
                                bstr = "//*[@id='content']/div/div[2]/section/div/ul/li[{0}]/div/a";
                                address = string.Format(bstr, data);
                                link = driver.FindElement(By.XPath(address)).GetAttribute("href");
                                using (StreamWriter sw = new StreamWriter(@"C:\밴드검색기\result.txt", true))
                                {
                                    sw.Write(KWList[cox] + ";");
                                    sw.Write(link + ";");
                                    sw.Write(mnum + ";");
                                    sw.WriteLine(name + ";");
                                    resultnum++;
                                }
                            }
                        }
                        else
                        {
                            if (minf <= mnum && mnum <= maxf)
                            {
                                //밴드주소, 밴드명 수집
                                string link;
                                bstr = "//*[@id='content']/div/div[2]/section/div/ul/li[{0}]/div/a";
                                address = string.Format(bstr, data);
                                link = driver.FindElement(By.XPath(address)).GetAttribute("href");
                                using (StreamWriter sw = new StreamWriter(@"C:\밴드검색기\result.txt", true))
                                {
                                    sw.Write(KWList[cox] + ";");
                                    sw.Write(link + ";");
                                    sw.Write(mnum + ";");
                                    sw.WriteLine(name + ";");
                                    resultnum++;
                                }
                            }
                        }
                    }
                }
                /*
                var LastRow = Sheet.UsedRange.Rows.Count;
                LastRow = LastRow + Sheet.UsedRange.Row;
                int ee = 0;

                for (ee = 1; ee <= LastRow+1; ee++)
                {
                    if (ExcelApp.WorksheetFunction.CountA(Sheet.Rows[ee]) == 0)
                        (Sheet.Rows[ee] as Microsoft.Office.Interop.Excel.Range).Delete();
                }
                _workbook.Save();
                */
                driver.Navigate().GoToUrl("http://band.us/#!/");
                using (StreamWriter sw = new StreamWriter(@"C:\밴드검색기\추출갯수.txt", true))
                {
                    sw.WriteLine(";" + KWList[cox] + ";" + resultnum + ";");
                }

            }

            /*
            //작업완료후 엑셀 끄기
            ExcelApp.Workbooks.Close(); // 별효과 없음.
            ExcelApp.Quit();            // 별효과 없음.
            // 강제로 Excel Porcess종료하기.
            Process[] ExCel = Process.GetProcessesByName("EXCEL");
            if (ExCel.Count() != 0)
            {
                ExCel[0].Kill();
            }
            */
            Console.WriteLine("작업완료");
            driver.Close();
            driver.Quit();
        }
    }
}
