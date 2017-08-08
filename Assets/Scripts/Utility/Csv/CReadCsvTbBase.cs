using System.Collections.Generic;
using System.Reflection;

public class CTbKey<TKEY>
{
    public TKEY key;        //int    作为key
}

public  class CReadCsvTbBase<T1,T2>
    where T2 : CTbKey<T1>, new()  
{
    private Dictionary<T1,T2> m_map;

    public delegate void CallBack(T2 value);

    private List<CMyFiledInfo> ls = null;

    public CReadCsvTbBase(string path)
    {
        m_map = new Dictionary<T1, T2>();
        T2 dstvalue = new T2();
        ls = CTypeBase.getAllFilelds(dstvalue);
        //CReadCsvBase cs = new CReadCsvBase(path, Readcolumn);
        dstvalue = null;
        ls = null;
    }

    public void ForeachValue(CallBack callback)
    {
        foreach (KeyValuePair<T1, T2> keyValuePair in m_map)
        {
            callback(keyValuePair.Value);
        }
    }

    public T2 GetKeyValue(T1 nkey)
    {
        if (m_map.ContainsKey(nkey))
        {
            return m_map[nkey];
        }
        return null;
    }
    
    private  void Readcolumn(string[] srcvalue,int column)
    {
        T2 dstvalue = new T2();
        object objvalue = dstvalue;
        CTypeBase.AutoPushvalue(srcvalue, ref objvalue, ls);
        m_map[dstvalue.key] = dstvalue;
    }
}


public class CReadCsvTbBaseNew<T2>
    where T2 : new()
{
    private List<CMyFiledInfo> ls = null;
    public delegate void CallBack(T2 value,int column);
    private CallBack _callback;

    public CReadCsvTbBaseNew(string path,CallBack callback)
    {
        _callback = callback;
        T2 dstvalue = new T2();
        ls = CTypeBase.getAllFilelds(dstvalue);
        new CReadCsvBase(path, Readcolumn);
        ls = null;
    }

    private void Readcolumn(string[] srcvalue, int column)
    {
        T2 dstvalue = new T2();
        object objvalue = dstvalue;
        CTypeBase.AutoPushvalue(srcvalue, ref objvalue, ls);
        _callback(dstvalue, column);
    }
}
