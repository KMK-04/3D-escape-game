﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChatManager))]
public class ChatEditor : Editor {
	ChatManager chatManager;
	string text;


	void OnEnable() {
		chatManager = target as ChatManager;
	}


	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		EditorGUILayout.BeginHorizontal();
		text = EditorGUILayout.TextArea(text);
		if (GUILayout.Button("보내기", GUILayout.Width(60)) && text.Trim() != "") {
			chatManager.Chat(false, text, "타인", null);
			text = "";
			GUI.FocusControl(null);
		}

		EditorGUILayout.EndHorizontal();
	}
}
