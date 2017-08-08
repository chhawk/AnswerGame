using System;
//using System.IO;
//using System.Text;

//读取 csv
using UnityEngine;

public class CReadCsvBase
{

    //private static Encoding gb2312Encoding = Encoding.GetEncoding("gb2312");
    //private const string strUnityRootPath = "Assets/Resources/";
    //private const string strCsvEndName = ".csv";
    private  string[]  splitStrings= { "\r\n" };
    private const char splitCsv = ',';
    private const StringSplitOptions splitOp = StringSplitOptions.None;

    private int columnCount;   //行
    private int rowCount;      //列

    private string[][] allArrary;

    public delegate void CallBackGetValue(string[] value,int column);

    public CReadCsvBase(string path,CallBackGetValue callback)
    {
        columnCount = 0;
        rowCount = 0;
        allArrary = null;
        ReadCsv(path);
        getAllValue(callback);
    }

    void ReadCsv(string path)
    {
		TextAsset tx = CResourceLoad.LoadTextAsset(path);
		
		string[] arary = tx.text.Split(splitStrings, splitOp);
		if (arary.Length>0)
		{
			columnCount = arary.Length;
			
			allArrary = new string[arary.Length][];
			for (int i = 0; i < arary.Length; i++)
			{
				allArrary[i] = arary[i].Split(splitCsv);
			}
			rowCount = allArrary[0].Length;    
		}

		/*StreamReader filereader = new StreamReader(strfullpath,Encoding.Default);
		string strline ="";
		int i=0;
		while(strline!=null)
		{
			strline = filereader.ReadLine();
			if(strline != null && strline.Length >0)
			{
				allArrary[i] = strline.Split(splitCsv);
				rowCount = allArrary[0].Length;    
				//Debug.Log(strline);
			}
			i++;
		}
		filereader.Close();*/
    }

    //获得第几行 第几列的值
    string getValue(int colum, int row)
    {
        if (row < allArrary[colum].Length)
        {
            return allArrary[colum][row];
            
        }
        return null;
    }

    public void getAllValue(CallBackGetValue callback)
    {
        //i=1 忽略第一行 列名
        for (int i = 1; i < columnCount; i++)
        {
            //判断行数是否相同
			if (callback != null && allArrary[i].Length==rowCount)
            {
                callback(allArrary[i], i);
            }
        }
    }
}

