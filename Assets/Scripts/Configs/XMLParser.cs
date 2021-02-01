using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Reflection;
using System;
/// <summary>
/// 专门解析XML的类
/// </summary>
public class XMLParser
{
    /// <summary>
    /// 1.根据传的xml格式文件的名称,加载读取解析
    /// 2.将xml值赋值给 T类型的类模板实例上；
    /// 3.以读取到的ID为key将T实例存到字典里面 返回出去
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xmlName"></param>
    /// <returns></returns>
    public static Dictionary<int, T> ParserXML<T>(string xmlName) {
        Dictionary<int, T> dic = new Dictionary<int, T>();
        TextAsset textAsset = Resources.Load<TextAsset>("Configs/XML/" + xmlName);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);//string加载给xml类型
        XmlNodeList nodeList = doc.SelectNodes("Nodes/Node");//获取所有Node节点集合
        for (int i = 0; i < nodeList.Count; i++) {
            //获取节点列表的一个子节点
            XmlNode node = nodeList[i];
            XmlElement elem = (XmlElement)node;

            T obj = GreateAndSetValue<T>(elem);
            FieldInfo fieldInfo = obj.GetType().GetField("ID");
            int id = int.Parse(fieldInfo.GetValue(obj).ToString());

            if (!dic.ContainsKey(id))
                dic.Add(id, obj);
        }
        return dic;
    }
    //greateT并且赋好值
    protected static T GreateAndSetValue<T>(XmlElement node) {
        //通过类型创建一个对象实例
        T obj = Activator.CreateInstance<T>();
        FieldInfo[] fields = typeof(T).GetFields();//获取一个类的所有字段
        for (int i = 0; i < fields.Length; i++) {
            string name = fields[i].Name;
            if (string.IsNullOrEmpty(name)) continue;
            string fieldValue = node.GetAttribute(name);
            if (string.IsNullOrEmpty(fieldValue)) continue;
            try {
                ParsePropertyValue<T>(obj, fields[i], fieldValue);//尝试给类字段赋值
            }
            catch (Exception)//出错的话 报错
            {
                /*Console.WriteLine*/
                Debug.LogErrorFormat(string.Format("XML读取错误：对象类型({2}) => 属性名({0}) => 属性类型({3}) => 属性值({1})",
fields[i].Name, fieldValue, typeof(T).ToString(), fields[i].FieldType.ToString()));
            }
        }
        return obj;
    }

    //给这个类具体赋值
    private static void ParsePropertyValue<T>(T obj, FieldInfo fieldInfo, string valueStr) {
        //System.Object value = valueStr;
        object value = valueStr;//将xml值转给一个Object类型
        if (fieldInfo.FieldType.IsEnum) {
            value = Enum.Parse(fieldInfo.FieldType, valueStr);
        }
        else {
            if (fieldInfo.FieldType == typeof(int))
                value = int.Parse(valueStr);
            else if (fieldInfo.FieldType == typeof(byte))
                value = byte.Parse(valueStr);
            else if (fieldInfo.FieldType == typeof(bool))
                value = bool.Parse(valueStr);
            else if (fieldInfo.FieldType == typeof(double))
                value = double.Parse(valueStr);
            else if (fieldInfo.FieldType == typeof(float))
                value = float.Parse(valueStr);
            else if (fieldInfo.FieldType == typeof(uint))
                value = uint.Parse(valueStr);
            else if (fieldInfo.FieldType == typeof(ulong))
                value = ulong.Parse(valueStr);
            else if (fieldInfo.FieldType == typeof(long))
                value = long.Parse(valueStr);
            //else if (fieldInfo.FieldType == typeof(string))//string默认可以自动转
            //    value = valueStr;
        }

        if (value == null)
            return;

        fieldInfo.SetValue(obj, value);//给类字段赋值
    }
}
