using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountryButtonClick : MonoBehaviour
{
	Country country;
	private GameObject awarenessText;
	private GameObject destructionText;
	private GameObject actionsContainer;

	private GameObject gameController;
	void Start()
	{
		gameController = GameObject.Find("GameController");
		awarenessText = gameController.GetComponent<Assigning>().awarenessText;
		destructionText = gameController.GetComponent<Assigning>().destructionText;
		actionsContainer = gameController.GetComponent<Assigning>().actionsContainer;

		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);

		country = GetComponent<Country>();
	}

	void TaskOnClick()
	{
        if (gameController.GetComponent<GameData>().activeCountryButton != null)
        {
			gameController.GetComponent<GameData>().activeCountryButton.GetComponent<Image>().color = Color.white;

		}
		gameController.GetComponent<GameData>().activeCountryButton = gameObject;

		gameController.GetComponent<GameData>().activeCountryButton.GetComponent<Image>().color = Color.gray;

		awarenessText.GetComponent<TMPro.TextMeshProUGUI>().text = "Awareness: " + country.awareness + "%";
		destructionText.GetComponent<TMPro.TextMeshProUGUI>().text = "Destruction: " + country.destruction + "%";

		for (int j = 0; j < actionsContainer.transform.childCount; j++)
		{
			GameObject actionButton = actionsContainer.transform.GetChild(j).gameObject;
            Action y = actionButton.GetComponent<Action>();
			
			Action action = country.actions[j];
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

			actionButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = y.name + "\n\nDifficulty: " + y.difficulty + $"%\n\nSuccess: <color=#{ColorUtility.ToHtmlStringRGBA(goodColor)}>-" + y.goodResult + $"%</color>\n\nFailure: <color=#{ColorUtility.ToHtmlStringRGBA(badColor)}>+" + y.badResult+"%</color>";
		}
	}
}
