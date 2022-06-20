# DocumentTemplateMaker.NET
透過範本檔案自動化繁複的文件操作流程

所有設定都寫在 Parameter.json 的 json 格式設定檔案內  
# DateRange  
執行程式後面帶參數 DateRange 如下:  
`DocumentTemplateMaker.NET.exe DateRange`  
預設範例是指定時間範圍內  
建立範例檔案內的內容置換與檔案名稱置換  

# JsonMap  
執行程式後面帶參數 JsonMap 如下:  
`DocumentTemplateMaker.NET.exe JsonMap`  
會自動取出文字檔案內容 json 格式  
取出 key 與 value  

# Replace  
執行程式後面帶參數 Replace 如下:  
`DocumentTemplateMaker.NET.exe Replace`  
置換文件內指定字串符號為我們設定的內容  

# ReplaceJsonMap
執行程式後面帶參數 ReplaceJsonMap 如下: 
`DocumentTemplateMaker.NET.exe ReplaceJsonMap`
JsonMap + Replace 功能組合

ReplaceJsonMap.txt 放 kibana 的 log 如下:
`{Cid:hmmHKhJWH4F12QPaPuc2Sh,Wid:RvK5l4h1ls9z20120404vS0616133312344, .... ,timeTick:1655386392344}`
Parameter.json 放 kibana 的 log 上沒有的資訊
KeyWords 取代成為 ReplaceWords 的內容，會輸出一個 map.txt 檔案  
參考說明: [補單與重新結算](https://hackmd.io/@chiisen/By7hm0ES9)


