# DocumentTemplateMaker.NET
透過範本檔案自動化繁複的文件操作流程

所有設定都寫在 Parameter.json 的 json 格式設定檔案內  
# DateRange  
執行程式後面帶參數 DateRange 如下:  
DocumentTemplateMaker.NET.exe DateRange  
預設範例是指定時間範圍內  
建立範例檔案內的內容置換與檔案名稱置換  

# JsonMap  
執行程式後面帶參數 JsonMap 如下:  
DocumentTemplateMaker.NET.exe JsonMap  
會自動取出文字檔案內容 json 格式  
取出 key 與 value  

# Replace  
執行程式後面帶參數 Replace 如下:  
DocumentTemplateMaker.NET.exe Replace  
置換文件內指定字串符號為我們設定的內容  

# ReplaceJsonMap
執行程式後面帶參數 ReplaceJsonMap 如下: 
DocumentTemplateMaker.NET.exe ReplaceJsonMap
JsonMap + Replace 功能組合
