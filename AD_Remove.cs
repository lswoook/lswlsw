using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

using AutoItX3Lib;

using Excel = Microsoft.Office.Interop.Excel;

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
            
            int rCnt = 1, cCnt = 1, idc; // 엑셀 연동에 필요한 행.열 갯수
            
            string str, str1, str2, str3, before, after;
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
            driver.FindElement(By.XPath("//*[@id='header']/div/div/div/a[3]")).Click();

            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='login_list']/li[2]/a")));
            driver.FindElement(By.XPath("//*[@id='login_list']/li[2]/a")).Click();

            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='input_email']")));
            driver.FindElement(By.XPath("//*[@id='input_email']")).Click();

            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='input_password']")));
            driver.FindElement(By.XPath("//*[@id='input_password']")).SendKeys(spw);

            driver.FindElement(By.XPath("//*[@id='email_login_form']/button")).Click();

            //탭갯수
            int ctab = 0;

            /*밴드 정렬*/
            try{
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='content']/div/section/header/div/div/button[1]")));
                driver.FindElement(By.XPath("//*[@id='content']/div/section/header/div/div/button[1]")).Click();
                Thread.Sleep(1000);

                wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='content']/div/section/header/div/div/section[1]/div/div[2]/article/div/div[3]/div/div/a/span")));
                driver.FindElement(By.XPath("//*[@id='content']/div/section/header/div/div/section[1]/div/div[2]/article/div/div[3]/div/div/a/span")).Click();
                Thread.Sleep(1000);
                
                action.SendKeys(Keys.Down).Perform();
                action.SendKeys(Keys.Down).Perform();
                action.SendKeys(Keys.Enter).Perform();

                wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='content']/div/section/header/div/div/section[1]/div/div[2]/article/footer/div/button")));
                driver.FindElement(By.XPath("//*[@id='content']/div/section/header/div/div/section[1]/div/div[2]/article/footer/div/button")).Click();
                Thread.Sleep(1000);

                autoit.Send("^t");
                ctab++;
                
                driver.SwitchTo().Window(driver.WindowHandles[ctab]);
                driver.Navigate().GoToUrl(URL);
            }
            catch (Exception){
                Console.WriteLine("로그인 실패");
                driver.Close();
                continue;
            }

            /*밴드정렬 한뒤, 차례대로 들어가서 검사 진행*/
            int startb = frontb + 1;
            Console.WriteLine(startb + "번째 블록에서 시작합니다");
            int Bnum = 0;

            while (true)
            {
                Thread.Sleep(1000);
                /*밴드 갯수 세기*/
                try{
                    wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='content']/div/section/div/ul/li")));
                    Bnum = driver.FindElements(By.XPath("//*[@id='content']/div/section/div/ul/li")).Count - frontb - backb;
                    Console.WriteLine("현재 밴드갯수:" + Bnum);
                }
                catch (Exception){
                    Console.WriteLine("모니터링 실패: " + IdList[n]);
                }
                Thread.Sleep(1000);

                //밴드 순차적으로 들어가기
                string bstr = "//*[@id='content']/div/section/div/ul/li[{0}]/div/a/div[1]/div/span";
                string data = startb.ToString();
                string address = string.Format(bstr, data);
                try{
                    wait.Until(ExpectedConditions.ElementExists(By.XPath(address)));
                    IWebElement band = driver.FindElement(By.XPath(address));
                    band.Click();
                }
                catch (Exception){
                    driver.SwitchTo().Window(driver.WindowHandles[ctab]).Close();
                    ctab--;
                    driver.SwitchTo().Window(driver.WindowHandles[ctab]);
                    break;
                }

                //포스팅 내용중 광고성 메세지 포함여부 검사
                try{
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
                
                /*포스팅 내용중 키워드 풀에 해당하는 단어가 있는지 체크*/
                int AD_ref_cnt = 0;
                for(int i = 0 ; i< keywords.length ; i++0)
                {
                    if (context.Contains(keywords[i]))
                    {
                        AD_ref_cnt++;
                    }
                }
                
                /*링크 + 악성키워드로 판단되면 삭제진행*/
                if (AD_reft_cnt >= 1 && context.Contains("http"))
                {
                        //광고성 포스팅 삭제
                        context.ClipPut(before);
                        try{
                            waitc.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='layerContainer']/div/section/div/div[2]/article/div/div/div[4]/div/button[2]")));
                            driver.FindElement(By.XPath("//*[@id='layerContainer']/div/section/div/div[2]/article/div/div/div[4]/div/button[2]")).Click(post2).Perform();
                        }
                        catch (Exception){
                            
                        }
                        Thread.Sleep(rand.Next(2000, 3000));
                    }
                }
            }
        
            //로그아웃
            while (true)
            {
                try{
                    driver.SwitchTo().Window(driver.WindowHandles[0]);
                    break;
                }
                catch (Exception){
                }
            }
            try{
                // Check the presence of alert
                Thread.Sleep(1000);
                IAlert alert = driver.SwitchTo().Alert();
                alert.Accept();
            }
            catch (NoAlertPresentException){
            }
        }
    }
}
