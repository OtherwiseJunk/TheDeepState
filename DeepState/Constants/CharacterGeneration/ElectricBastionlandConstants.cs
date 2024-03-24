using System;
using System.IO;
using System.Text.Json;
using DeepState.Models.RPGSystemModels.ElectricBastionland;

namespace DeepState.Constants.CharacterGeneration;
public class ElectricBastionlandConstants
{
    private static FailedCareer[] _failedCareers { get; set; }
    public static FailedCareer[] FailedCareers
    {
        get
        {
            if (_failedCareers == null)
            {
                using (StreamReader reader = new StreamReader("FailedCareers.json"))
                {
                    Console.WriteLine("Attempting to read in the failed careers json");
                    string failedCareerJson = reader.ReadToEnd();
                    Console.WriteLine($"Reader Output: {failedCareerJson}");
                    _failedCareers = JsonSerializer.Deserialize<FailedCareer[]>(failedCareerJson);
                }
            }

            return _failedCareers;
        }
    }
}
