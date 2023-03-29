using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Reference to the Text component that displays the bitcoins numbers
    public Text BitoinsCount;
    // The current count
    private int count = 0;
    // Reference to the pabel warning 
    public GameObject Warning_Panel;
    // Reference to the CashOut panel
    public GameObject CashOut_Panel;
    // Reference to the Bitcoin amount
    public Text BitcoinAmount;
    // Reference to the Slider
    public Slider Slider;
    // Reference to the Slider Amount
    public Text SlideAmount;

    public Text BitcoinConvertAmount;

    // one satoshi is 0.00000001 BTC
   public float satoshi = 0.00000001f;
    public void CollectBitcoins()
    {
        // Increase the score by 10
        count = count + 10 ;
        // Update the score text
        BitoinsCount.text = count.ToString();
    }

    public void CashOutBitcoin()
    {
        // Check if the number of Bitcoin is greater than 0 to open the withdraw panel or the warning panel.
        if ( count <= 0 )
        {
            Warning_Panel.SetActive(true);
        }
        else
        {
            CashOut_Panel.SetActive(true);
            GetBitcoinAmount();

            FillSlider();
        }
    }
    // get bitcoin amount to withdraw
    public void GetBitcoinAmount()
    {

        BitcoinAmount.text = count.ToString();
    }

    // Clear bitcoins amount  and slider value after withraw succefully
    public void ClearAmount()
    {
        count = 0;
        BitoinsCount.text = "0";
        Slider.value = 0;
        BitcoinConvertAmount.text = "0.000003";
    }
    //convert bitcoint amount to Slider
    public void FillSlider()
    {
        Slider.value = count;
        SlideAmount.text = count.ToString() +  " / 200  withraw"; 
    }
    // Convert Amount
    public void ConvertAmount()
    {
    
        float one_usd = count / satoshi;
        BitcoinConvertAmount.text = one_usd.ToString(); 

    }
    //Open the link for download the zebedee wallet
    public void GetZebedeeWallet()
    {
        
        Application.OpenURL("https://zebedee.io/app?partner=ZBD&game=Sarutobi&game_name=Sarutobi");
    }
}
