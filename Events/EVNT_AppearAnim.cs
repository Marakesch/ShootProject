using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EVNT_AppearAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.DOScale(1f, 1f).SetLoops(1, LoopType.Yoyo).SetEase(Ease.InOutElastic).Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
