## AD_Remove

### 해당 커뮤니티 접속 및 로그인
```C#
            driver.Navigate().GoToUrl(URL);
            autoit.Send("{ENTER}");
            IWait<IWebDriver> wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10.00));
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
```

### 최신 글 중 광고성 단어가 있는지, 로컬 단어 풀과 비교
```C#
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

```

### 광고성이라고 판단 될 시 삭제 버튼 찾기
```C#
if (context.Contains(keywords[i])){
    //광고성 포스팅 삭제
    context.ClipPut(before);    
    try{
        waitc.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='layerContainer']/div/section/div/div[2]/article/div/div/div[4]/div/button[2]")));
        IWebElement post2 = driver.FindElement(By.XPath("//*[@id='layerContainer']/div/section/div/div[2]/article/div/div/div[4]/div/button[2]"));
        action.Click(post2).Perform();
    }
    catch (Exception)
    {
     Thread.Sleep(rand.Next(2000, 3000));
    }
}
```
