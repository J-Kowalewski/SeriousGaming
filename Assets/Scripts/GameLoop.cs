using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    GameObject countriesContainer;
    GameObject actionsContainer;
    GameObject popupTemplate;
    List<Action> actionPool;

    private List<Action> actions;

    private GameObject awarenessText;
    private GameObject destructionText;
    // Start is called before the first frame update
    void Start()
    {
        countriesContainer = GetComponent<Assigning>().countriesContainer;
        actionsContainer = GetComponent<Assigning>().actionsContainer;
        actionPool = GetComponent<Assigning>().actionPool;
        popupTemplate = GetComponent<Assigning>().popupTemplate;
        awarenessText = GetComponent<Assigning>().awarenessText;
        destructionText = GetComponent<Assigning>().destructionText;
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


                    GameObject popup = Instantiate(popupTemplate, new Vector3(0,6), Quaternion.identity);
                    popup.transform.SetParent(GameObject.Find("Canvas").transform, false);
                    popup.name = "popup" + i;
                   /* popup.transform.position.Set(0, 6, 0);*/

                    if (rand <= calculatedDiff) //Success
                    {
                        country.awareness += -country.awareness * action.goodResult / 100;
                        print(country.name + " - " + action.name + " - " + "Succesful: -" + action.goodResult);

                        popup.transform.Find("PopUpTop/EventName").GetComponent<TMPro.TextMeshProUGUI>().text = $"<color=#{ColorUtility.ToHtmlStringRGBA(action.goodColor)}>" + action.name + "</color>";
                        popup.transform.Find("Image").GetComponent<Image>().sprite = action.goodImage;
                        popup.transform.Find("FlavorText").GetComponent<TMPro.TextMeshProUGUI>().text = action.goodFlavorText;
                        popup.transform.Find("Results").GetComponent<TMPro.TextMeshProUGUI>().text = $"<color=#{ColorUtility.ToHtmlStringRGBA(action.goodColor)}>" + country.name +": -"+action.goodResult+"% Awareness" + "</color>";
                    }
                    else
                    {
                        country.awareness += country.awareness * action.badResult / 100;
                        print(country.name + " - " + action.name + " - " + "Failed: +" + action.badResult);

                        popup.transform.Find("PopUpTop/EventName").GetComponent<TMPro.TextMeshProUGUI>().text = $"<color=#{ColorUtility.ToHtmlStringRGBA(action.badColor)}>" + action.name + "</color>";
                        popup.transform.Find("Image").GetComponent<Image>().sprite = action.badImage;
                        popup.transform.Find("FlavorText").GetComponent<TMPro.TextMeshProUGUI>().text = action.badFlavorText;
                        popup.transform.Find("Results").GetComponent<TMPro.TextMeshProUGUI>().text = $"<color=#{ColorUtility.ToHtmlStringRGBA(action.badColor)}>" + country.name + ": +" + action.badResult + "% Awareness" + "</color>";

                    }
                    break;
                }
            }
            country.awareness = Mathf.Clamp(country.awareness, 0, 100);
        }
    }
    private void ResetActions()
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
            country.destruction += (100 - country.awareness) / 5;
            if (country.destruction >= 100)
            {
                print(country.name + " - destroyed");
            }
            country.destruction = Mathf.Clamp(country.destruction, 0, 100);
        }
    }
    public void NextTurn()
    {
        ChangeAwareness();
        ChangeDestruction();
        ResetActions();
        ResetActionButtons();
        ResetTexts();
    }
}
