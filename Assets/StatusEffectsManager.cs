using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class StatusEffectsManager : MonoBehaviour
{
    public GameObject coffeeEffect, teaEffect, coffeeCanvas,inventoryCoffeeCanvas,shopCoffeeCanvas;
    public Text waterCountText, coffeeCountText, teaCountText, moneyCountText;
    public Button coffeeButton, teaButton, yourButton, buyWaterBtn, buyTeaBtn, buyCoffeeBtn;
    public float coffeeDuration, teaDuration;

    private double waterCount, coffeeCount, teaCount;
    private int moneyCount = 0;
    private Coroutine coffeeCoroutine, teaCoroutine;

    private bool isCoffeeCanvasActive = false;
    private bool isInventoryCoffeeCanvasActive = false;
    private bool isShopCoffeeCanvasActive = false;
    public bool isCoffee, isTea;

    public GameObject popUpPanel;
    public Text popUpMessageText;
    private void Awake()
    {
        popUpPanel.SetActive(false);
        InitializeUI();
        LoadCounts();
        AddButtonListeners();
        coffeeCanvas.SetActive(false);
        inventoryCoffeeCanvas.SetActive(false);
        shopCoffeeCanvas.SetActive(false);
    }

    void UpdateUI()
    {
        waterCountText.text = $"{waterCount / 1000}L";
        coffeeCountText.text = $"{coffeeCount / 1000}Kg";
        teaCountText.text = $"{teaCount / 1000}Kg";
        moneyCountText.text = $"{moneyCount}$";

        if (moneyCount >= 15 || isCoffeeCanvasActive)
        {
            coffeeCanvas.SetActive(true);
            isCoffeeCanvasActive = true; // Canvas remains active once enabled
        }

        if (moneyCount >= 15 || isInventoryCoffeeCanvasActive)
        {
            inventoryCoffeeCanvas.SetActive(true);
            isInventoryCoffeeCanvasActive = true; // Canvas remains active once enabled
        }

        if (moneyCount >= 15 || isShopCoffeeCanvasActive)
        {
            shopCoffeeCanvas.SetActive(true);
            isShopCoffeeCanvasActive = true; // Canvas remains active once enabled
        }
    }


    void InitializeUI()
    {
        yourButton.onClick.AddListener(() => SceneManager.LoadScene("FirstScreen"));
        UpdateUI();
    }

    void AddButtonListeners()
    {
        buyWaterBtn.onClick.AddListener(() => TryBuyWater(19000, 2));
        buyTeaBtn.onClick.AddListener(() => TryBuyTea(1000, 5));
        buyCoffeeBtn.onClick.AddListener(() => TryBuyCoffee(1000, 20));
        coffeeButton.onClick.AddListener(() => StartCoffeeEffect(ref coffeeCoroutine, coffeeEffect, "CoffeeRadialProgressBar", coffeeDuration, EndCoffeeEffect));
        teaButton.onClick.AddListener(() => StartTeaEffect(ref teaCoroutine, teaEffect, "TeaRadialProgressBar", teaDuration, EndTeaEffect));
    }

    void StartCoffeeEffect(ref Coroutine coroutine, GameObject effect, string progressBarName, float duration, System.Action endEffectAction)
    {
        if(coffeeCount>=14.0 && waterCount >= 350) 
        {
        CoffeeSold();
        effect.SetActive(true);
        effect.transform.Find(progressBarName).GetComponent<CircularProgressBar>().ActivateCountdown(duration);
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(EffectCoroutine(duration, endEffectAction));
        }
    }
    void StartTeaEffect(ref Coroutine coroutine, GameObject effect, string progressBarName, float duration, System.Action endEffectAction)
    {
        if(teaCount >= 4.0 && waterCount >=100) {
            TeaSold();
        effect.SetActive(true);
        effect.transform.Find(progressBarName).GetComponent<TeaProgressBar>().ActivateCountdown(duration);
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(EffectCoroutine(duration, endEffectAction));
        }
    }

    IEnumerator EffectCoroutine(float delay, System.Action endEffectAction)
    {
        yield return new WaitForSeconds(delay);
        endEffectAction?.Invoke();
    }

    void EndCoffeeEffect()
    {
        isCoffee = false;

        StartCoffeeEffect(ref coffeeCoroutine, coffeeEffect, "CoffeeRadialProgressBar", coffeeDuration, EndCoffeeEffect);
    }

    void EndTeaEffect()
    {
        isTea = false;
        
        StartTeaEffect(ref teaCoroutine, teaEffect, "TeaRadialProgressBar", teaDuration, EndTeaEffect);
    }

    void TryBuyWater(double amount, int cost)
    {
        if (moneyCount >= cost)
        {
            waterCount += amount;
            moneyCount -= cost;
            UpdateUI();
        }
        else
        {
            // Optionally, show a message to the player indicating insufficient funds
            ShowPopUp("Not enough money to buy water.");
        }
    }

    void TryBuyTea(double amount, int cost)
    {
        if (moneyCount >= cost)
        {
            teaCount += amount;
            moneyCount -= cost;
            UpdateUI();
            SaveCounts();
        }
        else
        {
            // Optionally, show a message to the player indicating insufficient funds
            ShowPopUp("Not enough money to buy tea.");
        }
    }

    void TryBuyCoffee(double amount, int cost)
    {
        if (moneyCount >= cost)
        {
            coffeeCount += amount;
            moneyCount -= cost;
            UpdateUI();
            SaveCounts();
        }
        else
        {
            // Optionally, show a message to the player indicating insufficient funds
            ShowPopUp("Not enough money to buy coffee.");
        }
    }

    void TeaSold()
    {
        teaCount -= 4.0;
        waterCount -= 100;
        moneyCount += 1;
        UpdateUI();
        SaveCounts();
    }

    void CoffeeSold()
    {
        coffeeCount -= 14.0;
        waterCount -= 350;
        moneyCount += 3;
        UpdateUI();
        SaveCounts();
    }

    void SaveCounts()
    {
        Counts counts = new Counts { WaterCount = waterCount, CoffeeCount = coffeeCount, TeaCount = teaCount, MoneyCount = moneyCount };
        string json = JsonUtility.ToJson(counts);
        try
        {
            File.WriteAllText(Application.persistentDataPath + "/counts.json", json);
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving counts: " + e.Message);
        }
    }

    void LoadCounts()
    {
        string path = Application.persistentDataPath + "/counts.json";
        if (File.Exists(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                Counts counts = JsonUtility.FromJson<Counts>(json);
                waterCount = counts.WaterCount;
                coffeeCount = counts.CoffeeCount;
                teaCount = counts.TeaCount;
                moneyCount = counts.MoneyCount;
                
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading counts: " + e.Message);
            }
        }
        else
        {
            moneyCount = 10;
        }
        UpdateUI();
    }
    public void ShowPopUp(string message)
    {
        popUpMessageText.text = message;
        popUpPanel.SetActive(true);
    }

    public void HidePopUp()
    {
        popUpPanel.SetActive(false);
    }
    [System.Serializable]
    private class Counts
    {
        public double WaterCount, CoffeeCount, TeaCount;
        public int MoneyCount;
    }
}
