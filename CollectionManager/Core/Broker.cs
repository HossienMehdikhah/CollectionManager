using System.Diagnostics;

namespace CollectionManager.Core
{
    public class Broker
    {
        public static bool AddToIDMDownLoadList(IEnumerable<Uri> uris)
        {
            foreach (var uri in uris)
            {
                try
                {
                    var temp = Process.Start(@"C:\Program Files (x86)\Internet Download Manager\IDMan.exe", $"/n /a /d {uri}");
                    temp.WaitForExit();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
