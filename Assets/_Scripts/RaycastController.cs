using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class RaycastController : MonoBehaviour
{
    private RaycastHit hit;
    private GameObject highlighted = null;
    private GameObject draggingObj = null;
    private Vector3 draggingObjPosition;
    private Quaternion draggingObjRotation;
    private Transform draggingObjParent = null;
    private bool isDragging = false;
    private Color highlightColor = new Color(0f, 0.744f, 0.83f);
    private Color defaultColor = new Color(1f, 1f, 1f, 0.7f);
    private LineRenderer lineRenderer;

    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        draggingObjParent = GameObject.Find("Anatomy").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 100, Color.blue);

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {

            GameObject currentObj = hit.collider.gameObject;

            if (currentObj.tag == "Draggable")
            {
                if (SteamVR_Actions._default.InteractUI.GetStateDown(SteamVR_Input_Sources.Any) && !isDragging)
                {
                    draggingObj = currentObj;
                    draggingObjPosition = draggingObj.transform.position;
                    draggingObjRotation = draggingObj.transform.rotation;

                    draggingObj.transform.parent = lineRenderer.transform;

                    isDragging = true;
                }

                if (SteamVR_Actions._default.InteractUI.GetStateUp(SteamVR_Input_Sources.Any) && isDragging)
                {
                    draggingObj.transform.parent = draggingObjParent;
                    draggingObj.transform.position = draggingObjPosition;
                    draggingObj.transform.rotation = draggingObjRotation;

                    draggingObj = null;
                    isDragging = false;
                }
            }
            else if (currentObj.tag == "Toggle")
            {
                if (SteamVR_Actions._default.InteractUI.GetStateDown(SteamVR_Input_Sources.LeftHand))
                {
                    Toggle toggleEl = currentObj.GetComponent<Toggle>();
                    ChangeToggleValue(toggleEl);
                }

                if (highlighted != null) {
                    Unhighlight(highlighted);
                }
                Highlight(currentObj);

            }
        }
        else
        {
            if (highlighted != null) { Unhighlight(highlighted); }
        }
    }

    private void ChangeToggleValue(Toggle toggleEl)
    {
        toggleEl.isOn = !toggleEl.isOn;
    }

    private void Highlight(GameObject obj)
    {
        obj.GetComponent<Image>().color = highlightColor;
        highlighted = obj;
    }

    private void Unhighlight(GameObject obj)
    {
        obj.GetComponent<Image>().color = defaultColor;
        highlighted = null;
    }
}
