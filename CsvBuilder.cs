using System.Collections.Generic;
using System.Text;

namespace Logof_Client;

public class CsvBuilder
{
    private readonly string Header;
    private readonly List<object> Instances;
    private readonly KasAddressList KasAddressList;

    public CsvBuilder(string header, List<object> instances)
    {
        Header = header;
        Instances = instances;
    }

    public CsvBuilder(string header, KasAddressList instances)
    {
        Header = header;
        KasAddressList = instances;
    }

    public string? BuildKas()
    {
        var result = new StringBuilder();

        result.AppendLine(Header);
        foreach (var l in KasAddressList.KasPersons)

            result.AppendLine(
                l.refsid + "," +
                l.anrede + "," +
                l.titel + "," +
                l.vorname + "," +
                l.adel + "," +
                l.name + "," +
                l.namezus + "," +
                l.anredzus + "," +
                l.strasse + "," +
                l.strasse2 + "," +
                l.plz + "," +
                l.ort + "," +
                l.land + "," +
                l.pplz + "," +
                l.postfach + "," +
                l.name1 + "," +
                l.name2 + "," +
                l.name3 + "," +
                l.name4 + "," +
                l.name5 + "," +
                l.funktion + "," +
                l.funktion2 + "," +
                l.abteilung + "," +
                l.funktionad);

        // weitere Cases
        return result.ToString();
    }
}