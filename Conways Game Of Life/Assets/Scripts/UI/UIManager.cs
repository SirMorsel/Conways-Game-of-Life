using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    [SerializeField] TMP_Text chanceOfLifePercentText;
    [SerializeField] Slider chanceOfLifeSlider;

    [SerializeField] TMP_InputField widthField;
    [SerializeField] TMP_InputField heightField;

    [SerializeField] TMP_Dropdown rulesDropdown;

    private int minSizeUI;
    private int maxSizeUI;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        InitializeRulesDropdown();
    }

    private int CheckInput(TMP_InputField inputField)
    {
        int convertedNumber;
        bool success = int.TryParse(inputField.text, out convertedNumber);
        if (success)
        {
            convertedNumber = Mathf.Clamp(convertedNumber, minSizeUI, maxSizeUI);
            inputField.text = convertedNumber.ToString();
            return convertedNumber;
        }
        else
        {
            Debug.LogWarning($"Not a number. Please just use positive integers. Default value is now selected {maxSizeUI}");
            inputField.text = maxSizeUI.ToString();
            return maxSizeUI;
        }
    }

    private void InitializeRulesDropdown()
    {
        rulesDropdown.ClearOptions();

        List<string> ruleOptions = new List<string>();
        ruleOptions.Add("Normal");
        ruleOptions.Add("Spread Right");
        ruleOptions.Add("Lazy");
        rulesDropdown.AddOptions(ruleOptions);
    }

    public void SetChanceOfLifeSlider(int value)
    {
        chanceOfLifeSlider.value = value;
        UpdateLifePercentText();
    }

    public void UpdateLifePercentText()
    {
        chanceOfLifePercentText.text = $"{chanceOfLifeSlider.value}%";
    }

    public int GetLifePercentageValue()
    {
        return (int)chanceOfLifeSlider.value;
    }

    public int[] GetFieldSizeUI()
    {
        int[] sizeOfField = { CheckInput(widthField), CheckInput(heightField) };
        return sizeOfField;
    }

    public void SetFieldSizeUI(int x, int y)
    {
        widthField.text = x.ToString();
        heightField.text = y.ToString();
    }

    public void SetFieldRangeUI(int min, int max)
    {
        minSizeUI = min;
        maxSizeUI = max;
    }
    
    public void CloseApplication()
    {
        Application.Quit();
    }

    public int GetRuleValue()
    {
        return rulesDropdown.value;
    }
}
