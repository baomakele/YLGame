using System;
using UnityEngine;

//testDataGenerate类
//该类自动生成请勿修改，以避免不必要的损失
public class testDataGenerate : DataGenerateBase 
{
	public string m_key;
	public string m_a; //abc

	public override void LoadData(string key) 
	{
		DataTable table =  DataManager.GetData("testData");

		if (!table.ContainsKey(key))
		{
			throw new Exception("testDataGenerate LoadData Exception Not Fond key ->" + key + "<-");
		}

		SingleData data = table[key];

		m_key = key;
		m_a = data.GetString("a");
	}
	 public override void LoadData(DataTable table,string key) 
	{
		SingleData data = table[key];

		m_key = key;
		m_a = data.GetString("a");
	}
}
