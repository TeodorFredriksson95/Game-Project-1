
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class Accessibility_Menu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject accessibilityMenu;
    [SerializeField] private GameObject mainMenu;

    [Header("Settings")]
    [SerializeField] private Slider colorBlindlessSlider;
    [SerializeField] private Slider pixelFilterSlider;
    [SerializeField] private Toggle bobToggle;
    [SerializeField] private Toggle ScreenShakeToggle;
    [SerializeField] private Toggle OneHandedToggle;

    [Header("Texts")]
    [SerializeField] private TMP_Text ColorBlindlessText;
    [SerializeField] private TMP_Text PixelFilterText;

    [Header("Other")]
    [SerializeField] private Material ColorBlindMat;
    [SerializeField] private InputActionReference oneHanded;
    [SerializeField] private InputActionReference standard;

   // private List<Navigation> btnNavigations = new List<Navigation>();
   // private List<Navigation> sliderNavigations = new List<Navigation>();
   // private List<Navigation> toggleNavigations = new List<Navigation>();

    void Start()
    {

    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(pixelFilterSlider.gameObject);
        colorBlindlessSlider.value = PlayerPrefs.GetFloat("ColorBlindlessValue");
        ColorBlindlessValueChanged();
        pixelFilterSlider.value = PlayerPrefs.GetInt("PixelFilterValue");
        onPixelFilterChanged();

        bobToggle.isOn = PlayerPrefs.GetInt("BobSetting") == 1;

        onBobToggleChanged();

        ScreenShakeToggle.isOn = PlayerPrefs.GetInt("ScreenShakeSetting") == 1;

        onScreenShakeToggleChanged();
    }

    private static readonly Color[,] colors = {

        { new Color(1, 0, 0), new Color(0, 1, 0), new Color(0, 0, 1) },

        { new Color(0.567f, 0.433f, 0), new Color(0.558f, 0.442f, 0), new Color(0, 0.242f, 0.758f) },

        { new Color(0.625f, 0.375f, 0), new Color(0.700f, 0.300f, 0), new Color(0, 0.300f, 0.700f) },

        { new Color(0.950f, 0.050f, 0), new Color(0, 0.433f, 0.567f), new Color(0, 0.475f, 0.525f) }

    };
    
    public void BackButton()
    {
        accessibilityMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ColorBlindlessValueChanged()
    {
        switch (colorBlindlessSlider.value)
        {
            case 3:
                ColorBlindMat.SetFloat("_Opacity", 0.7f);
                ColorBlindMat.SetColor("_R", colors[3, 0]);
                ColorBlindMat.SetColor("_G", colors[3, 1]);
                ColorBlindMat.SetColor("_B", colors[3, 2]);
                ColorBlindlessText.text = "Tritanopia";
                break;
            case 2:
                ColorBlindMat.SetFloat("_Opacity", 0.7f);
                ColorBlindMat.SetColor("_R", colors[2, 0]);
                ColorBlindMat.SetColor("_G", colors[2, 1]);
                ColorBlindMat.SetColor("_B", colors[2, 2]);
                ColorBlindlessText.text = "Deuteranopia";
                break;
            case 1:
                ColorBlindMat.SetFloat("_Opacity", 0.7f);
                ColorBlindMat.SetColor("_R", colors[1, 0]);
                ColorBlindMat.SetColor("_G", colors[1, 1]);
                ColorBlindMat.SetColor("_B", colors[1, 2]);
                ColorBlindlessText.text = "Protanopia";
                break;
            case 0:
                ColorBlindMat.SetInt("_Opacity", 0);
                ColorBlindlessText.text = "Default";
                break;
            default:
                ColorBlindMat.SetInt("_Opacity", 0);
                ColorBlindlessText.text = "Default";
                break;
        }
        PlayerPrefs.SetFloat("ColorBlindlessValue", colorBlindlessSlider.value);
    }

    public void onPixelFilterChanged()
    {
        switch (pixelFilterSlider.value)
        {
            case 1:
                PlayerPrefs.SetInt("PixelFilterValue", (int)pixelFilterSlider.value);
                PixelFilterText.text = "Off";
                break;
            case 2:
                PlayerPrefs.SetInt("PixelFilterValue", (int)pixelFilterSlider.value);
                PixelFilterText.text = "Medium";
                break;
            case 3:
                PlayerPrefs.SetInt("PixelFilterValue", (int)pixelFilterSlider.value);
                PixelFilterText.text = "High";
                break;
            default:
                PlayerPrefs.SetInt("PixelFilterValue", (int)pixelFilterSlider.value);
                break;
        }
    }

    public void onBobToggleChanged() => PlayerPrefs.SetInt("BobSetting", bobToggle.isOn ? 1 : 0);

    public void onScreenShakeToggleChanged() => PlayerPrefs.SetInt("ScreenShakeSetting", ScreenShakeToggle.isOn ? 1 : 0);

    public void onOneHandedToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("OneHandedSetting", isOn ? 1 : 0);
        if (isOn)
            EventSystem.current.GetComponent<InputSystemUIInputModule>().move = oneHanded;
        else
        EventSystem.current.GetComponent<InputSystemUIInputModule>().move = standard;
    }

 /*   void ChangeHands()
    {

        UnityEngine.Object[] btnObjects = FindObjectsByType(typeof(Button), FindObjectsInactive.Include, FindObjectsSortMode.None);
        UnityEngine.Object[] sliderObjects = FindObjectsByType(typeof(Slider), FindObjectsInactive.Include, FindObjectsSortMode.None);
        UnityEngine.Object[] toggleObjects = FindObjectsByType(typeof(Toggle), FindObjectsInactive.Include, FindObjectsSortMode.None);
     
        foreach (var i in btnObjects)
        {
                btnNavigations.Add(i.GetComponent<Button>().navigation);    
        }

        for (int i = 0; i < btnNavigations.Count; i++)
        {
            Selectable oldUp = btnNavigations[i].selectOnUp;
            Selectable oldDown = btnNavigations[i].selectOnDown;
            Selectable oldLeft = btnNavigations[i].selectOnLeft;
            Selectable oldRight = btnNavigations[i].selectOnRight;
            Navigation customNav = new Navigation();
            customNav.mode = Navigation.Mode.Explicit;
            customNav.selectOnUp = oldLeft;
            customNav.selectOnDown = oldRight;
            customNav.selectOnLeft = oldDown;
            customNav.selectOnRight = oldUp;
            btnObjects[i].GetComponent<Button>().navigation = customNav;
        }



        foreach (var i in sliderObjects)
        {
                sliderNavigations.Add(i.GetComponent<Slider>().navigation);        
        }

        for (int i = 0; i < sliderNavigations.Count; i++)
        {
            Selectable oldUp = sliderNavigations[i].selectOnUp;
            Selectable oldDown = sliderNavigations[i].selectOnDown;
            Selectable oldLeft = sliderNavigations[i].selectOnLeft;
            Selectable oldRight = sliderNavigations[i].selectOnRight;
            Navigation customNav = new Navigation();
            customNav.mode = Navigation.Mode.Explicit;
            customNav.selectOnUp = oldLeft;
            customNav.selectOnDown = oldRight;
            customNav.selectOnLeft = oldDown;
            customNav.selectOnRight = oldUp;

            sliderObjects[i].GetComponent<Slider>().navigation = customNav;
           // sliderObjects[i].GetComponent<Slider>().direction = Slider.Direction.BottomToTop;
        }
        

        foreach (var i in toggleObjects)
        {

                toggleNavigations.Add(i.GetComponent<Toggle>().navigation);
            
        }

        for (int i = 0; i < toggleNavigations.Count; i++)
        {
            Selectable oldUp = toggleNavigations[i].selectOnUp;
            Selectable oldDown = toggleNavigations[i].selectOnDown;
            Selectable oldLeft = toggleNavigations[i].selectOnLeft;
            Selectable oldRight = toggleNavigations[i].selectOnRight;
            Navigation customNav = new Navigation();
            customNav.mode = Navigation.Mode.Explicit;
            customNav.selectOnUp = oldLeft;
            customNav.selectOnDown = oldRight;
            customNav.selectOnLeft = oldDown;
            customNav.selectOnRight = oldUp;
            toggleObjects[i].GetComponent<Toggle>().navigation = customNav;

        }
        btnNavigations.Clear();
        sliderNavigations.Clear();
        toggleNavigations.Clear();
    }*/

}
