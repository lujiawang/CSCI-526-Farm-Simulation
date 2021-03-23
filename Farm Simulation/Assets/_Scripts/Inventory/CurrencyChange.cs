using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyChange : MonoBehaviour
{
    // "Animation of change in currencies"


    public Text currencyText;

    public int currentCurrency;
    public int newCurrency;
    public int growthRate;



    private int min;
    private int max;
    private bool currencyChange;

    
    private void Awake()
    {
        Debug.Log("testtesttest " + PlayerPrefs.GetInt("Currency",25));
        currentCurrency = PlayerStats.Currency;
        // newCurrency = 0;
        currencyChange = false;
        min = 0;
        max = 99999;
    }


    private void OnDestroy()
    {
        PlayerPrefs.SetInt("Currency", PlayerStats.Currency);
        PlayerPrefs.Save();
        Debug.Log("Currency saved " + PlayerPrefs.GetInt("Currency"));
    }

    public void SetTrue() {
        currencyChange = true;
    }

    public void ChangeCurrency(int value)
    {
        

        if (newCurrency != currentCurrency)
        {
            if (currentCurrency < newCurrency && currentCurrency <= max)
            {
                currentCurrency += growthRate;

                if (currentCurrency > newCurrency)
                {
                    currentCurrency = Mathf.Min(newCurrency,max);
                    currencyChange = false;
                    PlayerStats.Currency = currentCurrency;
                    //Debug.Log("Current Currency : " + PlayerStats.Currency);
                }

            }
            else if (currentCurrency >= min)
            {
                currentCurrency -= growthRate;

                if (currentCurrency < newCurrency)
                {
                    currentCurrency = Mathf.Max(0,newCurrency);
                    currencyChange = false;
                    PlayerStats.Currency = currentCurrency;
                   // Debug.Log("Current Currency : " + PlayerStats.Currency);
                }


            }
        }
        else
        {
            //stop incrementing
            currencyChange = false;
            PlayerStats.Currency = currentCurrency;

            Debug.Log("Current Currency : " + PlayerStats.Currency);
        }
    }

    // updates the new currency amount. use negative value for spending currency
    public void UpdateCurrency(int toAdd)
    {
        Debug.Log("test");
        
        newCurrency =  currentCurrency + toAdd;
        if (newCurrency > 0)
        {
            newCurrency = Mathf.Min(newCurrency, max);
        }
        else
        {
            newCurrency = Mathf.Max(min, newCurrency);
        }
        SetTrue();
    }

    // Update is called once per frame
    void Update()
    {
        currencyText.text = currentCurrency.ToString();
        if (currencyChange)
        {
            ChangeCurrency(newCurrency);
        }
    }
}
