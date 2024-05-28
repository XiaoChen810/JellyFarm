using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WordData
{
    public WordData(string word,string definition)
    {
        this.word = word;
        this.definition = definition;
    }

    public string word;
    public string definition;
}


public class DatabaseManager : MonoBehaviour
{
    // 文件路径
    private readonly static string filePath = "Assets/Resources/Words/3 四级-乱序.txt";

    // 存储词汇和定义的字典
    public Dictionary<int, WordData> vocabulary;

    void Start()
    {
        // 初始化字典
        vocabulary = new();

        // 调用加载文件方法
        LoadTxtFile(filePath);
    }

    // 加载txt文件并提取词汇和定义
    private void LoadTxtFile(string path)
    {
        int count = 0;

        // 使用StreamReader读取文件
        using (StreamReader reader = new StreamReader(path))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                // 分割行中的单词和定义
                string[] parts = line.Split('\t');

                // 确保分割的部分足够，以避免索引错误
                if (parts.Length >= 2)
                {
                    string word = parts[0];
                    string definition = parts[1];
                    vocabulary.Add(count++, new WordData(word, definition));
                }
            }
        }

    }

    /// <summary>
    /// 打印vocabulary
    /// </summary>
    private void PrintVocabulary()
    {
        foreach (var entry in vocabulary)
        {
            Debug.Log(entry.Key + ": " + entry.Value.word + ": " + entry.Value.definition);
        }
    }

}
