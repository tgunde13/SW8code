using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Task indicator.
/// Indicates a task by showing a process indicator while the task is executing.
/// Also makes some selectables uninteractable while the task is executing.
/// </summary>
public class TaskIndicator {
	public GameObject processIndicator;
	public Selectable[] selectables;

	public TaskIndicator(GameObject processIndicator, Selectable[] selectables) {
		this.processIndicator = processIndicator;
		this.selectables = selectables;
	}

	/// <summary>
	/// Makes selectables uninteractable and shows the process indicator.
	/// </summary>
	public void OnStart() {
		foreach (Selectable selectable in selectables) {
			selectable.interactable = false;
		}

		processIndicator.SetActive(true);
	}

	/// <summary>
	/// Hides the process indicator, but does not change selectables.
	/// </summary>
	public void OnPause() {
		processIndicator.SetActive(false);
	}

	/// <summary>
	/// Makes selectables interactable and hides the process indicator
	/// </summary>
	public void OnEnd() {
		foreach (Selectable selectable in selectables) {
			selectable.interactable = true;
		}

		processIndicator.SetActive(false);
	}
}
