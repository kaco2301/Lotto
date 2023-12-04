using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class Lotto : MonoBehaviour
{
    public TextMeshProUGUI[] Nums;
    public TextMeshProUGUI[] AnswerNums;
    public TextMeshProUGUI ResultText;
    private const int NumberCount = 6; 
    private const int MaxNumber = 43;  

    [System.Serializable]
    public class LottoNumbers
    {
        public List<int> Numbers { get; set; }
        public string Rank { get; set; }
    }
    public LottoNumbers AnswerNumbers()
    {
        var numbers = new HashSet<int>();
        var random = new System.Random();

        while (numbers.Count < NumberCount)
        {
            int number = random.Next(1, MaxNumber + 1);
            numbers.Add(number);
        }

        var lottoNumbers = new LottoNumbers { Numbers = new List<int>(numbers) };
        lottoNumbers.Numbers.Sort();  // 정렬
        for (int i = 0; i < NumberCount; i++)
        {
            AnswerNums[i].text = lottoNumbers.Numbers[i].ToString();
        }
        return lottoNumbers;
    }

    public void DrawNumbers()
    {
        AnswerNumbers();
        var numbers = new HashSet<int>();
        var random = new System.Random();

        while (numbers.Count < NumberCount)
        {
            int number = random.Next(1, MaxNumber + 1);
            numbers.Add(number);
        }

        var lottoNumbers = new LottoNumbers { Numbers = new List<int>(numbers) };
        lottoNumbers.Numbers.Sort();

        // Nums 배열에 할당
        for (int i = 0; i < NumberCount; i++)
        {
            Nums[i].text = lottoNumbers.Numbers[i].ToString();
        }

        var result = CheckResult();

        lottoNumbers.Rank = result;
        string json = JsonConvert.SerializeObject(lottoNumbers);

        string path = Path.Combine(Application.persistentDataPath, "lottoNumbers.json");
        File.WriteAllText(path, json);

        Debug.Log("Lotto numbers saved to: " + path);
    }

    private string CheckResult()
    {
        int matchCount = 0;
        foreach (var answerNum in AnswerNums)
        {
            foreach (var num in Nums)
            {
                if (answerNum.text == num.text)
                {
                    matchCount++;
                }
            }
        }

        string result;

        switch (matchCount)
        {
            case 6:
                result = "Jackpot";
                ResultText.color = Color.red;
                break;
            case 5:
                result = "2nd Prize";
                ResultText.color = Color.yellow;
                break;
            case 4:
                result = "3rd Prize";
                ResultText.color = Color.blue;
                break;
            case 3:
                result = "4th Prize";
                ResultText.color = Color.green;
                break;
            default:
                result = "Not a Winner";
                ResultText.color = Color.white;
                break;
        }

        ResultText.text = result;

        return result;
    }
}
