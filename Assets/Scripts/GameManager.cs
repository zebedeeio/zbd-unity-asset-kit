using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    #region Public variables

    [Header("Bitcoin Counter Button")]
    // Reference to the Text component that displays the bitcoins numbers of v1
    public Text BitoinsCount_V1;
    // Reference to the Text component that displays the bitcoins numbers of V2
    public Text BitoinsCount_V2;
    // Reference to the Text component that displays the bitcoins numbers of V3
    public Text BitoinsCount_V3;
    [Header("Cash Out Panels")]
    // Reference to the pabel warning 
    public GameObject Warning_Panel;
    // Reference to the CashOut panel
    public GameObject CashOut_Panel;
    // Reference to the CashOut panel limit reached
    public GameObject CashOut_Limtit_Reached_Panel;
    // Reference to the error panrl component 
    public GameObject ErrorPanel;
    // Reference to the SucessPanel
    public GameObject SuccessPanel;


    [Header("Cashout Bitcoin V2 Panel Elements")]
    // Reference to the Bitcoin amount to convert
    public Text BitcoinAmount;
    // Reference to the Slider
    public Slider Slider;
    // Reference to the Slider Amount
    public Text SlideAmount;
    // Reference to the GamerTag component 
    public InputField GamerTagInputField;
    // Reference to converted sats
    public Text convertedsatsamounts;

    [Header("BitCoin Counter - Cashout - V2 - Elements")]
    // Reference to the Animator component 
    public Animator animator;

    [Header("CashOut-Bitcoin - V2 - Limit reached")]
    // Reference to the gamertag inputfield
    public InputField gamtertaginputfield_limit;
    // Reference to the bitcoint text
    public Text bitcointext_limit;
    // Reference to the converted sats amount
    public Text satsamountconvertedlimit;

    [Header("Liste of Sprites to change Color")]
    // Reference to all images to change their color
    public Image[] ColorofImagesToChange;

    [Header("Earn Bitcoins List of Buttons")]
    // Reference to cashout buttons
    public Button[] CashOutButtons;
    #endregion

    #region private variables
    // The earned bitcoin amount by clicking on the earn bitcoin button
    private int BitcoinEarnedAmount = 0;
    // Color to change
    private Color SelectedColor;
    // Controle Arrow Animation
    private bool isAnimationPlaying = false;
    // The total stats amount
    private int Totalsatsamount = 0;
    #endregion
    public void CollectBitcoins_V1()
    {
        if( BitcoinEarnedAmount < 200)
        {
            // Increase the score by 10
            BitcoinEarnedAmount = BitcoinEarnedAmount + 10;
            // Update the score text
            BitoinsCount_V1.text = BitcoinEarnedAmount.ToString();
        }
        else
        {
            // make all button non interactable
            foreach (Button btn in CashOutButtons)
            {
                btn.interactable = false;
            }
        }

    }

    public void CollectBitcoins_V2()
    {
       if( BitcoinEarnedAmount < 200)
       {
            //start arrow animation
            StartCoroutine(UpdateTextAfterAnimation());
            isAnimationPlaying = true;
       }
       else
       {
            // make all button non interactable
            foreach (Button btn in CashOutButtons)
            {
                btn.interactable = false;
            }
        }
          
        

    }
    IEnumerator UpdateTextAfterAnimation()
    {
        animator.SetTrigger("PlayAnimation"); // replace "PlayAnimation" with the name of your animation trigger
                                             
        BitcoinEarnedAmount = BitcoinEarnedAmount + 10; // Increase the score by 10
        // Update the score text
        BitoinsCount_V2.text = BitcoinEarnedAmount.ToString() + " sats";
        //set animation controller to false
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
       
        isAnimationPlaying = false;
    }
    public void CollectBitcoins_V3()
    {
        if (BitcoinEarnedAmount < 200)
        {
            // Increase the score by 10
            BitcoinEarnedAmount = BitcoinEarnedAmount + 10;
            BitoinsCount_V3.text = BitcoinEarnedAmount.ToString() + " sats";
        }
        else
        {
            // Increase the score by 10
            foreach (Button btn in CashOutButtons)
            {
                btn.interactable = false;
            }
        }


    }
    public void CashOutBitcoin()
    {
       
        // Check if the number of Bitcoin is greater than 0 to open the withdraw panel if not open the warning panel.
        if (BitcoinEarnedAmount <= 0)
        {
            Warning_Panel.SetActive(true);
        }
        else
        {
            if(Totalsatsamount >= 200 )
            {
                gamtertaginputfield_limit.text = GamerTagInputField.text;
                CashOut_Limtit_Reached_Panel.SetActive(true);
                bitcointext_limit.text = BitcoinEarnedAmount.ToString();
                float statsamount = BitcoinEarnedAmount / 10000000f;
                string resultString = statsamount.ToString("F6");
                satsamountconvertedlimit.text = "₿" + resultString;


            }
            else
            {
                
              
                GetBitcoinAmount();
                FillSlider();
                calculatestatsamount();
                CashOut_Panel.SetActive(true);
            }
           
        }
    }
    // get bitcoin amount to withdraw and check if it reached the limit of 200 or no
    public void GetBitcoinAmount()
    {
        if (BitcoinEarnedAmount + Totalsatsamount > 200)
        {
             int rest = 200 - Totalsatsamount;
             BitcoinEarnedAmount = BitcoinEarnedAmount - rest;
           
            Totalsatsamount = Totalsatsamount + rest;
            BitcoinAmount.text = Totalsatsamount.ToString();
            BitoinsCount_V1.text = BitcoinEarnedAmount.ToString();
            BitoinsCount_V2.text = BitcoinEarnedAmount.ToString();
            BitoinsCount_V3.text = BitcoinEarnedAmount.ToString();
            bitcointext_limit.text = BitcoinEarnedAmount.ToString();    
            float statsamount = BitcoinEarnedAmount / 10000000f;
            string resultString = statsamount.ToString("F6");
            satsamountconvertedlimit.text = "₿" + resultString;
        }
        else
        {
            Totalsatsamount = Totalsatsamount + BitcoinEarnedAmount;
            BitcoinAmount.text = Totalsatsamount.ToString();
            BitcoinEarnedAmount = 0;
            BitoinsCount_V1.text = "0";
            BitoinsCount_V2.text = "0 sats";
            BitoinsCount_V3.text = "0 sats";
        }

       
    }
    //convert bitcoint amount to Slider
    public void FillSlider()
    {
        Slider.value = Totalsatsamount;
        SlideAmount.text = Totalsatsamount.ToString() + " / 200  withdrawn";
    }
    //Calculate sats amount
    public void calculatestatsamount()
    {
        int bitcoinamount;
        int.TryParse(BitcoinAmount.text, out bitcoinamount);
        float statsamount = bitcoinamount / 10000000f;
        string resultString = statsamount.ToString("F6");
        convertedsatsamounts.text = "₿" + resultString;



    }

  
    //Open the link for download the zebedee wallet
    public void GetZebedeeWallet()
    {

        Application.OpenURL("https://zebedee.io/app?partner=ZBD&game=Sarutobi&game_name=Sarutobi");
    }
    // change color of all the selected sprites
    public void ChangeColor(Image colorselected)
    {
        
        SelectedColor = colorselected.color;
        ApplyColorToOtherImages(SelectedColor);
        getsatsamount();
    }

    void ApplyColorToOtherImages(Color selectedColor)
    {
        
       
        foreach (Image image in ColorofImagesToChange)
        {
           
                image.color = SelectedColor;
           
        }

        
    }
    
    public void getsatsamount()
    {
        BitoinsCount_V1.text = BitcoinEarnedAmount.ToString();
        BitoinsCount_V2.text = BitcoinEarnedAmount.ToString() + " sats";
        BitoinsCount_V3.text = BitcoinEarnedAmount.ToString() + " sats";
    }
    // check if gamtertag inputfield is empty or no
    public void checkGamerTagFieal()
    {
       if(  string.IsNullOrEmpty(GamerTagInputField.text) )
       {
            ErrorPanel.SetActive(true);
       }
       else
       {
            ErrorPanel.SetActive(false);
            CashOut_Panel.SetActive(false);
            SuccessPanel.SetActive(true);
            foreach (Button btn in CashOutButtons)
            {
                btn.interactable = true;
            }
       }
    }


}
