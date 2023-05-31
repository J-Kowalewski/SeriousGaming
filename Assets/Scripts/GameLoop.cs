using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    GameObject countriesContainer;
    GameObject actionsContainer;
    GameObject popupTemplate;
    GameObject endPopup;
    List<Action> actionPool;

    private List<Action> actions;

    private GameObject awarenessText;
    private GameObject destructionText;

    private int destructionCounter = 0;
    private bool countryAware = false;

    public AudioSource successfulSoundEffect;
    public AudioSource failSoundEffect;
    // Start is called before the first frame update
    void Start()
    {
        countriesContainer = GetComponent<Assigning>().countriesContainer;
        actionsContainer = GetComponent<Assigning>().actionsContainer;
        actionPool = GetComponent<Assigning>().actionPool;
        popupTemplate = GetComponent<Assigning>().popupTemplate;
        awarenessText = GetComponent<Assigning>().awarenessText;
        destructionText = GetComponent<Assigning>().destructionText;
        endPopup = GetComponent<Assigning>().endPopupTemplate;
    }
    private void ChangeAwareness()
    {
        actions = new List<Action>(actionPool);

        for (int i = 0; i < countriesContainer.transform.childCount; i++)
        {
            Country country = countriesContainer.transform.GetChild(i).GetComponent<Country>();

            
            for (int j = 0; j < actionsContainer.transform.childCount; j++)
            {
                if (country.actions[j].selected)
                {
                    Action action = country.actions[j];
                    int rand = Random.Range(0, 100);
                    //TODO: calculated difficulty - for now base diff
                    // calculated difficulty = diffculty - destruction/factor
                    double calculatedDiff = action.difficulty - (country.destruction/action.factor);
                    print(calculatedDiff);

                    GameObject popup = Instantiate(popupTemplate, new Vector3(0,6), Quaternion.identity);
                    popup.transform.SetParent(GameObject.Find("Canvas").transform, false);
                    popup.name = "popup" + i;

                    if (rand <= calculatedDiff) //Success
                    {
                        country.awareness += -country.awareness * action.goodResult / 100;
                        /*country.awareness += -action.goodResult;*/
                        print(country.name + " - " + action.name + " - " + "Succesful: -" + action.goodResult);

                        popup.transform.Find("PopUpTop/EventName").GetComponent<TMPro.TextMeshProUGUI>().text = $"<color=#{ColorUtility.ToHtmlStringRGBA(action.goodColor)}>" + action.name + "</color>";
                        popup.transform.Find("Image").GetComponent<Image>().sprite = action.goodImage;
                        popup.transform.Find("FlavorText").GetComponent<TMPro.TextMeshProUGUI>().text = action.goodFlavorText;
                        popup.transform.Find("Results").GetComponent<TMPro.TextMeshProUGUI>().text = $"<color=#{ColorUtility.ToHtmlStringRGBA(action.goodColor)}>" + country.name +": -"+action.goodResult+"% Awareness" + "</color>";

                        successfulSoundEffect.Play();
                    }
                    else
                    {
                        country.awareness += country.awareness * action.badResult / 100;
                        /*country.awareness += action.badResult;*/
                        print(country.name + " - " + action.name + " - " + "Failed: +" + action.badResult);

                        popup.transform.Find("PopUpTop/EventName").GetComponent<TMPro.TextMeshProUGUI>().text = $"<color=#{ColorUtility.ToHtmlStringRGBA(action.badColor)}>" + action.name + "</color>";
                        popup.transform.Find("Image").GetComponent<Image>().sprite = action.badImage;
                        popup.transform.Find("FlavorText").GetComponent<TMPro.TextMeshProUGUI>().text = action.badFlavorText;
                        popup.transform.Find("Results").GetComponent<TMPro.TextMeshProUGUI>().text = $"<color=#{ColorUtility.ToHtmlStringRGBA(action.badColor)}>" + country.name + ": +" + action.badResult + "% Awareness" + "</color>";

                        failSoundEffect.Play();

                    }
                    break;
                }
            }
            country.awareness = Mathf.Clamp(country.awareness, 0, 100);
            if (country.awareness >= 100) countryAware = true;
        }
    }
    private void ResetCountries()
    {
        for (int i = 0; i < countriesContainer.transform.childCount; i++)
        {
            Country x = countriesContainer.transform.GetChild(i).GetComponent<Country>();

            x.actions.Clear();
            for (int j = 0; j < actionsContainer.transform.childCount; j++)
            {
                int random = Random.Range(0, actions.Count);
                Action action = actions[random];
                action.selected = false;
                actions.RemoveAt(random);

                x.actions.Add(action);
            }

            x.transform.Find("BarAwareness").gameObject.GetComponent<Image>().fillAmount = x.awareness / 100f;
            x.transform.Find("BarDestruction").gameObject.GetComponent<Image>().fillAmount = x.destruction / 100f;
        }
    }
    private void ResetActionButtons()
    {
        Country activeCountry = GetComponent<GameData>().activeCountryButton.GetComponent<Country>();

        for (int j = 0; j < actionsContainer.transform.childCount; j++)
        {
            GameObject actionButton = actionsContainer.transform.GetChild(j).gameObject;
            Action y = actionButton.GetComponent<Action>();

            Action action = activeCountry.actions[j];
            y.name = action.name;
            y.difficulty = action.difficulty;
            y.goodResult = action.goodResult;
            y.badResult = action.badResult;
            y.selected = action.selected;

            if (y.selected)
            {
                actionButton.GetComponent<Image>().color = Color.grey;
            }
            else
            {
                actionButton.GetComponent<Image>().color = Color.white;
            }

            Color goodColor = action.goodColor;
            Color badColor = action.badColor;

            actionButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = y.name + "\n\nDifficulty: " + y.difficulty + $"%\n\nSuccess: <color=#{ColorUtility.ToHtmlStringRGBA(goodColor)}>-" + y.goodResult + $"%</color>\n\nFailure: <color=#{ColorUtility.ToHtmlStringRGBA(badColor)}>+" + y.badResult + "%</color>";
        }
    }
    private void ResetTexts()
    {
        GetComponent<GameData>().actionsLeft = 3;
        GameObject y = GetComponent<Assigning>().actionsIndicator;
        y.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Actions " + GetComponent<GameData>().actionsLeft + "/" + "3";

        GetComponent<GameData>().turnCounter++;
        GameObject x = GetComponent<Assigning>().turnsIndicator;
        x.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "TURN " + GetComponent<GameData>().turnCounter;

        Country country = GetComponent<GameData>().activeCountryButton.GetComponent<Country>();
        awarenessText.GetComponent<TMPro.TextMeshProUGUI>().text = "Awareness: " + country.awareness + "%";
        destructionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Destruction: " + country.destruction + "%";

    }
    private void ChangeDestruction()
    {
        //Current Destruction + ((100-current suspicion)/10)
        for (int i = 0; i < countriesContainer.transform.childCount; i++)
        {
            Country country = countriesContainer.transform.GetChild(i).GetComponent<Country>();
            if (country.destruction < 100)
            {
                country.destruction += (100 - country.awareness) / 5;
                if (country.destruction >= 100)
                {
                    print(country.name + " - destroyed");
                    destructionCounter++;
                }
                country.destruction = Mathf.Clamp(country.destruction, 0, 100);
            }
        }
    }
    private void CheckForEnding()
    {
        if(countryAware || destructionCounter >= 3 || GetComponent<GameData>().turnCounter > 10)
        {
            GameObject popup = Instantiate(endPopup, new Vector3(0, 6), Quaternion.identity);
            EndingPopUpData popupData = popup.GetComponent<EndingPopUpData>();

            popup.transform.SetParent(GameObject.Find("Canvas").transform, false);
            popup.name = "endingPopUp";

            if (countryAware)
            {
                //1 country awareness>=100 -> lose
                popup.transform.Find("PopUpTop/EventName").GetComponent<TMPro.TextMeshProUGUI>().text = $"<color=red>" + popupData.loseName + "</color>";
                popup.transform.Find("Image").GetComponent<Image>().sprite = popupData.loseImage;
                popup.transform.Find("FlavorText").GetComponent<TMPro.TextMeshProUGUI>().text = popupData.loseText;
            }
            else if (destructionCounter >= 3)
            {
                //3 countries destroyed ->win
                popup.transform.Find("PopUpTop/EventName").GetComponent<TMPro.TextMeshProUGUI>().text = $"<color=green>" + popupData.winName + "</color>";
                popup.transform.Find("Image").GetComponent<Image>().sprite = popupData.winImage;
                popup.transform.Find("FlavorText").GetComponent<TMPro.TextMeshProUGUI>().text = popupData.winText;
            }
            else if (GetComponent<GameData>().turnCounter > 10)
            {
                //turn>10 -> failsafe lose
                popup.transform.Find("PopUpTop/EventName").GetComponent<TMPro.TextMeshProUGUI>().text = $"<color=yellow>" + popupData.failsafeName + "</color>";
                popup.transform.Find("Image").GetComponent<Image>().sprite = popupData.failsafeImage;
                popup.transform.Find("FlavorText").GetComponent<TMPro.TextMeshProUGUI>().text = popupData.failsafeText;
            }
        }
    }
    public void NextTurn()
    {
        if (GetComponent<GameData>().actionsLeft == 0)
        {
            ChangeAwareness();
            ChangeDestruction();
            ResetCountries();
            ResetActionButtons();
            ResetTexts();
            CheckForEnding();
        }
        else
        {
            print("You need to select 3 actions");
        }
        /*ChangeAwareness();
        ChangeDestruction();
        ResetCountries();
        ResetActionButtons();
        ResetTexts();
        CheckForEnding();*/
    }
}
