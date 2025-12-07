using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Project_AR.Editor
{
    [CustomEditor(typeof(TextAsset))]
    public class JsonInspector : UnityEditor.Editor
    {
        private ScoreData _scoreData;
        private bool _isScoreDataJson = false;
        public override void OnInspectorGUI()
        {
            TextAsset textAsset = (TextAsset)target;
            if (textAsset is null)
            {
                base.OnInspectorGUI();
                return;
            }
            // json判定とパース
            try
            {
                _scoreData = JsonConvert.DeserializeObject<ScoreData>(textAsset.text);
                _isScoreDataJson = _scoreData != null && _scoreData.Meta != null;
            }
            catch
            {
                _isScoreDataJson = false;
            }
            if (_isScoreDataJson)
            {
                EditorGUILayout.LabelField("json譜面 インスペクター", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Meta情報", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("タイトル", _scoreData.Meta.Title);
                EditorGUILayout.LabelField("アーティスト", _scoreData.Meta.Artist);
                EditorGUILayout.LabelField("マッパー", _scoreData.Meta.Mapper);
                EditorGUILayout.LabelField("BPM", _scoreData.Meta.Bpm.ToString());
                EditorGUILayout.LabelField("オフセット", _scoreData.Meta.Offset.ToString());
                EditorGUILayout.LabelField("レーン数", _scoreData.Meta.LaneCount.ToString());
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Notes一覧", EditorStyles.boldLabel);
                if (_scoreData.Notes != null)
                {
                    var typeCounts = new Dictionary<string, int>();
                    foreach (var note in _scoreData.Notes)
                    {
                        if (string.IsNullOrEmpty(note.Type)) continue;
                        typeCounts.TryAdd(note.Type, 0);
                        typeCounts[note.Type]++;
                    }
                    EditorGUILayout.LabelField($"ノーツ総数: {_scoreData.Notes.Count}");
                    foreach (var kv in typeCounts)
                    {
                        EditorGUILayout.LabelField($"{kv.Key}ノーツ: {kv.Value}");
                    }
                }
            }
            else
            {
                base.OnInspectorGUI();
            }
        }
    }

    public class ScoreData
    {
        public Meta Meta;
        public List<Note> Notes;
    }

    public class Meta
    {
        public string Title;
        public string Artist;
        public string Mapper;
        public int Bpm;
        public int Offset;
        public int LaneCount;
    }

    public class Note
    {
        public string Type;
        public int Lane;
        public int Bar;
        public int Lpb;
        public int Index;
        public EndPoint EndPoint;
    }

    public class EndPoint
    {
        public int Bar;
        public int Lpb;
        public int Index;
    }
}