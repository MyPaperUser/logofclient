using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace Logof_Client;

public class CombineAddresses
{
    private readonly ProgressWindow _progress;

    public CombineAddresses(ProgressWindow progressWindow)
    {
        _progress = progressWindow;
    }

    public async Task<KasAddressList> Perform(List<KasAddressList> address_lists)
    {
        KasAddressList result = new();
        for (var i = 0; i < address_lists.Count; i++)
            if (i == 0)
                result = address_lists[i];
            else
                result = await Merge(result, address_lists[i], i + 1, address_lists.Count);

        return result;
    }

    private async Task<KasAddressList> Merge(KasAddressList first, KasAddressList second, int num, int total)
    {
        KasAddressList result = new();
        foreach (var sec in second.KasPersons)
        {
            var is_new = true;
            foreach (var fi in first.KasPersons)
            {
                if (fi.refsid == sec.refsid)
                {
                    is_new = false;
                    break;
                }

                if (fi.name == sec.name &&
                    fi.anrede == sec.anrede &&
                    fi.anredzus == sec.anredzus &&
                    fi.namezus == sec.namezus &&
                    fi.titel == sec.titel &&
                    fi.adel == sec.adel &&
                    fi.strasse == sec.strasse &&
                    fi.strasse2 == sec.strasse2 &&
                    fi.vorname == sec.vorname &&
                    fi.ort == sec.ort &&
                    fi.land == sec.land &&
                    fi.plz == sec.plz &&
                    fi.pplz == sec.pplz &&
                    fi.funktion == sec.funktion &&
                    fi.funktion2 == sec.funktion2 &&
                    fi.funktionad == sec.funktionad &&
                    fi.abteilung == sec.abteilung &&
                    fi.postfach == sec.postfach &&
                    fi.name1 == sec.name1 &&
                    fi.name2 == sec.name2 &&
                    fi.name3 == sec.name3 &&
                    fi.name4 == sec.name4 &&
                    fi.name5 == sec.name5)
                {
                    is_new = false;
                    break;
                }
            }

            if (is_new) result.KasPersons.Add(sec);
            var subperc = second.KasPersons.IndexOf(sec) / second.KasPersons.Count;
            var percent = (num + (double)subperc) / total * 100;
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (is_new)
                    _progress.AddToLog($"Person mit refsid {sec.refsid} ergänzt");
                else
                    _progress.AddToLog($"Person mit refsid {sec.refsid} bereits vorhanden");

                _progress.ChangePercentage(percent);
            });
        }

        return result;
    }
}