using System.Collections.Generic;

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
        NoFirstName
    }

    public static List<(int, List<ErrorTypes>)> Perform(KasAddressList addresses)
    {
        var failed_refsids = new List<(int, List<ErrorTypes>)>();

        foreach (var person in addresses.KasPersons)
        {
            var errors = new List<ErrorTypes>();
            var hasFaults = false;


            if (person.plz < 10000)
            {
                hasFaults = true;
                errors.Add(ErrorTypes.PlzTooShort);
            }

            if (person.plz > 99999)
            {
                hasFaults = true;
                errors.Add(ErrorTypes.PlzTooLong);
            }

            if (person.ort == null || person.ort == "")
            {
                hasFaults = true;
                errors.Add(ErrorTypes.NoCity);
            }

            if (person.name == null || person.name == "")
            {
                hasFaults = true;
                errors.Add(ErrorTypes.NoLastName);
            }

            if (person.vorname == null || person.vorname == "")
            {
                hasFaults = true;
                errors.Add(ErrorTypes.NoFirstName);
            }

            if (person.strasse == null || person.strasse == "")
            {
                hasFaults = true;
                errors.Add(ErrorTypes.NoStreet);
            }

            ;
            // More errors...


            if (hasFaults) failed_refsids.Add((person.refsid, errors));
        }

        return failed_refsids;
    }
}