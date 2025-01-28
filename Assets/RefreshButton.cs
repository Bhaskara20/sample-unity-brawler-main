using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using baskara.FusionBites;
public class RefreshButton : MonoBehaviour
{
    private Button refreshButton;


    private void Awake()
    {
        if (refreshButton == null)
        {
            refreshButton = GetComponent<Button>();
        }
        refreshButton.onClick.AddListener(Refresh);
    }

    public void Refresh()
    {
        StartCoroutine(RefreshWait());
    }

    private IEnumerator RefreshWait()
    {
        refreshButton.interactable = false;
        FusionConnection.Instance.RefreshSessionListUI();
        yield return new WaitForSeconds(3f);
        refreshButton.interactable = true;
    }


}
