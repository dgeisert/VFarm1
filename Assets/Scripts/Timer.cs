using UnityEngine;
using System.Collections;

public class Timer {

	public float timerStart;
	public float timerDuration = 1f;
	public TimerObject timerObject;
	public StateMachine sm;

	public Timer(StateMachine setSm){
		timerStart = Time.time;
		sm = setSm;
	}
	public Timer(){
		timerStart = Time.time;
	}

	public void StartTimer(float duration = -1, bool hasTimerObject = false, Transform parent = null, bool bar = true, bool numbers = true){
		if (duration != -1) {
			timerDuration = duration;
		}
		timerStart = Time.time;
		if (hasTimerObject) {
			if (timerObject == null) {
				GameObject go = (GameObject) GameObject.Instantiate(StateMaster.instance.timer);
				timerObject = go.GetComponent<TimerObject> ();
				if (parent != null) {
					go.transform.SetParent (parent);
				}
				go.transform.localPosition = Vector3.zero;
				go.transform.localEulerAngles = Vector3.zero;
				timerObject.sm = sm;
				timerObject.timer = this;
				timerObject.StartTimer (timerDuration, bar, numbers);
			}
		}
	}
	public bool CheckTimer(bool immediate = false){
		if (immediate) {
			return (timerStart + timerDuration < Time.time) && (timerStart + timerDuration + 0.2f > Time.time);
		} else {
			return (timerStart + timerDuration < Time.time);
		}
	}
	public bool CheckTimer(float time){
		return (timerStart + time < Time.time);
	}
	public bool TimerActive(){
		return (timerStart + timerDuration > Time.time);
	}
	public float TimerPercent(){
		return Mathf.Min (Mathf.Max ((Time.time - timerStart)/timerDuration, 0), 1);
	}
	public float TimerRemaining(){
		return (timerDuration + timerStart - Time.time);
	}
}
