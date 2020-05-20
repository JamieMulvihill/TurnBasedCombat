using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTarget : MonoBehaviour
{
    public Camera cam;
    public GameObject target;

    public bool targetSelected;

    private void Start()
    {
        targetSelected = false; 
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.gameObject.GetComponent<Fighter>()){
                    if (!targetSelected){
                        target = hit.transform.gameObject;
                        target.transform.GetChild(0).gameObject.SetActive(true);
                        targetSelected = true;
                    }

                    else if (targetSelected) {
                        if (hit.transform.gameObject != target) {
                            target.transform.GetChild(0).gameObject.SetActive(false);
                            target = hit.transform.gameObject;
                            target.transform.GetChild(0).gameObject.SetActive(true);
                        }
                    }
                    Debug.Log(target.gameObject);
                }
            }
        }
    }
}
