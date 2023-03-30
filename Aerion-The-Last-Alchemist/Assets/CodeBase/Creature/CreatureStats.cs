using System;
using CodeBase.Enums;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Creature
{
    public class CreatureStats : EventArgs
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
        [Range(0, 100)] private float _balanceAllStats;

        public float BalanceAllStats1 => _balanceAllStats;

        public float GreenStat => greenStat;

        public float RedStat => redStat;

        public float BlueStat => blueStat;

        public float YellowStat => yellowStat;
        public const int MaxStatValue = 25;
        [Range(0, MaxStatValue)] private float greenStat;
        [Range(0, MaxStatValue)] private float redStat;
        [Range(0, MaxStatValue)] private float blueStat;
        [Range(0, MaxStatValue)] private float yellowStat;
        public Color BalanceColor;

        public CreatureStats(int greenStat, int redStat, int blueStat, int yellowStat, string id,
            CreatureTypeId creatureTypeId)
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
            BalanceColor = (Color.green * GreenStat / MaxStatValue + Color.red * RedStat / MaxStatValue +
                            Color.blue * RedStat / MaxStatValue + Color.yellow * RedStat / MaxStatValue) / 4;
            BalanceColor = new Color((BalanceColor.r*2>1?1:BalanceColor.r*2)*0.5882353f, (BalanceColor.g*2>1?1:BalanceColor.g*2)*0.0627451f, (BalanceColor.b*4>1?1:BalanceColor.b*4)*0.5333334f, 1);
        }

        public void Hill(PotionType potionType, int effect)
        {
            switch (potionType)
            {
                case PotionType.Blue:
                    blueStat = blueStat + effect > 25 ? 25 : blueStat + effect;
                    break;
                case PotionType.Green:
                    greenStat = greenStat + effect > 25 ? 25 : greenStat + effect;
                    break;
                case PotionType.Red:
                    redStat = redStat + effect > 25 ? 25 : redStat + effect;
                    break;
                case PotionType.Yellow:
                    yellowStat = yellowStat + effect > 25 ? 25 : yellowStat + effect;
                    break;
            }
        }
    }
}