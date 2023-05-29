using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Assigning : MonoBehaviour
{
    public GameObject countriesContainer;
    public GameObject actionsContainer;
    public GameObject actionsIndicator;
    public GameObject turnsIndicator;
    public GameObject popupTemplate;
    public GameObject awarenessText;
    public GameObject destructionText;

    public List<Country> countryPool;
    public List<Action> actionPool;

    private List<Action> actions;
    public void Start()
    {
        actions = new List<Action>(actionPool);

        for (int i = 0; i < countriesContainer.transform.childCount; i++)
        {
            GameObject countryButton = countriesContainer.transform.GetChild(i).gameObject;

            int rand = Random.Range(0, countryPool.Count);
            Country country = countryPool[rand];
            countryPool.RemoveAt(rand);
            
            Country x = countryButton.GetComponent<Country>();
            x.name = country.name;
            x.awareness = country.awareness;
            x.destruction = country.destruction;
            x.flag = country.flag;

            countryButton.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>().text = x.name;
            countryButton.transform.Find("Image/Flag").gameObject.GetComponent<Image>().sprite = x.flag;

            countryButton.transform.Find("BarAwareness").gameObject.GetComponent<Image>().fillAmount = x.awareness / 100f;
            countryButton.transform.Find("BarDestruction").gameObject.GetComponent<Image>().fillAmount = x.destruction / 100f;

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
    public void Assign()
    {
        actions = actionPool;
    }
}
