using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
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
    public InputField EmailInputField;
    // Reference to converted sats
    public Text convertedsatsamounts;
    // Reference to converted dollars
    public Text Converteddollarsamounts;
    [Header("BitCoin Counter - Cashout - V2 - Elements")]
    // Reference to the Animator component 
    public Animator animator;

    [Header("CashOut-Bitcoin - V2 - Limit reached")]
    // Reference to the gamertag inputfield
    public InputField Emailinputfield_limit;
    // Reference to the bitcoint text
    public Text bitcointext_limit;
    // Reference to the converted sats amount
    public Text satsamountconvertedlimit;
    // Reference to the converted bitcount amount
    public Text bitcoinamountconvertedlimit;

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
    private bool isAnimationPlaying;
    // The total stats amount
    private int Totalsatsamount = 0;

    private const int BITCOIN_LIMIT = 200;
    private const int BITCOIN_EARN_AMOUNT = 10;
    private float satsamount;

    private string bitcoinToDollarApiUrl = "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=usd";
    #endregion
    public void CollectBitcoins_V1()
    {


        if (BitcoinEarnedAmount >= BITCOIN_LIMIT)
        {
            // Make all buttons non-interactable
            foreach (Button button in CashOutButtons)
            {
                button.interactable = false;
            }

            return;
        }

        // Increase the score by 10
        BitcoinEarnedAmount += BITCOIN_EARN_AMOUNT;

        // Update the score text
        BitoinsCount_V1.text = BitcoinEarnedAmount.ToString();

    }

    public void CollectBitcoins_V2()
    {
        if (BitcoinEarnedAmount < BITCOIN_LIMIT)
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

        BitcoinEarnedAmount = BitcoinEarnedAmount + BITCOIN_EARN_AMOUNT; // Increase the score by 10
        // Update the score text
        BitoinsCount_V2.text = BitcoinEarnedAmount.ToString() + " sats";
        //set animation controller to false
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isAnimationPlaying = false;
    }
    public void CollectBitcoins_V3()
    {


        if (BitcoinEarnedAmount >= BITCOIN_LIMIT)
        {
            // Make all buttons non-interactable
            foreach (Button button in CashOutButtons)
            {
                button.interactable = false;
            }

            return;
        }

        // Increase the score by 10
        BitcoinEarnedAmount += BITCOIN_EARN_AMOUNT;

        // Update the score text
        BitoinsCount_V3.text = BitcoinEarnedAmount.ToString() + " sats";
    }
    public void CashOutBitcoin()
    {
        StartCoroutine(FetchExchangeRate());
        // Check if the number of Bitcoin is greater than 0 to open the withdraw panel if not open the warning panel.
        if (BitcoinEarnedAmount <= 0)
        {
            Warning_Panel.SetActive(true);
            return;
        }

        if (Totalsatsamount >= BITCOIN_LIMIT)
        {
            Emailinputfield_limit.text = EmailInputField.text;
            CashOut_Limtit_Reached_Panel.SetActive(true);
            bitcointext_limit.text = BitcoinEarnedAmount.ToString();
            satsamountconvertedlimit.text = ConvertBitcoinToStats(BitcoinEarnedAmount);
            return;

        }



        GetBitcoinAmount();
        FillSlider();
        calculatestatsamount();
        CashOut_Panel.SetActive(true);



    }

    // get bitcoin amount to withdraw and check if it reached the limit of 200 or no
    public void GetBitcoinAmount()
    {
        if (BitcoinEarnedAmount + Totalsatsamount > BITCOIN_LIMIT)
        {
            int rest = BITCOIN_LIMIT - Totalsatsamount;
            BitcoinEarnedAmount -= rest;

            Totalsatsamount += rest;
            BitcoinAmount.text = BitcoinEarnedAmount.ToString();
            BitoinsCount_V1.text = BitcoinEarnedAmount.ToString();
            BitoinsCount_V2.text = BitcoinEarnedAmount.ToString();
            BitoinsCount_V3.text = BitcoinEarnedAmount.ToString();
            bitcointext_limit.text = BitcoinEarnedAmount.ToString();

            satsamountconvertedlimit.text = ConvertBitcoinToStats(BitcoinEarnedAmount);
        }
        else
        {
            Totalsatsamount += BitcoinEarnedAmount;
            BitcoinAmount.text = BitcoinEarnedAmount.ToString();
            BitcoinEarnedAmount = 0;
            BitoinsCount_V1.text = "0";
            BitoinsCount_V2.text = "0 sats";
            BitoinsCount_V3.text = "0 sats";
        }


    }

    private string ConvertBitcoinToStats(float bitcoinAmount)
    {
        float statsAmount = bitcoinAmount / 10000000f;
        satsamount = statsAmount;
        return "₿" + statsAmount.ToString("F6");
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
        satsamount = statsamount;
        string resultString = statsamount.ToString("F6");
        convertedsatsamounts.text = "₿" + resultString;



    }


    //Open the link for download the zebedee wallet
    public void GetZebedeeWallet()
    {

        Application.OpenURL("https://apps.apple.com/us/app/zbd-games-rewards-bitcoin/id1484394401?mt=8");
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

        string email = EmailInputField.text;
        if (string.IsNullOrEmpty(EmailInputField.text) || !IsValidEmail(email))
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

    bool IsValidEmail(string email)
    {
        // This regex pattern is a simple one for basic email validation.
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }



    IEnumerator FetchExchangeRate()
    {
        UnityWebRequest request = UnityWebRequest.Get(bitcoinToDollarApiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;

            float exchangeRate = ParseExchangeRate(response);
            float dollarsAmount = ConvertBitcoinToDollars(satsamount, exchangeRate);
            Debug.Log("Bitcoin: " + satsamount + " BTC is equivalent to $" + dollarsAmount);
            float roundedBitcoinAmount = Mathf.Round(dollarsAmount * 100) / 100; // Round to two decimal places
            Converteddollarsamounts.text = "$ " + roundedBitcoinAmount.ToString();
            bitcoinamountconvertedlimit.text = "$ " + roundedBitcoinAmount.ToString();

        }
        else
        {
            Debug.LogError("Failed to fetch exchange rate: " + request.error);
            float exchangeRate = 50000f;
            float dollarsAmount = ConvertBitcoinToDollars(satsamount, exchangeRate);
            float roundedBitcoinAmount = Mathf.Round(dollarsAmount * 100) / 100; // Round to two decimal places
            Converteddollarsamounts.text = "$ " + roundedBitcoinAmount.ToString();
            bitcoinamountconvertedlimit.text = "$ " + roundedBitcoinAmount.ToString();

        }
    }

    float ParseExchangeRate(string jsonResponse)
    {
        ExchangeRateData exchangeRateData = JsonUtility.FromJson<ExchangeRateData>(jsonResponse);
        return exchangeRateData.bitcoin.usd;
    }

    float ConvertBitcoinToDollars(float bitcoinAmount, float exchangeRate)
    {
        return bitcoinAmount * exchangeRate;
    }

}

[System.Serializable]
public class ExchangeRateData
{
    public BitcoinData bitcoin;
}

[System.Serializable]
public class BitcoinData
{
    public float usd;
}