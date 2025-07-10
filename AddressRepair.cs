using System.Collections.Generic;

namespace Logof_Client;

public class AddressRepair(ProgressWindow progressWindow)
{
    private readonly ProgressWindow _progress = progressWindow;

    public KasAddressList Perform(KasAddressList all_addresses,
        List<(int, List<AddressCheck.ErrorTypes>)> failed_addresses)
    {
        foreach (var k in all_addresses.KasPersons)
        foreach (var p in failed_addresses)
        {
            if (k.refsid != p.Item1) continue;

            if (p.Item2.Contains(AddressCheck.ErrorTypes.DoubledRefsid))
            {
            }
        }

        return null;
    }
}