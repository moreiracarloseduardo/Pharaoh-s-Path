using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_ : MonoBehaviour {
    public LevelPrefab_[] levels;
    public Transform levelHolder;
    public LevelPrefab_ currentLevel;
    [Tooltip("if true, upon reaching the last level, it loops back to the first level and continues forward. If false, it repeatedly loads the last level..")]
    public bool repeatLevels = true;
    public int minLevelRepeat;

    [Header("Editor Level")]
    public bool useEditorLevel;
    public int editorLevel;

    private void Start() {
        if (Application.isEditor && editorLevel != 0 && useEditorLevel) {
            currentLevel = LoadLevel(editorLevel);
        } else {
            currentLevel = LoadLevel();
        }
    }

    public LevelPrefab_ LoadLevel(int level = 0) {
        if (level == 0) {
            level = PlayerPrefs.GetInt("currentLevel", 0);
        }
        if (repeatLevels) {
            if (level >= levels.Length) {
                level = Mathf.Clamp(level % levels.Length, minLevelRepeat, levels.Length);
            }

        } else {
            if (level >= levels.Length) {
                level = levels.Length;
            }
        }
        GameObject levelObject = Instantiate(levels[level].gameObject, Vector3.zero, Quaternion.identity);
        levelObject.transform.parent = levelHolder.transform;
        LevelPrefab_ currentLevelPrefab = levelObject.GetComponent<LevelPrefab_>();
        currentLevel = currentLevelPrefab;
        return currentLevelPrefab;

    }

    public void EndLevel(bool levelComplete, int score) {
        PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("currentLevel", 0) + 1);
    }

}
