using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class WordFrequency
{
    public static async Task Main(string[] args)
    {
        string text = await DownloadTextAsync("https://lms-cdn.skillfactory.ru/assets/courseware/v1/dc9cf029ae4d0ae3ab9e490ef767588f/asset-v1:SkillFactory+CDEV+2021+type@asset+block/Text1.txt");

        // Удаляем знаки пунктуации
        var noPunctuationText = new string(text.Where(c => !char.IsPunctuation(c)).ToArray());

        // Разделяем текст на слова и приводим к нижнему регистру для точного подсчета
        string[] words = noPunctuationText.ToLower().Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        // Подсчет частоты встречаемости слов
        var wordFrequencies = words
            .GroupBy(word => word)
            .Select(group => new { Word = group.Key, Count = group.Count() })
            .OrderByDescending(item => item.Count)
            .Take(10);


        // Вывод результатов
        Console.WriteLine("Топ 10 самых часто встречающихся слов:");
        foreach (var item in wordFrequencies)
        {
            Console.WriteLine($"{item.Word}: {item.Count}");
        }
    }

    static async Task<string> DownloadTextAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}