using System;
using System.IO;

namespace Logof_Client;

public class DataImport
{
    public static (bool, KasAddressList) ImportKasAddressList(Uri pathToCsv, char separator = ',')
    {
        if (!File.Exists(pathToCsv.LocalPath))
        {
            Console.WriteLine($"File not found: {pathToCsv}");
            return (false, null);
        }

        using var reader = new StreamReader(pathToCsv.LocalPath);
        var headerLine = reader.ReadLine();
        if (headerLine == null)
        {
            Console.WriteLine("File is empty.");
            return (false, null);
        }

        var imported = new KasAddressList();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split(separator);

            if (parts.Length < 24)
            {
                Console.WriteLine($"No enough columns in line: {line}");
                continue;
            }

            try
            {
                var person = new KasPerson(
                    ParseInt(parts[0]),
                    parts[1],
                    parts[2],
                    parts[3],
                    parts[4],
                    parts[5],
                    parts[6],
                    parts[7],
                    parts[8],
                    parts[9],
                    ParseInt(parts[10]),
                    parts[11],
                    parts[12],
                    parts[13],
                    parts[14],
                    parts[15],
                    parts[16],
                    parts[17],
                    parts[18],
                    parts[19],
                    parts[20],
                    parts[21],
                    parts[22],
                    parts[23]
                );
                imported.KasPersons.Add(person);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while parsing line: {line} - {ex.Message}");
                Console.WriteLine($"{ex.StackTrace}");
                return (false, null);
            }
        }

        return (true, imported);
    }

    private static int ParseInt(string input)
    {
        return int.TryParse(input, out var result) ? result : 0;
    }
}