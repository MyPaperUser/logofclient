using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace Logof_Client;

public class AddressCheck
{
    public enum ErrorTypes
    {
        PlzTooShort,
        PlzTooLong,
        NoCity,
        NoStreet,
        NoLastName,
        NoFirstName,

        // empty,
        FullAddressTooLong,
        NoStreetNumber,
        DoubledRefsid,
        MayBeSameAddress
    }

    private readonly ProgressWindow _progress;

    public AddressCheck(ProgressWindow progressWindow)
    {
        _progress = progressWindow;
    }

    public async Task<List<(int, List<ErrorTypes>)>> Perform(KasAddressList addresses)
    {
        var failed_refsids = new List<(int, List<ErrorTypes>)>();
        var total = addresses.KasPersons.Count;
        var current = 0;

        await Task.Run(async () =>
        {
            foreach (var person in addresses.KasPersons)
            {
                var errors = new List<ErrorTypes>();
                var hasFaults = false;

                var address_component_count = 2; // cause anrede and name are first

                // Prüfung
                if (person.plz < 10000)
                {
                    hasFaults = true;
                    errors.Add(ErrorTypes.PlzTooShort);
                }
                else if (person.plz > 99999)
                {
                    hasFaults = true;
                    errors.Add(ErrorTypes.PlzTooLong);
                }

                if (string.IsNullOrWhiteSpace(person.ort))
                {
                    hasFaults = true;
                    errors.Add(ErrorTypes.NoCity);
                }
                else
                {
                    address_component_count++;
                }

                var street = person.strasse.ToCharArray();
                var intcount = 0;
                foreach (var c in street)
                {
                    int maybe;
                    if (int.TryParse(c.ToString(), out maybe)) intcount++;
                }

                if (intcount == 0)
                {
                    hasFaults = true;
                    errors.Add(ErrorTypes.NoStreetNumber);
                }


                if (string.IsNullOrWhiteSpace(person.name))
                {
                    hasFaults = true;
                    errors.Add(ErrorTypes.NoLastName);
                }

                if (string.IsNullOrWhiteSpace(person.vorname))
                {
                    hasFaults = true;
                    errors.Add(ErrorTypes.NoFirstName);
                }

                if (string.IsNullOrWhiteSpace(person.strasse))
                {
                    hasFaults = true;
                    errors.Add(ErrorTypes.NoStreet);
                }
                else
                {
                    address_component_count++;
                }

                if (!string.IsNullOrWhiteSpace(person.strasse2)) address_component_count++;
                if (!string.IsNullOrWhiteSpace(person.land)) address_component_count++;
                if (!string.IsNullOrWhiteSpace(person.name1)) address_component_count++;
                if (!string.IsNullOrWhiteSpace(person.name2)) address_component_count++;
                if (!string.IsNullOrWhiteSpace(person.name3)) address_component_count++;
                if (!string.IsNullOrWhiteSpace(person.name4)) address_component_count++;
                if (!string.IsNullOrWhiteSpace(person.name5)) address_component_count++;
                if (!string.IsNullOrWhiteSpace(person.funktion)) address_component_count++;
                if (!string.IsNullOrWhiteSpace(person.funktion2)) address_component_count++;
                if (!string.IsNullOrWhiteSpace(person.funktionad)) address_component_count++;
                if (!string.IsNullOrWhiteSpace(person.abteilung)) address_component_count++;


                foreach (var person2 in addresses.KasPersons)
                {
                    if (addresses.KasPersons.IndexOf(person) == addresses.KasPersons.IndexOf(person2)) continue;

                    if (person.refsid == person2.refsid)
                    {
                        hasFaults = true;
                        errors.Add(ErrorTypes.DoubledRefsid);
                    }

                    if (person.name == person2.name &&
                        person.strasse == person2.strasse &&
                        person.vorname == person2.vorname &&
                        person.ort == person2.ort &&
                        person.funktion == person2.funktion &&
                        person.funktion2 == person2.funktion2 &&
                        person.funktionad == person2.funktionad &&
                        person.abteilung == person2.abteilung &&
                        person.name1 == person2.name1 &&
                        person.name2 == person2.name2 &&
                        person.name3 == person2.name3 &&
                        person.name4 == person2.name4 &&
                        person.name5 == person2.name5)

                    {
                        hasFaults = true;
                        errors.Add(ErrorTypes.MayBeSameAddress);
                    }
                }

                if (address_component_count > 10)
                {
                    hasFaults = true;
                    errors.Add(ErrorTypes.FullAddressTooLong);
                }

                if (hasFaults)
                    lock (failed_refsids)
                    {
                        failed_refsids.Add((person.refsid, errors));
                    }

                // Fortschritt aktualisieren
                Interlocked.Increment(ref current);
                var percent = current / (double)total * 100;
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    if (hasFaults)
                        _progress.AddToLog($"Person mit refsid {person.refsid} ist fehlerhaft");

                    _progress.ChangePercentage(percent);
                });
            }
        });
        return failed_refsids;
    }
}