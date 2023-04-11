using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{





    [Header("Theme One")]
    // Reference to the Text component that displays the bitcoins numbers
    public Text BitoinsCount_V1;
    // Reference to the Text component that displays the bitcoins numbers of V2
    public Text BitoinsCount_V2;
    // Reference to the Text component that displays the bitcoins numbers of V3
    public Text BitoinsCount_V3;
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
    // Reference to the Animator component 
    public Animator animator;
    // Reference to the GamerTag component 
    public InputField GamerTag;
    // Reference to the error text component 
    public GameObject ErrorText ;
    // Reference to the GamerTag component 
    public GameObject SuccessPanel;
    // Reference to the GamerTag component 
    public GameObject WithdrawPanel;



    [Header("Images to change Color")]
    // Reference to all images to change their color
    public Image[] ColorofImagesToChange;



    // Color to change
    private Color SelectedColor;
    // Controle Arrow Animation
    private bool isAnimationPlaying = false;
    // The current count
    private int count = 0;


    public void CollectBitcoins_V1()
    {
        // Increase the score by 10
        count = count + 10;
        // Update the score text
        BitoinsCount_V1.text = count.ToString();
    }

    public void CollectBitcoins_V2()
    {
        //start arrow animation
        StartCoroutine(UpdateTextAfterAnimation());
        isAnimationPlaying = true;
    }
    IEnumerator UpdateTextAfterAnimation()
    {
        animator.SetTrigger("PlayAnimation"); // replace "PlayAnimation" with the name of your animation trigger
                                              // Increase the score by 10
        count = count + 10;
        // Update the score text
        BitoinsCount_V2.text = count.ToString() + " sats";
        //set animation controller to false
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
       
        isAnimationPlaying = false;
    }
    public void CollectBitcoins_V3()
    {
        // Increase the score by 10
        count = count + 10;
        BitoinsCount_V3.text = count.ToString() + " sats";
    }
    public void CashOutBitcoin()
    {
        // Check if the number of Bitcoin is greater than 0 to open the withdraw panel or the warning panel.
        if (count <= 0)
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
        BitoinsCount_V1.text = "0";
        BitoinsCount_V2.text = "0 sats";
        BitoinsCount_V3.text = "0 sats";
        Slider.value = 0;
        GamerTag.text = "";
    }
    //convert bitcoint amount to Slider
    public void FillSlider()
    {
        Slider.value = count;
        SlideAmount.text = count.ToString() + " / 200  withraw";
    }
    //Open the link for download the zebedee wallet
    public void GetZebedeeWallet()
    {

        Application.OpenURL("https://zebedee.io/app?partner=ZBD&game=Sarutobi&game_name=Sarutobi");
    }
    public void ChangeColor(Image colorselected)
    {
        
        SelectedColor = colorselected.color;
        ApplyColorToOtherImages(colorselected);
    }

    void ApplyColorToOtherImages(Image selectedColor)
    {
        
       
        foreach (Image image in ColorofImagesToChange)
        {
            if (image != selectedColor)
            {
                image.color = SelectedColor;
            }
        }
    }


    public void checkGamerTagFieal()
    {
       if(  string.IsNullOrEmpty(GamerTag.text) )
       {
            ErrorText.SetActive(true);
       }
       else
       {
            ErrorText.SetActive(false);
            WithdrawPanel.SetActive(false);
            SuccessPanel.SetActive(true);
            ClearAmount();
        }
    }


}
