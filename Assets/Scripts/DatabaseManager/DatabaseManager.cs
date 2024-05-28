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
    // �ļ�·��
    private readonly static string filePath = "Assets/Resources/Words/3 �ļ�-����.txt";

    // �洢�ʻ�Ͷ�����ֵ�
    public Dictionary<int, WordData> vocabulary;

    void Start()
    {
        // ��ʼ���ֵ�
        vocabulary = new();

        // ���ü����ļ�����
        LoadTxtFile(filePath);
    }

    // ����txt�ļ�����ȡ�ʻ�Ͷ���
    private void LoadTxtFile(string path)
    {
        int count = 0;

        // ʹ��StreamReader��ȡ�ļ�
        using (StreamReader reader = new StreamReader(path))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                // �ָ����еĵ��ʺͶ���
                string[] parts = line.Split('\t');

                // ȷ���ָ�Ĳ����㹻���Ա�����������
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
    /// ��ӡvocabulary
    /// </summary>
    private void PrintVocabulary()
    {
        foreach (var entry in vocabulary)
        {
            Debug.Log(entry.Key + ": " + entry.Value.word + ": " + entry.Value.definition);
        }
    }

}
