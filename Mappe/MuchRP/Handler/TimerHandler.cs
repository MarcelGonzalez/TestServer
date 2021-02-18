using AltV.Net;
using MuchRP.Factories;
using MuchRP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace MuchRP.Handler
{
    class TimerHandler : IScript
    {
        internal static void PlayerTimer(object sender, ElapsedEventArgs e)
        {
			try
			{
				foreach(MuchPlayer player in Alt.GetAllPlayers().Where(x => x != null && x.Exists && ((MuchPlayer)x).accountId > 0))
				{
					Database.PlayerDatabase.UpdateAccountLastPosition(player.accountId, player.Position, player.Dimension);
					Console.WriteLine($"Position für {player.accountName} gespeichert.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"{ex}");
			}
        }

        internal static void LaborTimer(Object unused)
		{
			try
			{
				if (DateTime.Now.Minute != 0 && DateTime.Now.Minute != 15 && DateTime.Now.Minute != 30 && DateTime.Now.Minute != 45) return;
				foreach(var faction in ServerFactions.ServerFactions_.ToList())
				{
					if (faction.type != 2 && faction.type != 3) continue;
					foreach(var factionMember in ServerFactions.ServerFactionMembers_.ToList().Where(x => x.factionId == faction.id))
					{
						if (faction.type == 2)
						{
							if (ServerFactions.GetLaborItemAmount(factionMember.factionId, factionMember.accountId, "Batteriezellen") < 10 || ServerFactions.GetLaborItemAmount(factionMember.factionId, factionMember.accountId, "Hanfsamenpulver") < 10 || ServerFactions.GetLaborItemAmount(factionMember.factionId, factionMember.accountId, "Dünger") < 10) continue;
							ServerFactions.RemoveLaborItemAmount(factionMember.factionId, factionMember.accountId, "Batteriezellen", 10);
							ServerFactions.RemoveLaborItemAmount(factionMember.factionId, factionMember.accountId, "Hanfsamenpulver", 10);
							ServerFactions.RemoveLaborItemAmount(factionMember.factionId, factionMember.accountId, "Dünger", 10);
							ServerFactions.AddLaborItem(factionMember.factionId, factionMember.accountId, "Cannabiskiste", 1);
						}
						else if (faction.type == 3)
						{
							if (ServerFactions.GetLaborItemAmount(factionMember.factionId, factionMember.accountId, "Batteriezellen") < 10 || ServerFactions.GetLaborItemAmount(factionMember.factionId, factionMember.accountId, "Ephedrinpulver") < 10 || ServerFactions.GetLaborItemAmount(factionMember.factionId, factionMember.accountId, "Toilettenreiniger") < 10) continue;
							ServerFactions.RemoveLaborItemAmount(factionMember.factionId, factionMember.accountId, "Batteriezellen", 10);
							ServerFactions.RemoveLaborItemAmount(factionMember.factionId, factionMember.accountId, "Ephedrinpulver", 10);
							ServerFactions.RemoveLaborItemAmount(factionMember.factionId, factionMember.accountId, "Toilettenreiniger", 10);
							ServerFactions.AddLaborItem(factionMember.factionId, factionMember.accountId, "Methkiste", 1);
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
        }
    }
}
