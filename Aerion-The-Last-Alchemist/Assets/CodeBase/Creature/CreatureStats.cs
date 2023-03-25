using UnityEngine;

namespace CodeBase.Creature
{
   public class CreatureStats
   {
      [Range(0,100)]
      private float _balanceAllStats;

      public float BalanceAllStats1 => _balanceAllStats;

      public float GreenStat => greenStat;

      public float RedStat => redStat;

      public float BlueStat => blueStat;

      public float YellowStat => yellowStat;
      public const float MaxStatValue=25;
      [Range(0,MaxStatValue)] private float greenStat;
      [Range(0,MaxStatValue)] private float redStat;
      [Range(0,MaxStatValue)] private float blueStat;
      [Range(0,MaxStatValue)] private float yellowStat;
      public Color BalanceColor;

      public CreatureStats(float balanceAllStats, float greenStat, float redStat, float blueStat, float yellowStat)
      {
         this._balanceAllStats = balanceAllStats;
         this.greenStat = greenStat;
         this.redStat = redStat;
         this.blueStat = blueStat;
         this.yellowStat = yellowStat;
         CalculateBalance();
      }

      public void CalculateBalance()
      {
         _balanceAllStats = GreenStat + RedStat + BlueStat + YellowStat;
         BalanceColor=(Color.green*GreenStat/MaxStatValue+Color.red*RedStat/MaxStatValue+Color.blue*RedStat/MaxStatValue+Color.yellow*RedStat/MaxStatValue)/4;
      }
   }
}
