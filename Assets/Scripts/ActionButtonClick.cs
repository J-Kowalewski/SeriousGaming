using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonClick : MonoBehaviour
{
	Action action;
	GameObject gameController;
	void Start()
	{
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);

		gameController = GameObject.Find("GameController");
	}

	void TaskOnClick()
    {
		GameObject activeCountry = gameController.GetComponent<GameData>().activeCountryButton;

		action = GetComponent<Action>();

		action.selected = !action.selected;

		activeCountry.GetComponent<Country>().actions[transform.GetSiblingIndex()].selected = !activeCountry.GetComponent<Country>().actions[transform.GetSiblingIndex()].selected;

		if (action.selected)
        {
			GetComponent<Image>().color = Color.grey;
			gameController.GetComponent<GameData>().actionsLeft--;
		}
        else
        {
			GetComponent<Image>().color = Color.white;
			gameController.GetComponent<GameData>().actionsLeft++;
		}

		for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (i != transform.GetSiblingIndex())
            {
                GameObject sibling = transform.parent.GetChild(i).gameObject;
                if (sibling.GetComponent<Action>().selected)
                {
					gameController.GetComponent<GameData>().actionsLeft++;

				}
				sibling.GetComponent<Action>().selected = false;
                sibling.GetComponent<Image>().color = Color.white;

                activeCountry.GetComponent<Country>().actions[i].selected = false;
            }
        }
		GameObject y = gameController.GetComponent<Assigning>().actionsIndicator;
		y.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Actions " + gameController.GetComponent<GameData>().actionsLeft + "/" + "3";
	}
}
