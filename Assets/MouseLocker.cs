using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLocker : MonoBehaviour {
    private string _sequence = "";
    private const char LOCK = 'l';
    private const char UNLOCK = 'u';
    private const char WAIT = '-';
    private PointerLockUtility _plu;
    private Coroutine _pluSeq;
    private GUIStyle style = new GUIStyle();
    private string _keycache = "";

    private void Awake() {
        style.fontSize = 20;
        style.wordWrap = true;
        style.richText = true;
        _plu = gameObject.AddComponent<PointerLockUtility>();
    }

    private void Update() {
        if (_pluSeq == null) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                _keycache += "esc ";
            }

            if (Input.GetKeyDown(KeyCode.J)) {
                _keycache += "{lock} ";
                _plu.RequestPointerLock(true);
            }

            if (Input.GetKeyDown(KeyCode.K)) {
                _keycache += "{unlock} ";
                _plu.RequestPointerLock(false);
            }

            foreach (char c in Input.inputString) {
                if (c == LOCK || c == UNLOCK || c == WAIT) {
                    _sequence += c;
                }
            }

            if (_sequence.Length > 0 && Input.GetKeyDown(KeyCode.Backspace)) {
                _sequence = _sequence.Substring(0, _sequence.Length - 1);
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                _sequence = "";
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
                _keycache += $"[{_sequence}] ";
                _pluSeq = StartCoroutine(RunMouseSequence(_sequence));
            }
        }
    }

    private void OnGUI() {
        GUI.Label(new Rect(20, 20, Screen.width - 40, 40), "Sequence symbols: L = seq lock, U = seq unlock, - = seq wait (500ms)", style);   
        GUI.Label(new Rect(20, 50, Screen.width - 40, 40), "Hotkeys: Return = run sequence, C = clear sequence, Backspace, J = manual lock, K = manual unlock", style);   
        GUI.Label(new Rect(20, 100, Screen.width - 40, 40), "Sequence: " + _sequence, style);   
        GUI.Label(new Rect(20, 130, Screen.width - 40, Screen.height - 150), _keycache, style);

        if (_pluSeq != null) {
            GUI.Label(new Rect(20, Screen.height - 60, Screen.width - 40, Screen.height - 100), "Sequence in progress", style);
        }
    }

    private IEnumerator RunMouseSequence(string sequence) {
        sequence = sequence.ToLowerInvariant();
        for (int i = 0; i < sequence.Length; i++) {
            var pos = i + 1;
            _sequence = "<color=green>" + sequence.Substring(0, pos) + "</color>" + sequence.Substring(pos, sequence.Length - pos);
            switch (sequence[i]) {
                case LOCK:
                    _plu.RequestPointerLock(true);
                    break;
                case UNLOCK:
                    _plu.RequestPointerLock(false);
                    break;
                case WAIT:
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
        }

        yield return new WaitForSeconds(0.5f);
        _sequence = sequence;

        _pluSeq = null;
    }
}
