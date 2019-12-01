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
using AutoItX3Lib;
using OpenQA.Selenium.Firefox;
using System.Windows;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using OpenQA.Selenium.Edge;
using System.IO;

namespace Bandposting2._6
{
    class Program
    {
        static void Main(string[] args)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;
            string str, str1, str2, str3, before, after, chat;

            int rCnt = 1, cCnt = 1, idc; // 열 갯수
            int cCnt = 1; // 행 갯수
            string[] IdList = new string[1000];
            string[] LbF = new string[1000];
            string[] LaF = new string[1000];
            string[] Lct = new string[1000];
            AutoItX3 autoit = new AutoItX3();
            Random rand = new Random();
            IWebDriver driver = new ChromeDriver(options);

            Actions action = new Actions(driver);


            //메인화면 접속
            driver.Navigate().GoToUrl(URL);
            autoit.Send("{ENTER}");
            IWait<IWebDriver> wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10.00));
            IWait<IWebDriver> waitc = new WebDriverWait(driver, TimeSpan.FromSeconds(2.00));
            IWait<IWebDriver> waitcc = new WebDriverWait(driver, TimeSpan.FromSeconds(1.00));
            IWebElement query;

            /*메인 로그인*/
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='header']/div/div/div/a[3]")));
            query = driver.FindElement(By.XPath("//*[@id='header']/div/div/div/a[3]"));
            query.Click();

            /*이메일 로그인 선택*/
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='login_list']/li[2]/a")));
            query = driver.FindElement(By.XPath("//*[@id='login_list']/li[2]/a"));
            query.Click();

            /* 아이디입력 */
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='input_email']")));
            query = driver.FindElement(By.XPath("//*[@id='input_email']"));
            query.SendKeys(IdList[n]);

            Thread.Sleep(1000);

            /* 비번입력 */
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='input_password']")));
            query = driver.FindElement(By.XPath("//*[@id='input_password']"));
            query.SendKeys(spw);

            Thread.Sleep(1000);

            /*로그인 버튼 누르기 >_< */
            IWebElement submit = driver.FindElement(By.XPath("//*[@id='email_login_form']/button"));
            submit.Click();

            //캡챠
            if (n == 0)
            {
                Console.WriteLine("캡챠 완료후 콘솔창 엔터하세요.");
                Console.ReadKey();
                Console.WriteLine("실행된 크롬창을 활성화 해주세요.");
                Thread.Sleep(10000);
            }
            try
            {
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='content']/div/section/header/div/div/button[1]")));
                Console.WriteLine(IdList[n] + "로그인성공");
            }
            catch (Exception)
            {
                Console.WriteLine(IdList[n] + "로그인 실패 다시 시도합니다");
                driver.Close();
                continue;
            }

            //탭갯수
            int ctab = 0;

            /*밴드 정렬*/
            try
            {
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='content']/div/section/header/div/div/button[1]")));
                IWebElement sort = driver.FindElement(By.XPath("//*[@id='content']/div/section/header/div/div/button[1]"));
                sort.Click();
                Thread.Sleep(1000);

                wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='content']/div/section/header/div/div/section[1]/div/div[2]/article/div/div[3]/div/div/a/span")));
                IWebElement sort3 = driver.FindElement(By.XPath("//*[@id='content']/div/section/header/div/div/section[1]/div/div[2]/article/div/div[3]/div/div/a/span"));
                sort3.Click();
                Thread.Sleep(1000);
                action.SendKeys(Keys.Down).Perform();
                action.SendKeys(Keys.Down).Perform();
                action.SendKeys(Keys.Enter).Perform();
                Thread.Sleep(500);

                wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='content']/div/section/header/div/div/section[1]/div/div[2]/article/footer/div/button")));
                IWebElement sortc = driver.FindElement(By.XPath("//*[@id='content']/div/section/header/div/div/section[1]/div/div[2]/article/footer/div/button"));
                sortc.Click();
                Thread.Sleep(1000);

                autoit.Send("^t");
                ctab++;
                Thread.Sleep(1000);
                driver.SwitchTo().Window(driver.WindowHandles[ctab]);
                driver.Navigate().GoToUrl(URL);
            }
            catch (Exception)
            {
                Console.WriteLine("로그인 실패");
                driver.Close();
                continue;
            }

            int startb = frontb + 1;
            Console.WriteLine(startb + "번째 블록에서 시작합니다");
            int Bnum = 0;

            while (true)
            {
                Thread.Sleep(1000);
                /*밴드 갯수 세기*/
                try
                {
                    wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='content']/div/section/div/ul/li")));
                    Bnum = driver.FindElements(By.XPath("//*[@id='content']/div/section/div/ul/li")).Count - frontb - backb;
                    Console.WriteLine("현재 밴드갯수:" + Bnum);
                }
                catch (Exception)
                {
                    Console.WriteLine("모니터링 실패: " + IdList[n]);
                }
                Thread.Sleep(1000);

                //밴드 순차적으로 들어가기
                string bstr = "//*[@id='content']/div/section/div/ul/li[{0}]/div/a/div[1]/div/span";
                string data = startb.ToString();
                string address = string.Format(bstr, data);
                try
                {
                    wait.Until(ExpectedConditions.ElementExists(By.XPath(address)));
                    IWebElement band = driver.FindElement(By.XPath(address));
                    band.Click();
                }
                catch (Exception)
                {
                    driver.SwitchTo().Window(driver.WindowHandles[ctab]).Close();
                    ctab--;
                    driver.SwitchTo().Window(driver.WindowHandles[ctab]);
                    break;
                }

                //포스팅 내용중 광고성 메세지 포함여부 검사
                try
                {
                    Thread.Sleep(2000);
                    IWebElement textbox = driver.FindElement(By.XPath("//*[@id='content']/section/div[2]/div/button"));
                    str context = textbox.getText; // 포스팅 내용을 context에 저장

                    //이미 삭제 된 경우
                    try
                    {
                        // Check the presence of alert
                        IAlert alert = driver.SwitchTo().Alert();
                        alert.Accept();
                        driver.Navigate().GoToUrl(URL);
                        if (startb == Bnum)
                        {
                            break;
                        }
                        continue;
                    }
                    catch (NoAlertPresentException) { }
                }
                catch (Exception)
                {
                    Thread.Sleep(1000);
                    driver.SwitchTo().Window(driver.WindowHandles[ctab]);
                    startb++;
                    driver.Navigate().GoToUrl(URL);
                    continue;
                }

                for (int i = 0; i < n; i++)
                {

                    if (context.Contains(keywords[i]))
                        {

                        //광고성 포스팅 삭제
                        context.ClipPut(before);
                        try
                        {
                            waitc.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='layerContainer']/div/section/div/div[2]/article/div/div/div[4]/div/button[2]")));
                            IWebElement post2 = driver.FindElement(By.XPath("//*[@id='layerContainer']/div/section/div/div[2]/article/div/div/div[4]/div/button[2]"));
                            action.Click(post2).Perform();
                        }
                        catch (Exception)
                        {
                        }
                        Thread.Sleep(rand.Next(2000, 3000));
                    }
                }
            }
            //로그아웃
            while (true)
            {
                try
                {
                    driver.SwitchTo().Window(driver.WindowHandles[0]);
                    break;
                }
                catch (Exception)
                {
                }
            }
            try
            {
                // Check the presence of alert
                Thread.Sleep(1000);
                IAlert alert = driver.SwitchTo().Alert();
                alert.Accept();
            }
            catch (NoAlertPresentException)
            {
            }
        }
    }
}
