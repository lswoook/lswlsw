## Band_Search

### 엑셀파일내 기록된 키워드를 배열에 저장
```C#
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
```

### 키워드를 검색하여 타겟정보 기록
```C#
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
```



