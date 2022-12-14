using Muscurdi.Models;
using Bogus;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
namespace Muscurdi.Libs;
public static class MasterPasswordHelper
{
    const string WORDLIST_PATH = "config/wordlist.json";
    public static MasterPassword Generate()
    {
        // for some reason this wont work on published bin
        // string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        // string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
        // Console.WriteLine($"{strExeFilePath} , {strWorkPath}");

        var strWorkPath = "./";
        string wordsListFilePath = System.IO.Path.Combine(strWorkPath, WORDLIST_PATH);

        var faker = new Faker();
        var words = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(wordsListFilePath)) ?? new();
        var initialWords = faker.PickRandom<string>(items: words, amountToPick: 3);
        var finalWord = faker.PickRandom<string>(items: words);
        var number = faker.Random.Int(1000, 9999);

        return MasterPassword.Make(new(initialWords), finalWord, number);
    }
}
