using System;
using System.Collections.Generic;

namespace Logof_Client;

public class KasAddressList
{
    public List<KasPerson> KasPersons;

    public KasAddressList()
    {
        KasPersons = new List<KasPerson>();
    }
}

public class KasPerson
{
    public KasPerson()
    {
        refsid = 0;
        anrede = "";
        titel = "";
        vorname = "";
        adel = "";
        name = "";
        namezus = "";
        anredzus = "";
        strasse = "";
        strasse2 = "";
        plz = 0;
        ort = "";
        land = "";
        pplz = "";
        postfach = "";
        name1 = "";
        name2 = "";
        name3 = "";
        name4 = "";
        name5 = "";
        funktion = "";
        funktion2 = "";
        abteilung = "";
        funktionad = "";
    }

    public KasPerson(int refsid,
        string anrede,
        string titel,
        string vorname,
        string adel,
        string name,
        string namezus,
        string anredzus,
        string strasse,
        string strasse2,
        int plz,
        string ort,
        string land,
        string pplz,
        string postfach,
        string name1,
        string name2,
        string name3,
        string name4,
        string name5,
        string funktion,
        string funktion2,
        string abteilung,
        string funktionad)
    {
        this.refsid = refsid;
        this.anrede = anrede;
        this.titel = titel;
        this.vorname = vorname;
        this.adel = adel;
        this.name = name;
        this.namezus = namezus;
        this.anredzus = anredzus;
        this.strasse = strasse;
        this.strasse2 = strasse2;
        this.plz = plz;
        this.ort = ort;
        this.land = land;
        this.pplz = pplz;
        this.postfach = postfach;
        this.name1 = name1;
        this.name2 = name2;
        this.name3 = name3;
        this.name4 = name4;
        this.name5 = name5;
        this.funktion = funktion;
        this.funktion2 = funktion2;
        this.abteilung = abteilung;
        this.funktionad = funktionad;
    }

    public int refsid { get; set; }
    public string anrede { get; set; }
    public string titel { get; set; }
    public string vorname { get; set; }
    public string adel { get; set; }
    public string name { get; set; }
    public string namezus { get; set; }
    public string anredzus { get; set; }
    public string strasse { get; set; }
    public string strasse2 { get; set; }
    public int plz { get; set; }
    public string ort { get; set; }
    public string land { get; set; }
    public string pplz { get; set; }
    public string postfach { get; set; }
    public string name1 { get; set; }
    public string name2 { get; set; }
    public string name3 { get; set; }
    public string name4 { get; set; }
    public string name5 { get; set; }
    public string funktion { get; set; }
    public string funktion2 { get; set; }
    public string abteilung { get; set; }
    public string funktionad { get; set; }
}

public class KasPersonError
{
    public KasPersonError((int, List<AddressCheck.ErrorTypes>, List<AddressCheck.WarningTypes>) single_result)
    {
        refsid = single_result.Item1;
        try
        {
            foreach (var err in single_result.Item2) errors += err + ", ";
            errors = errors.Trim();
            errors = errors.TrimEnd(',');
        }
        catch
        {
        }

        try
        {
            if (single_result.Item3 != null)
            {
                foreach (var err in single_result.Item3) warnings += err + ", ";
                warnings = warnings.Trim();
                warnings = warnings.TrimEnd(',');
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public int refsid { get; set; }
    public string errors { get; set; } = "";
    public string warnings { get; set; } = "";
}