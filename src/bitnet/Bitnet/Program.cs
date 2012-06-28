// COPYRIGHT 2011 Konstantin Ineshin, Irkutsk, Russia.
// If you like this project please donate BTC 18TdCC4TwGN7PHyuRAm8XV88gcCmAHqGNs

using System;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;

using Bitnet.Client;

namespace Bitnet
{
  internal sealed class Program
  {
    [STAThread]
    static void Main()
    {
      BitnetClient bc = new BitnetClient("http://127.0.0.1:8332");
      bc.Credentials = new NetworkCredential("user", "pass");
      var p = bc.GetDifficulty();
      Console.WriteLine(p);
    }
  }
}