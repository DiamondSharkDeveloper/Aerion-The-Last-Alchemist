using System;
using System.Collections;
using CodeBase.Effects;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Creature
{
    public class CreatureBalanceBar : MonoBehaviour
    {
        [SerializeField] private Liquid balanceBar;
        [SerializeField] private Material balanceBarColor;

        [SerializeField] private Image redBarImage;
        [SerializeField] private Image blueBarImage;
        [SerializeField] private Image yellowBarImage;
        [SerializeField] private Image greenBarImage;

        public void SetBarImage(CreatureStats creatureStats)
        {
           UpdateBar(redBarImage,creatureStats.RedStat/CreatureStats.MaxStatValue);
           UpdateBar(greenBarImage,creatureStats.GreenStat/CreatureStats.MaxStatValue);
           UpdateBar(blueBarImage,creatureStats.BlueStat/CreatureStats.MaxStatValue);
           UpdateBar(yellowBarImage,creatureStats.YellowStat/CreatureStats.MaxStatValue);
          
            // if (balanceBarColor.color != creatureStats.BalanceColor)
            // {
              //  StartCoroutine(SmoothColourChange(balanceBarColor.GetColor("Foam Line Color"), creatureStats.BalanceColor));
                StartCoroutine(SmoothBalanceBarFillAmountChange(creatureStats.BalanceAllStats1/100));
           // }
        }

        private void UpdateBar(Image image,float amount)
        {
            if (image.fillAmount!=amount)
            {
                StartCoroutine(SmoothFillAmountChange(image, amount));
            }
        }
        
        private IEnumerator SmoothColourChange(Color startColor, Color targetColour)
        {
            for (float i = 0; i < 1; i += Time.deltaTime / 4)
            {
                balanceBarColor.SetColor(("Foam Line Color"),Color.Lerp(startColor, targetColour, i));
                yield return null;
            }
        }
      

        private IEnumerator SmoothFillAmountChange(Image image,float amount)
        {
            for (float i = 0; i < 1; i += Time.deltaTime / 4)
            {
                image.fillAmount += (amount - image.fillAmount) * i;
                if (image.fillAmount>=amount)
                {
                    image.fillAmount = amount;
                    yield break;
                }
                yield return null;
            }
        }  
        private IEnumerator SmoothBalanceBarFillAmountChange(float amount)
        {
            for (float i = 0; i < 1; i += Time.deltaTime / 4)
            {
                
               balanceBar.SetAmount(amount -balanceBar.GetAmount() * i);
                if (balanceBar.GetAmount()>=amount)
                {
                    balanceBar.SetAmount(amount);
                    yield break;
                }
                yield return null;
            }
        }

        private void OnDestroy()
        {
            balanceBarColor.color = Color.white;
        }
    }
}