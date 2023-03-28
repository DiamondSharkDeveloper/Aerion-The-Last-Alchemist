using CodeBase.Services.Randomizer;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Creature
{
   public class CreatureStats
   {
      private string creatureId;

      public string CreatureId
      {
         get => creatureId;
         set => creatureId = value;
      }

      public CreatureTypeId TypeId
      {
         get => typeId;
         set => typeId = value;
      }

     private CreatureTypeId typeId;
      [Range(0,100)]
      private float _balanceAllStats;

      public float BalanceAllStats1 => _balanceAllStats;

      public float GreenStat => greenStat;

      public float RedStat => redStat;

      public float BlueStat => blueStat;

      public float YellowStat => yellowStat;
      public const int MaxStatValue=25;
      [Range(0,MaxStatValue)] private float greenStat;
      [Range(0,MaxStatValue)] private float redStat;
      [Range(0,MaxStatValue)] private float blueStat;
      [Range(0,MaxStatValue)] private float yellowStat;
      public Color BalanceColor;

      public CreatureStats(int greenStat, int redStat, int blueStat, int yellowStat,string id,CreatureTypeId creatureTypeId)
      {
         this.greenStat = greenStat;
         this.redStat = redStat;
         this.blueStat = blueStat;
         this.yellowStat = yellowStat;
         typeId = creatureTypeId;
         creatureId = id;
         CalculateBalance();
      }

      public void CalculateBalance()
      {
         _balanceAllStats = GreenStat + RedStat + BlueStat + YellowStat;
         BalanceColor=(Color.green*GreenStat/MaxStatValue+Color.red*RedStat/MaxStatValue+Color.blue*RedStat/MaxStatValue+Color.yellow*RedStat/MaxStatValue)/4;
         BalanceColor = new Color(BalanceColor.a,BalanceColor.b,BalanceColor.g,1);
      }
      
   }
}
