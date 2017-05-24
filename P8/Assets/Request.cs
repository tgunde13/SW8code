using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mgl;
using Firebase.Database;
using System;

/// <summary>
/// Class to send requests to Firebase with.
/// To send a request, instantiate this and call Start.
/// </summary>
public class Request {
	private TaskIndicator taskIndicator;
	private DatabaseReference responseRef;
	private DialogPanel dialog;
	private Dictionary<string, object> requestData;
	private ResponseHandler responseHandler;
	private MonoBehaviour behaviour;

	public delegate bool ResponseHandler(DataSnapshot snapshot);

	/// <summary>
	/// Initializes a new instance of the <see cref="Request"/> class.
	/// </summary>
	/// <param name="behaviour">Behaviour to start a coroutine with.</param>
	/// <param name="taskIndicator">Task indicator.</param>
	/// <param name="dialog">Dialog to show errors with.</param>
	/// <param name="responseHandler">Response handler.</param>
	/// <param name="requestData">Request data.</param>
	public Request(MonoBehaviour behaviour, TaskIndicator taskIndicator, DialogPanel dialog, ResponseHandler responseHandler, Dictionary<string, object> requestData) {
		this.behaviour = behaviour;
		this.taskIndicator = taskIndicator;
		this.dialog = dialog;
		this.responseHandler = responseHandler;
		this.requestData = requestData;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Request"/> class.
	/// </summary>
	/// <param name="behaviour">Behaviour to start a coroutine with.</param>
	/// <param name="dialog">Dialog to show errors with.</param>
	/// <param name="responseHandler">Response handler.</param>
	/// <param name="requestData">Request data.</param>
	public Request(MonoBehaviour behaviour, DialogPanel dialog, ResponseHandler responseHandler, Dictionary<string, object> requestData){
		this.behaviour = behaviour;
		this.taskIndicator = null;
		this.dialog = dialog;
		this.responseHandler = responseHandler;
		this.requestData = requestData;
	}

	/// <summary>
	/// Start this request.
	/// </summary>
	public void Start() {
		if (taskIndicator != null) {
			taskIndicator.OnStart ();
		}

		behaviour.StartCoroutine(InternetConnectionHelper.CheckInternetConnection ((isConnected) => {
			Debug.Log ("TOB: Request, Start, isConnected: " + isConnected);
			if (!isConnected) {
				if(taskIndicator != null){
					// Show error message, hide process indicator while message is shown
					dialog.show (I18n.Instance.__ ("ErrorInternet"), taskIndicator);
					return;
				} else {
					dialog.show (I18n.Instance.__ ("ErrorInternet"));
					return;
				}
			}

			MakeRequest ();
		}));
	}

	/// <summary>
	/// Makes the request to configure.
	/// </summary>
	private void MakeRequest () {
		DatabaseReference taskRef = FirebaseDatabase.DefaultInstance
			.GetReference (Constants.FirebaseTasksNode);
		string userId = FirebaseAuthHandler.getUserId ();
		responseRef = taskRef.Child (Constants.FirebaseResponsesNode)
			.Child (userId);

		// Remove old reponse
		responseRef.RemoveValueAsync ()
			.ContinueWith (task1 => {
				if (task1.IsFaulted) {
					Debug.Log ("TOB: Request, Remove old reponse IsFaulted");
					if(taskIndicator != null){
						dialog.show (I18n.Instance.__ ("ErrorFirebase"), taskIndicator);
					} else {
						dialog.show (I18n.Instance.__ ("ErrorFirebase"));
					}
				} else if (task1.IsCompleted) {
					Debug.Log ("TOB: Request, Remove old reponse IsCompleted");
					// Listen to response
					responseRef.ValueChanged += OnResponseChanged;

					// Request
					taskRef.Child (Constants.FirebaseRequestsNode)
						.Child (userId)
						.SetValueAsync (requestData)
						.ContinueWith (task2 => {
							if (task2.IsFaulted) {
								Debug.Log ("TOB: Request, Request IsFaulted");
								if(taskIndicator != null){
									dialog.show (I18n.Instance.__ ("ErrorFirebase"), taskIndicator);
								} else {
									dialog.show (I18n.Instance.__ ("ErrorFirebase"));
								}
								// Stop listening
								responseRef.ValueChanged -= OnResponseChanged;
							}
						});
				}
			});
	}

	/// <summary>
	/// Handles a change in response from the server.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void OnResponseChanged (object sender, ValueChangedEventArgs args) {
		// If no response exists, do nothing and wait for another response change
		if (!args.Snapshot.Exists) {
			Debug.Log ("TOB: Request, OnResponseChanged, !args.Snapshot.Exists");
			return;
		}

		// Call response handler
		// if the handler did not handle the response, show an error message
		if (!responseHandler (args.Snapshot)) {
			Debug.Log ("TOB: Request, OnResponseChanged, default");
			if (taskIndicator != null) {
				dialog.show (I18n.Instance.__ ("ErrorFirebase"), taskIndicator);
			} else {
				dialog.show (I18n.Instance.__ ("ErrorFirebase"));
			}
		}

		// Stop listening
		responseRef.ValueChanged -= OnResponseChanged;

		// Remove response
		responseRef.RemoveValueAsync ();
	}
}
